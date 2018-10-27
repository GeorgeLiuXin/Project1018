using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;

namespace Galaxy.AssetPipeline
{
    public class ABMonitorDetailsTreeItem : TreeViewItem
    {
        private ABMonitorNode m_MonitorNode;
        
        public ABMonitorDetailsTreeItem(ABMonitorNode node, int depth, Texture2D iconTexture) : base(node.GetHashCode(), depth, node.Path)
        {
            m_MonitorNode = node;
            icon = iconTexture;
            children = new List<TreeViewItem>();
        }

        public ABMonitorNode MonitorNode
        {
            get
            {
                return m_MonitorNode;
            }
        }
    }
}
