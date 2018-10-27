using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Galaxy.AssetBundleBrowser
{
    public class VersionAnalyseTab
    {
        private EditorWindow m_parentWindow;

        internal void OnEnable(Rect pos, EditorWindow parent)
        {
            m_parentWindow = parent;
        }

        internal void OnGUI(Rect pos)
        {
            
        }

        internal void OnDisable()
        {
        }
    }
}
