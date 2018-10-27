using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using System.Linq;
using System;

namespace Galaxy.AssetPipeline
{
    public class ABBundleMonitorTree : TreeView
    {
        public AssetPipelineAction<BundleReference> OnBundleRefSelected;
        bool m_ShouldRefresh = false;
        private Color m_RefColor;
        private int m_SelectId;
        private List<int> m_SubSelections = new List<int>();
        BundleRefCache m_Cache;
        public ABBundleMonitorTree(BundleRefCache cache, TreeViewState state, MultiColumnHeaderState mchs) : base(state, new MultiColumnHeader(mchs))
        {
            m_Cache = cache;
            m_Cache.RefChangedEvent += Refresh;
            m_Cache.RefShutdownEvent += Shutdown;
            m_RefColor = new Color(0 / 255f, 255f / 255f, 255f / 255f);
            showBorder = true;
            showAlternatingRowBackgrounds = true;
            DefaultStyles.label.richText = true;
        }

        ~ABBundleMonitorTree()
        {
            if (m_Cache != null)
            {
                m_Cache.RefChangedEvent -= Refresh;
                m_Cache.RefShutdownEvent -= Shutdown;
            }
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(-1, -1);
            root.children = new List<TreeViewItem>();

            Dictionary<string, BundleReference> refDict = m_Cache.BundleRefDict;

            foreach (BundleReference b in refDict.Values)
            {
                if (b.RefCount > 0)
                    root.AddChild(new ABBundleMonitorTreeItem(m_Cache, b, 0, null));
            }
            return root;
        }
        
        public void Refresh()
        {
            m_ShouldRefresh = true;

        }
        private void Shutdown()
        {
            rootItem.children.Clear();
            Reload();
        }

        public void Update(float deltaTime)
        {
            if (m_ShouldRefresh)
            {
                if (m_Cache != null)
                {
                    rootItem.children.Clear();

                    Dictionary<string, BundleReference> refDict = m_Cache.BundleRefDict;
                    foreach (BundleReference b in refDict.Values)
                    {
                        rootItem.AddChild(new ABBundleMonitorTreeItem(m_Cache, b, 0, null));
                    }
                    Reload();
                }
                else
                {
                    rootItem.children.Clear();
                    Reload();
                }
            }
        }
        
        protected override void RowGUI(RowGUIArgs args)
        {
            var bundleItem = (args.item as ABBundleMonitorTreeItem);

            if (bundleItem == null)
            {
                base.RowGUI(args);
                return;
            }

            BundleReference reference = bundleItem.Reference;
            if (reference == null)
            {
                base.RowGUI(args);
                return;
            }

            int columns = args.GetNumVisibleColumns();

            for (int i = 0; i < columns; ++i)
                CellGUI(args.GetCellRect(i), reference, args.GetColumn(i), bundleItem.id, ref args);
        }

