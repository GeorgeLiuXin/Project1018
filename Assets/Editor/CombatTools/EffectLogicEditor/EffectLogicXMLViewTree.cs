using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using XWorld.XmlData;
using System;

namespace XWorld
{

    public class EffectLogicXMLViewTree : TreeView
    {
        private Dictionary<string, int> m_DesToDictIndex;
        private readonly string m_AddNodeDisplayName = "Add new Effect...";

        private bool m_IsAddState;

        private int m_XmlNodeID;
        private string m_XmlNodeName;

        public EffectLogicXMLViewTree(TreeViewState treeViewState, Dictionary<string, int> _dict) : base(treeViewState)
        {
            showAlternatingRowBackgrounds = true;
            showBorder = true;
            m_IsAddState = false;
            m_XmlNodeID = 0;
            m_XmlNodeName = "";
            m_DesToDictIndex = new Dictionary<string, int>(_dict);
            curDataIndex = -1;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };

            List<TreeViewItem> allItems = new List<TreeViewItem> { };

            foreach (KeyValuePair<string, int> item in m_DesToDictIndex)
            {
                string sDisplayName = "id:" + item.Value.ToString() + "\t" + item.Key;
                TreeViewItem node = new TreeViewItem { id = item.Value, depth = 0, displayName = sDisplayName };
                allItems.Add(node);
            }

            TreeViewItem nodeAdd = new TreeViewItem { depth = 0, displayName = m_AddNodeDisplayName };
            allItems.Add(nodeAdd);

            SetupParentsAndChildrenFromDepths(root, allItems);
            return root;
        }

        public override void OnGUI(Rect rect)
        {
            base.OnGUI(rect);

            TreeViewItem item = FindItem(state.lastClickedID, rootItem);
            if (item != null && curDataIndex != item.id)
            {
                SetCurString(item.id);
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            base.RowGUI(args);

            TreeViewItem node = args.item;

            if (node.displayName.Equals(m_AddNodeDisplayName))
            {
                MyRowGUI(args, node);
            }
            else
            {
                MyRowGUI(args, node.id);
            }
        }
        private void MyRowGUI(RowGUIArgs args, TreeViewItem node)
        {
            Color oldColor = GUI.color;
            if (!m_IsAddState)
            {
                if (GUI.Button(args.rowRect, node.displayName))
                {
                    m_IsAddState = !m_IsAddState;
                    Repaint();
                }
            }
            else
            {
                GUILayout.BeginArea(args.rowRect);
                using (new EditorGUILayout.HorizontalScope())
                {
                    m_XmlNodeID = EditorGUILayout.IntField(m_XmlNodeID, GUILayout.MinWidth(70), GUILayout.MaxWidth(70));
                    m_XmlNodeName = EditorGUILayout.TextField(m_XmlNodeName);
                    if (GUILayout.Button("添加", GUILayout.Width(50)))
                    {
                        if (m_XmlNodeID == 0)
                        {
                            UnityEditor.EditorUtility.DisplayDialog("添加错误", "当前ID非数字!", "OK");
                            return;
                        }
                        if (m_XmlNodeName.IsNE())
                        {
                            UnityEditor.EditorUtility.DisplayDialog("添加错误", "当前描述为空!", "OK");
                            return;
                        }

                        m_IsAddState = !m_IsAddState;
                        AddXmlNode();
                        Repaint();
                    }
                }
                GUILayout.EndArea();
            }
            GUI.color = oldColor;
        }
        private void MyRowGUI(RowGUIArgs args, int nodeIndex)
        {
            Rect rectDeleteBtn = new Rect(args.rowRect.width - 50, args.rowRect.y, 50, args.rowRect.height);
            if (GUI.Button(rectDeleteBtn, "删除"))
            {
                DeleteXmlNode(nodeIndex);
            }
        }

        private int curDataIndex;
        public delegate void EffectLogicXmlCurDataHandle(string curStr);

        public EffectLogicXmlCurDataHandle OnAdd;
        private void AddXmlNode()
        {
            OnAdd(m_XmlNodeID.ToString() + '\t' + m_XmlNodeName);
            m_XmlNodeID = 0;
            m_XmlNodeName = "";
        }

        public EffectLogicXmlCurDataHandle OnChange;
        private void SetCurString(int index)
        {
            curDataIndex = index;
            OnChange(index.ToString());
        }
        public EffectLogicXmlCurDataHandle OnDelete;
        private void DeleteXmlNode(int nodeIndex)
        {
            OnDelete(nodeIndex.ToString());
        }

        public void RefreshXmlClassList(Dictionary<string, int> _dict)
        {
            m_DesToDictIndex.Clear();
            m_DesToDictIndex = new Dictionary<string, int>(_dict);

            Reload();
            Repaint();
        }

    }

}