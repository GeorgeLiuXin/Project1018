using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Galaxy.AssetPipeline
{
    public class AssetMonitorWindow
    {
        public AssetPipelineAction<AssetReference> OnReferenceSelected;

        private Rect m_Position;
        private Vector2 m_AssetScrollPosition;
        private Vector2 m_CollectionScrollPosition;
        private Vector2 m_DetailScrollPosition;
        private GUIStyle m_styleFoldout;
        private bool state;
        private float value = 0.0f;

        private AssetManager m_AssetManager;
        private AssetRefCache m_AssetCache;
        private ABMonitor m_Monitor;
        private ABMonitorWindowMain m_ParentWindow;

        public void OnEnable(Rect pos, AssetManager manager, ABMonitorWindowMain parentWindow)
        {
            m_ParentWindow = parentWindow;
            m_AssetManager = manager;
            m_AssetCache = manager.Cache;
        }
        
        private Vector2 m_CollectionScrollVec;
        private Rect m_AssetPos;
        private Rect m_CollectionPos;
        private Rect m_DetailPos;

        private static float s_CollectionHeight = 150f;
        private static float s_DetailWidth = 600f;
        //private static float 

        void CalculatePos(Rect pos)
        {
            m_AssetPos = new Rect(pos.x, pos.y, pos.width, pos.height);
        }

        public void OnGUI(Rect pos)
        {
            if (m_AssetManager == null)
            {
                return;
            }

            if (m_styleFoldout == null)
            {
                PrepareStyles();
            }

            if (m_AssetTree == null)
            {
                PrepareAssetTree();
            }
            

            CalculatePos(pos);

            if (m_AssetTree != null)
            {
                m_AssetTree.OnGUI(m_AssetPos);
                m_AssetTree.Repaint();
            }
        }

        public void Update(float deltaTime)
        {
            if (m_AssetTree != null)
            {
                m_AssetTree.Update(deltaTime);
            }
            
        }

        void PrepareAssetTree()
        {
            var headerState = ABAssetMonitorTree.CreateDefaultMultiColumnHeaderState();
            m_AssetListMCHState = headerState;

            m_AssetTreeState = new TreeViewState();
            m_AssetTree = new ABAssetMonitorTree(m_AssetManager, m_AssetTreeState, m_AssetListMCHState);
            m_AssetTree.Reload();

            m_AssetTree.OnAssetRefSelected += OnReferenceSelected;
        }
        
        TreeViewState m_AssetTreeState;
        ABAssetMonitorTree m_AssetTree;
        MultiColumnHeaderState m_AssetListMCHState;
        
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
