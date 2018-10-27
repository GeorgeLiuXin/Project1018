namespace XWorld.AssetPipeline
{
    using CusEncoding;
    using System.Collections.Generic;
    using System.Text;
    using System;
    using UnityEngine;

    internal class BundleResourceLoader : ResourceLoader
    {
        public const string RES_RELATIVE_PATH = "Assets/AssetDatas/";
        public const int RES_RELATIVE_PATH_LENGTH = 18;

        private AssetManager m_AssetManager;
        private Dictionary<UnityEngine.Object, string> m_loadedAssetToPathDict;
        private Dictionary<string, List<AssetLoader>> m_AssetLoaderDict;
        private List<AssetLoaderGroup> m_AssetLoaderGroups;

        public BundleResourceLoader()
        {
            m_AssetLoaderDict = new Dictionary<string, List<AssetLoader>>();
            m_loadedAssetToPathDict = new Dictionary<UnityEngine.Object, string>();
            m_AssetLoaderGroups = new List<AssetLoaderGroup>();
        }

        public override void Init()
        {
            m_AssetManager = XWorldGameModule.GetGameManager<AssetManager>();
            m_AssetManager.AssetLoadFail += AssetLoadFailure;
            m_AssetManager.AssetLoadStart += AssetLoadStart;
            m_AssetManager.AssetLoadSuccess += AssetLoadSuccess;
            m_AssetManager.AssetLoadUpdate += AssetLoadUpdate;
        }

        private void AssetLoadUpdate(object sender, AssetLoadUpdateEventArgs e)
        {
            string assetPath = e.AssetPath.Remove(0, RES_RELATIVE_PATH_LENGTH);
            if (OnResourceLoadUpdate != null)
            {
                OnResourceLoadUpdate(assetPath);
            }
        }

        private void AssetLoadSuccess(object sender, AssetLoadSuccessEventArgs e)
        {
            string assetPath = e.AssetPath.Remove(0, RES_RELATIVE_PATH_LENGTH);
            bool isScene = e.IsScene;
            UnityEngine.Object target = e.Asset;

            if (OnResourceLoadSuccess != null)
            {
                OnResourceLoadSuccess(assetPath);
            }
            LoadResult result = null;

            #region single
            if (m_AssetLoaderDict.ContainsKey(assetPath))
            {
                List<AssetLoader> loaders = m_AssetLoaderDict[assetPath];
                foreach (AssetLoader loader in loaders)
                {
                    result = loader.FireCallback(true, e.IsScene, e.Asset);
                }
                m_AssetLoaderDict.Remove(assetPath);
            }
            #endregion

            #region group
            int groupLength = m_AssetLoaderGroups.Count;
            for (int i = groupLength - 1; i >= 0; i--)
            {
                AssetLoaderGroup group = m_AssetLoaderGroups[i];
                if (group.ContainsPath(assetPath))
                {
                    if (group.FireCallback(assetPath, true, e.IsScene, target))
                    {
                        m_AssetLoaderGroups.RemoveAt(i);
                    }
                }
            }
            #endregion

            if (result == null)
            {
                result = new LoadResult();
                result.isSuccess = true;
                result.assets = new UnityEngine.Object[1] { target };
                result.assetNames = new string[1] { assetPath };
            }
            if (ResourceCallBack != null)
                ResourceCallBack.Invoke(result);
        }

        private void AssetLoadStart(object sender, AssetLoadStartEventArgs e)
        {
            string assetPath = e.AssetPath.Remove(0, RES_RELATIVE_PATH_LENGTH);
            if (OnResourceLoadStart != null)
            {
                OnResourceLoadStart(assetPath);
            }
        }

        private void AssetLoadFailure(object sender, AssetLoadFailureEventArgs e)
        {
            string assetPath = e.AssetPath.Remove(0, RES_RELATIVE_PATH_LENGTH);

            if (OnResourceLoadFailure != null)
                OnResourceLoadFailure(assetPath, e.ErrorContent);
            LoadResult result = null;

            #region single
            if (m_AssetLoaderDict.ContainsKey(assetPath))
            {
                List<AssetLoader> loaders = m_AssetLoaderDict[assetPath];
                foreach (AssetLoader loader in loaders)
                {
                    result = loader.FireCallback(false, false, null);
                }
                m_AssetLoaderDict.Remove(assetPath);
            }
            #endregion

            #region group
            int groupLength = m_AssetLoaderGroups.Count;
            for (int i = groupLength - 1; i >= 0; i--)
            {
                AssetLoaderGroup group = m_AssetLoaderGroups[i];
                if (group.ContainsPath(assetPath))
                {
                    if (group.FireCallback(assetPath, false, false, null))
                    {
                        m_AssetLoaderGroups.RemoveAt(i);
                    }
                }
            }
            #endregion

            if (result == null)
            {
                result = new LoadResult();
                result.isSuccess = false;
                result.assets = new UnityEngine.Object[1] { null };
                result.assetNames = new string[1] { assetPath };
            }
            if (ResourceCallBack != null)
                ResourceCallBack.Invoke(result);
        }

        public override void DestroyAsset(string path)
        {
            m_AssetManager.Unload(RES_RELATIVE_PATH + path, this, false);
        }

        public override void DestroyAsset(UnityEngine.Object obj)
        {
            if (obj != null && m_loadedAssetToPathDict.ContainsKey(obj))
            {
                string path = m_loadedAssetToPathDict[obj];
                DestroyAsset(path);
            }
        }

        public override void DestroyAssetImmediately(string path)
        {
            m_AssetManager.Unload(RES_RELATIVE_PATH + path, this, true);
        }

        public override void DestroyAssetImmediately(UnityEngine.Object obj)
        {
            if (obj != null && m_loadedAssetToPathDict.ContainsKey(obj))
            {
                string path = m_loadedAssetToPathDict[obj];
                DestroyAssetImmediately(path);
            }
        }

        public override byte[] LoadByteBuffer(string mainPath)
        {
            return m_StreamingAssetsLoader.Load(mainPath);
        }

        public override byte[] LoadFileInStreamingAssets(string fileName)
        {
            return LoadByteBuffer(fileName);
        }

        public override byte[] LoadTextBuffer(string mainPath, bool decode)
        {
            return EncodingUtil.FileByteToLocal(this.LoadByteBuffer(mainPath));
        }

        public override string LoadTextString(string mainPath, bool decode)
        {
            return new string(Encoding.UTF8.GetChars(EncodingUtil.FileByteToLocal(this.LoadByteBuffer(mainPath))));
        }

        public override void PrintBundleList()
        {
        }

        public override void Release()
        {
            m_AssetLoaderDict.Clear();
            m_loadedAssetToPathDict.Clear();
        }
        public override int LoadAsset(string rc, LoadAssetCallback callback, object userdata = null)
        {
            return LoadAsset(rc, 1, callback, userdata);
        }
        
        public override int LoadAsset(string path, int priority, LoadAssetCallback callback, object userdata = null)
        {
            AssetLoader loader = new AssetLoader(path, priority, callback, userdata);
            m_AssetLoaderDict.ForceListAdd(path, loader);

            if (callback != null)
                m_AssetManager.StartLoad(RES_RELATIVE_PATH + path, callback.Target, priority);
            else
                m_AssetManager.StartLoad(RES_RELATIVE_PATH + path, this, priority);

            return m_AssetManager.GetRefCount(path);
        }

        public override int[] LoadAssets(string[] paths, LoadAssetCallback callback, LoadAssetCallback singleCallback, object userdata = null)
        {
            if (paths == null || paths.Length == 0)
            {
                return null;
            }
            int length = paths.Length;
            int[] priorities = new int[length];
            for (int i = 0; i < length; i++)
            {
                priorities[i] = 1;
            }
            return LoadAssets(paths, priorities, callback, singleCallback, userdata);
        }

        public override int[] LoadAssets(string[] paths, int[] priorities, LoadAssetCallback callback, LoadAssetCallback singleCallback, object userdata = null)
        {
            if (paths == null || paths.Length == 0)
            {
                return null;
            }
            int length = paths.Length;

            int[] refCounts = new int[length];
           
            AssetLoaderGroup loadArray = new AssetLoaderGroup(paths, priorities, singleCallback, callback, userdata);
            m_AssetLoaderGroups.Add(loadArray);

            for (int i = 0; i < length; i++)
            {
                string path = paths[i];
                int priority = priorities[i];
                if (singleCallback != null)
                    m_AssetManager.StartLoad(RES_RELATIVE_PATH + path, singleCallback.Target, priority);
                else
                    m_AssetManager.StartLoad(RES_RELATIVE_PATH + path, this, priority);
                int refCount = m_AssetManager.GetRefCount(path);
                refCounts[i] = refCount;
            }
            return refCounts;
        }

        public override void Tick(uint uDeltaTimeMS)
        {

        }

        public override void LoadImagePack(string relativePath, LoadAssetCallback callback, LoadAssetCallback singleCallback, object userdata = null)
        {
            List<string> bundleNames = XWorldGameModule.GetGameManager<VersionManager>().GetAssetBundlesByRelativePath(relativePath);
            LoadAssets(bundleNames.ToArray(), callback, singleCallback, userdata);
        }

    }
}
