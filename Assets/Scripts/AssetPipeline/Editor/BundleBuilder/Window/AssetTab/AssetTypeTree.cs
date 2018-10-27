using Galaxy.DataNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Events;

namespace Galaxy.AssetPipeline
{
    internal class AssetTypeTree : TreeView
    {
        public class AssetTypeEvent : UnityEvent<IDataNode> { }
        public AssetTypeEvent m_OnSelectionChangeEvent = new AssetTypeEvent();
        public AssetTypeTree(TreeViewState state, DataNodeManager manager) : base(state)
        {
            showBorder = true;
            m_AssetDataNodeManager = manager;
        }

        DataNodeManager m_AssetDataNodeManager;
        AssetViewItem m_Root;

        protected override TreeViewItem BuildRoot()
        {
            m_Root = new AssetViewItem(-1, m_AssetDataNodeManager.Root);
            return m_Root;
        }

        public override void OnGUI(Rect rect)
        {
            base.OnGUI(rect);

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect.Contains(Event.current.mousePosition))
            {
                SetSelection(new int[0], TreeViewSelectionOptions.FireSelectionChanged);
            }
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            foreach (var i in selectedIds)
            {
                TreeViewItem t = FindItem(i, m_Root);
                AssetViewItem f = (AssetViewItem)t;
                if (f.Node != null)
                    m_OnSelectionChangeEvent.Invoke(f.Node);
            }
        }
    }
}
