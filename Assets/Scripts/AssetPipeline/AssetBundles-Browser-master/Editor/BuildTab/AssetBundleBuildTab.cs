using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Galaxy.AssetBundleBrowser.AssetBundleDataSource;

namespace Galaxy.AssetBundleBrowser
{
    [System.Serializable]
    internal class AssetBundleBuildTab
    {
        const string k_BuildPrefPrefix = "ABBBuild:";

        private string m_streamingPath = "Assets/StreamingAssets";

        [SerializeField]
        private bool m_AdvancedSettings;

        [SerializeField]
        private Vector2 m_ScrollPosition;


        class ToggleData
        {
            internal ToggleData(bool s,
                string title,
                string tooltip,
                List<string> onToggles,
                BuildAssetBundleOptions opt = BuildAssetBundleOptions.None)
            {
                if (onToggles.Contains(title))
                    state = true;
                else
                    state = s;
                content = new GUIContent(title, tooltip);
                option = opt;
            }
            //internal string prefsKey
            //{ get { return k_BuildPrefPrefix + content.text; } }
            internal bool state;
            internal GUIContent content;
            internal BuildAssetBundleOptions option;
        }

        private AssetBundleInspectTab m_InspectTab;

        [SerializeField]
        private BuildTabData m_UserData;

        List<ToggleData> m_ToggleData;
        ToggleData m_ForceRebuild;
        ToggleData m_CopyToStreaming;
        GUIContent m_TargetContent;
        GUIContent m_ChannelContent;
        GUIContent m_CompressionContent;
        internal enum CompressOptions
        {
            Uncompressed = 0,
            StandardCompression,
            ChunkBasedCompression,
        }
        GUIContent[] m_CompressionOptions =
        {
            new GUIContent("No Compression"),
            new GUIContent("Standard Compression (LZMA)"),
            new GUIContent("Chunk Based Compression (LZ4)")
        };
        int[] m_CompressionValues = { 0, 1, 2 };


        internal AssetBundleBuildTab()
        {
            m_AdvancedSettings = false;
            m_UserData = new BuildTabData();
            m_UserData.OnToggles = new List<string>();
            m_UserData.UseDefaultPath = true;
        }