        private void CellGUI(Rect cellRect, BundleReference item, int column, int id, ref RowGUIArgs args)
        {
            Color oldColor = GUI.color;
            Color oldBgColor = GUI.contentColor;
            Texture2D iconTex = null;
            if (m_SelectId == id)
            {
                iconTex = ABMonitorSetting.GetFullEyeIcon();
                //cellRect = new Rect(cellRect.x + 5, cellRect.y, cellRect.width, cellRect.height);
            }
            else if (m_SubSelections.Contains(id))
            {
                GUI.contentColor = m_RefColor;
                iconTex = ABMonitorSetting.GetHalfEyeIcon();
            }
            else
            {
                iconTex = ABMonitorSetting.GetGrayEyeIcon();
            }

            CenterRectUsingSingleLineHeight(ref cellRect);
            if (column == 3)
                GUI.color = ABMonitorSetting.GetBackGroudColor(item);

            switch (column)
            {
                case 0:
                    {
                        var iconRect = new Rect(cellRect.x + 1, cellRect.y + 1, cellRect.height - 2, cellRect.height - 2);

                        GUI.DrawTexture(iconRect, iconTex, ScaleMode.ScaleToFit);
                        DefaultGUI.Label(
                        new Rect(cellRect.x + iconRect.xMax + 1, cellRect.y, cellRect.width - iconRect.width, cellRect.height),
                        item.BundlePath,
                        args.selected,
                        args.focused);
                    }
                    break;
                case 1:
                    DefaultGUI.Label(cellRect, item.CRC.ToString(), args.selected, args.focused);
                    break;
                case 2:
                    DefaultGUI.Label(cellRect, EditorUtility.FormatBytes(item.Size), args.selected, args.focused);
                    break;
                case 3:
                    DefaultGUI.Label(cellRect, ABMonitorSetting.GetStateString(item), args.selected, args.focused);
                    break;
                case 4:
                    DefaultGUI.Label(cellRect, item.RefCount.ToString(), args.selected, args.focused);
                    break;
                case 5:
                    var barRect = new Rect(cellRect.x, cellRect.y, cellRect.width, cellRect.height);
                    EditorGUI.ProgressBar(barRect, item.Progress, item.Progress.ToString("P2"));
                    break;
            }
            GUI.contentColor = oldBgColor;
            GUI.color = oldColor;
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            base.SelectionChanged(selectedIds);
            m_SelectId = selectedIds[0];
            m_SubSelections = new List<int>();

            TreeViewItem targetViewItem = null;
            foreach (TreeViewItem viewItem in this.rootItem.children)
            {
                if (viewItem.id == m_SelectId)
                {
                    targetViewItem = viewItem;
                }
            }

            if (targetViewItem != null)
            {
                var bundleItem = (targetViewItem as ABBundleMonitorTreeItem);
                if (bundleItem != null)
                {
                    m_SubSelections = bundleItem.ChildIndexs;
                    if (OnBundleRefSelected != null)
                    {
                        OnBundleRefSelected(bundleItem.Reference);
                    }
                }
            }
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState()
        {
            return new MultiColumnHeaderState(GetColumns());
        }
        private static MultiColumnHeaderState.Column[] GetColumns()
        {
            var retVal = new MultiColumnHeaderState.Column[] {
                new MultiColumnHeaderState.Column(),
                new MultiColumnHeaderState.Column(),
                new MultiColumnHeaderState.Column(),
                new MultiColumnHeaderState.Column(),
                new MultiColumnHeaderState.Column(),
                new MultiColumnHeaderState.Column(),
            };
            int i = 0;
            //retVal[i].headerContent = new GUIContent("类型", "AB包类型");
            //retVal[i].minWidth = 10;
            //retVal[i].width = 30;
            //retVal[i].maxWidth = 50;
            //retVal[i].headerTextAlignment = TextAlignment.Left;
            //retVal[i].canSort = true;
            //retVal[i].autoResize = true;

            //i++;
            retVal[i].headerContent = new GUIContent("AB路径", "对应配置表中AB包相对路径");
            retVal[i].minWidth = 300;
            retVal[i].width = 550;
            retVal[i].maxWidth = 800;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = true;

            i++;
            retVal[i].headerContent = new GUIContent("AB校验码", "AB包的CRC");
            retVal[i].minWidth = 100;
            retVal[i].width = 125;
            retVal[i].maxWidth = 190;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = true;

            i++;
            retVal[i].headerContent = new GUIContent("硬盘大小", "硬盘大小,非内存");
            retVal[i].minWidth = 40;
            retVal[i].width = 65;
            retVal[i].maxWidth = 100;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = true;

            i++;
            retVal[i].headerContent = new GUIContent("状态", "AB任务状态");
            retVal[i].minWidth = 86;
            retVal[i].width = 86;
            retVal[i].maxWidth = 86;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = false;

            i++;
            retVal[i].headerContent = new GUIContent("引用数", "AB引用计数");
            retVal[i].minWidth = 46;
            retVal[i].width = 46;
            retVal[i].maxWidth = 46;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = false;
            
            i++;
            retVal[i].headerContent = new GUIContent("进度", "AB任务进度条");
            retVal[i].minWidth = 70;
            retVal[i].width = 200;
            retVal[i].maxWidth = 300;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = false;
            retVal[i].autoResize = false;

            return retVal;
        }

    }
}
