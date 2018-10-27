using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;

namespace Galaxy.AssetPipeline
{
    public class ABBundleMonitorTreeItem : TreeViewItem
    {
        private BundleRefCache m_Cache;
        private List<int> m_ChildIndexs;
        private BundleReference m_Reference;

        public BundleReference Reference
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


        public ABBundleMonitorTreeItem(BundleRefCache cache, BundleReference b, int depth, Texture2D iconTexture) : base(b.BundlePath.GetHashCode(), depth, b.BundlePath)
        {
            m_ChildIndexs = new List<int>();
            m_Cache = cache;
            m_Reference = b;
            icon = iconTexture;
            children = new List<TreeViewItem>();
            if (b.SubDependencies != null && b.SubDependencies.Length > 0)
            {
                foreach (string s in b.SubDependencies)
                    if (cache.ContainsKey(s))
                    {
                        m_ChildIndexs.Add(s.GetHashCode());
                    }
            }
        }
    }
}
