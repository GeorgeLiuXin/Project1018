using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Collections;
using XWorld;

namespace XWorld
{
    public class VersionManager : XWorldGameManagerBase
    {
        public const bool EnableAppendHashToAssetBundlePath = true;
        public const string ASSETBUNDLE_STREAMING_ASSETS_DIR = "Bundles";
        public const string ASSETBUNDLE_HASH_SEPARATOR = "_";
        public const string ASSETBUNDLE_MAP_FILENAME = "assetbundlemap.json";
        public const string ASSETBUNDLE_MAP_MD5_FILENAME = "assetbundlemap.json.md5";
        private const string ASSETBUNDLE_MAP_MD5_TEMP_FILENAME = "assetbundlemap.json.md5.temp";
        public const string STREAMINGASSET_BUNDLE_LIST_FILENAME = "streamingassetlist";
        private const string ASSETBUNDLE_HASH_QUERY_PARAMETER = "?hash=";
        public const string META_EXTENSION = "meta";

        public List<string> RemoteUpdateList
        {
            get
            {
                return _remoteUpdateList;
            }
        }

        public List<string> LocalUpdateList
        {
            get
            {
                return _localUpdateList;
            }
        }

        public List<string> NameUpdateList
        {
            get
            {
                return _nameUpdateList;
            }
        }

        public override void InitManager()
        {

        }

        public override void Update(float fElapseTimes)
        {
            if (_isCompress)
            {
                if (_compressProgressAction != null)
                {
                    _compressProgressAction(_compressProgress, _compressfileCount, _compresscompleteCount);
                }

                if (_compressProgress >= 1f)
                {
                    _isCompress = false;
                }
            }

            if (m_CompressLogs.Count > 0)
            {
                lock (m_Lock)
                {
                    while (m_CompressLogs.Count > 0)
                    {
                        string logStr = m_CompressLogs.Dequeue();
                        GameLogger.DebugLog(LOG_CHANNEL.LOGIC, logStr);
                    }
                }
            }
        }

        public override void ShutDown()
        {

        }

        public string GetCachePath()
        {
            return GetCachePath(ASSETBUNDLE_MAP_FILENAME);
        }

        public bool CheckClientConfig()
        {
            string file = GetDocumentPath(ASSETBUNDLE_MAP_FILENAME);
            if (!File.Exists(file))
            {
                return false;
            }
            return true;
        }

        Thread m_CompressThread;

        //本地目录与客户端目录转换
        public void CacheStreamingAssets(Action<float, int, int> progress)
        {
            _compressProgressAction = progress;
            if (Application.platform == RuntimePlatform.Android)
            {
                GameLogger.DebugLog(LOG_CHANNEL.LOGIC, "Android 平台解压开始");
                _isCompress = true;
                StreamingAssets.temporaryCachePath = GlobalAssetSetting.PersistentBundlePath;
                StreamingAssets.dataPath = Application.dataPath;
                StreamingAssets.processHandle = OnCompressProgress;
                StreamingAssets.logHandle = OnCompressLogHandle;
                m_CompressThread = new Thread(StreamingAssets.ExtractStreamingAssets);
                m_CompressThread.Start();
            }
            else
            {
                CopyStreamingAsset();
            }
        }

        private static object m_Lock = new object();
        private Queue<string> m_CompressLogs = new Queue<string>();
        private void OnCompressLogHandle(string obj)
        {
            lock (m_Lock)
            {
                m_CompressLogs.Enqueue(obj);
            }
        }

        public void CopyStreamingAsset(bool isFirstTime = true) {
            string streamingAssetsBundleDir = GlobalAssetSetting.StreamingDataPath;

            if (!Directory.Exists(streamingAssetsBundleDir))
            {
                if (isFirstTime)
                {
                    _compressProgressAction(1, 1, 1);
                }
                return;
            }

            this._streamingAssetsBundles = new Dictionary<string, string>();

            var files = Directory.GetFiles(streamingAssetsBundleDir, "*", SearchOption.AllDirectories);

            int length = files.Length;

            for (int i = 0; i < length; i++)
            {
                string fileName = Path.GetFileName(files[i]);
                //string extensionName = Path.GetExtension(files[i]);

                if (fileName == ASSETBUNDLE_MAP_FILENAME || fileName == ASSETBUNDLE_MAP_MD5_FILENAME)
                {
                    // 非手机端强制模拟删除沙箱缓存操作
                    if (!isFirstTime && !Application.isMobilePlatform)
                    {
                        if (File.Exists(GetDocumentPath(fileName)))
                        {
                            File.Delete(GetDocumentPath(fileName));
                        }
                    }
                    if (!File.Exists(GetDocumentPath(fileName)))
                    {
                        File.Copy(files[i], GetDocumentPath(fileName));
                    }
                    continue;
                }
                string fileRelativePath = Utility.GetRelativePath(files[i], streamingAssetsBundleDir);
                this._streamingAssetsBundles.Add(fileRelativePath.Replace("\\", "/"), files[i]);

            }
            if (isFirstTime)
            {
                if (_compressProgressAction != null)
                {
                    _compressProgressAction(1, 1, 1);
                }
            }
        }

