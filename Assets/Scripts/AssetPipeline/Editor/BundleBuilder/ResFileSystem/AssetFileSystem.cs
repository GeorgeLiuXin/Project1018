/**************************************************
 *  创建人   : 夏佳文
 *  创建时间 : 2018.6.25
 *  说明     : Bundle文件夹系统,记录对应的文件信息,Bundle信息
 * ************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Reflection;
using Galaxy.DataNode;
using UnityEngine.Events;
using System.Collections;

namespace Galaxy.AssetPipeline
{
    internal class AssetFileSystem
    {
        public static bool IsStrictMode = false;
        public static readonly List<string> IGNORE_EXTENSIONS = new List<string>() { ".meta", ".cs", ".exr" ,
            ".physicMaterial", ".meta",  ".dll", ".exe", ".js" };
        public static readonly string FOLDER_NAME = "AssetDatas";
        public static readonly string ROOT_NAME = "Assets";

        public event EventHandler<AssetScanCompleteEventArgs> AssetScanComplete;
        public event EventHandler<AssetScanUpdateEventArgs> AssetScanUpdate;

        private bool m_IsInitAllAssetNodesSuccess = false;
        private bool m_IsScanRootFolderSuccess = false;

        public Dictionary<EAssetTag, List<CrudeAssetNode>> CrudeAssetNodeMap
        {   
            get { return m_CrudeAssetNodeMap; }
        }

        public string AssetFolderRoot
        {
            get;
            set;
        }

        public DataNodeManager AssetDatasManager
        {
            get
            {
                return m_AssetDatasManager;
            }
        }

        public DataNodeManager AssetTagsManager
        {
            get
            {
                return m_AssetTagsManager;
            }
        }

        private Dictionary<string, EAssetTag> m_ADPathToAssetTagMap = new Dictionary<string, EAssetTag>();
        private Dictionary<Type, List<EAssetType>> m_TypeToAssetTypeMap = new Dictionary<Type, List<EAssetType>>();
        private Dictionary<string, List<EAssetType>> m_SuffixToAssetTypeMap = new Dictionary<string, List<EAssetType>>();
        private Dictionary<string, FileInfo> m_RelativePathToFileInfoMap = new Dictionary<string, FileInfo>();
        
        private List<Type> m_AllUnityTypes = new List<Type>();
        private Dictionary<string, CrudeAssetNode> m_RelativePathToAssetNodesMap = new Dictionary<string, CrudeAssetNode>();

        //对外暴露
        private Dictionary<EAssetTag, List<CrudeAssetNode>> m_CrudeAssetNodeMap = new Dictionary<EAssetTag, List<CrudeAssetNode>>();
        
        private DataNodeManager m_AssetDatasManager = new DataNodeManager();
        private DataNodeManager m_AssetTagsManager = new DataNodeManager();

        // 需外部初始化
        //public AssetFileSystem()
        //{
        //    AssetFolderRoot = Application.dataPath + "/" + FOLDER_NAME;
        //    GalaxyBundleBrowser.ShowProgress(GalaxyBundleBrowser.EProgressType.Asset, "扫描Asset类型配置", 0f);
        //    //顺序不能改变
        //    ScanAssetType();
        //    GalaxyBundleBrowser.ShowProgress(GalaxyBundleBrowser.EProgressType.Asset, "扫描AssetDatas文件夹", 0.3f);
        //    if (!ScanRootFolder())
        //    {
        //        BundleLog.Error("ScanRootFolder失败");
        //    }
        //    GalaxyBundleBrowser.ShowProgress(GalaxyBundleBrowser.EProgressType.Asset, "正在分析资源", 0.5f);
        //    InitAllAssetNodes();
        //    GalaxyBundleBrowser.CloseProgress();
        //}
        public void Start()
        {
            EditorCoroutineRunner.StartEditorCoroutine(CoInit());
        }

        private IEnumerator CoInit()
        {
            m_IsInitAllAssetNodesSuccess = false;
            m_IsScanRootFolderSuccess = false;

            AssetFolderRoot = Application.dataPath + "/" + FOLDER_NAME;
            GalaxyBundleBrowser.ShowProgress(GalaxyBundleBrowser.EProgressType.Asset, "(1/6)扫描Asset类型配置", 0.05f);
            if (AssetScanUpdate != null)
            {
                AssetScanUpdate.Invoke(this, new AssetScanUpdateEventArgs(0.05f, 0, 0, "扫描Asset类型配置"));
            }
            yield return new WaitForEndOfFrame();
            ScanAssetType();

            GalaxyBundleBrowser.ShowProgress(GalaxyBundleBrowser.EProgressType.Asset, "(2/6)扫描AssetDatas文件夹", 0.1f);
            if (AssetScanUpdate != null)
            {
                AssetScanUpdate.Invoke(this, new AssetScanUpdateEventArgs(0.1f, 0, 0, "扫描AssetDatas文件夹"));
            }
            yield return new WaitForEndOfFrame();

            EditorCoroutineRunner.StartEditorCoroutine(CoScanRootFolder());
            while (!m_IsScanRootFolderSuccess)
            {
                yield return new WaitForEndOfFrame();
            }

            EditorCoroutineRunner.StartEditorCoroutine(CoInitAllAssetNodes());
            while (!m_IsInitAllAssetNodesSuccess)
            {
                yield return new WaitForEndOfFrame();
            }
            if (AssetScanComplete != null)
            {
                AssetScanComplete.Invoke(this, new AssetScanCompleteEventArgs(true));
            }
            GalaxyBundleBrowser.CloseProgress();
        }

        private IEnumerator CoInitAllAssetNodes()
        {
            float progress = 0;
            int totalFileCount = m_RelativePathToFileInfoMap.Count;
            totalFileCount = totalFileCount > 0 ? totalFileCount : 1;
            int currentFileCount = 0;
            string ouputLog = "正在分析资源节点: " + m_RelativePathToFileInfoMap.Count;
            BundleLog.Log(ouputLog);
            GalaxyBundleBrowser.ShowProgress(GalaxyBundleBrowser.EProgressType.Asset, "(4/6)" + ouputLog, 0.6f);
            if (AssetScanUpdate != null)
            {
                AssetScanUpdate.Invoke(this, new AssetScanUpdateEventArgs(0.6f, 0, 0, ouputLog));
            }
            yield return new WaitForEndOfFrame();

            Dictionary<string, FileInfo>.Enumerator itor = m_RelativePathToFileInfoMap.GetEnumerator();
            while (itor.MoveNext())
            {
                string relativePath = itor.Current.Key;

                currentFileCount++;
                progress = 0.6f + (currentFileCount / (float)totalFileCount) * 0.3f;
                ouputLog = string.Format("正在分析资源节点: {0}\n[{1}/{2}]", relativePath, currentFileCount, totalFileCount);
                GalaxyBundleBrowser.ShowProgress(GalaxyBundleBrowser.EProgressType.Asset, ouputLog, progress);
                if (AssetScanUpdate != null)
                {
                    AssetScanUpdate.Invoke(this, new AssetScanUpdateEventArgs(progress, currentFileCount, totalFileCount, ouputLog));
                }
                yield return new WaitForEndOfFrame();
                CrudeAssetNode node = InitAssetNode(relativePath, null);
            }

            progress = 0.9f;
            ouputLog = "初始化所有资源的标签";
            GalaxyBundleBrowser.ShowProgress(GalaxyBundleBrowser.EProgressType.Asset, "(5/6)" + ouputLog, progress);
            if (AssetScanUpdate != null)
            {
                AssetScanUpdate.Invoke(this, new AssetScanUpdateEventArgs(progress, 0, 0, ouputLog));
            }
            yield return new WaitForEndOfFrame();
            // 初始化tag 必须要等到所有Node初始完再进行
            Dictionary<string, CrudeAssetNode>.Enumerator nodeItor = m_RelativePathToAssetNodesMap.GetEnumerator();
            while (nodeItor.MoveNext())
            {
                CrudeAssetNode node = nodeItor.Current.Value;
                InitRefenceTag(node);
            }

            progress = 0.95f;
            ouputLog = "正在生成管理器";
            GalaxyBundleBrowser.ShowProgress(GalaxyBundleBrowser.EProgressType.Asset, "(6/6)" + ouputLog, progress);
            if (AssetScanUpdate != null)
            {
                AssetScanUpdate.Invoke(this, new AssetScanUpdateEventArgs(progress, 0, 0, ouputLog));
            }
            yield return new WaitForEndOfFrame();

            // 初始化DataNodeManager 必须要等到所有tag初始完再进行
            Dictionary<string, CrudeAssetNode>.Enumerator nodeItor2 = m_RelativePathToAssetNodesMap.GetEnumerator();
            while (nodeItor2.MoveNext())
            {
                CrudeAssetNode node = nodeItor2.Current.Value;
                if (!node.IsRefence)
                {
                    string nodePsath = node.AssetPath.Substring(node.AssetPath.IndexOf(FOLDER_NAME));
                    m_AssetDatasManager.GetOrAddNode(nodePsath).SetData<RecordVariable>(new RecordVariable(new RefinedAssetNode(node)));
                }

                string tagPath = node.AssetTag.ToString() + "/" + node.IsRefence.ToString() + "/" + node.AssetName;
                m_AssetDatasManager.GetOrAddNode(tagPath).SetData<RecordVariable>(new RecordVariable(new RefinedAssetNode(node)));

                m_CrudeAssetNodeMap.ForceListAdd(node.AssetTag, node);
            }
            m_IsInitAllAssetNodesSuccess = true;
        }

        private void InitAllAssetNodes()
        {
            BundleLog.Log("InitAllAssetNodes, Count = " + m_RelativePathToFileInfoMap.Count);
            Dictionary<string, FileInfo>.Enumerator itor = m_RelativePathToFileInfoMap.GetEnumerator();
            while (itor.MoveNext())
            {
                string relativePath = itor.Current.Key;
                CrudeAssetNode node = InitAssetNode(relativePath, null);
            }

            // 初始化tag 必须要等到所有Node初始完再进行
            Dictionary<string, CrudeAssetNode>.Enumerator nodeItor = m_RelativePathToAssetNodesMap.GetEnumerator();
            while (nodeItor.MoveNext())
            {
                CrudeAssetNode node = nodeItor.Current.Value;
                InitRefenceTag(node);
            }

            // 初始化DataNodeManager 必须要等到所有tag初始完再进行
            Dictionary<string, CrudeAssetNode>.Enumerator nodeItor2 = m_RelativePathToAssetNodesMap.GetEnumerator();
            while (nodeItor2.MoveNext())
            {
                CrudeAssetNode node = nodeItor2.Current.Value;
                if (!node.IsRefence)
                {
                    string nodePsath = node.AssetPath.Substring(node.AssetPath.IndexOf(FOLDER_NAME));
                    m_AssetDatasManager.GetOrAddNode(nodePsath).SetData<RecordVariable>(new RecordVariable(new RefinedAssetNode(node)));
                }

                string tagPath = node.AssetTag.ToString() + "/" + node.IsRefence.ToString() + "/" + node.AssetName;
                m_AssetDatasManager.GetOrAddNode(tagPath).SetData<RecordVariable>(new RecordVariable(new RefinedAssetNode(node)));

                m_CrudeAssetNodeMap.ForceListAdd(node.AssetTag, node);
            }
        }

        private CrudeAssetNode InitAssetNode(string relativePath, CrudeAssetNode parent)
        {
            CrudeAssetNode crudeAssetNode = new CrudeAssetNode();
            if (m_RelativePathToAssetNodesMap.ContainsKey(relativePath))
            {
                crudeAssetNode = m_RelativePathToAssetNodesMap[relativePath];
                return crudeAssetNode;
            }
            else
            {
                m_RelativePathToAssetNodesMap.Add(relativePath, crudeAssetNode);

                UnityEngine.Object target = AssetDatabase.LoadMainAssetAtPath(relativePath);
                string filePath = Path.GetFullPath(relativePath);
                FileInfo fileInfo = new FileInfo(filePath);

                string md5 = BuildHelper.GetFileMD5(fileInfo.FullName);
                string strTempPath = fileInfo.DirectoryName.Replace(@"\", "/");
                strTempPath = strTempPath.Substring(strTempPath.IndexOf(ROOT_NAME));

                crudeAssetNode.AssetName = fileInfo.Name;
                crudeAssetNode.AssetPathWithoutName = strTempPath;
                crudeAssetNode.AssetSize = fileInfo.Length;
                crudeAssetNode.AssetHash = md5;
                crudeAssetNode.AssetGUID = AssetDatabase.AssetPathToGUID(relativePath);
                crudeAssetNode.AssetTag = GetTag(strTempPath);

                Texture texTarget = target as Texture;
                if (texTarget)
                {
                    var type = System.Reflection.Assembly.Load("UnityEditor.dll").GetType("UnityEditor.TextureUtil");
                    MethodInfo methodInfo = type.GetMethod("GetStorageMemorySizeLong", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
                    crudeAssetNode.AssetSize = (long)methodInfo.Invoke(null, new object[] { target });
                    string ssss = texTarget.ToString();
                }

                try
                {
                    crudeAssetNode.AssetType = GetType(AssetDatabase.LoadMainAssetAtPath(relativePath), fileInfo.Extension);
                }
                catch (Exception e)
                {
                    BundleLog.Error(relativePath + "\n" + e);
                }
                
                if (m_RelativePathToFileInfoMap.ContainsKey(relativePath))
                {
                    crudeAssetNode.IsRefence = false;
                }

                if (parent != null)
                {
                    crudeAssetNode.Owners.Add(parent);
                }
                
                string[] refences = AssetDatabase.GetDependencies(relativePath);
                if (refences != null && refences.Length > 0)
                {
                    int length = refences.Length;
                    for (int i = 0; i < length; i++)
                    {
                        string refencePath = refences[i];

                        // 有两种情况需要排除: 1.依赖自己 2.依赖cs文件
                        if (refencePath == relativePath || !IsUnityFileValid(refencePath))
                        {
                            continue;
                        }

                        CrudeAssetNode node = InitAssetNode(refencePath, crudeAssetNode);
                        crudeAssetNode.Childs.SafeAdd(node, false);
                    }
                }
            }
            return crudeAssetNode;
        }

        private EAssetTag GetTag(string relativePath)
        {
            foreach (string adPath in m_ADPathToAssetTagMap.Keys)
            {
                if (relativePath.StartsWith(adPath))
                {
                    return m_ADPathToAssetTagMap[adPath];
                }
            }
            return EAssetTag.Unkown;
        }

        private EAssetType GetType(UnityEngine.Object target, string suffix)
        {
            foreach (Type type in m_AllUnityTypes)
            {
                if (type.IsInstanceOfType(target))
                {
                    if (!m_TypeToAssetTypeMap.ContainsKey(type))
                    {
                        BundleLog.Error("TypeToAssetTypeMap 中没有该type : " + type);
                    }
                    if (!m_SuffixToAssetTypeMap.ContainsKey(suffix))
                    {
                        BundleLog.Error("SuffixToAssetTypeMap 中没有该suffix : " + suffix);
                    }
                    List<EAssetType> typeTotypes = m_TypeToAssetTypeMap[type];
                    List<EAssetType> suffixTotypes = m_SuffixToAssetTypeMap[suffix];

                    foreach (EAssetType a in typeTotypes)
                    {
                        foreach (EAssetType b in suffixTotypes)
                        {
                            if (a == b)
                                return a;
                        }
                    }
                }
            }
            BundleLog.Error(string.Format("[{0}] 不是设定的类型,SUFFIX [{1}], 应该是[{2}]", target.name, suffix, target.GetType().ToString()));
            return EAssetType.UNKOWN;
        }

        private void ScanAssetTag(string[] tempRelativeDirectoryPaths)
        {
            BundleLog.Log("扫描AssetTag");
            List<string> tempRelatives = new List<string>(tempRelativeDirectoryPaths);
            string[] tagPaths = null;
            foreach (EAssetTag tag in Enum.GetValues(typeof(EAssetTag)))
            {
                List<string> subPaths = GetSubPathsByAssetTag(tag);
                GetTagPaths(tag, out tagPaths);
                if (tagPaths != null)
                {
                    foreach (string tagPath in tagPaths)
                    {
                        string relativePath = ROOT_NAME + BundleConfig.DEFAULT_SPLIT + FOLDER_NAME + BundleConfig.DEFAULT_SPLIT + tagPath;
                        if (tempRelatives.Contains(relativePath))
                        {
                            tempRelatives.Remove(relativePath);
                            m_ADPathToAssetTagMap.Add(relativePath, tag);
                        }
                    }
                }
            }
            foreach (string relativePath in tempRelatives)
            {
                // 默认都是Normal
                m_ADPathToAssetTagMap.Add(relativePath, EAssetTag.Normal);
            }
        }

        private List<string> GetSubPathsByAssetTag(EAssetTag tag)
        {
            List<string> subPaths = new List<string>();

            string[] tagPaths = null;
            GetTagPaths(tag, out tagPaths);
            if (tagPaths != null)
            {
                foreach (string tagPath in tagPaths)
                {
                    string relativePath = ROOT_NAME + BundleConfig.DEFAULT_SPLIT + FOLDER_NAME + BundleConfig.DEFAULT_SPLIT + tagPath;
                    subPaths.SafeAdd(relativePath, false);
                    string fullPath = Path.GetFullPath(relativePath);
                    DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
                    if (directoryInfo != null)
                    {
                        DirectoryInfo[] childDirectories = directoryInfo.GetDirectories("*", SearchOption.AllDirectories);
                        foreach (DirectoryInfo di in childDirectories)
                        {
                            string diFullPath = di.FullName.Replace("\\","/");
                            string subRelativePath = diFullPath.Substring(diFullPath.IndexOf(ROOT_NAME));
                            subPaths.SafeAdd(subRelativePath, false);
                        }
                    }
                }
            }
            return subPaths;
        }

        private void ScanAssetType()
        {
            BundleLog.Log("扫描AssetType");
            string[] suffixs = null;
            Type[] types = null;
            foreach (EAssetType assetType in Enum.GetValues(typeof(EAssetType)))
            {
                GetSuffixAndTypes(assetType, out suffixs, out types);

                if (suffixs != null)
                    foreach (string suffix in suffixs)
                    {
                        m_SuffixToAssetTypeMap.ForceListAdd(suffix, assetType, false);
                    }
                if (types != null)
                    foreach (Type type in types)
                    {
                        m_TypeToAssetTypeMap.ForceListAdd(type, assetType, false);
                        m_AllUnityTypes.SafeAdd(type, false);
                    }
            }
        }

        private bool IsUnityFileValid(string unityPath)
        {
            string fullPath = Path.GetFullPath(unityPath);
            FileInfo file = new FileInfo(fullPath);
            if (file != null)
            {
                string fullName = file.FullName;
                string extension = file.Extension;
                string strTempPath = fullName.Replace(@"\", "/");

                if (IGNORE_EXTENSIONS.Contains(extension))
                {
                    return false;
                }

                if (!m_SuffixToAssetTypeMap.ContainsKey(extension))
                {
                    BundleLog.Warning(string.Format("[{0}] EAssetType没有配置此格式的后缀", fullName));
                    return false;
                }
                return true;
            }
            return false;
        }

        private IEnumerator CoScanRootFolder()
        {
            DirectoryInfo rootDirectory = new DirectoryInfo(AssetFolderRoot);
            DirectoryInfo[] childDirectoryInfos = rootDirectory.GetDirectories("*", SearchOption.AllDirectories);

            BundleLog.Log("扫描目标文件夹 : " + rootDirectory);
            GalaxyBundleBrowser.ShowProgress(GalaxyBundleBrowser.EProgressType.Asset, "(3/6)扫描目标文件夹 : " + rootDirectory, 0.1f);
            if (AssetScanUpdate != null)
            {
                AssetScanUpdate.Invoke(this, new AssetScanUpdateEventArgs(0.1f, 0, 0, "扫描目标文件夹 : " + rootDirectory));
            }
            yield return new WaitForSeconds(0.5f);

            List<string> tempRelativeDirectoryPaths = new List<string>();
            FileInfo[] fileInfos = rootDirectory.GetFiles("*", SearchOption.AllDirectories);
            int totalFileCount = fileInfos.Length;
            totalFileCount = totalFileCount > 0 ? totalFileCount : 1; 
            int currentFileCount = 0;
            string ouputLog = "";

            foreach (FileInfo file in fileInfos)
            {
                currentFileCount++;
                
                string fullName = file.FullName;
                string extension = file.Extension;
                string strTempPath = fullName.Replace(@"\", "/");

                if (IGNORE_EXTENSIONS.Contains(extension))
                {
                    continue;
                }

                float progress = 0.1f + (currentFileCount / (float)totalFileCount) * 0.5f;
                ouputLog = string.Format("扫描目标文件夹 : {2}\n[{0}/{1}]", currentFileCount, totalFileCount, file.FullName);
                GalaxyBundleBrowser.ShowProgress(GalaxyBundleBrowser.EProgressType.Asset, ouputLog, progress);
                if (AssetScanUpdate != null)
                {
                    AssetScanUpdate.Invoke(this, new AssetScanUpdateEventArgs(progress, currentFileCount, totalFileCount, ouputLog));
                }
                yield return new WaitForEndOfFrame();

                if (!m_SuffixToAssetTypeMap.ContainsKey(extension))
                {
                    BundleLog.Warning(string.Format("[{0}] EAssetType没有配置此格式的后缀", fullName));
                    continue;
                }

                string relativePath = strTempPath.Substring(strTempPath.IndexOf(ROOT_NAME));

                string systemDirectoryPath = file.Directory.FullName.Replace(@"\", "/");
                string relativeDirectoryPath = systemDirectoryPath.Substring(systemDirectoryPath.IndexOf(ROOT_NAME));

                UnityEngine.Object target = AssetDatabase.LoadMainAssetAtPath(relativePath);
                if (target != null)
                {
                    m_RelativePathToFileInfoMap.Add(relativePath, file);
                    tempRelativeDirectoryPaths.SafeAdd(relativeDirectoryPath, false);
                }
                else
                {
                    BundleLog.Error(string.Format("[{0}] 不是UNITY支持的格式", relativePath));
                    continue;
                }
            }

            ouputLog = "扫描AssetTag配置";
            BundleLog.Log(ouputLog);
            GalaxyBundleBrowser.ShowProgress(GalaxyBundleBrowser.EProgressType.Asset, ouputLog, 0.6f);
            if (AssetScanUpdate != null)
            {
                AssetScanUpdate.Invoke(this, new AssetScanUpdateEventArgs(0.6f, 0, 0, ouputLog));
            }
            yield return new WaitForEndOfFrame();
            ScanAssetTag(tempRelativeDirectoryPaths.ToArray());
            m_IsScanRootFolderSuccess = true;
        }


        private bool ScanRootFolder()
        {
            bool isSuccess = true;
            DirectoryInfo rootDirectory = new DirectoryInfo(AssetFolderRoot);
            DirectoryInfo[] childDirectoryInfos = rootDirectory.GetDirectories("*", SearchOption.AllDirectories);
            BundleLog.Log("扫描目标文件夹 : " + rootDirectory);

            List<string> tempRelativeDirectoryPaths = new List<string>();
            FileInfo[] fileInfos = rootDirectory.GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo file in fileInfos)
            {
                string fullName = file.FullName;
                string extension = file.Extension;
                string strTempPath = fullName.Replace(@"\", "/");

                if (IGNORE_EXTENSIONS.Contains(extension))
                {
                    continue;
                }

                if (!m_SuffixToAssetTypeMap.ContainsKey(extension))
                {
                    BundleLog.Error(string.Format("[{0}] EAssetType没有配置此格式的后缀", fullName));
                    if (IsStrictMode) isSuccess = false;
                    continue;
                }

                string relativePath = strTempPath.Substring(strTempPath.IndexOf(ROOT_NAME));
                
                string systemDirectoryPath = file.Directory.FullName.Replace(@"\", "/");
                string relativeDirectoryPath = systemDirectoryPath.Substring(systemDirectoryPath.IndexOf(ROOT_NAME));

                UnityEngine.Object target = AssetDatabase.LoadMainAssetAtPath(relativePath);
                if (target != null)
                {
                    m_RelativePathToFileInfoMap.Add(relativePath, file);
                    tempRelativeDirectoryPaths.SafeAdd(relativeDirectoryPath, false);
                }
                else
                {
                    BundleLog.Error(string.Format("[{0}] 不是UNITY支持的格式", relativePath));
                    if (IsStrictMode) isSuccess = false;
                    continue;
                }
            }
            ScanAssetTag(tempRelativeDirectoryPaths.ToArray());
            return isSuccess;
        }
        private EAssetTag InitRefenceTag(CrudeAssetNode node)
        {
            if (node.IsRefence)
            {
                if (node.AssetTag == EAssetTag.Unkown)
                {
                    if (node.Owners.Count > 0)
                    {
                        List<EAssetTag> tempTag = new List<EAssetTag>(); ;
                        foreach (CrudeAssetNode parent in node.Owners)
                        {
                            EAssetTag parentTag = EAssetTag.Unkown;
                            if (parent.AssetTag == EAssetTag.Unkown)
                            {
                                parentTag = InitRefenceTag(parent);
                            }
                            else
                            {
                                parentTag = parent.AssetTag;
                            }
                            tempTag.Add(parentTag);
                        }
                        bool isParentSameTag = true;
                        foreach (EAssetTag tag1 in tempTag)
                        {
                            foreach (EAssetTag tag2 in tempTag)
                            {
                                if (tag1 != tag2)
                                {
                                    isParentSameTag = false;
                                    break;
                                }
                            }
                        }
                        if (isParentSameTag)
                            node.AssetTag = tempTag[0];
                        else
                            node.AssetTag = EAssetTag.Normal;
                    }
                    else
                    {
                        BundleLog.Error(string.Format("Node [{0}] 不是依赖的资源", node.AssetPath));
                    }
                }
            }
            else if (node.AssetTag == EAssetTag.Unkown)
            {
                BundleLog.Error(string.Format("Node [{0}] 是AssetDatas中的资源,但是却没有assetTag", node.AssetPath));
            }
            return node.AssetTag;
        }

        public static void GetTagPaths(EAssetTag oa, out string[] tagPaths)
        {
            tagPaths = null;
            Type type = oa.GetType();
            string name = Enum.GetName(type, oa);
            FieldInfo field = type.GetField(name);
            AssetBuildAttribute attribute = Attribute.GetCustomAttribute(field, typeof(AssetBuildAttribute)) as AssetBuildAttribute;
            tagPaths = attribute == null ? null : attribute.TagPaths;
        }

        public static void GetSuffixAndTypes(EAssetType oa, out string[] suffixs, out Type[] types)
        {
            suffixs = null;
            types = null;

            Type type = oa.GetType();
            string name = Enum.GetName(type, oa);
            FieldInfo field = type.GetField(name);
            AssetBuildAttribute attribute = Attribute.GetCustomAttribute(field, typeof(AssetBuildAttribute)) as AssetBuildAttribute;
            suffixs = attribute == null ? null : attribute.Suffix;
            types = attribute == null ? null : attribute.Types;
        }
    }
}