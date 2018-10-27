using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Galaxy.AssetBundleBrowser
{
    public class HelpNoticeTab
    {
        private string m_mdContent;
        private List<string> m_notices;

        internal void OnEnable(Rect subPos, EditorWindow assetBundleBrowserMain)
        {
            if (string.IsNullOrEmpty(m_mdContent))
            {
                ReadNotice();
            }
        }

        internal void OnGUI(Rect pos)
        {
            EditorGUILayout.TextArea(m_mdContent);
        }

        private void ReadNotice()
        {
            string[] icons = AssetDatabase.FindAssets("ABundleBrowserHelperNotice");
            foreach (string i in icons)
            {
                string name = AssetDatabase.GUIDToAssetPath(i);
                
                TextAsset ta = (TextAsset)AssetDatabase.LoadAssetAtPath(name, typeof(TextAsset));
                if (ta)
                {
                    m_mdContent = ta.text;
                }
                else
                {
                    m_mdContent = "aaaa阿萨斯多\n/ndef";
                }
            }
        }

    }
}
