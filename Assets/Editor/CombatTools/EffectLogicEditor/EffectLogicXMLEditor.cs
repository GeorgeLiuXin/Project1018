using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System;
using Extension;
using System.Linq;
using Galaxy.XmlData;

namespace Galaxy
{
    public class EffectLogicXMLEditor : Editor
    {
        public static void ShowEditor()
        {
            EffectLogicXMLEditorWindow window = EditorWindow.GetWindow<EffectLogicXMLEditorWindow>();
            if (window)
            {
                window.ShowUtility();
                var position = window.position;
                position.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
                window.position = position;
                window.Show();
            }
        }
    }

    /// <summary>
    /// 仅在非游戏模式下使用
    /// </summary>
    public class EffectLogicXMLEditorWindow : EditorWindow
    {
        #region Xml数据

        public EffectLogicReader reader;
        private PerformanceLogicFactory factory;

        #endregion

        [NonSerialized]
        bool m_Initialized;
        [SerializeField]
        TreeViewState m_TreeViewState;
        SearchField m_SearchField;
        EffectLogicViewTree m_TreeView;

        [SerializeField]
        TreeViewState m_XmlTreeViewState;
        EffectLogicXMLViewTree m_XmlTreeView;

        private void InitObject()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }
        }

        private void OnEnable()
        {
            EditorApplication.update += inspectorUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= inspectorUpdate;
        }

        private void OnGUI()
        {
            InitIfNeeded();

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("当前表现效果XML列表: ");
                        if (GUILayout.Button("刷新", "miniButton"))
                        {
                            RefreshDataType();
                            RefreshXmlView();
                        }
                    }
                    DoXmlTreeView(xmlRect);
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        titleBar(titleRect);
                        SearchBar(toolbarRect);

                        //tree 生成与初始化
                        DoTreeView(treeViewRect);

                        BottomToolBar(bottomToolbarRect);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            manualUpdate();
            GUILayout.EndHorizontal();
        }

        void InitData()
        {
            reader = new EffectLogicReader();
            m_dataDict = new Dictionary<int, XmlDataList>();
            m_DesToDictIndex = new Dictionary<string, int>();
            factory = new PerformanceLogicFactory();
            ReadXmlData();

            m_AddClassIndex = 0;
            m_CurIndex = 0;
            m_CurDataDes = "";
            m_ClassName = factory.m_dict.Keys.ToArray();

            bNeedToExpand = false;
        }

        void InitIfNeeded()
        {
            if (!m_Initialized)
            {
                InitData();

                // Check if it already exists (deserialized from window layout file or scriptable object)
                if (m_XmlTreeViewState == null)
                    m_XmlTreeViewState = new TreeViewState();

                m_XmlTreeView = new EffectLogicXMLViewTree(m_XmlTreeViewState, m_DesToDictIndex);
                InitHandle();

                if (m_TreeViewState == null)
                    m_TreeViewState = new TreeViewState();

                m_TreeView = new EffectLogicViewTree(m_TreeViewState, EffectLogicViewTree.CreateDefaultMultiColumnHeaderState());
                
                m_SearchField = new SearchField();
                m_SearchField.downOrUpArrowKeyPressed += m_TreeView.SetFocusAndEnsureSelectedItem;

                m_Initialized = true;
            }
        }

        Rect titleRect
        {
            get { return new Rect(300, 0, position.width - 340, 30); }
        }

        Rect toolbarRect
        {
            get { return new Rect(300, 30, position.width - 340, 30); }
        }

        Rect treeViewRect
        {
            get { return new Rect(300, 50, position.width - 340, position.height - 80); }
        }

        Rect bottomToolbarRect
        {
            get { return new Rect(300, position.height - 27, position.width - 340, 30); }
        }

        void titleBar(Rect rect)
        {
            GUILayout.BeginArea(rect);
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("当前表现效果包含了以下效果: ");
                if (GUILayout.Button("保存", GUILayout.Width(80)))
                {
                    SaveClassData();
                }
            }
            GUILayout.EndArea();
        }

        void SearchBar(Rect rect)
        {
            m_TreeView.searchString = m_SearchField.OnGUI(rect, m_TreeView.searchString);
        }

        void DoTreeView(Rect rect)
        {
            m_TreeView.OnGUI(rect);
        }

        void BottomToolBar(Rect rect)
        {
            GUILayout.BeginArea(rect);

            using (new EditorGUILayout.HorizontalScope())
            {
                var style = "miniButton";
                if (GUILayout.Button("Expand All", style))
                {
                    m_TreeView.ExpandAll();
                }

                if (GUILayout.Button("Collapse All", style))
                {
                    m_TreeView.CollapseAll();
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    m_AddClassIndex = EditorGUILayout.Popup(m_AddClassIndex, m_ClassName);
                    if (GUILayout.Button("添加", GUILayout.Width(80)))
                    {
                        AddXmlClassData();
                        Repaint();
                    }
                }
            }

            GUILayout.EndArea();
        }

        
        Rect xmlRect
        {
            get { return new Rect(10, 30, 270, position.height - 40); }
        }

        void DoXmlTreeView(Rect rect)
        {
            m_XmlTreeView.OnGUI(rect);
        }

        #region 具体数据逻辑

        private XmlDataList m_CurData;


        private bool bNeedToExpand;

        private void manualUpdate()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }

            if (bNeedToExpand)
            {
                m_TreeView.ExpandAll();
                bNeedToExpand = !bNeedToExpand;
            }
        }

        private void inspectorUpdate()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }

        }
        
        #endregion

        #region Handle

        public void InitHandle()
        {
            m_XmlTreeView.OnAdd += AddXmlNode;
            m_XmlTreeView.OnChange += SetCurData;
            m_XmlTreeView.OnDelete += DeleteXmlNode;
        }

        #endregion

        #region CurData

        private int m_CurIndex;
        private string m_CurDataDes;
        
        public void SetCurData(string curStr)
        {
            if (curStr.IsNE())
                return;

            if (!m_DesToDictIndex.ContainsKey(curStr))
                return;

            int index = m_DesToDictIndex[curStr];
            if (!m_dataDict.ContainsKey(index))
                return;

            m_CurIndex = index;
            m_CurDataDes = curStr;

            m_CurData = m_dataDict[m_CurIndex];
            m_TreeView.RefreshByNewData(ref m_CurData);

            bNeedToExpand = true;
        }

        #endregion

        #region 当前可被创建使用的数据基类

        protected System.Reflection.FieldInfo[] m_DataFields = null;

        private string[] m_ClassName;

        private void RefreshDataType()
        {
            m_ClassName = factory.m_dict.Keys.ToArray();
        }

        private int m_AddClassIndex = 0;
        private void AddXmlClassData()
        {
            PerformanceLogic logic = factory.GetPerformanceLogic(m_AddClassIndex);
            m_DataFields = logic.GetType().GetFields();
            m_TreeView.AddNewClassData(logic.GetType().Name, m_DataFields);
            Repaint();
        }

        private void SaveClassData()
        {
            reader.UpdateXml(m_dataDict, true);
            RefreshXmlView();
        }

        #endregion

        #region XmlView 当前Xml包含数据

        //描述转换为表现效果对应id
        private Dictionary<string, int> m_DesToDictIndex;
        private Dictionary<int, XmlDataList> m_dataDict;
        
        /// <summary>
        /// 读取Xml中所有的数据
        /// </summary>
        private void ReadXmlData()
        {
            m_dataDict.Clear();
            reader.ReadXml(ref m_dataDict);
            m_DesToDictIndex.Clear();
            foreach (var item in m_dataDict)
            {
                m_DesToDictIndex.Add(item.Value.sDescribe, item.Key);
            }
        }

        private void RefreshXmlView()
        {
            ReadXmlData();

            RefreshLogicViewTree();
            RefreshXMLViewTree();

            Repaint();
        }
        private void RefreshLogicViewTree()
        {
            SetCurData(m_CurDataDes);

            m_TreeView.Reload();
            m_TreeView.Repaint();
        }
        private void RefreshXMLViewTree()
        {
            m_XmlTreeView.RefreshXmlClassList(m_DesToDictIndex);

            m_XmlTreeView.Reload();
            m_XmlTreeView.Repaint();
        }
        
        private void AddXmlNode(string nodeName)
        {
            XmlDataList data = new XmlDataList();
            data.sDescribe = nodeName;
            AddXmlNode(data);
        }
        private void AddXmlNode(XmlDataList data)
        {
            reader.AddXml(data);
            RefreshXmlView();
        }

        private void DeleteXmlNode(string nodeName)
        {
            if (!m_DesToDictIndex.ContainsKey(nodeName))
                return;

            int id = m_DesToDictIndex[nodeName];
            if (!m_dataDict.ContainsKey(id))
                return;

            reader.DeleteXml(id.ToString());
            
            RefreshXmlView();
        }

        #endregion

    }

}