using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace Galaxy.AssetPipeline
{
    public static class GalaxyBundleEditorGUISetting
    {
        internal static /*const*/ Color k_LightGrey = Color.grey * 1.5f;

        static private Texture2D m_folderIcon = null;
        static private Texture2D m_bundleIcon = null;
        static private Texture2D m_sceneIcon = null;
        static internal Texture2D GetFolderIcon()
        {
            if (m_folderIcon == null)
                FindBundleIcons();
            return m_folderIcon;
        }
        static internal Texture2D GetBundleIcon()
        {
            if (m_bundleIcon == null)
                FindBundleIcons();
            return m_bundleIcon;
        }
        static internal Texture2D GetSceneIcon()
        {
            if (m_sceneIcon == null)
                FindBundleIcons();
            return m_sceneIcon;
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
            }
        }

    }
}