        internal void OnDisable()
        {
            string dataPath = BuildHelper.CheckConfigPath(BuildHelper.BUILDER_NAME);

            using (FileStream file = File.Create(dataPath))
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    LitJson.JsonWriter jw = new LitJson.JsonWriter(sw);
                    jw.PrettyPrint = true;
                    LitJson.JsonMapper.ToJson(this.m_UserData, jw);
                }
            }
        }

        internal void OnEnable(Rect pos, EditorWindow parent)
        {
            m_InspectTab = (parent as AssetBundleBrowserMain).m_InspectTab;

            //LoadData...
            string dataPath = BuildHelper.CheckConfigPath(BuildHelper.BUILDER_NAME);

            if (File.Exists(dataPath))
            {
                using (FileStream file = File.Open(dataPath, FileMode.Open))
                {
                    using (StreamReader tr = new StreamReader(file))
                    {
                        this.m_UserData = LitJson.JsonMapper.ToObject<BuildTabData>(tr);
                    }
                }
            }

            m_ToggleData = new List<ToggleData>();
            m_ToggleData.Add(new ToggleData(
                false,
                "Exclude Type Information",
                "Do not include type information within the asset bundle (don't write type tree).",
                m_UserData.OnToggles,
                BuildAssetBundleOptions.DisableWriteTypeTree));
            m_ToggleData.Add(new ToggleData(
                false,
                "Force Rebuild",
                "Force rebuild the asset bundles",
                m_UserData.OnToggles,
                BuildAssetBundleOptions.ForceRebuildAssetBundle));
            m_ToggleData.Add(new ToggleData(
                false,
                "Ignore Type Tree Changes",
                "Ignore the type tree changes when doing the incremental build check.",
                m_UserData.OnToggles,
                BuildAssetBundleOptions.IgnoreTypeTreeChanges));
            m_ToggleData.Add(new ToggleData(
                false,
                "Append Hash",
                "Append the hash to the assetBundle name.",
                m_UserData.OnToggles,
                BuildAssetBundleOptions.AppendHashToAssetBundleName));
            m_ToggleData.Add(new ToggleData(
                false,
                "Strict Mode",
                "Do not allow the build to succeed if any errors are reporting during it.",
                m_UserData.OnToggles,
                BuildAssetBundleOptions.StrictMode));
            m_ToggleData.Add(new ToggleData(
                false,
                "Dry Run Build",
                "Do a dry run build.",
                m_UserData.OnToggles,
                BuildAssetBundleOptions.DryRunBuild));


            m_ForceRebuild = new ToggleData(
                false,
                "清空文件夹",
                "清空目标文件夹的所有文件，如果你选择拷贝到StreamingAssets文件夹，也会清空StreamingAssets/AssetBundles文件夹。",
                m_UserData.OnToggles);
            m_CopyToStreaming = new ToggleData(
                false,
                "拷贝到StreamingAssets",
                "当打包完成后，资源将会被拷贝到 " + m_streamingPath + "，使得玩家可以变为单机模式",
                m_UserData.OnToggles);

            m_TargetContent = new GUIContent("打包平台", "选择需要打包的目标平台。");
            m_ChannelContent = new GUIContent("打包渠道", "选择需要打包的渠道。");
            m_CompressionContent = new GUIContent("压缩格式", "Choose no compress, standard (LZMA), or chunk based (LZ4)");

            if (m_UserData.UseDefaultPath)
            {
                //ResetPathToDefault();
            }
        }

        internal void OnGUI(Rect pos)
        {
            m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);
            bool newState = false;
            var centeredStyle = GUI.skin.GetStyle("PR BrokenPrefabLabel");
            centeredStyle.alignment = TextAnchor.MiddleCenter;
            centeredStyle.fontSize = 14;
            centeredStyle.fixedHeight = 30;
            centeredStyle.fixedWidth = pos.width - 10;
            GUILayout.Label(new GUIContent("请慎重修改此界面数值"), centeredStyle);
            //basic options
            EditorGUILayout.Space();
            GUILayout.BeginVertical();

            // build target
            using (new EditorGUI.DisabledScope(!AssetBundleModel.Model.DataSource.CanSpecifyBuildTarget))
            {
                CustomTarget tgt = (CustomTarget)EditorGUILayout.EnumPopup(m_TargetContent, m_UserData.BuildTarget);
                if (tgt != m_UserData.BuildTarget)
                {
                    m_UserData.BuildTarget = tgt;
                    if (m_UserData.UseDefaultPath)
                    {
                        ResetPathToDefault();
                    }
                }
            }

            using (new EditorGUI.DisabledScope(!AssetBundleModel.Model.DataSource.CanSpecifyBuildTarget))
            {
                EnumChannelType tgt = (EnumChannelType)EditorGUILayout.EnumPopup(m_ChannelContent, m_UserData.ChannelTarget);
                if (tgt != m_UserData.ChannelTarget)
                {
                    m_UserData.ChannelTarget = tgt;
                    if (m_UserData.UseDefaultPath)
                    {
                        ResetPathToDefault();
                    }
                }
            }

            
            ////output path
            using (new EditorGUI.DisabledScope(!AssetBundleModel.Model.DataSource.CanSpecifyBuildOutputDirectory))
            {
                EditorGUILayout.Space();
                GUILayout.BeginHorizontal();

                var newPath = EditorGUILayout.TextField("导出目录(仅限测试时可手动更改!)", m_UserData.OutputPath);
                if ((newPath != m_UserData.OutputPath) &&
                     (newPath != string.Empty))
                {
                    m_UserData.UseDefaultPath = false;
                    m_UserData.OutputPath = newPath;
                    //EditorUserBuildSettings.SetPlatformSettings(EditorUserBuildSettings.activeBuildTarget.ToString(), "AssetBundleOutputPath", m_OutputPath);
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("浏览", GUILayout.MaxWidth(75f)))
                    BrowseForFolder();
                //if (GUILayout.Button("重置", GUILayout.MaxWidth(75f)))
                //    ResetPathToDefault();
                
                //if (string.IsNullOrEmpty(m_OutputPath))
                //    m_OutputPath = EditorUserBuildSettings.GetPlatformSettings(EditorUserBuildSettings.activeBuildTarget.ToString(), "AssetBundleOutputPath");
                GUILayout.EndHorizontal();
                EditorGUILayout.Space();

                //版本号
                {
                    centeredStyle = GUI.skin.GetStyle("GUIEditor.BreadcrumbMid");
                    centeredStyle.alignment = TextAnchor.UpperCenter;
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(150f);
                    GUILayout.Label("游戏版本", centeredStyle, GUILayout.MaxWidth(125f));
                    GUILayout.Space(5f);
                    this.m_UserData.GameVersion = EditorGUILayout.IntField(this.m_UserData.GameVersion, GUILayout.MaxWidth(40f));
                    GUILayout.Space(25f);

                    GUILayout.Label("资源第一版本", centeredStyle, GUILayout.MaxWidth(125f));
                    GUILayout.Space(5f);
                    this.m_UserData.ResFirstVersion = EditorGUILayout.IntField(this.m_UserData.ResFirstVersion, GUILayout.MaxWidth(40f));
                    GUILayout.Space(25f);

                    GUILayout.Label("资源第二版本", centeredStyle, GUILayout.MaxWidth(125f));
                    GUILayout.Space(5f);
                    this.m_UserData.ResSecondVersion = EditorGUILayout.IntField(this.m_UserData.ResSecondVersion, GUILayout.MaxWidth(40f));

                    GUILayout.EndHorizontal();

                }


                EditorGUILayout.Space();
                newState = GUILayout.Toggle(
                    m_ForceRebuild.state,
                    m_ForceRebuild.content);
                if (newState != m_ForceRebuild.state)
                {
                    if (newState)
                        m_UserData.OnToggles.Add(m_ForceRebuild.content.text);
                    else
                        m_UserData.OnToggles.Remove(m_ForceRebuild.content.text);
                    m_ForceRebuild.state = newState;
                }
                newState = GUILayout.Toggle(
                    m_CopyToStreaming.state,
                    m_CopyToStreaming.content);
                if (newState != m_CopyToStreaming.state)
                {
                    if (newState)
                        m_UserData.OnToggles.Add(m_CopyToStreaming.content.text);
                    else
                        m_UserData.OnToggles.Remove(m_CopyToStreaming.content.text);
                    m_CopyToStreaming.state = newState;
                }
            }

            // advanced options
            using (new EditorGUI.DisabledScope(!AssetBundleModel.Model.DataSource.CanSpecifyBuildOptions))
            {
                EditorGUILayout.Space();

                GUIStyle tbLabel = new GUIStyle(EditorStyles.foldout);
                tbLabel.alignment = TextAnchor.MiddleRight;
                tbLabel.fixedWidth = 250f;
                tbLabel.fontSize = 12;
                
                m_AdvancedSettings = EditorGUILayout.Foldout(m_AdvancedSettings, "打包设定(建议打包时选择Force Rebuild!)", tbLabel);
                GUI.contentColor = Color.white;
                if (m_AdvancedSettings)
                {
                    var indent = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 1;
                    CompressOptions cmp = (CompressOptions)EditorGUILayout.IntPopup(
                        m_CompressionContent,
                        (int)m_UserData.Compression,
                        m_CompressionOptions,
                        m_CompressionValues);

                    if (cmp != m_UserData.Compression)
                    {
                        m_UserData.Compression = cmp;
                    }
                    foreach (var tog in m_ToggleData)
                    {
                        newState = EditorGUILayout.ToggleLeft(
                            tog.content,
                            tog.state);
                        if (newState != tog.state)
                        {

                            if (newState)
                                m_UserData.OnToggles.Add(tog.content.text);
                            else
                                m_UserData.OnToggles.Remove(tog.content.text);
                            tog.state = newState;
                        }
                    }
                    EditorGUILayout.Space();
                    EditorGUI.indentLevel = indent;
                }
            }

            // build.
            GUILayout.Space(50f);


            if (GUILayout.Button("自动生成路径", "LargeButton", GUILayout.Width(pos.width - 10)))
            { ResetPathToDefault(); }
            GUILayout.Space(5f);

            if (GUILayout.Button("打开资源输出路径", "LargeButton", GUILayout.Width(pos.width - 10)))
            { System.Diagnostics.Process.Start(m_UserData.OutputPath); }
            GUILayout.Space(5f);

            GUI.color = new Color(152 / 255f, 250 / 255f, 152 / 255f);
            if (GUILayout.Button("开始", "LargeButton", GUILayout.Width(pos.width - 10)))
            {
                EditorApplication.delayCall += ExecuteBuild;
            }
            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private void ExecuteBuild()
        {
            if (AssetBundleModel.Model.DataSource.CanSpecifyBuildOutputDirectory)
            {
                if (string.IsNullOrEmpty(m_UserData.OutputPath))
                    BrowseForFolder();

                if (string.IsNullOrEmpty(m_UserData.OutputPath)) //in case they hit "cancel" on the open browser
                {
                    Debug.LogError("AssetBundle Build: No valid output path for build.");
                    return;
                }
                
                if (m_ForceRebuild.state)
                {
                    string message = "Do you want to delete all files in the directory " + m_UserData.OutputPath;
                    if (m_CopyToStreaming.state)
                        message += " and " + m_streamingPath;
                    message += "?";
                    if (EditorUtility.DisplayDialog("File delete confirmation", message, "Yes", "No"))
                    {
                        try
                        {
                            if (Directory.Exists(m_UserData.OutputPath))
                                Directory.Delete(m_UserData.OutputPath, true);

                            if (m_CopyToStreaming.state)
                                if (Directory.Exists(m_streamingPath))
                                    Directory.Delete(m_streamingPath, true);
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                }
                if (!Directory.Exists(m_UserData.OutputPath))
                    Directory.CreateDirectory(m_UserData.OutputPath);
            }

            BuildAssetBundleOptions opt = BuildAssetBundleOptions.None;

            if (AssetBundleModel.Model.DataSource.CanSpecifyBuildOptions)
            {
                if (m_UserData.Compression == CompressOptions.Uncompressed)
                    opt |= BuildAssetBundleOptions.UncompressedAssetBundle;
                else if (m_UserData.Compression == CompressOptions.ChunkBasedCompression)
                    opt |= BuildAssetBundleOptions.ChunkBasedCompression;
                foreach (var tog in m_ToggleData)
                {
                    if (tog.state)
                        opt |= tog.option;
                }
            }

            ABBuildInfo buildInfo = new ABBuildInfo();

            buildInfo.outputDirectory = m_UserData.OutputPath;
            buildInfo.options = opt;
            buildInfo.buildTarget = (BuildTarget)m_UserData.BuildTarget;
            buildInfo.onBuild = (assetBundleName) =>
            {
                if (m_InspectTab == null)
                    return;
                m_InspectTab.AddBundleFolder(buildInfo.outputDirectory);
                m_InspectTab.RefreshBundles();
            };

            AssetBundleModel.Model.DataSource.BuildAssetBundles(buildInfo);

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            if (m_CopyToStreaming.state)
                DirectoryCopy(m_UserData.OutputPath, m_streamingPath);
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            foreach (string folderPath in Directory.GetDirectories(sourceDirName, "*", SearchOption.AllDirectories))
            {
                if (!Directory.Exists(folderPath.Replace(sourceDirName, destDirName)))
                    Directory.CreateDirectory(folderPath.Replace(sourceDirName, destDirName));
            }

            foreach (string filePath in Directory.GetFiles(sourceDirName, "*.*", SearchOption.AllDirectories))
            {
                string newFilePath = Path.Combine(Path.GetDirectoryName(filePath).Replace(sourceDirName, destDirName),
                    Path.GetFileName(filePath));

                File.Copy(filePath, newFilePath, true);
            }
        }

        private void BrowseForFolder()
        {
            m_UserData.UseDefaultPath = false;
            var newPath = EditorUtility.OpenFolderPanel("Bundle Folder", m_UserData.OutputPath, string.Empty);
            if (!string.IsNullOrEmpty(newPath))
            {
                var gamePath = System.IO.Path.GetFullPath(".");
                gamePath = gamePath.Replace("\\", "/");
                if (newPath.StartsWith(gamePath) && newPath.Length > gamePath.Length)
                    newPath = newPath.Remove(0, gamePath.Length + 1);
                m_UserData.OutputPath = newPath;
                //EditorUserBuildSettings.SetPlatformSettings(EditorUserBuildSettings.activeBuildTarget.ToString(), "AssetBundleOutputPath", m_OutputPath);
            }
        }
        private void ResetPathToDefault()
        {
            m_UserData.UseDefaultPath = true;
            m_UserData.OutputPath = BuildHelper.CheckBundlePath(
                BuildHelper.CustomTargetToName(m_UserData.BuildTarget),
                BuildHelper.ChannelToName(m_UserData.ChannelTarget),
                BuildHelper.GameVersionStr(m_UserData.GameVersion, m_UserData.ResFirstVersion, m_UserData.ResSecondVersion)
                );
            //EditorUserBuildSettings.SetPlatformSettings(EditorUserBuildSettings.activeBuildTarget.ToString(), "AssetBundleOutputPath", m_OutputPath);
        }
        
        [System.Serializable]
        internal class BuildTabData
        {
            private List<string> onToggles;
            private CustomTarget buildTarget = CustomTarget.WinPhone;
            private EnumChannelType channelTarget = EnumChannelType.Common;
            private CompressOptions compression = CompressOptions.StandardCompression;
            private string outputPath = string.Empty;
            private bool useDefaultPath = true;
            private int gameVersion;   // 游戏app版本
            private int resFirstVersion;   // 资源第一版本
            private int resSecondVersion;  // 资源第二版本


            public List<string> OnToggles
            {
                get
                {
                    return onToggles;
                }

                set
                {
                    onToggles = value;
                }
            }

            public CustomTarget BuildTarget
            {
                get
                {
                    return buildTarget;
                }

                set
                {
                    buildTarget = value;
                }
            }

            public CompressOptions Compression
            {
                get
                {
                    return compression;
                }

                set
                {
                    compression = value;
                }
            }

            public string OutputPath
            {
                get
                {
                    return outputPath;
                }

                set
                {
                    outputPath = value;
                }
            }

            public bool UseDefaultPath
            {
                get
                {
                    return useDefaultPath;
                }

                set
                {
                    useDefaultPath = value;
                }
            }

            public int GameVersion
            {
                get
                {
                    return gameVersion;
                }

                set
                {
                    gameVersion = value;
                }
            }

            public int ResFirstVersion
            {
                get
                {
                    return resFirstVersion;
                }

                set
                {
                    resFirstVersion = value;
                }
            }

            public int ResSecondVersion
            {
                get
                {
                    return resSecondVersion;
                }

                set
                {
                    resSecondVersion = value;
                }
            }

            public EnumChannelType ChannelTarget
            {
                get
                {
                    return channelTarget;
                }

                set
                {
                    channelTarget = value;
                }
            }
        }
    }

}