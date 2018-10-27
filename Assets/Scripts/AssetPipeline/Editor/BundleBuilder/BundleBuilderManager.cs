/**************************************************
 *  创建人   : 夏佳文
 *  创建时间 : 2018.6.15
 *  说明     : 新的打包类，采用自动分析与打包
 * ************************************************/

using System.Collections.Generic;
using UnityEngine;
using Galaxy.DataNode;
using System.IO;
using System;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Xml;
using YamlDotNet.Serialization;
using LitJson;

namespace Galaxy.AssetPipeline
{
    internal partial class BundleBuilderManager
    {
        private Builder m_Builder;
        private IABDataSource m_DataSource;
        private List<IBundleBuilderController> m_BuildControllers;
        private AssetFileSystem m_AssetFileSystem;
        private DataNodeManager m_LocalHistoryNodeManager;
        private BuildInfo m_BuildInfo;

        public event EventHandler<AssetScanCompleteEventArgs> AssetScanComplete;

        public static event EventHandler<BundleBuildCompleteEventArgs> BundleBuildComplete;

        public BuildInfo BuildInfo
        {
            get
            {
                return m_BuildInfo;
            }
        }

        public AssetFileSystem AssetFileSystem
        {
            get { return m_AssetFileSystem; }
        }

        public DataNodeManager LocalHistoryNodeManager
        {
            get { return m_LocalHistoryNodeManager; }
        }

        public string BundleEditorPath
        {
            get;
            private set;
        }

        public string BundleEditorVersionPath
        {
            get
            {
                return BundleEditorPath + "/Versions";
            }
        }

        public VersionInfo LastVersionInfo
        {
            get;
            private set;
        }

        public List<VersionInfo> HistoryVersionInfoArray
        {
            get;
            private set;
        }

        public BundleBuilderManager()
        {
            m_BuildControllers = new List<IBundleBuilderController>();
            HistoryVersionInfoArray = new List<VersionInfo>();
            m_LocalHistoryNodeManager = new DataNodeManager();
        }

        // 需要调用UINTY接口。必须在OnEnable里面初始化，否则将报错并无效
        public void OnEnable()
        {
            BundleEditorPath = BundleConfig.BundleEditorPath;
            BuildHelper.CheckCustomPath(BundleEditorPath);
            BundleLog.OnEnable();

            bool isContinue = true;
            if (!InitLastVersion())
            {
                isContinue = false;
            }

            if (isContinue && !InitDirectoryVersionInfo())
            {
                isContinue = false;
            }

            // 可能卡一下 为了多次打包测试 可适当优化
            m_AssetFileSystem = new AssetFileSystem();
            m_AssetFileSystem.AssetScanComplete += OnScanComplete;
            m_AssetFileSystem.Start();
        }

        private void OnScanComplete(object sender, AssetScanCompleteEventArgs e)
        {
            m_AssetFileSystem.AssetScanComplete -= OnScanComplete;
            m_IsAssetFileSystemInitComplete = true;
            AssetScanComplete.Invoke(this, new AssetScanCompleteEventArgs(m_IsAssetFileSystemInitComplete));
        }
        

        private bool m_IsAssetFileSystemInitComplete = false;

        public static void BuildBundle()
        {
            BuildBundle(-1, "0/0/1", "0/0/2", "IPhonePlayer", "Common", "");
        }