        public void InitAssetBundleMap()
        {
            using (var sr = new StreamReader(GetDocumentPath(ASSETBUNDLE_MAP_FILENAME)))
            {
                this._assetBundleMap = LitJson.JsonMapper.ToObject<AssetBundleMap>(sr);

                this._assetToBundle = new Dictionary<string, string>();
                this._relativePathBundleDict = new Dictionary<string, List<string>>();
                this._bundleDict = new Dictionary<string, AssetBundleMap.AssetBundleItem>();

                for (int i = 0; i < this._assetBundleMap.assetBundleBuilds.Length; i++)
                {
                    var assetBundleBuild = this._assetBundleMap.assetBundleBuilds[i];

                    for (int j = 0; j < assetBundleBuild.assetNames.Length; j++)
                    {
                        this._assetToBundle.Add(assetBundleBuild.assetNames[j], assetBundleBuild.assetBundlePath);
                        this._relativePathBundleDict.ForceListAdd(assetBundleBuild.relativePath, assetBundleBuild.bundleName);
                    }

                    this._bundleDict.Add(assetBundleBuild.assetBundlePath, assetBundleBuild);
                }
            }
            this._variant.Init(this._assetBundleMap.allAssetBundlesWithVariant, new string[] { "hd", "sd" });
        }

        //检查所有文件,取得更新列表,轻度预防用户自己修改了文件
        public bool CheckAllAssetBundles(out List<string> remoteUpdateList, out List<string> localUpdateList, out long downloadLength)
        {
            downloadLength = 0;
            this._remoteUpdateList = new List<string>();
            this._localUpdateList = new List<string>();
            this._nameUpdateList = new List<string>();
            this._bundlePathDict = new Dictionary<string, AssetBundleMap.AssetBundleItem>();

            var uselessFiles = new List<string>(Directory.GetFiles(GlobalAssetSetting.PersistentBundlePath, "*.dat", SearchOption.AllDirectories));
            List<string> uselessFileList = new List<string>();
            foreach (string name in uselessFiles)
            {
                uselessFileList.Add(name.Replace("\\", "/"));
            }

            for (int i = 0; i < this._assetBundleMap.allAssetBundles.Length; i++)
            {
                string assetBundlePath = this._assetBundleMap.allAssetBundles[i];
                string localBundlePath = GetLocalBundlePath(assetBundlePath.Replace("\\", "/"));

                var item = new AssetBundleMap.AssetBundleItem();
                item.assetBundlePath = localBundlePath;
                var list = new List<string>();
                for (int j = 0; j < this._bundleDict[assetBundlePath].directDependencies.Length; j++)
                {
                    var directDependencies = this._bundleDict[assetBundlePath].directDependencies;
                    string dependencyLocalPath = GetLocalBundlePath(directDependencies[j].Replace("\\", "/"));
                    list.Add(dependencyLocalPath);
                }
                item.directDependencies = list.ToArray();
                item.CRC = this._bundleDict[assetBundlePath].CRC;
                this._bundlePathDict.Add(assetBundlePath, item);

                int length = this._bundleDict[assetBundlePath].length;
                var fi = new FileInfo(localBundlePath);

                if (!fi.Exists || fi.Length != length)
                {
                    if (fi.Exists)
                    {
                        Debug.Log("<color=#FFBBFF>VersionManager local file corrupted " + fi.FullName +
                            " length: " + fi.Length + " should be: " + length + "</color>");
                    }

                    string hash = this._bundleDict[assetBundlePath].assetBundleHash;

#pragma warning disable 0429
                    string query = EnableAppendHashToAssetBundlePath ?
                        string.Empty : ASSETBUNDLE_HASH_QUERY_PARAMETER + hash;
#pragma warning restore 0429

                    string md5 = this._bundleDict[assetBundlePath].md5;
                    string url = GetRemoteUrl(GlobalAssetSetting.RemoteUrl, assetBundlePath + query);
                    string tempDownPath = GetLocalDownloadCachePath(assetBundlePath);

                    downloadLength += length;
                    this._remoteUpdateList.Add(url);
                    this._localUpdateList.Add(tempDownPath);
                    this._nameUpdateList.Add(assetBundlePath);
                }
                else
                {
                    uselessFileList.Remove(localBundlePath.Replace("\\", "/"));
                }
            }

            // Remove files not in current assetbundlemap.json
            for (int i = 0; i < uselessFileList.Count; i++)
            {
                string uselesspath = uselessFileList[i];
                GameLogger.DebugLog(LOG_CHANNEL.ASSET, "删除无用资源 " + uselesspath);
                File.Delete(uselesspath);
            }

            remoteUpdateList = this._remoteUpdateList;
            localUpdateList = this._localUpdateList;
            if (this._remoteUpdateList.Count == 0)
            {
                this._localUpdateList.Clear();
                this._remoteUpdateList.Clear();
                this._nameUpdateList.Clear();
                return true;
            }
            return false;
        }

