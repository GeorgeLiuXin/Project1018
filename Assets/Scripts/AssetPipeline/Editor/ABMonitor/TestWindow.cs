
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Galaxy.AssetPipeline
{
    public class TestWindow
    {
        private Rect m_Position;
        private Vector2 m_ScrollPosition;
        private GUIStyle m_styleFoldout;
        private bool state;
        private float value = 0.0f;

        private BundleManager m_BundleManager;
        private BundleRefCache m_BundleCache;
        private BundleCollection m_BundleGC;

        public void OnEnable(Rect pos, BundleManager manager)
        {

            m_BundleManager = manager;
            m_BundleCache = manager.Cache;
            // m_BundleGC = manager.Cache
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

            GUILayout.BeginArea(pos);
            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
            //for (int i = 0; i < 4; i++)
            //{
            //    Foldout("jaaj" + i, i + 1);
            //}

            Dictionary<string, BundleReference> refMap = m_BundleCache.BundleRefDict;

            foreach (BundleReference br in refMap.Values)
            {
                Foldout(br.BundlePath, br.Progress);
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();

        }

        private bool Foldout(string name, float progress)
        {
            GUILayout.BeginVertical();
            bool state = EditorPrefs.GetBool("LUTBrowser.Foldout." + name, false);
            //if (GUILayout.Button("", GUILayout.ExpandWidth(true), GUILayout.Height(20)))
            //    state = !state;
            if (GUILayout.Button("", GUILayout.Height(20)))
                state = !state;
            GUILayout.Space(-20);
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);
            //EditorGUILayout.Foldout(state, " " + name, m_styleFoldout);

            Rect rect = GUILayoutUtility.GetRect(100, 200, 20, 20, m_styleFoldout);
            // Debug.Log(rect);
            EditorGUI.Foldout(rect, state, " " + name);
            rect = GUILayoutUtility.GetRect(100, 200, 16, 16, m_styleFoldout);
            //Debug.Log(rect);
            EditorGUI.ProgressBar(rect, progress, progress.ToString("p2"));
            //rect = GUILayoutUtility.GetRect(100, 200, 16, 16, m_styleFoldout);
            //EditorGUI.ProgressBar(rect, value + i / 10f, (value + i / 10f).ToString("0.00"));
            //rect = GUILayoutUtility.GetRect(100, 200, 16, 16, m_styleFoldout);
            //EditorGUI.ProgressBar(rect, value + i / 10f, (value + i / 10f).ToString("0.00"));
            //rect = GUILayoutUtility.GetRect(50, 100, 16, 16, m_styleFoldout);
            //EditorGUI.ProgressBar(rect, value + i / 10f, (value + i / 10f).ToString("0.00"));
            //rect = GUILayoutUtility.GetRect(25, 50, 16, 16, m_styleFoldout);
            //EditorGUI.ProgressBar(rect, value + i / 10f, (value + i / 10f).ToString("0.00"));
            //  GUILayout.HorizontalSlider(value + i / 10f, 0f, 1f, GUI.skin.GetStyle("ProgressBarBack"), GUI.skin.GetStyle("ProgressBarBar"));


            GUILayout.EndHorizontal();
            EditorPrefs.SetBool("LUTBrowser.Foldout." + name, state);

            GUILayout.EndVertical();

            return state;
        }

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