        /// <summary>
        /// 外部打包接口
        /// </summary>
        /// <param name="testId">大于0标识是测试版本</param>
        /// <param name="oldVersionStr">上一次的版本号,不填或填空代表重新打</param>
        /// <param name="newVersionStr">现在的版本号,例如"1/1/1"</param>
        /// <param name="platformStr">RuntimePlatform枚举字符串,目前可用中的字段是:IPhonePlayer, Android, WindowsEditor</param>
        /// <param name="channelStr">EnumChannelType枚举字符串,目前可用的字段是:Common, Google, AppStore, WindowsPC</param>
        /// <param name="context">备注,可以不填</param>
        public static void BuildBundle(int testId, string oldVersionStr, string newVersionStr, string platformStr, string channelStr, string context = "")
        {
            BundleBuilderManager build = new BundleBuilderManager();
            build.OnEnable();
            build.AssetScanComplete += (object arg1, AssetScanCompleteEventArgs arg) =>
            {
                bool ignoreLastOne = false;
                VersionInfo oldVersion = null;
                if (!string.IsNullOrEmpty(oldVersionStr))
                {
                    oldVersion = VersionInfo.SplitStr(oldVersionStr);
                }
                else
                {
                    ignoreLastOne = true;
                }

                VersionInfo newVersion = VersionInfo.SplitStr(newVersionStr);
                bool isTest = false;
                if (testId > 0)
                {
                    isTest = true;
                    newVersion.Test = testId;
                }

                RuntimePlatform platform = (RuntimePlatform)Enum.Parse(typeof(RuntimePlatform), platformStr);
                EnumChannelType channel = (EnumChannelType)Enum.Parse(typeof(EnumChannelType), channelStr);

                build.Prepare(oldVersion, newVersion, isTest, ignoreLastOne, platform, channel, context);
                build.Build();

                if (BundleBuildComplete != null)
                {
                    BundleBuildComplete.Invoke(build, new BundleBuildCompleteEventArgs(true));
                }
            };
        }

        public void Prepare(VersionInfo oldVersion, VersionInfo newVersion, bool isTest, bool ignoreLastOne,
            RuntimePlatform platform, EnumChannelType channel, string context = "")
        {
            if (newVersion == null)
            {
                throw new Exception("newVersion is Null");
            }
            //[Root] --/-- [Channel] --/-- [Version] --/-- [Platform]
            string oldNodePath = "";
            if (oldVersion == null)
            {
                ignoreLastOne = true;
            }
            else
            {
                string platformName = BuildHelper.CustomTargetToName(platform);
                oldNodePath = string.Format("{0}/{1}/{2}", channel.ToString(), oldVersion.ToString(), platformName);
            }

            BundleLog.StartNewLog(platform, channel, LastVersionInfo, newVersion, isTest, ignoreLastOne, context);

            DataNodeManager newRecorder = new DataNodeManager();
            DataNodeManager lastRecorder = null;
            if (!ignoreLastOne)
            {
                string filePath = GetBundleBinFilePath(BundleEditorVersionPath + "/" + oldNodePath);
                BundleLog.Log(string.Format("从老版本文件[{0}]中生成新的Builder", filePath));
                newRecorder = GetBundleBuilderByFile(filePath);
            }

            if (oldVersion != null && !ignoreLastOne)
            {
                // 在记录版本信息的时候读取的是文件夹信息,导致不分大小写
                // oldNodePath = oldNodePath.ToLower();
                lastRecorder = m_LocalHistoryNodeManager.GetData<BundleHistoryVariable>(oldNodePath).GetValue<DataNodeManager>();
                if (lastRecorder == null)
                {
                    BundleLog.Error("lastRecorder is NULL : " + oldNodePath);
                    throw new Exception("lastRecorder is NULL : " + oldNodePath);
                }
            }

            // 其中newRecorder的引用会被各个controller依次修改, 优先级按照foreach顺序,即Enum(EAssetTag)中的顺序
            foreach (EAssetTag tag in m_AssetFileSystem.CrudeAssetNodeMap.Keys)
            {
                CrudeAssetNode[] builds = m_AssetFileSystem.CrudeAssetNodeMap[tag].ToArray();
                IBundleBuilderController controller = GetController(tag, builds, ignoreLastOne, newRecorder, lastRecorder, newVersion);

                BundleLog.Log(string.Format("创建tag为[{0}]的打包控制器[{1}]", tag, controller.GetType().ToString()));

                controller.Prepare();
                controller.Handle();
            }

            m_BuildInfo = new BuildInfo(isTest, newRecorder, newVersion, platform, channel);


            BundleLog.EndLog();
        }

