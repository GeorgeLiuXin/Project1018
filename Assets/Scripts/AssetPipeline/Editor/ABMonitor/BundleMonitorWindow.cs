using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Galaxy.AssetPipeline
{
    public class BundleMonitorWindow
    {
        private Rect m_Position;
        private Vector2 m_BundleScrollPosition;
        private Vector2 m_CollectionScrollPosition;
        private Vector2 m_DetailScrollPosition;
        private GUIStyle m_styleFoldout;
        private bool state;
        private float value = 0.0f;

        private BundleManager m_BundleManager;
        private BundleRefCache m_BundleCache;
        private BundleCollection m_ABCollection;
        private ABMonitor m_Monitor;
        private ABMonitorWindowMain m_ParentWindow;

        public void OnEnable(Rect pos, BundleManager manager, ABMonitorWindowMain parentWindow) {
            m_ParentWindow = parentWindow;
            m_BundleManager = manager;
            m_BundleCache = manager.Cache;
            m_ABCollection = manager.ABCollection;
            // m_BundleGC = manager.Cache
        }

        private Vector2 m_CollectionScrollVec;
        private Rect m_BundlePos;
        private Rect m_CollectionPos;
        private Rect m_DetailPos;

        private static float s_CollectionHeight = 150f;
        private static float s_DetailWidth = 600f;
        //private static float 

        void CalculatePos(Rect pos)
        {
            m_BundlePos = new Rect(pos.x, pos.y, pos.width, pos.height - s_CollectionHeight);
            m_CollectionPos = new Rect(pos.x + s_DetailWidth, pos.y + pos.height - s_CollectionHeight, pos.width - s_DetailWidth, s_CollectionHeight);

            m_DetailPos = new Rect(pos.x, pos.y + pos.height - s_CollectionHeight, s_DetailWidth, s_CollectionHeight);
        }

        public void OnGUI(Rect pos)
        {
            if (m_BundleManager == null)
            {
                return;
            }

            if (m_styleFoldout == null)
            {
                PrepareStyles();
            }

            if (m_BundleTree == null)
            {
                PrepareBundleTree();
            }

            if (m_CollectionTree == null)
            {
                PrepareCollectionTree();
            }

            CalculatePos(pos);

            if (m_BundleTree != null)
            {
                m_BundleTree.OnGUI(m_BundlePos);
                m_BundleTree.Repaint();
            }

            if (m_CollectionTree != null)
            {
                m_CollectionTree.OnGUI(m_CollectionPos);
                m_CollectionTree.Repaint();
            }

            if (m_MonitorTree != null)
            {
                m_MonitorTree.OnGUI(m_DetailPos);
                m_MonitorTree.Repaint();
            }
        }

        public void Update(float deltaTime)
        {
            if (m_BundleTree != null)
            {
                m_BundleTree.Update(deltaTime);
            }

            if (m_CollectionTree != null)
            {
                m_CollectionTree.Update(deltaTime);
            }
        }

        void PrepareBundleTree()
        {
            var headerState = ABBundleMonitorTree.CreateDefaultMultiColumnHeaderState();
            m_BundleListMCHState = headerState;

            m_BundleTreeState = new TreeViewState();
            m_BundleTree = new ABBundleMonitorTree(m_BundleCache, m_BundleTreeState, m_BundleListMCHState);
            m_BundleTree.Reload();

            m_BundleTree.OnBundleRefSelected += OnSelectMonitor;
        }

        void PrepareCollectionTree()
        {
            var headerState = ABCollectionTree.CreateDefaultMultiColumnHeaderState();
            m_CollectionListMCHState = headerState;

            m_CollectionTreeState = new TreeViewState();
            m_CollectionTree = new ABCollectionTree(m_ABCollection, m_CollectionTreeState, m_CollectionListMCHState);
            m_CollectionTree.Reload();
        }

        void OnSelectMonitor(BundleReference br)
        {
            if (m_MonitorTree == null)
            {
                PrepareMonitorTree(br);
            }
            else
            {
                m_MonitorTree.Reset(br);
            }
        }

        public void OnSelectMonitor(AssetReference ar)
        {
            if (m_MonitorTree == null)
            {
                PrepareMonitorTree(ar);
            }
            else
            {
                m_MonitorTree.Reset(ar);
            }
        }

        void PrepareMonitorTree(BundleReference br)
        {
            var headerState = ABMonitorDetailsTree.CreateDefaultMultiColumnHeaderState();
            m_MonitorListMCHState = headerState;

            m_MonitorTreeState = new TreeViewState();
            m_MonitorTree = new ABMonitorDetailsTree(br.Monitor, m_MonitorTreeState, m_MonitorListMCHState);
            m_MonitorTree.Reload();
        }

        void PrepareMonitorTree(AssetReference ar)
        {
            var headerState = ABMonitorDetailsTree.CreateDefaultMultiColumnHeaderState();
            m_MonitorListMCHState = headerState;

            m_MonitorTreeState = new TreeViewState();
            m_MonitorTree = new ABMonitorDetailsTree(ar.Monitor, m_MonitorTreeState, m_MonitorListMCHState);
            m_MonitorTree.Reload();
        }

        TreeViewState m_BundleTreeState;
        ABBundleMonitorTree m_BundleTree;
        MultiColumnHeaderState m_BundleListMCHState;

        TreeViewState m_CollectionTreeState;
        ABCollectionTree m_CollectionTree;
        MultiColumnHeaderState m_CollectionListMCHState;

        TreeViewState m_MonitorTreeState;
        ABMonitorDetailsTree m_MonitorTree;
        MultiColumnHeaderState m_MonitorListMCHState;
        
        void PrepareStyles()
        {
            
            m_styleFoldout = new GUIStyle(EditorStyles.foldout);

            m_styleFoldout.normal.textColor = EditorStyles.textField.normal.textColor;
            m_styleFoldout.onNormal.textColor = EditorStyles.textField.normal.textColor;
            m_styleFoldout.hover.textColor = EditorStyles.textField.normal.textColor;
            m_styleFoldout.onHover.textColor = EditorStyles.textField.normal.textColor;
            m_styleFoldout.focused.textColor = EditorStyles.textField.normal.textColor;
            m_styleFoldout.onFocused.textColor = EditorStyles.textField.normal.textColor;
            m_styleFoldout.active.textColor = EditorStyles.textField.normal.textColor;
            m_styleFoldout.onActive.textColor = EditorStyles.textField.normal.textColor;
            m_styleFoldout.normal.background = ABMonitorSetting.GetProgressIcon();
        }
    }
}
