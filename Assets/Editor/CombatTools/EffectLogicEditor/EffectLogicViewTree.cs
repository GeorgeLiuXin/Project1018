using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Extension;
using UnityEditor;
using Galaxy.XmlData;
using System.Reflection;

namespace Galaxy
{
    public class EffectLogicViewTree : TreeView
    {
        // All columns
        enum classColumns
        {
            Name,
            nameValue,
            Des,
            Btn,
        }
        enum propertyColumns
        {
            Name,
            Type,
            Des,
            Value,
        }

        private PerformanceLogicFactory factory;

        private GUIStyle style;
        public XmlDataList m_data;

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
            factory = new PerformanceLogicFactory();
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            root.children = new List<TreeViewItem>();
            if (m_data != null)
            {
                List<XmlClassData>.Enumerator itor = m_data.GetEnumerator();

                while (itor.MoveNext())
                {
                    XmlClassData _class = itor.Current;

                    TemplatePerformanceLogic logic = factory.GetTemplatePerformanceLogic(_class.sLogicName);
                    EffectLogicListViewTreeItem arrayItem = new EffectLogicListViewTreeItem(_class, 0, logic);
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
                    if (GUI.Button(args.rowRect, "  " + treeItem.m_class.sLogicName, style))
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

                Rect rect = args.rowRect;
                Rect rectClassBtn = new Rect(rect.x, rect.y, rect.width - 100, rect.height);
                Rect rectDeleteBtn = new Rect(rect.width - 60, rect.y, 100, rect.height);

                bool expended = IsExpanded(treeItem.id);
                string des = treeItem.displayName;
                PerformanceLogicDesAttribute attr = CombatToolHelper.GetAttribute<PerformanceLogicDesAttribute>(treeItem.m_templateLogic.GetType());
                if (attr != null)
                {
                    des = attr.Description;
                }
                if (GUI.Button(rectClassBtn, "  " + des, style))
                {
                    treeItem.IsFold = !treeItem.IsFold;
                    expended = !expended;
                    SetExpanded(treeItem.id, expended);
                }

                if (GUI.Button(rectDeleteBtn, "  " + "删除", style))
                {
                    RemoveClassData(treeItem.m_class.sLogicName);
                }

                GUI.color = oldColor;
            }
            else
            {
                EffectLogicViewTreeItem treeItem = args.item as EffectLogicViewTreeItem;
                for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
                    CellGUI(args.GetCellRect(i), treeItem, treeItem.m_property, args.GetColumn(i), ref args);
            }
        }