        public void Build()
        {
            VersionInfo versionInfo = m_BuildInfo.versionInfo;
            bool isTest = m_BuildInfo.isTest;
            RuntimePlatform platform = m_BuildInfo.platform;
            EnumChannelType channel = m_BuildInfo.channel;
            DataNodeManager newRecorder = m_BuildInfo.builder;
            if (Build(newRecorder, versionInfo, platform, channel))
            {
                ChangeLastVersion(versionInfo, isTest);

                string platformName = BuildHelper.CustomTargetToName(platform);
                string newNodePath = string.Format("{0}/{1}/{2}/{3}", BundleEditorVersionPath, channel.ToString(), versionInfo.ToString(), platformName);
                SaveVersionFile(newRecorder, versionInfo, newNodePath);
            }
        }

        public void RecordBuilder()
        {
            VersionInfo versionInfo = m_BuildInfo.versionInfo;
            RuntimePlatform platform = m_BuildInfo.platform;
            EnumChannelType channel = m_BuildInfo.channel;
            DataNodeManager newRecorder = m_BuildInfo.builder;
            string platformName = BuildHelper.CustomTargetToName(platform);
            string newNodePath = string.Format("{0}/{1}/{2}/{3}", BundleEditorVersionPath, channel.ToString(), versionInfo.ToString(), platformName);
            newNodePath = BuildHelper.CheckCustomPath(newNodePath);
            SaveVersionFile(newRecorder, versionInfo, newNodePath);
        }

        private bool Build(DataNodeManager builder, VersionInfo versionInfo, RuntimePlatform platform, EnumChannelType channel)
        {
            m_DataSource = new CustomAssetDataSource();
            m_Builder = new Builder();
            List<IDataNode> allLeafNodes = new List<IDataNode>();
            builder.Root.GetAllLeafNodes(ref allLeafNodes);
            foreach (IDataNode node in allLeafNodes)
            {
                RefinedAssetNode assetNode = node.GetData<RecordVariable>().GetValue<RefinedAssetNode>();
                string bundleName = assetNode.BundleName;
                string assetPath = assetNode.CrudeNode.AssetPath;
                m_DataSource.SetAssetBundleNameAndVariant(assetPath, bundleName, "");
            }
            m_Builder.Build(m_DataSource, versionInfo, platform, channel);
            return true;
        }

        private IBundleBuilderController GetController(EAssetTag assetTag, CrudeAssetNode[] builds,
            bool ignoreLastVersion, DataNodeManager newRecorder, DataNodeManager lastRecorder, VersionInfo versionInfo)
        {
            switch (assetTag)
            {
                case EAssetTag.Atlas:
                    return (new AtlasBuilderController(assetTag, builds, this.m_AssetFileSystem.AssetDatasManager, ignoreLastVersion, newRecorder, lastRecorder, versionInfo));
                default:
                    return (new DefaultBuilderController(assetTag, builds, this.m_AssetFileSystem.AssetDatasManager, ignoreLastVersion, newRecorder, lastRecorder, versionInfo));
            }
        }

