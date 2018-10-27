using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Galaxy.AssetPipeline
{
    public partial class GalaxyBundleBrowser : EditorWindow
    {
        internal const float kButtonWidth = 150;
        internal const float k_ToolbarPadding = 15;
        internal const float k_MenubarPadding = 32;

        private static GalaxyBundleBrowser m_instance = null;
        internal static GalaxyBundleBrowser instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = GetWindow<GalaxyBundleBrowser>();
                    m_instance.minSize = new Vector2(800, 800);
                }
                return m_instance;
            }
        }

        private void OnDisable()
        {
            m_instance = null;
        }

        enum Mode
        {
            Builder,
            AssetBrowser,
            VersionBrowser
        }

        private BundleBuilderManager m_BuildManager;

        private Mode m_Mode;
        private int m_DataSourceIndex;

        private IBundleTab m_AssetBrowser;
        private IBundleTab m_VersionBrowser;
        private IBundleTab m_Builder;
        private List<IBundleTab> m_Tabs = new List<IBundleTab>();

        private Texture2D m_RefreshTexture;
        string[] toolbarLabels = new string[3] { "打包界面", "资源文件夹",
                "版本文件夹" };
        private void CustomEnable()
        {
            m_RefreshTexture = EditorGUIUtility.FindTexture("Refresh");
            m_BuildManager = new BundleBuilderManager();
            m_BuildManager.OnEnable();
            m_BuildManager.AssetScanComplete += (object arg1, AssetScanCompleteEventArgs arg) =>
            {
                Rect subPos = GetSubWindowArea();
                if (m_AssetBrowser == null)
                    m_AssetBrowser = new AssetTab();
                m_AssetBrowser.OnEnable(subPos, this, m_BuildManager);
                m_Tabs.Add(m_AssetBrowser);

                if (m_VersionBrowser == null)
                    m_VersionBrowser = new VersionTab();
                m_VersionBrowser.OnEnable(subPos, this, m_BuildManager);
                m_Tabs.Add(m_VersionBrowser);

                if (m_Builder == null)
                    m_Builder = new BuildTab();
                m_Builder.OnEnable(subPos, this, m_BuildManager);
                m_Tabs.Add(m_Builder);
            };
        }
        
        private Rect GetSubWindowArea()
        {
            float padding = k_MenubarPadding;
            padding += k_MenubarPadding * 0.5f;
            Rect subPos = new Rect(0, padding, position.width, position.height - padding);
            return subPos;
        }

        private void OnInspectorUpdate()
        {
            Repaint();

            if (m_AssetBrowser == null)
            {
                return;
            }

            foreach (IBundleTab t in m_Tabs)
            {
                t.Update();
            }
        }
        private void OnGUI()
        {
            if (m_BuildManager == null)
            {
                if (GUILayout.Button("开始分析资源"))
                {
                    CustomEnable();
                }
                return;
            }
            else if (m_AssetBrowser == null)
            {
                return;
            }
            
            ModeToggle();

            switch (m_Mode)
            {
                case Mode.AssetBrowser:
                    m_AssetBrowser.OnGUI(GetSubWindowArea());
                    break;
                case Mode.VersionBrowser:
                    m_VersionBrowser.OnGUI(GetSubWindowArea());
                    break;
                case Mode.Builder:
                    m_Builder.OnGUI(GetSubWindowArea());
                    break;

                default:
                    m_Builder.OnGUI(GetSubWindowArea());
                    break;
            }
        }
        void ModeToggle()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(k_ToolbarPadding);
            bool clicked = GUILayout.Button(m_RefreshTexture);
            if (clicked)
                foreach (IBundleTab t in m_Tabs)
                {
                    t.Refresh();
                }

            float toolbarWidth = position.width - k_ToolbarPadding * 4 - m_RefreshTexture.width;

            m_Mode = (Mode)GUILayout.Toolbar((int)m_Mode, toolbarLabels, "LargeButton", GUILayout.Width(toolbarWidth));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

        }
        
        private static string m_Title;
        private static string m_Info;
        private static float m_Progress;

        public static void CloseProgress()
        {
            EditorUtility.ClearProgressBar();
        }

        public static void ShowProgress(EProgressType type, float progress)
        {
            m_Progress = progress;
            GetProgressInfoAndTitle(type);
            EditorUtility.DisplayProgressBar(m_Title, m_Info, m_Progress);
        }

        public static void ShowProgress(EProgressType type, string info, float progress)
        {
            m_Progress = progress;
            GetProgressInfoAndTitle(type);
            m_Info = info;
            EditorUtility.DisplayProgressBar(m_Title, m_Info, m_Progress);
        }

        private static void GetProgressInfoAndTitle(EProgressType type)
        {
            switch (type)
            {
                case EProgressType.Asset:
                    m_Info = "正在检测本地资源";
                    m_Title = "资源";
                    break;
                case EProgressType.Bundle:
                    m_Info = "正在分析Bundle信息";
                    m_Title = "AB包";
                    break;
                case EProgressType.Build:
                    m_Info = "正在打包";
                    m_Title = "打包";
                    break;
            }
        }

        public enum EProgressType
        {
            Asset,
            Bundle,
            Build,
        }
    }
}
