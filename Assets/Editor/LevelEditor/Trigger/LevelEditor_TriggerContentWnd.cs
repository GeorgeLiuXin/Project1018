using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public class LevelEditor_TriggerContentWnd : EditorWindow
    {
        private bool m_bNeedUpdate = false;
        private List<TriggerFlag> m_arrTrigger = new List<TriggerFlag>();

        public LevelEditorFrameWnd FrameWindow
        {
            get;
            set;
        }

        private void OnEnable()
        {
            if(m_arrTrigger.Count == 0)
            {
                TriggerFlag[] arrCom = GameObject.FindObjectsOfType<TriggerFlag>();
                if (arrCom == null || arrCom.Length == 0)
                    return;

                m_arrTrigger.AddRange(arrCom);
                m_arrTrigger.Sort(CompareTrigger);
            }
        }

        private int CompareTrigger(TriggerFlag a1, TriggerFlag a2)
        {
            if (a1.index > a2.index)
                return -1;

            if (a1.index < a2.index)
                return 1;

            return 0;
        }

        private void OnDisable()
        {
            m_bNeedUpdate = false;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("请选择一个触发内容");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("添加一个触发内容"))
            {
                if(Selection.activeObject != null)
                {
                    GameObject select = Selection.activeObject as GameObject;
                    if(select.GetComponent<TriggerFlag>() ==null)
                    {
                        TriggerFlag newCom = select.AddComponent<TriggerFlag>();
                        newCom.index = m_arrTrigger.Count;
                        m_arrTrigger.Add(newCom);
                        m_bNeedUpdate = true;
                    }
                }
            }
            if(GUILayout.Button("删除一个触发内容"))
            {
                if (Selection.activeObject != null)
                {
                    GameObject select = Selection.activeObject as GameObject;
                    TriggerFlag deleteCom = select.GetComponent<TriggerFlag>();
                    if (deleteCom != null)
                    {
                        GameObject.DestroyImmediate(deleteCom);
                        m_arrTrigger.Remove(deleteCom);
                        m_bNeedUpdate = true;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();

            DrawCollisionGroupList();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("关闭当前界面"))
            {
                this.Close();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawCollisionGroupList()
        {
            if (m_arrTrigger == null || m_arrTrigger.Count == 0)
                return;

            string strBaseContent = "当前触发内容列表 ";
            GUIStyle style = "Label";

            if(m_bNeedUpdate)
            {
                m_arrTrigger.Sort(CompareTrigger);
                m_bNeedUpdate = false;
            }

            for (int i = 0; i < m_arrTrigger.Count; ++i)
            {
                GUIContent content = new GUIContent(i + " " + m_arrTrigger[i].gameObject.name);

                EditorGUILayout.BeginHorizontal();
                Rect rt = GUILayoutUtility.GetRect(content, style);
                if (GUI.Button(rt, content, style))
                {
                    //m_arrTrigger[i].index;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}