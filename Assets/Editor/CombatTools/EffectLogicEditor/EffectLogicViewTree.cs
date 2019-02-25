using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace XWorld
{
    public class EffectLogicViewTree : TreeView
    {
        // All columns
        enum classColumns
        {
            Name,
            nameValue,
            Des,
        }
        enum propertyColumns
        {
            Name,
            Type,
            Des,
            Value,
        }

        private GUIStyle style;
        public EffectLogicParamData m_data;

        private bool m_InSearchState;

        public EffectLogicViewTree(TreeViewState state, MultiColumnHeaderState mchs)
            : base(state, new MultiColumnHeader(mchs))
        {
            showBorder = true;
            showAlternatingRowBackgrounds = true;
            DefaultStyles.label.richText = true;
            
            style = new GUIStyle(GUI.skin.button);
            style.alignment = TextAnchor.MiddleLeft;

            m_data = null;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            root.children = new List<TreeViewItem>();
            
            if (m_data != null)
            {
                List<EffectLogicParamList>.Enumerator itor = m_data.GetEnumerator();

                while (itor.MoveNext())
                {
                    EffectLogicParamList _class = itor.Current;

                    EffectLogicListViewTreeItem arrayItem = new EffectLogicListViewTreeItem(_class, 0);
                    root.AddChild(arrayItem);
                }
            }
            return root;
        }

        public override void OnGUI(Rect rect)
        {
            base.OnGUI(rect);
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            base.RowGUI(args);

            if (!state.searchString.IsNE())
            {
                //数据是否同步，不同步则viewtree保留唯一一份数据，其余保留索引
                if (args.item is EffectLogicListViewTreeItem)
                {
                    EffectLogicListViewTreeItem treeItem = args.item as EffectLogicListViewTreeItem;
                    Color oldColor = GUI.color;
                    bool expended = IsExpanded(treeItem.id);
                    if (GUI.Button(args.rowRect, "  " + treeItem.m_class.sLogicName + "     Des", style))
                    {
                        treeItem.IsFold = !treeItem.IsFold;
                        expended = !expended;
                        SetExpanded(treeItem.id, expended);
                    }
                    GUI.color = oldColor;
                }
            }

            if (args.item is EffectLogicListViewTreeItem)
            {
                EffectLogicListViewTreeItem treeItem = args.item as EffectLogicListViewTreeItem;
                Color oldColor = GUI.color;
                bool expended = IsExpanded(treeItem.id);
                if (GUI.Button(args.rowRect, "  " + treeItem.m_class.sLogicName + "     Des", style))
                {
                    treeItem.IsFold = !treeItem.IsFold;
                    expended = !expended;
                    SetExpanded(treeItem.id, expended);
                }
                GUI.color = oldColor;
            }
            else
            {
                EffectLogicViewTreeItem treeItem = args.item as EffectLogicViewTreeItem;
                for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
                    CellGUI(args.GetCellRect(i), treeItem.m_property, args.GetColumn(i), ref args);
            }
        }

        private void CellGUI(Rect cellRect, EffectLogicListViewTreeItem item, int column, ref RowGUIArgs args)
        {
            Color oldColor = GUI.color;
            CenterRectUsingSingleLineHeight(ref cellRect);

            switch ((classColumns)column)
            {
                case classColumns.Name:
                    GUI.Label(cellRect, "类名: ");
                    break;
                case classColumns.nameValue:
                    GUI.Label(cellRect, item.m_class.sLogicName);
                    break;
                case classColumns.Des:
                    GUI.Label(cellRect, item.m_class.sLogicName);
                    break;
                default:
                    break;
            }
            GUI.color = oldColor;
        }
        private void CellGUI(Rect cellRect, EffectLogicParamItem item, int column, ref RowGUIArgs args)
        {
            CenterRectUsingSingleLineHeight(ref cellRect);
            switch ((propertyColumns)column)
            {
                case propertyColumns.Name:
                    GUI.Label(cellRect, item.sName);
                    break;
                case propertyColumns.Type:
                    GUI.Label(cellRect, item.sType);
                    break;
                case propertyColumns.Des:
                    GUI.Label(cellRect, item.sName);
                    break;
                case propertyColumns.Value:
                    item.sValue = GUI.TextField(cellRect, item.sValue);
                    break;
                default:
                    break;
            }
        }

        protected override bool DoesItemMatchSearch(TreeViewItem item, string search)
        {
            bool bSearchValue = false;
            if (search.StartsWith("Value_") || search.StartsWith("value_"))
            {
                bSearchValue = true;
                search = search.Substring(6);
            }

            if (item is EffectLogicListViewTreeItem)
            {
                EffectLogicListViewTreeItem treeItem = item as EffectLogicListViewTreeItem;

                if (!bSearchValue && treeItem.m_class.sLogicName.Contains(search))
                {
                    return true;
                }
                else
                {
                    foreach (EffectLogicViewTreeItem property in treeItem.children)
                    {
                        if (bSearchValue && property.m_property.sValue.Contains(search))
                        {
                            return true;
                        }
                        else if (property.m_property.sName.Contains(search))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
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
                new MultiColumnHeaderState.Column()
            };
            retVal[0].headerContent = new GUIContent("属性名", "");
            retVal[0].minWidth = 60;
            retVal[0].width = 150;
            retVal[0].maxWidth = 400;
            retVal[0].headerTextAlignment = TextAlignment.Left;
            retVal[0].canSort = false;
            retVal[0].autoResize = true;

            retVal[1].headerContent = new GUIContent("类型", "");
            retVal[1].minWidth = 40;
            retVal[1].width = 50;
            retVal[1].maxWidth = 90;
            retVal[1].headerTextAlignment = TextAlignment.Left;
            retVal[1].canSort = false;
            retVal[1].autoResize = true;

            retVal[2].headerContent = new GUIContent("描述", "");
            retVal[2].minWidth = 150;
            retVal[2].width = 300;
            retVal[2].maxWidth = 600;
            retVal[2].headerTextAlignment = TextAlignment.Left;
            retVal[2].canSort = false;
            retVal[2].autoResize = true;

            retVal[3].headerContent = new GUIContent("属性值", "");
            retVal[3].minWidth = 50;
            retVal[3].width = 150;
            retVal[3].maxWidth = 300;
            retVal[3].headerTextAlignment = TextAlignment.Left;
            retVal[3].canSort = false;
            retVal[3].autoResize = true;
            
            return retVal;
        }

		public void AddNewClassData(string logicName, System.Reflection.FieldInfo[] _DataFields)
		{
			EffectLogicParamList list = new EffectLogicParamList();
			list.sLogicName = logicName;
			foreach (var field in _DataFields)
			{
				EffectLogicParamItem item = new EffectLogicParamItem();
				item.sName = field.Name;
				item.sType = field.GetType().ToString();
				item.sValue = "";
			}
		}

		public void RefreshByNewData(EffectLogicParamData data)
        {
            m_data = data;
            rootItem.children.Clear();

            Reload();
            Repaint();
        }

        private void Refresh()
        {

        }
        

    }

    public class EffectLogicListViewTreeItem : TreeViewItem
    {
        public EffectLogicParamList m_class;

        public bool IsFold;

        public EffectLogicListViewTreeItem(EffectLogicParamList _class, int depth) : base(_class.GetHashCode(), depth)
        {
            m_class = _class;
            foreach (EffectLogicParamItem item in _class)
            {
                AddChild(new EffectLogicViewTreeItem(item, depth + 1));
            }
        }
    }
    public class EffectLogicViewTreeItem : TreeViewItem
    {
        public EffectLogicParamItem m_property;

        public EffectLogicViewTreeItem(EffectLogicParamItem _property, int depth) : base(_property.GetHashCode(), depth)
        {
            m_property = _property;
        }
    }

}