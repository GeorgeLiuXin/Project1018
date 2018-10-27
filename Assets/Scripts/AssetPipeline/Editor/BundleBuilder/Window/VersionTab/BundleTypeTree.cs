using System;
using Galaxy.DataNode;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Galaxy.AssetPipeline
{
    public class BundleTypeTree : TreeView
    {
        internal static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState()
        {
            return new MultiColumnHeaderState(GetColumns());
        }
        private static MultiColumnHeaderState.Column[] GetColumns()
        {
            var retVal = new MultiColumnHeaderState.Column[] {
                new MultiColumnHeaderState.Column(),
                new MultiColumnHeaderState.Column(),
                new MultiColumnHeaderState.Column(),
                new MultiColumnHeaderState.Column()
         //       new MultiColumnHeaderState.Column()
            };
            retVal[0].headerContent = new GUIContent("Asset名字", "Short name of asset. For full name select asset and see message below");
            retVal[0].minWidth = 50;
            retVal[0].width = 155;
            retVal[0].maxWidth = 300;
            retVal[0].headerTextAlignment = TextAlignment.Left;
            retVal[0].canSort = true;
            retVal[0].autoResize = true;

            retVal[1].headerContent = new GUIContent("Asset路径", "Short name of asset. For full name select asset and see message below");
            retVal[1].minWidth = 50;
            retVal[1].width = 325;
            retVal[1].maxWidth = 400;
            retVal[1].headerTextAlignment = TextAlignment.Left;
            retVal[1].canSort = true;
            retVal[1].autoResize = true;

            retVal[2].headerContent = new GUIContent("Bundle名字", "Bundle name. 'auto' means asset was pulled in due to dependency");
            retVal[2].minWidth = 50;
            retVal[2].width = 225;
            retVal[2].maxWidth = 300;
            retVal[2].headerTextAlignment = TextAlignment.Left;
            retVal[2].canSort = true;
            retVal[2].autoResize = true;

            retVal[3].headerContent = new GUIContent("大小(仅供参考)", "Size on disk");
            retVal[3].minWidth = 100;
            retVal[3].width = 175;
            retVal[3].maxWidth = 125;
            retVal[3].headerTextAlignment = TextAlignment.Left;
            retVal[3].canSort = true;
            retVal[3].autoResize = true;

            //retVal[3].headerContent = new GUIContent("!", "Errors, Warnings, or Info");
            //retVal[3].minWidth = 16;
            //retVal[3].width = 16;
            //retVal[3].maxWidth = 16;
            //retVal[3].headerTextAlignment = TextAlignment.Left;
            //retVal[3].canSort = true;
            //retVal[3].autoResize = false;

            return retVal;
        }

        private IDataNode m_ParentNode;
        private bool IsChild
        {
            get
            {
                return m_ParentNode.ChildCount == 0;
            }
        }

        internal BundleTypeTree(TreeViewState state, MultiColumnHeaderState mchs, IDataNode parentNode) : base(state, new MultiColumnHeader(mchs))
        {
            m_ParentNode = parentNode;
            showBorder = true;
            showAlternatingRowBackgrounds = true;
            DefaultStyles.label.richText = true;
            //   multiColumnHeader.sortingChanged += OnSortingChanged;
        }

        protected override TreeViewItem BuildRoot()
        {
            BundleViewItem viewItem = new BundleViewItem(-1, m_ParentNode);
            return viewItem;
        }

        //protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        //{
        //    var rows = base.BuildRows(root);
        //    SortIfNeeded(root, rows);
        //    return rows;
        //}

        //private void SortIfNeeded(TreeViewItem root, IList<TreeViewItem> rows)
        //{
        //    throw new NotImplementedException();
        //}

        protected override void RowGUI(RowGUIArgs args)
        {
            for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
            {
                CellGUI(args.GetCellRect(i), args.item as BundleViewItem, args.GetColumn(i), ref args);
            }
        }

        private void CellGUI(Rect cellRect, BundleViewItem item, int column, ref RowGUIArgs args)
        {
            if (item == null || item.Node == null)
            {
                return;
            }

            Color oldColor = GUI.color;
            CenterRectUsingSingleLineHeight(ref cellRect);

            switch (column)
            {
                case 0:
                    {
                        var iconRect = new Rect(cellRect.x + 1, cellRect.y + 1, cellRect.height - 2, cellRect.height - 2);
                        if (item.icon != null)
                            GUI.DrawTexture(iconRect, item.icon, ScaleMode.ScaleToFit);
                        DefaultGUI.Label(
                            new Rect(cellRect.x + iconRect.xMax + 1, cellRect.y, cellRect.width - iconRect.width, cellRect.height),
                            item.displayName,
                            args.selected,
                            args.focused);
                    }
                    break;
                case 1:
                    DefaultGUI.Label(cellRect, item.Node.CrudeNode.AssetPath, args.selected, args.focused);
                    break;
                case 2:
                    DefaultGUI.Label(cellRect, item.Node.BundleName, args.selected, args.focused);
                    break;
                case 3:
                    DefaultGUI.Label(cellRect, FomatSize(item.Node.CrudeNode.AssetSize), args.selected, args.focused);
                    break;
            }
            GUI.color = oldColor;
        }

        protected override void DoubleClickedItem(int id)
        {
            var assetItem = FindItem(id, rootItem) as BundleViewItem;
            if (assetItem != null)
            {
                UnityEngine.Object o = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetItem.Node.CrudeNode.AssetPath);
                EditorGUIUtility.PingObject(o);
                Selection.activeObject = o;
            }
        }

        internal void SetSelectedBundles(IDataNode m_SelectAssetNode)
        {
            this.m_ParentNode = m_SelectAssetNode;
            BuildRoot();
            SetSelection(new List<int>());
            Reload();
        }

        protected override bool CanBeParent(TreeViewItem item)
        {
            return false;
        }

        public string FomatSize(long bSize)
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

    public class BundleViewItem : TreeViewItem
    {
        internal RefinedAssetNode Node;
        internal BundleViewItem(int depth, IDataNode node) : base(node.GetHashCode(), depth, node.Name)
        {
            if (depth < 0)
            {
                List<IDataNode> leafNodes = new List<IDataNode>();
                node.GetAllLeafNodes(ref leafNodes);
                foreach (IDataNode dataNode in leafNodes)
                {
                    this.AddChild(new BundleViewItem(depth + 1, dataNode));
                }
            }
            else
            {
                Node = node.GetData<RecordVariable>().GetValue<RefinedAssetNode>();
            }
            this.icon = GalaxyBundleEditorGUISetting.GetBundleIcon();
        }
    }
}
