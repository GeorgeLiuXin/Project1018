using Galaxy.DataNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Galaxy.AssetPipeline
{
    public partial class GalaxyBundleBrowser : EditorWindow
    {
        private class VersionTab : IBundleTab
        {
            bool m_ResizingHorizontalSplitter = false;
            bool m_ResizingVerticalSplitterRight = false;
            bool m_ResizingVerticalSplitterLeft = false;
            Rect m_HorizontalSplitterRect, m_VerticalSplitterRectRight, m_VerticalSplitterRectLeft;
            float m_HorizontalSplitterPercent = 0.4f;
            float m_VerticalSplitterPercentRight = 0.7f;
            float m_VerticalSplitterPercentLeft = 0.85f;
            const float k_SplitterWidth = 3f;

            Rect m_Position;
            GalaxyBundleBrowser m_Broswer;
            TreeViewState m_PrefabsTreeState;
            BundleBuilderManager m_Builder;
            AssetTypeTree m_AssetTypeTree;
            BundleDetailView m_DetailView;
            BundleTypeTree m_AssetList;
            private TreeViewState m_AssetListState;
            private MultiColumnHeaderState m_AssetListMCHState;

            private IDataNode m_SelectAssetNode;
            private TreeViewState m_BundleDetailState;

            public void OnDisable()
            {

            }

            public void OnEnable(Rect pos, EditorWindow parent, BundleBuilderManager manager)
            {
                m_Position = pos;
                m_HorizontalSplitterRect = new Rect(
                    (int)(m_Position.x + m_Position.width * m_HorizontalSplitterPercent),
                    m_Position.y,
                    k_SplitterWidth,
                    m_Position.height);
                m_VerticalSplitterRectRight = new Rect(
                    m_HorizontalSplitterRect.x,
                    (int)(m_Position.y + m_HorizontalSplitterRect.height * m_VerticalSplitterPercentRight),
                    (m_Position.width - m_HorizontalSplitterRect.width) - k_SplitterWidth,
                    k_SplitterWidth);
                m_VerticalSplitterRectLeft = new Rect(
                    m_Position.x,
                    (int)(m_Position.y + m_HorizontalSplitterRect.height * m_VerticalSplitterPercentLeft),
                    (m_HorizontalSplitterRect.width) - k_SplitterWidth,
                    k_SplitterWidth);

                m_Broswer = parent as GalaxyBundleBrowser;
                m_PrefabsTreeState = new TreeViewState();
                m_Builder = manager;

                m_AssetListState = new TreeViewState();
            }

            public void OnGUI(Rect pos)
            {
                m_Position = pos;

                var style = GUI.skin.label;
                style.alignment = TextAnchor.MiddleCenter;
                style.wordWrap = true;

                string warningContent = "";
                if (m_Builder.AssetFileSystem == null)
                {
                    warningContent = "文件系统尚未初始化";
                }
                else if (m_Builder.BuildInfo == null)
                {
                    warningContent = "打包信息尚未生成";
                }
                else if (Application.isPlaying)
                {
                    warningContent = "不能在运行时操作";
                }

                GUI.Label(
                    new Rect(m_Position.x + 1f, m_Position.y + 1f, m_Position.width - 2f, m_Position.height - 2f),
                    new GUIContent(warningContent),
                    style);

                if (GUILayout.Button("生成打包信息"))
                {
                    m_AssetTypeTree = new AssetTypeTree(m_PrefabsTreeState, m_Builder.BuildInfo.builder);
                    m_AssetTypeTree.m_OnSelectionChangeEvent.AddListener(OnSelectionChangeEvent);
                }

                if (m_AssetTypeTree != null)
                {
                    HandleHorizontalResize();
                    HandleVerticalResize();

                    int halfWidth = (int)(m_Position.width / 2.0f);

                    //Left half
                    var bundleTreeRect = new Rect(
                        m_Position.x,
                        m_Position.y,
                        m_HorizontalSplitterRect.x,
                        m_VerticalSplitterRectLeft.y - m_Position.y);
                    m_AssetTypeTree.Reload();
                    m_AssetTypeTree.OnGUI(bundleTreeRect);


                    //Right half
                    float panelLeft = m_HorizontalSplitterRect.x + k_SplitterWidth;
                    float panelWidth = m_VerticalSplitterRectRight.width - k_SplitterWidth * 2;
                    float searchHeight = 20f;
                    float panelTop = m_Position.y + searchHeight;
                    float panelHeight = m_VerticalSplitterRectRight.y - panelTop;

                    if (m_SelectAssetNode != null)
                    {
                        if (m_AssetList == null)
                        {
                            var headerState = BundleTypeTree.CreateDefaultMultiColumnHeaderState();
                            if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_AssetListMCHState, headerState))
                                MultiColumnHeaderState.OverwriteSerializedFields(m_AssetListMCHState, headerState);
                            m_AssetListMCHState = headerState;
                            m_AssetList = new BundleTypeTree(m_AssetListState, m_AssetListMCHState, m_SelectAssetNode);
                            m_AssetList.Reload();
                        }
                        else
                        {
                            m_AssetList.OnGUI(new Rect(
                            panelLeft,
                            panelTop,
                            panelWidth,
                            panelHeight));
                        }

                        if (m_DetailView == null)
                        {
                            if (m_BundleDetailState == null)
                                m_BundleDetailState = new TreeViewState();
                            m_DetailView = new BundleDetailView(m_BundleDetailState, m_SelectAssetNode);
                            m_DetailView.Reload();
                        }
                        else
                        {
                            m_DetailView.OnGUI(new Rect(
                            bundleTreeRect.x,
                            bundleTreeRect.y + bundleTreeRect.height + k_SplitterWidth,
                            bundleTreeRect.width,
                            m_Position.height - bundleTreeRect.height - k_SplitterWidth * 2));
                        }
                    }
                }
            }

            private void OnSelectionChangeEvent(IDataNode arg0)
            {
                m_SelectAssetNode = arg0;
                if (m_AssetList != null)
                {
                    m_AssetList.SetSelectedBundles(m_SelectAssetNode);
                }

                if (m_DetailView != null)
                {
                    m_DetailView.SetSelectedBundles(m_SelectAssetNode);
                }
            }

            public void Refresh()
            {

            }

            public void Update()
            {

            }


            private void HandleHorizontalResize()
            {
                m_HorizontalSplitterRect.x = (int)(m_Position.width * m_HorizontalSplitterPercent);
                m_HorizontalSplitterRect.height = m_Position.height;

                EditorGUIUtility.AddCursorRect(m_HorizontalSplitterRect, MouseCursor.ResizeHorizontal);
                if (Event.current.type == EventType.MouseDown && m_HorizontalSplitterRect.Contains(Event.current.mousePosition))
                    m_ResizingHorizontalSplitter = true;

                if (m_ResizingHorizontalSplitter)
                {
                    m_HorizontalSplitterPercent = Mathf.Clamp(Event.current.mousePosition.x / m_Position.width, 0.1f, 0.9f);
                    m_HorizontalSplitterRect.x = (int)(m_Position.width * m_HorizontalSplitterPercent);
                }

                if (Event.current.type == EventType.MouseUp)
                    m_ResizingHorizontalSplitter = false;
            }

            private void HandleVerticalResize()
            {
                m_VerticalSplitterRectRight.x = m_HorizontalSplitterRect.x;
                m_VerticalSplitterRectRight.y = (int)(m_HorizontalSplitterRect.height * m_VerticalSplitterPercentRight);
                m_VerticalSplitterRectRight.width = m_Position.width - m_HorizontalSplitterRect.x;
                m_VerticalSplitterRectLeft.y = (int)(m_HorizontalSplitterRect.height * m_VerticalSplitterPercentLeft);
                m_VerticalSplitterRectLeft.width = m_VerticalSplitterRectRight.width;


                EditorGUIUtility.AddCursorRect(m_VerticalSplitterRectRight, MouseCursor.ResizeVertical);
                if (Event.current.type == EventType.MouseDown && m_VerticalSplitterRectRight.Contains(Event.current.mousePosition))
                    m_ResizingVerticalSplitterRight = true;

                EditorGUIUtility.AddCursorRect(m_VerticalSplitterRectLeft, MouseCursor.ResizeVertical);
                if (Event.current.type == EventType.MouseDown && m_VerticalSplitterRectLeft.Contains(Event.current.mousePosition))
                    m_ResizingVerticalSplitterLeft = true;


                if (m_ResizingVerticalSplitterRight)
                {
                    m_VerticalSplitterPercentRight = Mathf.Clamp(Event.current.mousePosition.y / m_HorizontalSplitterRect.height, 0.2f, 0.98f);
                    m_VerticalSplitterRectRight.y = (int)(m_HorizontalSplitterRect.height * m_VerticalSplitterPercentRight);
                }
                else if (m_ResizingVerticalSplitterLeft)
                {
                    m_VerticalSplitterPercentLeft = Mathf.Clamp(Event.current.mousePosition.y / m_HorizontalSplitterRect.height, 0.25f, 0.98f);
                    m_VerticalSplitterRectLeft.y = (int)(m_HorizontalSplitterRect.height * m_VerticalSplitterPercentLeft);
                }


                if (Event.current.type == EventType.MouseUp)
                {
                    m_ResizingVerticalSplitterRight = false;
                    m_ResizingVerticalSplitterLeft = false;
                }
            }
        }
    }
}
