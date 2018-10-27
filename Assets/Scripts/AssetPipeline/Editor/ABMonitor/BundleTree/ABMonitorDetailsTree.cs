using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using System.Linq;
using System;

namespace Galaxy.AssetPipeline
{
    public class ABMonitorDetailsTree : TreeView
    {
        ABMonitor m_Monitor;
        public ABMonitorDetailsTree(ABMonitor monitor, TreeViewState state, MultiColumnHeaderState mchs) : base(state, new MultiColumnHeader(mchs))
        {
            m_Monitor = monitor;
            m_Monitor.OnMonitorUpdate += OnMonitorUpdate;
            showBorder = true;
            showAlternatingRowBackgrounds = true;
            DefaultStyles.label.richText = true;
        }
        
        ~ABMonitorDetailsTree()
        {
            if (m_Monitor != null)
                m_Monitor.OnMonitorUpdate -= OnMonitorUpdate;
        }

        private void OnMonitorUpdate()
        {
            rootItem.children.Clear();

            List<ABMonitorNode> nodes = m_Monitor.RefenceNodess;

            foreach (ABMonitorNode b in nodes)
            {
                if (b.RefCount > 0)
                    this.rootItem.AddChild(new ABMonitorDetailsTreeItem(b, 0, null));
            }
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(-1, -1);
            root.children = new List<TreeViewItem>();

            List<ABMonitorNode> nodes = m_Monitor.RefenceNodess;
            
            foreach (ABMonitorNode b in nodes)
            {
                if (b.RefCount > 0)
                    root.AddChild(new ABMonitorDetailsTreeItem(b, 0, null));
            }
            return root;
        }

        public void Reset(BundleReference br)
        {
            m_Monitor.OnMonitorUpdate -= OnMonitorUpdate;
            m_Monitor = br.Monitor;
            m_Monitor.OnMonitorUpdate += OnMonitorUpdate;
            OnMonitorUpdate();
        }

        public void Reset(AssetReference ar)
        {
            m_Monitor.OnMonitorUpdate -= OnMonitorUpdate;
            m_Monitor = ar.Monitor;
            m_Monitor.OnMonitorUpdate += OnMonitorUpdate;
            OnMonitorUpdate();
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var bundleItem = (args.item as ABMonitorDetailsTreeItem);

            if (bundleItem == null)
            {
                base.RowGUI(args);
                return;
            }

            ABMonitorNode item = bundleItem.MonitorNode;
            if (item == null)
            {
                base.RowGUI(args);
                return;
            }
            bool isExpended = IsExpanded(args.row);
            int columns = args.GetNumVisibleColumns();

            for (int i = 0; i < columns; ++i)
                CellGUI(args.GetCellRect(i), item, args.GetColumn(i), bundleItem.depth, ref args);
        }

        private void CellGUI(Rect cellRect, ABMonitorNode item, int column, int depth, ref RowGUIArgs args)
        {
            Color oldColor = GUI.color;
            if (depth >= 1)
            {
                GUI.color = Color.grey;
                cellRect = new Rect(cellRect.x + 3, cellRect.y, cellRect.width, cellRect.height);
            }

            CenterRectUsingSingleLineHeight(ref cellRect);
           
            switch (column)
            {
                case 0:
                    {
                        string name = "Missing";
                        if (item.Target != null)
                        {
                            name = item.Target.ToString();
                        }
                        
                        DefaultGUI.Label(
                        cellRect,
                        name,
                        args.selected,
                        args.focused);
                    }
                    break;
                case 1:
                    DefaultGUI.Label(cellRect, item.Path.ToString(), args.selected, args.focused);
                    break;
                case 2:
                    DefaultGUI.Label(cellRect, item.Time.ToShortTimeString(), args.selected, args.focused);
                    break;
                case 3:
                    DefaultGUI.Label(cellRect, item.RefCount.ToString(), args.selected, args.focused);
                    break;
                case 4:
                    DefaultGUI.Label(cellRect, item.IsAwakeCaller.ToString(), args.selected, args.focused);
                    break;
                case 5:
                    DefaultGUI.Label(cellRect, item.IsAlive.ToString(), args.selected, args.focused);
                    break;
            }
            GUI.color = oldColor;
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
            retVal[i].headerContent = new GUIContent("目标", "引用类型");
            retVal[i].minWidth = 40;
            retVal[i].width = 200;
            retVal[i].maxWidth = 800;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = true;

            i++;
            retVal[i].headerContent = new GUIContent("路径", "引用项目中路径");
            retVal[i].minWidth = 100;
            retVal[i].width = 250;
            retVal[i].maxWidth = 600;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = true;

            i++;
            retVal[i].headerContent = new GUIContent("修改时间", "最后一次修改时间");
            retVal[i].minWidth = 80;
            retVal[i].width = 80;
            retVal[i].maxWidth = 80;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = true;

            i++;
            retVal[i].headerContent = new GUIContent("引用数", "引用的次数");
            retVal[i].minWidth = 46;
            retVal[i].width = 46;
            retVal[i].maxWidth = 46;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = true;

            i++;
            retVal[i].headerContent = new GUIContent("唤起者", "是否由此开始");
            retVal[i].minWidth = 46;
            retVal[i].width = 46;
            retVal[i].maxWidth = 46;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = false;

            i++;
            retVal[i].headerContent = new GUIContent("销毁", "内存是否已经被回收");
            retVal[i].minWidth = 46;
            retVal[i].width = 46;
            retVal[i].maxWidth = 46;
            retVal[i].headerTextAlignment = TextAlignment.Left;
            retVal[i].canSort = true;
            retVal[i].autoResize = false;

            //i++;
            //retVal[i].headerContent = new GUIContent("引用数", "AB引用计数");
            //retVal[i].minWidth = 46;
            //retVal[i].width = 46;
            //retVal[i].maxWidth = 46;
            //retVal[i].headerTextAlignment = TextAlignment.Left;
            //retVal[i].canSort = true;
            //retVal[i].autoResize = false;
            

            return retVal;
        }

  
    }
}
