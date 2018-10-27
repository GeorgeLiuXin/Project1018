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
        private class AssetTab : IBundleTab
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
                m_AssetTypeTree = new AssetTypeTree(m_PrefabsTreeState, m_Builder.AssetFileSystem.AssetDatasManager);
            }

            public void OnGUI(Rect pos)
            {
                m_Position = pos;
                if (Application.isPlaying)
                {
                    var style = GUI.skin.label;
                    style.alignment = TextAnchor.MiddleCenter;
                    style.wordWrap = true;
                    GUI.Label(
                        new Rect(m_Position.x + 1f, m_Position.y + 1f, m_Position.width - 2f, m_Position.height - 2f),
                        new GUIContent("Inspector unavailable while in PLAY mode"),
                        style);
                }

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
