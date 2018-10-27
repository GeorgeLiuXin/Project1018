using Galaxy.AssetBundleBrowser.AssetBundleDataSource;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

//[assembly: System.Runtime.CompilerServices.InternalsVisibleToAttribute("Unity.AssetBundleBrowser.Editor.Tests")]

namespace Galaxy.AssetBundleBrowser
{

    public class AssetBundleBrowserMain : EditorWindow, IHasCustomMenu, ISerializationCallbackReceiver
    {

        private static AssetBundleBrowserMain m_instance = null;
        internal static AssetBundleBrowserMain instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = GetWindow<AssetBundleBrowserMain>();
                return m_instance;
            }
        }

        internal const float kButtonWidth = 150;

        enum Mode
        {
            Browser,
            Builder,
            Inspect,
            Version,
            Help,
        }

        Mode m_Mode;

        int m_DataSourceIndex;

        internal AssetBundleManageTab m_ManageTab;

        internal AssetBundleBuildTab m_BuildTab;

        internal AssetBundleInspectTab m_InspectTab;

        internal AssetBundleVersionTab m_VersionTab;

        internal HelpNoticeTab m_HelpTab;

        private Texture2D m_RefreshTexture;

        const float k_ToolbarPadding = 15;
        const float k_MenubarPadding = 32;

        static void ShowWindow()
        {
            m_instance = null;
            instance.titleContent = new GUIContent("AssetBundles");
            instance.Show();
        }

        internal bool multiDataSource = false;
        List<AssetBundleDataSource.ABDataSource> m_DataSourceList = null;
        public virtual void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Custom Sources"), multiDataSource, FlipDataSource);
        }
        internal void FlipDataSource()
        {
            multiDataSource = !multiDataSource;
        }

        private void OnEnable()
        {
            Rect subPos = GetSubWindowArea();
            if (m_ManageTab == null)
                m_ManageTab = new AssetBundleManageTab();
            m_ManageTab.OnEnable(subPos, this);
            if (m_BuildTab == null)
                m_BuildTab = new AssetBundleBuildTab();
            m_BuildTab.OnEnable(subPos, this);
            if (m_InspectTab == null)
                m_InspectTab = new AssetBundleInspectTab();
            m_InspectTab.OnEnable(subPos, this);
            if (m_HelpTab == null)
                m_HelpTab = new HelpNoticeTab();
            if (m_VersionTab == null)
                m_VersionTab = new AssetBundleVersionTab();
            m_HelpTab.OnEnable(subPos, this);
            m_VersionTab.OnEnable();
            m_RefreshTexture = EditorGUIUtility.FindTexture("Refresh");

            InitDataSources();
        }
        private void InitDataSources()
        { 
            //determine if we are "multi source" or not...
            multiDataSource = false;
            m_DataSourceList = new List<AssetBundleDataSource.ABDataSource>();
            List<System.Type> CustomABDataSourceTypes = AssetBundleDataSource.ABDataSourceProviderUtility.CustomABDataSourceTypes;

            foreach (var info in CustomABDataSourceTypes)
            {
                if (info == typeof(AssetDatabaseABDataTextualSource))
                {
                    // TODO 暂时改成这种写法
                    m_DataSourceList.AddRange(AssetDatabaseABDataTextualSource.CreateDataSources());
                }
                //  m_DataSourceList.AddRange(info.GetMethod("CreateDataSources").Invoke(null, null) as List<AssetBundleDataSource.ABDataSource>);
            }

            if (m_DataSourceList.Count > 1)
            {
                multiDataSource = true;
                if (m_DataSourceIndex >= m_DataSourceList.Count)
                    m_DataSourceIndex = 0;
                AssetBundleModel.Model.DataSource = m_DataSourceList[m_DataSourceIndex];
            }
        }
        private void OnDisable()
        {
            if (m_BuildTab != null)
                m_BuildTab.OnDisable();
            if (m_InspectTab != null)
                m_InspectTab.OnDisable();

            // TODO 暂时这么写
            AssetDatabaseABDataTextualSource.Over();
        }

        public void OnBeforeSerialize()
        {
        }
        public void OnAfterDeserialize()
        {
            InitDataSources();
        }

        private Rect GetSubWindowArea()
        {
            float padding = k_MenubarPadding;
            if (multiDataSource)
                padding += k_MenubarPadding * 0.5f;
            Rect subPos = new Rect(0, padding, position.width, position.height - padding);
            return subPos;
        }

        private void OnInspectorUpdate()
        {
            switch (m_Mode)
            {

            }
        }

        private void Update()
        {
            switch (m_Mode)
            {
                case Mode.Builder:
                    break;
                case Mode.Inspect:
                    break;
                case Mode.Browser:
                default:
                    m_ManageTab.Update();
                    break;
            }
        }

        private void OnGUI()
        {
            ModeToggle();

            switch (m_Mode)
            {
                case Mode.Builder:
                    m_BuildTab.OnGUI(GetSubWindowArea());
                    break;
                case Mode.Inspect:
                    m_InspectTab.OnGUI(GetSubWindowArea());
                    break;
                case Mode.Help:
                    m_HelpTab.OnGUI(GetSubWindowArea());
                    break;
                case Mode.Version:
                    m_VersionTab.OnGUI(GetSubWindowArea());
                    break;
                case Mode.Browser:
                default:
                    m_ManageTab.OnGUI(GetSubWindowArea());
                    break;
            }
        }


        string[] toolbarLabels = new string[5] { "配置",
                "打包", "检阅", "版本", "帮助" };

        void ModeToggle()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(k_ToolbarPadding);
            bool clicked = false;
            switch (m_Mode)
            {
                case Mode.Browser:
                    clicked = GUILayout.Button(m_RefreshTexture);
                    if (clicked)
                        m_ManageTab.ForceReloadData();
                    break;
                case Mode.Builder:
                    GUILayout.Space(m_RefreshTexture.width + k_ToolbarPadding);
                    break;
                case Mode.Help:
                    GUILayout.Space(m_RefreshTexture.width + k_ToolbarPadding);
                    break;
                case Mode.Version:
                    GUILayout.Space(m_RefreshTexture.width + k_ToolbarPadding);
                    break;
                case Mode.Inspect:
                    clicked = GUILayout.Button(m_RefreshTexture);
                    if (clicked)
                        m_InspectTab.RefreshBundles();
                    break;
              
            }
            float toolbarWidth = position.width - k_ToolbarPadding * 4 - m_RefreshTexture.width;

            m_Mode = (Mode)GUILayout.Toolbar((int)m_Mode, toolbarLabels, "LargeButton", GUILayout.Width(toolbarWidth));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();


            if (multiDataSource)
            {
                //GUILayout.BeginArea(r);
                GUILayout.BeginHorizontal();

                using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    GUILayout.Label("Bundle Data Source:");
                    GUILayout.FlexibleSpace();
                    var c = new GUIContent(string.Format("{0} ({1})", AssetBundleModel.Model.DataSource.Name, AssetBundleModel.Model.DataSource.ProviderName), "Select Asset Bundle Set");
                    if (GUILayout.Button(c, EditorStyles.toolbarPopup))
                    {
                        GenericMenu menu = new GenericMenu();

                        for (int index = 0; index < m_DataSourceList.Count; index++)
                        {
                            var ds = m_DataSourceList[index];
                            if (ds == null)
                                continue;

                            if (index > 0)
                                menu.AddSeparator("");

                            var counter = index;
                            menu.AddItem(new GUIContent(string.Format("{0} ({1})", ds.Name, ds.ProviderName)), false,
                                () =>
                                {
                                    m_DataSourceIndex = counter;
                                    var thisDataSource = ds;
                                    AssetBundleModel.Model.DataSource = thisDataSource;
                                    m_ManageTab.ForceReloadData();
                                }
                            );

                        }

                        menu.ShowAsContext();
                    }

                    GUILayout.FlexibleSpace();
                    if (AssetBundleModel.Model.DataSource.IsReadOnly())
                    {
                        GUIStyle tbLabel = new GUIStyle(EditorStyles.toolbar);
                        tbLabel.alignment = TextAnchor.MiddleRight;

                        GUILayout.Label("Read Only", tbLabel);
                    }
                }

                GUILayout.EndHorizontal();
            }
        }
    }
}
