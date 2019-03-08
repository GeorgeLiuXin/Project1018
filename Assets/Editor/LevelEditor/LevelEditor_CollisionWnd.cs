using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public class LevelEditor_CollisionWnd : EditorWindow
    {
        private int m_selectedGroupID = -1;
        private List<int> m_lCollisionGroup = new List<int>();
        public LevelEditorFrameWnd FrameWindow
        {
            get;
            set;
        }

        private void OnEnable()
        {
            m_selectedGroupID = -1;
        }

        private void OnDisable()
        {
            m_lCollisionGroup.Clear();
            FrameWindow = null;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("请选择一个碰撞组");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();

            DrawCollisionGroupList();
           
        }

        public void CopyData(ref Dictionary<int,List<CollisionData>> collisionData)
        {
            m_lCollisionGroup.Clear();
            foreach (KeyValuePair<int,List<CollisionData>> item in collisionData)
            {
                if (item.Value == null)
                    continue;

                m_lCollisionGroup.Add(item.Key);
            }
        }

        private void DrawSelectButton()
        {
            //if (GUILayout.Button("确定"))
            {
                if (m_selectedGroupID != -1)
                {
                    FrameWindow.SelectCollisionGroup(m_selectedGroupID);
                    this.Close();
                }
            }
        }

        private void DrawCollisionGroupList()
        {
            if (m_lCollisionGroup == null || m_lCollisionGroup.Count == 0)
                return;

            string strBaseContent = "碰撞组 ";
            GUIStyle style = "Label";
            
            for (int i = 0; i < m_lCollisionGroup.Count; ++i)
            {
                GUIContent content = new GUIContent(strBaseContent + m_lCollisionGroup[i].ToString());

                EditorGUILayout.BeginHorizontal();
                Rect rt = GUILayoutUtility.GetRect(content, style);
                if (GUI.Button(rt, content, style))
                {
                    m_selectedGroupID = m_lCollisionGroup[i];
                    DrawSelectButton();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}