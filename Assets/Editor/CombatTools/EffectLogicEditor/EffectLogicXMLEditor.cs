using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System;

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

    public class EffectLogicXMLEditorWindow : EditorWindow
    {
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
                        if (GUILayout.Button("刷新类型", "miniButton"))
                        {
                            refresh();
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

        void InitIfNeeded()
        {
            if (!m_Initialized)
            {
                //temp
                m_DesToDictIndex = new Dictionary<string, int>();
                m_DesToDictIndex.Add("test1", 1);
                m_DesToDictIndex.Add("test2", 2);
                m_CurData = new EffectLogicParamData();
                EffectLogicParamList list1 = new EffectLogicParamList();
                EffectLogicParamItem item1 = new EffectLogicParamItem();
                item1.sName = "test1";
                item1.sType = "int32";
                item1.sValue = "0";
                EffectLogicParamItem item2 = new EffectLogicParamItem();
                item2.sName = "test2";
                item2.sType = "f32";
                item2.sValue = "0.5";
                EffectLogicParamItem item3 = new EffectLogicParamItem();
                item3.sName = "test3";
                item3.sType = "char";
                item3.sValue = "hahahahah";
                list1.sLogicName = "class1";
                list1.Add(item1);
                list1.Add(item2);
                list1.Add(item3);

                EffectLogicParamList list2 = new EffectLogicParamList();
                EffectLogicParamItem item4 = new EffectLogicParamItem();
                item4.sName = "test4";
                item4.sType = "bool";
                item4.sValue = "true";
                EffectLogicParamItem item5 = new EffectLogicParamItem();
                item5.sName = "test5";
                item5.sType = "int32";
                item5.sValue = "1";
                list2.sLogicName = "class2";
                list2.Add(item1);
                list2.Add(item4);
                list2.Add(item5);
                m_CurData.Add(list1);
                m_CurData.Add(list2);

                // Check if it already exists (deserialized from window layout file or scriptable object)
                if (m_XmlTreeViewState == null)
                    m_XmlTreeViewState = new TreeViewState();

                m_XmlTreeView = new EffectLogicXMLViewTree(m_XmlTreeViewState, m_DesToDictIndex);

                if (m_TreeViewState == null)
                    m_TreeViewState = new TreeViewState();

                m_TreeView = new EffectLogicViewTree(m_TreeViewState, EffectLogicViewTree.CreateDefaultMultiColumnHeaderState(), m_CurData);
                
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
                if (GUILayout.Button("保存", "miniButton"))
                {
                    save();
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

                if (GUILayout.Button("Add ...", style))
                {
                    add();
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

        protected System.Reflection.FieldInfo[] m_DataFields = null;

        private Dictionary<string, PerformanceLogic> m_ClassNameToLogic;

        //描述转换为表现效果对应id
        private Dictionary<string, int> m_DesToDictIndex;


        private EffectLogicParamData m_CurData;
        
        private void add()
        {

        }

        private void save()
        {

        }

        private void refresh()
        {

        }
        
        /// <summary>
        /// 非预览播放状态下，通过滑杆来播放当前动画帧
        /// </summary>
        private void manualUpdate()
        {
            if (EditorApplication.isPlaying)
            {
                return;
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

    }

}