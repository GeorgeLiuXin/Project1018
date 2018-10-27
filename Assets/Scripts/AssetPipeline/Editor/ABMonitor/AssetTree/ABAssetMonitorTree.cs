using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using System.Linq;
using System;

namespace Galaxy.AssetPipeline
{
    public class ABAssetMonitorTree : TreeView
    {
        public AssetPipelineAction<AssetReference> OnAssetRefSelected;
        bool m_ShouldRefresh = false;
        private int m_SelectId;
        AssetManager m_Manager;
        public ABAssetMonitorTree(AssetManager manager, TreeViewState state, MultiColumnHeaderState mchs) : base(state, new MultiColumnHeader(mchs))
        {
            m_Manager = manager;
            m_Manager.Cache.RefChangedEvent += Refresh;
            m_Manager.Cache.RefShutdownEvent += Shutdown;
            m_Manager.OnReInitManger += OnReInitManger;
            showBorder = true;
            showAlternatingRowBackgrounds = true;
            DefaultStyles.label.richText = true;
        }

        private void OnReInitManger()
        {
            m_Manager.Cache.RefChangedEvent += Refresh;
            m_Manager.Cache.RefShutdownEvent += Shutdown;
        }

        ~ABAssetMonitorTree()
        {
            if (m_Manager.Cache != null)
            {
                m_Manager.Cache.RefChangedEvent -= Refresh;
                m_Manager.Cache.RefShutdownEvent -= Shutdown;
            }
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(-1, -1);
            root.children = new List<TreeViewItem>();

            Dictionary<string, AssetReference> refDict = m_Manager.Cache.AssetRefDict;

            foreach (AssetReference b in refDict.Values)
            {
                if (b.RefCount > 0)
                    root.AddChild(new ABAssetMonitorTreeItem(m_Manager.Cache, b, 0, null));
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
                if (m_Manager.Cache != null)
                {
                    rootItem.children.Clear();

                    Dictionary<string, AssetReference> refDict = m_Manager.Cache.AssetRefDict;
                    foreach (AssetReference b in refDict.Values)
                    {
                        rootItem.AddChild(new ABAssetMonitorTreeItem(m_Manager.Cache, b, 0, null));
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
            var assetItem = (args.item as ABAssetMonitorTreeItem);

            if (assetItem == null)
            {
                base.RowGUI(args);
                return;
            }

            AssetReference reference = assetItem.Reference;
            if (reference == null)
            {
                base.RowGUI(args);
                return;
            }

            int columns = args.GetNumVisibleColumns();

            for (int i = 0; i < columns; ++i)
                CellGUI(args.GetCellRect(i), reference, args.GetColumn(i), assetItem.id, ref args);
        }

        private void CellGUI(Rect cellRect, AssetReference item, int column, int id, ref RowGUIArgs args)
        {
            Color oldColor = GUI.color;
            Color oldBgColor = GUI.contentColor;
            Texture2D iconTex = null;
            if (item.IsScene)
            {
                iconTex = ABMonitorSetting.GetSceneIcon();
            }
            else
            {
                iconTex = ABMonitorSetting.GetBundleIcon();
            }

            CenterRectUsingSingleLineHeight(ref cellRect);
            if (column == 2)
                GUI.color = ABMonitorSetting.GetBackGroudColor(item);

            switch (column)
            {
                case 0:
                    {
                        var iconRect = new Rect(cellRect.x + 1, cellRect.y + 1, cellRect.height - 2, cellRect.height - 2);

                        GUI.DrawTexture(iconRect, iconTex, ScaleMode.ScaleToFit);
                        DefaultGUI.Label(
                        new Rect(cellRect.x + iconRect.xMax + 1, cellRect.y, cellRect.width - iconRect.width, cellRect.height),
                        item.AssetPath,
                        args.selected,
                        args.focused);
                    }
                    break;
                case 1:
                    DefaultGUI.Label(cellRect, item.BundlePath.ToString(), args.selected, args.focused);
                    break;
               
                case 2:
                    DefaultGUI.Label(cellRect, ABMonitorSetting.GetStateString(item), args.selected, args.focused);
                    break;
                case 3:
                    DefaultGUI.Label(cellRect, item.RefCount.ToString(), args.selected, args.focused);
                    break;
                case 4:
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
                var assetItem = (targetViewItem as ABAssetMonitorTreeItem);
                if (assetItem != null)
                {
                    if (OnAssetRefSelected != null)
                    {
                        OnAssetRefSelected(assetItem.Reference);
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
            };
            int i = 0;
            retVal[i].headerContent = new GUIContent("资源路径", "资源路径");
            retVal[i].minWidth = 300;
            retVal[i].width = 350;
            retVal[i].maxWidth = 800;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = true;

            i++;
            retVal[i].headerContent = new GUIContent("AB路径", "对应配置表中AB包相对路径");
            retVal[i].minWidth = 300;
            retVal[i].width = 450;
            retVal[i].maxWidth = 800;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = true;

            //i++;
            //retVal[i].headerContent = new GUIContent("AB校验码", "AB包的CRC");
            //retVal[i].minWidth = 100;
            //retVal[i].width = 125;
            //retVal[i].maxWidth = 190;
            //retVal[i].headerTextAlignment = TextAlignment.Left;
            //retVal[i].canSort = true;
            //retVal[i].autoResize = true;

            //i++;
            //retVal[i].headerContent = new GUIContent("硬盘大小", "硬盘大小,非内存");
            //retVal[i].minWidth = 40;
            //retVal[i].width = 65;
            //retVal[i].maxWidth = 100;
            //retVal[i].headerTextAlignment = TextAlignment.Left;
            //retVal[i].canSort = true;
            //retVal[i].autoResize = true;

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
