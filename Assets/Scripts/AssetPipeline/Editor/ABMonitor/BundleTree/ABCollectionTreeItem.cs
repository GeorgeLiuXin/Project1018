using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;

namespace Galaxy.AssetPipeline
{
    public class ABCollectionTreeItem : TreeViewItem
    {
      //  private BundleRefCache m_Cache;
        public GarbageData m_Data;

        public GarbageData Data
        {
            get { return m_Data; }
        }

        //public ABBundleMonitorTreeItem(string bundlePath, ) : base(b.GetHashCode(), depth, b.BundlePath)
        //{

        //}

        public ABCollectionTreeItem(GarbageData b, int depth, Texture2D iconTexture) : base(b.GetHashCode(), depth, b.BundleName)
        {
            m_Data = b;
            icon = iconTexture;
        }
    }
}
