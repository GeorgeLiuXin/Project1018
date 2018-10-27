using Galaxy.DataNode;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Galaxy.AssetPipeline
{
    public partial class GalaxyBundleBrowser : EditorWindow
    {
        private class BuildTab : IBundleTab
        {
            BundleBuilderManager m_Manager;
      
            private DataNodeManager LocalHistoryNodeManager
            {
                get { return m_Manager.LocalHistoryNodeManager; }
            }

            private bool m_IgnoreOldVersion = false;

            private bool m_IsTest;

            private VersionInfo m_OldVersion = new VersionInfo();
            private VersionInfo m_CurrentVersion = new VersionInfo();

            private RuntimePlatform m_Platform;
            string[] m_Selects = new string[3] {
                "Iphone","PC",
                "Android"
            };

            private int m_SelectId = 0;
            private int SelectId
            {
                set
                {
                    m_SelectId = value;
                    switch (value)
                    {
                        case 0:
                            m_Platform = RuntimePlatform.IPhonePlayer;break;
                        case 1:
                            m_Platform = RuntimePlatform.WindowsPlayer;break;
                        case 2:
                            m_Platform = RuntimePlatform.Android; break;
                    }
                }
                get {
                    return m_SelectId;
                }
            }

            public void OnEnable(Rect pos, EditorWindow parent, BundleBuilderManager manager)
            {
                m_Manager = manager;
            }

            public void OnGUI(Rect pos)
            {
                GUILayout.BeginVertical();
                {
                    SelectId = EditorGUILayout.Popup(m_SelectId, m_Selects);

                    m_IgnoreOldVersion = GUILayout.Toggle(m_IgnoreOldVersion, "忽略过去的版本");
                    GUILayout.Space(10);
                    if (!m_IgnoreOldVersion)
                    {
                        GUILayout.Label("过去版本:");
                        GUILayout.BeginHorizontal();
                        {
                            m_OldVersion.GameVersion = EditorGUILayout.IntField("游戏版本号:", m_OldVersion.GameVersion);
                            m_OldVersion.First = EditorGUILayout.IntField("第一资源号:", m_OldVersion.First);
                            m_OldVersion.Second = EditorGUILayout.IntField("第二资源号:", m_OldVersion.Second);
                        }
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.Space(20);

                    GUILayout.Label("现在版本:");
                    GUILayout.BeginHorizontal();
                    {
                        m_CurrentVersion.GameVersion = EditorGUILayout.IntField("游戏版本号:", m_CurrentVersion.GameVersion);
                        m_CurrentVersion.First = EditorGUILayout.IntField("第一资源号:", m_CurrentVersion.First);
                        m_CurrentVersion.Second = EditorGUILayout.IntField("第二资源号:", m_CurrentVersion.Second);
                    }
                    GUILayout.EndHorizontal();

                    m_IsTest = GUILayout.Toggle(m_IsTest, "是否测试版本");
                    if (m_IsTest)
                    {
                        m_CurrentVersion.Test = EditorGUILayout.IntField("测试版本号:", m_CurrentVersion.Test);
                    }

                    GUILayout.Space(50);
                    if (GUILayout.Button("生成打包信息"))
                    {
                        m_Manager.Prepare(m_OldVersion, m_CurrentVersion, m_IsTest, m_IgnoreOldVersion, m_Platform, EnumChannelType.Common);
                    }

                    GUILayout.Space(20);
                    if (GUILayout.Button("打开输出路径"))
                    {
                        OpenOutputPath();
                    }

                    GUILayout.Space(20);
                    if (GUILayout.Button("跳过打包 直接保存Bundle信息"))
                    {
                        m_Manager.RecordBuilder();
                    }
                    
                    GUILayout.Space(20);
                    if (GUILayout.Button("开始打包"))
                    {
                        m_Manager.Build();
                    }
                }
                GUILayout.EndVertical();
            }

            public void Refresh()
            {
               
            }

            private void OpenOutputPath()
            {
                string bundleOutputPath = BuildHelper.CheckBundlePath(
               BuildHelper.ChannelToName(EnumChannelType.Common),
               BuildHelper.GameVersionStr(m_CurrentVersion.GameVersion, m_CurrentVersion.First, m_CurrentVersion.Second),
               BuildHelper.CustomTargetToName(m_Platform)
               );
                System.Diagnostics.Process.Start(bundleOutputPath);

                string versionOutputPath = BuildHelper.CheckCustomPath(BundleConfig.BundleEditorPath,
                    "Versions",
                    BuildHelper.ChannelToName(EnumChannelType.Common),
                     BuildHelper.GameVersionStr(m_CurrentVersion.GameVersion, m_CurrentVersion.First, m_CurrentVersion.Second),
                      BuildHelper.CustomTargetToName(m_Platform)
                    );
                System.Diagnostics.Process.Start(versionOutputPath);
            }

            public void Update()
            {
                
            }
            
            public void OnDisable()
            {

            }

        }
    }
}
