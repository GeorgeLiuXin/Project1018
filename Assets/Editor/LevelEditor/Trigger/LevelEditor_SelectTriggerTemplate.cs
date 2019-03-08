using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Galaxy
{
    public class LevelEditor_SelectTriggerTemplate : EditorWindow
    {
        private Dictionary<string,string> m_lTemplate;
        private string LOAD_TEMP_PATH;

        public LevelEditorFrameWnd FrameWindow
        {
            get;
            set;
        }

        private void OnEnable()
        {
            LOAD_TEMP_PATH = Application.dataPath + "/AssetPrefabs/Trigger/";
            m_lTemplate = new Dictionary<string,string>();

            if(!Directory.Exists(LOAD_TEMP_PATH))
            {
                if (EditorUtility.DisplayDialog("警告", "Trigger版本路径丢失，请检查本地配置 " + LOAD_TEMP_PATH, "知道了"))
                    return;
            }

            foreach (string path in Directory.GetFiles(LOAD_TEMP_PATH))
            {
                if (path.Contains(".meta"))
                    continue;

                string[] arrSplit = path.Split('/');
                string tempName = arrSplit[arrSplit.Length - 1];

                string fullPath = "Assets/AssetPrefabs/Trigger/" + tempName;
                m_lTemplate.Add(tempName, fullPath);
            }
        }

        private void OnDisable()
        {
            m_lTemplate.Clear();
            m_lTemplate = null;
            FrameWindow.OnSelectEnd();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("请选择一个配置模板");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();

            DrawTriggerTemplateList();
        }

        private void OnSelectValue(string strName,string strPath)
        {
            GameObject prefab = UnityEditor.AssetDatabase.LoadMainAssetAtPath(strPath) as GameObject;
            if (prefab == null)
                return;

            GameObject ConfigGameObject = Instantiate(prefab);
            FrameWindow.OnCreateNewConfig(ConfigGameObject);
        }

        private void DrawTriggerTemplateList()
        {
            if (m_lTemplate == null || m_lTemplate.Count == 0)
                return;

            GUIStyle style = "Label";

            int index = 0;
            foreach(KeyValuePair<string,string> pairs in m_lTemplate)
            {
                GUIContent content = new GUIContent(index + "  " + pairs.Key);

                EditorGUILayout.BeginHorizontal();
                Rect rt = GUILayoutUtility.GetRect(content, style);
                if (GUI.Button(rt, content, style))
                {
                    //m_arrTrigger[i].index;
                    OnSelectValue(pairs.Key, pairs.Value);
                }
                EditorGUILayout.EndHorizontal();
                index++;
            }
        }
    }
}