        //检查刚下载文件,不对需要重下
        public bool CheckDownloadBundles(out List<string> remoteUpdateList, out List<string> localUpdateList, out long downloadLength)
        {
            downloadLength = 0;
            remoteUpdateList = new List<string>();
            localUpdateList = new List<string>();
            List<string> tmpRemoteUpdateList = new List<string>();
            List<string> tmpLocalUpdateList = new List<string>();
            List<string> tmpNameUpdateList = new List<string>();

            if (_localUpdateList == null) {
                return true;
            }
            int legth = _localUpdateList.Count;
            for (int i = 0; i < legth; i++)
            {
                string assetBundlePath = this._nameUpdateList[i];
                if (!this._bundleDict.ContainsKey(assetBundlePath))
                {
                    continue;
                }
                int configLegth = this._bundleDict[assetBundlePath].length;
                string configMd5 = this._bundleDict[assetBundlePath].md5;
                bool checkL = CheckFileLength(_localUpdateList[i], configLegth);
                bool checkM = CheckFileMD5(_localUpdateList[i], configMd5);
                if (checkL && checkM) {
                    continue;
                }
                downloadLength += this._bundleDict[assetBundlePath].length;
                tmpRemoteUpdateList.Add(_remoteUpdateList[i]);
                tmpLocalUpdateList.Add(_localUpdateList[i]);
                tmpNameUpdateList.Add(assetBundlePath);
            }

            this._remoteUpdateList = tmpRemoteUpdateList;
            this._localUpdateList = tmpLocalUpdateList;
            this._nameUpdateList = tmpNameUpdateList;
            if (this._remoteUpdateList.Count == 0)
            {
                this._localUpdateList.Clear();
                this._remoteUpdateList.Clear();
                this._nameUpdateList.Clear();
                return true;
            }
            
            remoteUpdateList = this._remoteUpdateList;
            localUpdateList = this._localUpdateList;
            return false;
        }

        private bool CheckFileLength(string filePath, int configLength)
        {
            var fi = new FileInfo(filePath);
            if (!fi.Exists)
            {
                return false;
            }
            int length = (int)fi.Length;
            if (length != 0 && configLength != length)
            {
                Debug.LogErrorFormat("File length error! expected " + configLength + " now " + length,
                    length, string.Empty);
                return false;
            }
            return true;
        }

        private bool CheckFileMD5(string filePath, string configMd5)
        {
            string md5 = Utility.ComputeMd5Hash(filePath);
            if (!string.IsNullOrEmpty(md5) && configMd5 != md5)
            {
                Debug.LogErrorFormat("File md5 error! expected " + configMd5 + " now " + md5, 0, md5);
                return false;
            }
            return true;
        }

        public string GetDocumentPath(string fileName)
        {
            return Path.Combine(GlobalAssetSetting.PersistentBundlePath, fileName);
        }

        public string GetCachePath(string fileName)
        {
            return Path.Combine(GlobalAssetSetting.TemporaryBundlePath, fileName);
        }

        private void OnCompressProgress(float progress, int fileCount, int completeCount)
        {
            _compressProgress = progress;
            _compressfileCount = fileCount;
            _compresscompleteCount = completeCount;

        }
        
        public string[] GetAllAssetBundles()
        {
            if (this._assetBundleMap != null)
            {
                return this._assetBundleMap.allAssetBundles;
            }

            return this._emptyStringArray;
        }

