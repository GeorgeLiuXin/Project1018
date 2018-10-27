
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Galaxy.AssetPipeline
{
    public enum WindowStatus
    {
        Wait,
        Doing
    }

    public class ABMonitorWindowMain : EditorWindow
    {
        static public void ShowWindow()
        {
            ABMonitorWindowMain window = EditorWindow.GetWindow<ABMonitorWindowMain>();
            window.minSize = new Vector2(400, 800);
            window.Show();
        }

        static float k_ToolbarPadding = 30;
        static float k_MenubarPaddingRatio = 0.50f;

        const string APPlICATION_NOT_PLAYING_CONTENT = "Application is not playing.";

        private BundleMonitorWindow m_BundleWindow;
        private AssetMonitorWindow m_AssetWindow;

        private AssetManager m_AssetManager;
        private BundleManager m_BundleManager;
        private WindowStatus m_Status = WindowStatus.Wait;
        private Rect m_BundlePosition;
        private Rect m_AssetPosition;
        private void OnEnable()
        {
            GetSubWindowArea();
            m_BundleWindow = new BundleMonitorWindow();
            m_AssetWindow = new AssetMonitorWindow();
            m_AssetWindow.OnReferenceSelected += m_BundleWindow.OnSelectMonitor;

            m_BundleManager = GalaxyGameModule.GetGameManager<BundleManager>();
            m_AssetManager = GalaxyGameModule.GetGameManager<AssetManager>();
            m_Status = WindowStatus.Doing;

            m_BundleWindow.OnEnable(m_BundlePosition, m_BundleManager, this);
            m_AssetWindow.OnEnable(m_AssetPosition, m_AssetManager, this);
        }
        
        private void OnGUI()
        {
            GUILayout.BeginVertical();
            k_MenubarPaddingRatio = GUILayout.HorizontalSlider(k_MenubarPaddingRatio, 0.1f, 0.9f, GUILayout.Height(k_ToolbarPadding));
            GetSubWindowArea();

            m_BundleWindow.OnGUI(m_BundlePosition);
            m_AssetWindow.OnGUI(m_AssetPosition);
            GUILayout.EndVertical();
            Repaint();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            if (m_BundleWindow != null)
            {
                m_BundleWindow.Update(deltaTime);
            }
            if (m_AssetWindow != null)
            {
                m_AssetWindow.Update(deltaTime);
            }
        }

        private void onApplicationQuit() {

        }

        private void GetSubWindowArea()
        {
            float padding = k_MenubarPaddingRatio * position.height;

            Rect subPos = new Rect(0, padding, position.width, position.height - padding - k_ToolbarPadding);
            m_BundlePosition = subPos;

            m_AssetPosition = new Rect(0, k_ToolbarPadding, position.width, padding - k_ToolbarPadding);
        }
    }
}
