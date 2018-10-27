using Galaxy.DataNode;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Galaxy.AssetPipeline
{
    internal class AssetViewItem : TreeViewItem
    {
        internal IDataNode Node;
        internal AssetViewItem(int depth, IDataNode node) : base(node.GetHashCode(), depth, node.Name)
        {
            Node = node;
            IDataNode[] childrenNodes = node.GetAllChild();
            foreach (IDataNode dataNode in childrenNodes)
            {
                this.AddChild(new AssetViewItem(depth + 1, dataNode));
            }

            if (childrenNodes.Length > 0)
            {
                this.icon = GalaxyBundleEditorGUISetting.GetFolderIcon();
            }
            else
            {
                this.icon = GalaxyBundleEditorGUISetting.GetBundleIcon();
            }
        }
    }
}