        public string GetAssetBundlePath(string assetPath)
        {
            if (this._assetToBundle == null || string.IsNullOrEmpty(assetPath))
            {
                return string.Empty;
            }

            if (this._assetToBundle.ContainsKey(assetPath))
            {
                return this._assetToBundle[assetPath];
            }

            return string.Empty;
        }

        public List<string> GetAssetBundlesByRelativePath(string relativePath)
        {
            List<string> bundles = new List<string>();
            if (this._relativePathBundleDict == null || string.IsNullOrEmpty(relativePath))
            {
                return bundles;
            }

            if (this._relativePathBundleDict.ContainsKey(relativePath))
            {
                bundles = this._relativePathBundleDict[relativePath];
            }

            return bundles;
        }

        public string[] GetDirectDependenciesPath(string assetBundlePath)
        {
            if (this._bundleDict == null || string.IsNullOrEmpty(assetBundlePath))
            {
                return this._emptyStringArray;
            }

            if (this._bundleDict.ContainsKey(assetBundlePath))
            {
                return this._bundleDict[assetBundlePath].directDependencies;
            }

            return this._emptyStringArray;
        }

        public string GetAssetBundleFilePath(string assetBundlePath)
        {
            if (this._bundlePathDict == null || string.IsNullOrEmpty(assetBundlePath))
            {
                return string.Empty;
            }

            assetBundlePath = this._variant.RemapVariantPath(assetBundlePath);
            
            if (this._bundlePathDict.ContainsKey(assetBundlePath))
            {
                return this._bundlePathDict[assetBundlePath].assetBundlePath;
            }
            return string.Empty;
        }

        public uint GetAssetBundleCRC(string assetBundlePath)
        {
            if (this._bundlePathDict == null || string.IsNullOrEmpty(assetBundlePath))
            {
                return 0u;
            }

            assetBundlePath = this._variant.RemapVariantPath(assetBundlePath);

            if (this._bundlePathDict.ContainsKey(assetBundlePath))
            {
                return this._bundlePathDict[assetBundlePath].CRC;
            }
            return 0u;
        }

        public void SetVariants(string[] variants)
        {
            this._variant.SetVariants(variants);
        }
        
        private string GetAssetBundlePathWithHash(string assetBundlePath)
        {
            #pragma warning disable 0162
            if (EnableAppendHashToAssetBundlePath)
            {
                return BuildHelper.PathNormalized(assetBundlePath);
            }
            return BuildHelper.PathNormalized(Utility.InsertHashToAssetBundlePath(assetBundlePath,
                this._bundleDict[assetBundlePath].assetBundleHash));
            #pragma warning restore 0162
        }

        public string GetRemoteUrl(string remote, string filename)
        {
            return remote + "/" + BuildHelper.CustomTargetToName(Application.platform) + "/" + filename;
        }


        private string GetLocalBundlePath(string assetBundlePath)
        {
            string assetBundlePathWithHash = GetAssetBundlePathWithHash(assetBundlePath);

            if (this._streamingAssetsBundles != null &&
                this._streamingAssetsBundles.ContainsKey(assetBundlePathWithHash))
            {
                return this._streamingAssetsBundles[assetBundlePathWithHash];
            }
            else
            {
                return GetDocumentPath(assetBundlePathWithHash);
            }
        }



        public string GetLocalDownloadCachePath(string assetBundlePath)
        {
            string assetBundlePathWithHash = GetAssetBundlePathWithHash(assetBundlePath);
            return GetCachePath(assetBundlePathWithHash);
        }
        
        private Action<bool> _completeCallback;
        private AssetBundleMap _assetBundleMap;
        private Dictionary<string, string> _assetToBundle;
        private Dictionary<string, List<string>> _relativePathBundleDict;
        private Dictionary<string, AssetBundleMap.AssetBundleItem> _bundleDict;
        private Action<bool> _updateCompleteCallback;
        private List<string> _remoteUpdateList;
        private List<string> _localUpdateList;
        private List<string> _nameUpdateList;
        private Dictionary<string, AssetBundleMap.AssetBundleItem> _bundlePathDict;
        private Dictionary<string, string> _streamingAssetsBundles;
        private Variant _variant = new Variant();
        private string[] _emptyStringArray = new string[0];
        
        private Action<float, int, int> _compressProgressAction;
        private bool _isCompress = false;
        private float _compressProgress = 0;
        private int _compressfileCount = 0;
        private int _compresscompleteCount = 0;
    }
}
