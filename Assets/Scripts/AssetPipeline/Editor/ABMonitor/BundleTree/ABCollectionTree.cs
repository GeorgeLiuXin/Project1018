using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using System.Linq;
using System;

namespace Galaxy.AssetPipeline
{
    public class ABCollectionTree : TreeView
    {
        BundleCollection m_Collection;
        bool m_ShouldRefresh = false;
        public ABCollectionTree(BundleCollection gc, TreeViewState state, MultiColumnHeaderState mchs) : base(state, new MultiColumnHeader(mchs))
        {
            m_Collection = gc;
            m_Collection.CollectionChangedEvent += Refresh;
            m_Collection.CollectionShutdownEvent += Shutdown;
            showBorder = true;
            showAlternatingRowBackgrounds = true;
            DefaultStyles.label.richText = true;
        }
        
        ~ABCollectionTree()
        {
            if (m_Collection != null)
            {
                m_Collection.CollectionChangedEvent -= Refresh;
                m_Collection.CollectionShutdownEvent -= Shutdown;
            }
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(-1, -1);
            root.children = new List<TreeViewItem>();

            List<GarbageData> refDict = m_Collection.GarbageAssetList;
            
            foreach (GarbageData b in refDict)
            {
                root.AddChild(new ABCollectionTreeItem(b, 0, null));
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
                if (m_Collection != null)
                {
                    rootItem.children.Clear();

                    List<GarbageData> refDict = m_Collection.GarbageAssetList;

                    foreach (GarbageData b in refDict)
                    {
                        rootItem.AddChild(new ABCollectionTreeItem(b, 0, null));
                    }
                    Reload();
                }
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var bundleItem = (args.item as ABCollectionTreeItem);

            if (bundleItem == null)
            {
                base.RowGUI(args);
                return;
            }

            GarbageData item = bundleItem.Data;
            if (item == null)
            {
                base.RowGUI(args);
                return;
            }

            int columns = args.GetNumVisibleColumns();

            for (int i = 0; i < columns; ++i)
                CellGUI(args.GetCellRect(i), item, args.GetColumn(i), bundleItem.depth, ref args);
        }

        private void CellGUI(Rect cellRect, GarbageData item, int column, int depth, ref RowGUIArgs args)
        {
            CenterRectUsingSingleLineHeight(ref cellRect);

            switch (column)
            {
                case 0:
                    {
                        var iconRect = new Rect(cellRect.x + 1, cellRect.y + 1, cellRect.height - 2, cellRect.height - 2);
                        GUI.DrawTexture(iconRect, ABMonitorSetting.GetBundleIcon(), ScaleMode.ScaleToFit);
                        DefaultGUI.Label(
                            new Rect(cellRect.x + iconRect.xMax + 1, cellRect.y, cellRect.width - iconRect.width, cellRect.height),
                            item.BundleName,
                            args.selected,
                            args.focused);
                    }
                    break;
                case 1:
                    DefaultGUI.Label(cellRect, item.Generation.ToString(), args.selected, args.focused);
                    break;
                case 2:
                    DefaultGUI.Label(cellRect, item.Interval.ToString("0.00") + "s", args.selected, args.focused);
                    break;
                case 3:
                    DefaultGUI.Label(cellRect, m_Collection.UnloadInterval.ToString("0.00") + "s", args.selected, args.focused);
                    break;
            }
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
                new MultiColumnHeaderState.Column()
            };
            int i = 0;
       
            retVal[i].headerContent = new GUIContent("AB路径", "对应配置表中AB包相对路径");
            retVal[i].minWidth = 200;
            retVal[i].width = 350;
            retVal[i].maxWidth = 500;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = true;

            i++;
            retVal[i].headerContent = new GUIContent("AB迭代", "重复引用的次数");
            retVal[i].minWidth = 40;
            retVal[i].width = 55;
            retVal[i].maxWidth = 60;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = true;

            i++;
            retVal[i].headerContent = new GUIContent("倒计时", "倒计时为0时执行卸载");
            retVal[i].minWidth = 40;
            retVal[i].width = 55;
            retVal[i].maxWidth = 60;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = true;

            i++;
            retVal[i].headerContent = new GUIContent("卸载计时", "计时超过即将在Tick中被卸载");
            retVal[i].minWidth = 40;
            retVal[i].width = 55;
            retVal[i].maxWidth = 60;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = true;
            return retVal;
        }

    }
}
