using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;

namespace Galaxy.AssetPipeline
{
    public class ABAssetMonitorTreeItem : TreeViewItem
    {
        private AssetRefCache m_Cache;
        private List<int> m_ChildIndexs;
        private AssetReference m_Reference;

        public AssetReference Reference
        {
            get { return m_Reference; }
        }

        public List<int> ChildIndexs
        {
            get
            {
                return m_ChildIndexs;
            }
        }

        public bool ContainsId(int id)
        {
            return m_ChildIndexs.Contains(id);
        }


        public ABAssetMonitorTreeItem(AssetRefCache cache, AssetReference b, int depth, Texture2D iconTexture) : base(b.AssetPath.GetHashCode(), depth, b.AssetPath)
        {
            m_ChildIndexs = new List<int>();
            m_Cache = cache;
            m_Reference = b;
            icon = iconTexture;
        }
    }
}
