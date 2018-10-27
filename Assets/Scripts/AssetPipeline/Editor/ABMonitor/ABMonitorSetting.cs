using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Galaxy.AssetPipeline
{
    public static class ABMonitorSetting
    {
        static private Texture2D m_folderIcon = null;
        static private Texture2D m_bundleIcon = null;
        static private Texture2D m_sceneIcon = null;

        static private Texture2D m_FullEyeIcon = null;
        static private Texture2D m_HalfEyeIcon = null;
        static private Texture2D m_GrayEyeIcon = null;
        static private Texture2D m_ProgressBarIcon = null;

        static public Texture2D GetFolderIcon()
        {
            if (m_folderIcon == null)
                FindBundleIcons();
            return m_folderIcon;
        }
        static public Texture2D GetBundleIcon()
        {
            if (m_bundleIcon == null)
                FindBundleIcons();
            return m_bundleIcon;
        }
        static public Texture2D GetSceneIcon()
        {
            if (m_sceneIcon == null)
                FindBundleIcons();
            return m_sceneIcon;
        }

        static public Texture2D GetFullEyeIcon()
        {
            if (m_folderIcon == null)
                FindBundleIcons();
            return m_FullEyeIcon;
        }
        static public Texture2D GetHalfEyeIcon()
        {
            if (m_bundleIcon == null)
                FindBundleIcons();
            return m_HalfEyeIcon;
        }
        static public Texture2D GetGrayEyeIcon()
        {
            if (m_sceneIcon == null)
                FindBundleIcons();
            return m_GrayEyeIcon;
        }

        public static Color GetBackGroudColor(BundleReference reference)
        {
            if (reference.Error)
            {
                return Color.red;
            }
            if (reference.Invalid)
            {
                return Color.yellow;
            }
            if (reference.Done)
            {
                return Color.white;
            }
            else if (reference.Progress == 1)
            {
                return Color.green;
            }
            return Color.white;
        }


        public static Color GetBackGroudColor(AssetReference reference)
        {
            if (reference.Error)
            {
                return Color.red;
            }
            if (reference.Invalid)
            {
                return Color.yellow;
            }
            if (reference.Done)
            {
                return Color.white;
            }
            else if (reference.Progress == 1)
            {
                return Color.green;
            }
            return Color.white;
        }


        static public Texture2D GetProgressIcon()
        {
            if (m_ProgressBarIcon == null)
                FindBundleIcons();
            return m_ProgressBarIcon;
        }

        public static void OnEnable() {

        }

        static private void FindBundleIcons()
        {
            m_folderIcon = EditorGUIUtility.FindTexture("Folder Icon");
            string[] icons = AssetDatabase.FindAssets("ABundleBrowserIconY1756");
            foreach (string i in icons)
            {
                string name = AssetDatabase.GUIDToAssetPath(i);
                if (name.Contains("ABundleBrowserIconY1756Basic.png"))
                    m_bundleIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(name, typeof(Texture2D));
                else if (name.Contains("ABundleBrowserIconY1756Scene.png"))
                    m_sceneIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(name, typeof(Texture2D));
                else if(name.Contains("ABundleBrowserIconY1756Progress.png"))
                    m_ProgressBarIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(name, typeof(Texture2D));
                else if (name.Contains("ABundleBrowserIconY1756FullEye.png"))
                    m_FullEyeIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(name, typeof(Texture2D));
                else if (name.Contains("ABundleBrowserIconY1756HalfEye.png"))
                    m_HalfEyeIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(name, typeof(Texture2D));
                else if (name.Contains("ABundleBrowserIconY1756GrayEye.png"))
                    m_GrayEyeIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(name, typeof(Texture2D));
            }
        }


        public static string GetStateString(BundleReference reference)
        {
            if (reference.Error)
            {
                return STATE_ERROR;
            }
            if (reference.Invalid)
            {
                return STATE_WARNING;
            }
            if (reference.Done)
            {
                return STATE_SUCCESS;
            }
            else if (reference.Progress == 1)
            {
                return STATE_WAIT;
            }
            return STATE_DOING;
        }

        public static string GetStateString(AssetReference reference)
        {
            if (reference.Error)
            {
                return STATE_ERROR;
            }
            if (reference.Invalid)
            {
                return STATE_WAITING_BUNDLE;
            }
            if (reference.Done)
            {
                return STATE_SUCCESS;
            }
            return STATE_PARSE_BUNDLE;
        }

        static string STATE_ERROR = "失败";
        static string STATE_SUCCESS = "成功";
        static string STATE_WAIT = "等待依赖";
        static string STATE_WARNING = "依赖丢失";
        static string STATE_WAITING_BUNDLE = "等待AB资源";
        static string STATE_PARSE_BUNDLE = "解析资源";
        static string STATE_DOING = "进行中";
    }
}
