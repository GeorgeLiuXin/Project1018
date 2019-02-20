using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Galaxy
{

    public class EffectLogicXMLViewTree : TreeView
    {
        private Dictionary<string, int> m_DesToDictIndex;
        private readonly string m_AddNodeDisplayName = "Add new Effect...";

        private bool m_IsAddState;
        private string m_XmlNodeName;

        public EffectLogicXMLViewTree(TreeViewState treeViewState, Dictionary<string, int> _dict) : base(treeViewState)
        {
            showAlternatingRowBackgrounds = true;
            showBorder = true;
            m_IsAddState = false;
            m_XmlNodeName = "Enter new Xml name...";
            m_DesToDictIndex = new Dictionary<string, int>(_dict);
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };

            List<TreeViewItem> allItems = new List<TreeViewItem> { };

            foreach (KeyValuePair<string, int> item in m_DesToDictIndex)
            {
                TreeViewItem node = new TreeViewItem { id = item.Value, depth = 0, displayName = item.Key };
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
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            base.RowGUI(args);

            TreeViewItem node = args.item;
            if (node.displayName.Equals(m_AddNodeDisplayName))
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
                        m_XmlNodeName = GUILayout.TextField(m_XmlNodeName);
                        if (GUILayout.Button("添加"))
                        {
                            m_IsAddState = !m_IsAddState;
                            AddEffectLogicParamDataToXml();
                            Repaint();
                        }
                    }
                    GUILayout.EndArea();
                }
                GUI.color = oldColor;
            }
        }

        private void AddEffectLogicParamDataToXml()
        {

            m_XmlNodeName = "Enter new Xml name...";
        }
    }
    
}