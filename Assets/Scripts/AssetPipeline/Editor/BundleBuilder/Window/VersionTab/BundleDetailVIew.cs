using Galaxy.DataNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Galaxy.AssetPipeline
{

    internal class BundleDetailView : TreeView
    {
        IDataNode m_ParentNode;
        List<IDataNode> m_Selecteditems;
        Rect m_TotalRect;

        private string m_SizeStr;
        private List<string> m_DependenceStrs = new List<string>();

        const float k_DoubleIndent = 32f;
        const string k_SizeHeader = "大小: ";
        const string k_DependencyHeader = "依赖:";
        const string k_DependencyEmpty = k_DependencyHeader + " - None";
        const string k_MessageHeader = "消息:";
        const string k_MessageEmpty = k_MessageHeader + " - None";


        internal BundleDetailView(TreeViewState state, IDataNode parent) : base(state)
        {
            m_ParentNode = parent;
            m_Selecteditems = new List<IDataNode>();
            showBorder = true;
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(-1, -1);

            m_Selecteditems.Clear();
            m_DependenceStrs.Clear();
            m_ParentNode.GetAllLeafNodes(ref m_Selecteditems);
            root.children = new List<TreeViewItem>();

            long totalSize = 0;
            if (m_Selecteditems != null)
            {
                foreach (var bundle in m_Selecteditems)
                {
                    CrudeAssetNode node = bundle.GetData<RecordVariable>().GetValue<RefinedAssetNode>().CrudeNode;
                    totalSize += node.AssetSize;
                    m_DependenceStrs.Add(node.AssetPath);
                }
            }
            m_SizeStr = FormatSize(totalSize);

            root.AddChild(AppendBundleToTree(m_ParentNode.Name));
            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            if ((args.item as BundleDetailItem) != null)
            {
                EditorGUI.HelpBox(
                    new Rect(args.rowRect.x + k_DoubleIndent, args.rowRect.y, args.rowRect.width - k_DoubleIndent, args.rowRect.height),
                    args.item.displayName,
                    (args.item as BundleDetailItem).MessageLevel);
            }
            else
            {
                base.RowGUI(args);
            }
        }
        public override void OnGUI(Rect rect)
        {
            m_TotalRect = rect;
            base.OnGUI(rect);
        }
        protected override float GetCustomRowHeight(int row, TreeViewItem item)
        {
            if ((item as BundleDetailItem) != null)
            {
                float height = DefaultStyles.backgroundEven.CalcHeight(new GUIContent(item.displayName), m_TotalRect.width);
                return height + 3f;
            }
            return base.GetCustomRowHeight(row, item);
        }


        internal TreeViewItem AppendBundleToTree(IDataNode datanode)
        {
            CrudeAssetNode node = datanode.GetData<RecordVariable>().GetValue<RefinedAssetNode>().CrudeNode;
            var itemName = node.AssetName;
            var bunRoot = new TreeViewItem(itemName.GetHashCode(), 0, itemName);

            var str = itemName + k_SizeHeader;
            var sz = new TreeViewItem(str.GetHashCode(), 1, k_SizeHeader + node.AssetSize);

            str = itemName + k_DependencyHeader;
            var dependency = new TreeViewItem(str.GetHashCode(), 1, k_DependencyEmpty);
            var depList = node.Childs;
            if (depList.Count > 0)
            {
                dependency.displayName = k_DependencyHeader;
                foreach (var dep in node.Childs)
                {
                    str = itemName + dep;
                    dependency.AddChild(new TreeViewItem(str.GetHashCode(), 2, dep.AssetPath));
                }
            }

            bunRoot.AddChild(sz);
            bunRoot.AddChild(dependency);

            return bunRoot;
        }
        internal TreeViewItem AppendBundleToTree(string itemName)
        {
            var bunRoot = new TreeViewItem(itemName.GetHashCode(), 0, itemName);

            var str = itemName + k_SizeHeader;
            var sz = new TreeViewItem(str.GetHashCode(), 1, k_SizeHeader + m_SizeStr);

            str = itemName + k_DependencyHeader;
            var dependency = new TreeViewItem(str.GetHashCode(), 1, k_DependencyEmpty);
            var depList = m_DependenceStrs;
            if (depList.Count > 0)
            {
                dependency.displayName = k_DependencyHeader;
                foreach (var dep in m_DependenceStrs)
                {
                    str = itemName + dep;
                    dependency.AddChild(new TreeViewItem(str.GetHashCode(), 2, dep));
                }
            }

            bunRoot.AddChild(sz);
            bunRoot.AddChild(dependency);

            return bunRoot;
        }


        internal void SetSelectedBundles(IDataNode dataNode)
        {
            this.m_ParentNode = dataNode;
            m_Selecteditems.Clear();
            BuildRoot();
            SetSelection(new List<int>());
            Reload();
            ExpandAll();
        }
        private string FormatSize(long bSize)
        {
            if (bSize < 1024)
            {
                return bSize + "B";
            }
            else if (bSize < 1024 * 1024)
            {
                return ((float)bSize / 1024f).ToString("0.00") + "KB";
            }
            else
            {
                return ((float)bSize / (1024f * 1024f)).ToString("0.00") + "M";
            }
        }
    }

    internal class BundleDetailItem : TreeViewItem
    {
        internal BundleDetailItem(int id, int depth, string displayName, MessageType type) : base(id, depth, displayName)
        {
            MessageLevel = type;
        }

        internal MessageType MessageLevel
        { get; set; }
    }
}
