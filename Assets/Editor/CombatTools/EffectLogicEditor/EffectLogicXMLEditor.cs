using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System;
using System.Linq;
using XWorld.XmlData;

namespace XWorld
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
                    XmlTitleBar(xmlTitleRect);
                    SearchBar(toolbarRect);
                    DoXmlTreeView(xmlRect);
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        TitleBar(titleRect);

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
                m_SearchField.downOrUpArrowKeyPressed += m_XmlTreeView.SetFocusAndEnsureSelectedItem;

                m_Initialized = true;
            }
        }

        Rect titleRect
        {
            get { return new Rect(300, 0, position.width - 340, 30); }
        }

        Rect treeViewRect
        {
            get { return new Rect(300, 25, position.width - 340, position.height - 60); }
        }

        Rect bottomToolbarRect
        {
            get { return new Rect(300, position.height - 27, position.width - 340, 30); }
        }

        Rect xmlTitleRect
        {
            get { return new Rect(10, 0, 270, 30); }
        }

        Rect toolbarRect
        {
            get { return new Rect(10, 25, 270, 30); }
        }

        Rect xmlRect
        {
            get { return new Rect(10, 50, 270, position.height - 75); }
        }

        void XmlTitleBar(Rect rect)
        {
            GUILayout.BeginArea(rect);
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("当前表现效果XML列表: ");
                if (GUILayout.Button("刷新", "miniButton"))
                {
                    RefreshDataType();
                    RefreshXmlView();
                }
            }
            GUILayout.EndArea();
        }

        void TitleBar(Rect rect)
        {
            GUILayout.BeginArea(rect);
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("当前表现效果包含了以下效果: ");
                if (GUILayout.Button("保存", GUILayout.Width(80)))
                {
                    SaveClassData();
                    UnityEditor.EditorUtility.DisplayDialog("保存成功", "战斗xml保存完成!", "OK");
                }
            }
            GUILayout.EndArea();
        }

        void SearchBar(Rect rect)
        {
            m_XmlTreeView.searchString = m_SearchField.OnGUI(rect, m_XmlTreeView.searchString);
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
        
        public void SetCurData(string sIndex)
        {
            if (sIndex.IsNE())
                return;

            int index = Convert.ToInt32(sIndex);
            SetCurData(index);
        }
        public void SetCurData(int index)
        {
            if (!m_DesToDictIndex.ContainsValue(index))
                return;
            
            if (!m_dataDict.ContainsKey(index))
                return;

            m_CurIndex = index;
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
            SetCurData(m_CurIndex);

            m_TreeView.Reload();
            m_TreeView.Repaint();
        }
        private void RefreshXMLViewTree()
        {
            m_XmlTreeView.RefreshXmlClassList(m_DesToDictIndex);

            m_XmlTreeView.Reload();
            m_XmlTreeView.Repaint();
        }

        private void AddXmlNode(string nodeInfo)
        {
            string[] strs = nodeInfo.Split('\t');
            string nodeId = strs[0];
            string nodeName = strs[1];

            int id = 0;
            if (!int.TryParse(nodeId, out id))
            {
                UnityEditor.EditorUtility.DisplayDialog("添加错误", "当前ID非数字!", "OK");
                return;
            }
            XmlDataList data = new XmlDataList();
            data.iIndex = id;
            data.sDescribe = nodeName;
            AddXmlNode(data);
        }
        private void AddXmlNode(XmlDataList data)
        {
            if (data.iIndex == 0 || data.sDescribe.IsNE())
            {
                UnityEditor.EditorUtility.DisplayDialog("添加错误", "当前战斗xml中已包含当前描述的节点!", "OK");
                return;
            }
            if (m_DesToDictIndex.ContainsValue(data.iIndex))
            {
                UnityEditor.EditorUtility.DisplayDialog("添加错误", "当前战斗xml中已包含当前ID的节点!", "OK");
                return;
            }
            if (m_DesToDictIndex.ContainsKey(data.sDescribe))
            {
                UnityEditor.EditorUtility.DisplayDialog("添加错误", "当前战斗xml中已包含当前描述的节点!", "OK");
                return;
            }

            reader.AddXml(data);
            RefreshXmlView();
        }

        private void DeleteXmlNode(string nodeIndex)
        {
            int id = Convert.ToInt32(nodeIndex);

            if (!m_DesToDictIndex.ContainsValue(id))
                return;

            if (!m_dataDict.ContainsKey(id))
                return;

            if (UnityEditor.EditorUtility.DisplayDialog("确认", "是否确认删除改节点?", "Y", "N"))
            {
                reader.DeleteXml(id.ToString());
            }
            
            RefreshXmlView();
        }

        #endregion

    }

}