        private void SaveVersionFile(DataNodeManager builder, VersionInfo versionInfo, string relativePath)
        {
            string versionInfoFile = BuildHelper.CheckCustomPath(GetBundleVersionFilePath(relativePath));
            string buildBinFile = BuildHelper.CheckCustomPath(GetBundleBinFilePath(relativePath));
            string buildXmlFile = BuildHelper.CheckCustomPath(GetBundleXmlFilePath(relativePath));
            using (FileStream fs = new FileStream(versionInfoFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(versionInfo.ToString());
                }
            }

            using (FileStream fs = new FileStream(buildBinFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                BinaryFormatter binFormat = new BinaryFormatter();
                binFormat.Serialize(fs, builder);
            }

            using (FileStream fs = new FileStream(buildXmlFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    var serializer = new Serializer();
                    serializer.Serialize(sw, builder);
                }
            }
        }

        private void ChangeLastVersion(VersionInfo versionInfo, bool isTest)
        {
            if (isTest)
            {
                BundleLog.Log("测试版本,跳过保存版本信息");
            }
            else
            {
                string versionPath = BundleEditorPath + "/" + BundleConfig.VERSION_FILE_NAME;
                if (!File.Exists(versionPath))
                {
                    using (File.Create(versionPath))
                    {

                    }
                }
                File.WriteAllText(versionPath, versionInfo.ToString());
            }
        }

        private bool InitLastVersion()
        {
            string versionPath = BundleEditorPath + "/" + BundleConfig.VERSION_FILE_NAME;
            try
            {
                if (!File.Exists(versionPath))
                {
                    BundleLog.Log(string.Format("没有找到版本文件[{0}],自动跳过", versionPath));
                    return true;
                }

                string content = File.ReadAllText(versionPath);
                LastVersionInfo = VersionInfo.SplitStr(content);
            }
            catch (Exception e)
            {
                Debug.LogError(e);

                EditorUtility.DisplayDialog("打包错误", "解析本地版本文件" + versionPath + "时出错", "Awesome");
                return false;
            }
            return true;
        }

        // 暂时只信任本地保存得所有的文件夹，以后可能会接入SVN接口
        private bool InitDirectoryVersionInfo()
        {
            try
            {
                string DirectoryPath = BuildHelper.CheckCustomPath(BundleEditorVersionPath);
                //[Root] --/-- [Channel] --/-- [Version] --/-- [Platform]

                DirectoryInfo di = new DirectoryInfo(DirectoryPath);
                DirectoryInfo[] channelChilds = di.GetDirectories();
                foreach (DirectoryInfo channelChild in channelChilds)
                {
                    DirectoryInfo[] versionChilds = channelChild.GetDirectories();
                    foreach (DirectoryInfo versionChild in versionChilds)
                    {
                        DirectoryInfo[] platformChilds = versionChild.GetDirectories();
                        foreach (DirectoryInfo platformChild in platformChilds)
                        {
                            string nodePath = string.Format("{0}/{1}/{2}", channelChild.Name, versionChild.Name, platformChild.Name);
                            string relativePath = platformChild.FullName;
                            DataNodeManager br = InitDirectoryBundleBuilder(relativePath);
                            m_LocalHistoryNodeManager.GetOrAddNode(nodePath).SetData<BundleHistoryVariable>(new BundleHistoryVariable(br));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return true;
        }

        private DataNodeManager InitDirectoryBundleBuilder(string relativePath)
        {
            BundleLog.Log(string.Format("检测[{0}]文件夹", relativePath));
            string childVersionPath = GetBundleVersionFilePath(relativePath);
            if (!File.Exists(childVersionPath))
            {
                throw new Exception(childVersionPath + "  is Not Exited");
            }

            string content = File.ReadAllText(childVersionPath);
            VersionInfo info = VersionInfo.SplitStr(content);
            HistoryVersionInfoArray.Add(info);

            string childBundleMapPath = GetBundleBinFilePath(relativePath);
            if (!File.Exists(childBundleMapPath))
            {
                throw new Exception(childBundleMapPath + "  is Not Exited");
            }
            DataNodeManager br = GetBundleBuilderByFile(childBundleMapPath);
            return br;
        }

        private string GetBundleVersionFilePath(string relativePath)
        {
            return relativePath + "/" + BundleConfig.VERSION_FILE_NAME;
        }

        private string GetBundleBinFilePath(string relativePath)
        {
            return relativePath + "/" + BundleConfig.BUNDLE_MAP_FILE_NAME_BIN;
        }

        private string GetBundleXmlFilePath(string relativePath)
        {
            return relativePath + "/" + BundleConfig.BUNDLE_MAP_FILE_NAME_XML;
        }

        private DataNodeManager GetBundleBuilderByFile(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                BinaryFormatter binFormat = new BinaryFormatter();
                DataNodeManager br = binFormat.Deserialize(fs) as DataNodeManager;
                if (br == null)
                {
                    throw new Exception(filePath + "  Deserialize Failed");
                }
                return br;
            }
        }
    }

    public class BuildInfo
    {
        public BuildInfo(bool isTest, DataNodeManager builder, VersionInfo versionInfo, RuntimePlatform platform, EnumChannelType channel)
        {
            this.isTest = isTest;
            this.builder = builder;
            this.versionInfo = versionInfo;
            this.platform = platform;
            this.channel = channel;
        }

        public bool isTest;
        public DataNodeManager builder;
        public VersionInfo versionInfo;
        public RuntimePlatform platform;
        public EnumChannelType channel;
    }
}