        private void CellGUI(Rect cellRect, EffectLogicViewTreeItem treeItem, XmlParamItem item, int column, ref RowGUIArgs args)
        {
            CenterRectUsingSingleLineHeight(ref cellRect);

            switch ((propertyColumns)column)
            {
                case propertyColumns.Name:
                    EditorGUI.LabelField(cellRect, item.sName);
                    break;
                case propertyColumns.Type:
                    EditorGUI.LabelField(cellRect, item.sType);
                    break;
                case propertyColumns.Des:
                    EditorGUI.LabelField(cellRect, treeItem.sTemplateParamDes);
                    break;
                case propertyColumns.Value:
                    if (item.sType == "System.Boolean")
                    {
                        bool.TryParse(item.sValue, out treeItem.bValue);
                        treeItem.bValue = EditorGUI.Toggle(cellRect, treeItem.bValue);
                        item.sValue = treeItem.bValue.ToString();
                    }
                    else if (item.sType == "System.Int32")
                    {
                        int.TryParse(item.sValue, out treeItem.nValue);
                        treeItem.nValue = EditorGUI.IntField(cellRect, treeItem.nValue);
                        item.sValue = treeItem.nValue.ToString();
                    }
                    else if (item.sType == "System.Single")
                    {
                        float.TryParse(item.sValue, out treeItem.fValue);
                        treeItem.fValue = EditorGUI.FloatField(cellRect, treeItem.fValue);
                        item.sValue = treeItem.fValue.ToString();
                    }
                    else if (item.sType == "System.String")
                    {
                        treeItem.sValue = item.sValue;
                        treeItem.sValue = EditorGUI.TextField(cellRect, treeItem.sValue);
                        item.sValue = treeItem.sValue;
                    }
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
            retVal[0].maxWidth = 300;
            retVal[0].headerTextAlignment = TextAlignment.Left;
            retVal[0].canSort = false;
            retVal[0].autoResize = true;

            retVal[1].headerContent = new GUIContent("类型", "");
            retVal[1].minWidth = 100;
            retVal[1].width = 110;
            retVal[1].maxWidth = 120;
            retVal[1].headerTextAlignment = TextAlignment.Left;
            retVal[1].canSort = false;
            retVal[1].autoResize = true;

            retVal[2].headerContent = new GUIContent("描述", "");
            retVal[2].minWidth = 240;
            retVal[2].width = 270;
            retVal[2].maxWidth = 320;
            retVal[2].headerTextAlignment = TextAlignment.Left;
            retVal[2].canSort = false;
            retVal[2].autoResize = true;

            retVal[3].headerContent = new GUIContent("属性值", "");
            retVal[3].minWidth = 120;
            retVal[3].width = 240;
            retVal[3].maxWidth = 500;
            retVal[3].headerTextAlignment = TextAlignment.Left;
            retVal[3].canSort = false;
            retVal[3].autoResize = true;

            return retVal;
        }


        public void RefreshByNewData(ref XmlDataList data)
        {
            m_data = data;
            Reload();
            Repaint();
        }

        private void Refresh()
        {

        }

        public void AddNewClassData(string logicName, System.Reflection.FieldInfo[] _DataFields)
        {
            XmlClassData list = new XmlClassData();
            list.sLogicName = logicName;
            foreach (var field in _DataFields)
            {
                XmlParamItem item = new XmlParamItem();
                item.sName = field.Name;
                item.sType = field.FieldType.ToString();
                item.sValue = "";
                list.Add(item);
            }
            m_data.SafeAdd(list);

            Reload();
            Repaint();
        }

        public void RemoveClassData(string logicName)
        {
            XmlClassData list = null;
            foreach (var item in m_data)
            {
                if (item.sLogicName.Equals(logicName))
                {
                    list = item;
                    break;
                }
            }
            if (list != null)
            {
                m_data.Remove(list);
            }
            Reload();
            Repaint();
        }

    }

    public class EffectLogicListViewTreeItem : TreeViewItem
    {
        public XmlClassData m_class;
        public TemplatePerformanceLogic m_templateLogic;

        public bool IsFold;

        public EffectLogicListViewTreeItem(XmlClassData _class, int depth, TemplatePerformanceLogic templateLogic) : base(_class.GetHashCode(), depth)
        {
            m_class = _class;

            m_templateLogic = templateLogic;

            IsFold = true;
            string des;
            MemberInfo info;
            foreach (XmlParamItem item in _class)
            {
                info = m_templateLogic.GetType().GetField(item.sName);
                if (info != null)
                {
                    PerformanceLogicItemDesAttribute attr = CombatToolHelper.GetAttribute<PerformanceLogicItemDesAttribute>(info);
                    if (attr != null)
                    {
                        des = attr.Description;
                    }
                    else
                    {
                        des = item.sName;
                    }
                }
                else
                {
                    des = item.sName;
                }
                AddChild(new EffectLogicViewTreeItem(item, depth + 1, des));
            }
        }
    }
    public class EffectLogicViewTreeItem : TreeViewItem
    {
        public string sTemplateParamDes;
        public XmlParamItem m_property;

        public bool bValue;
        public int nValue;
        public float fValue;
        public string sValue;

        public EffectLogicViewTreeItem(XmlParamItem _property, int depth, string paramDes) : base(_property.GetHashCode(), depth)
        {
            m_property = _property;

            sTemplateParamDes = paramDes;
        }
    }

}