using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Galaxy.XmlData;
using System;

namespace Galaxy
{
    public class MapEditor : Editor
    {
        public static void ShowEditor()
        {
            MapEditorWindow window = EditorWindow.GetWindow<MapEditorWindow>();
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

    public class MapEditorWindow : EditorWindow
    {
        //XML数据获取
        private XmlReaderBase reader;

        //地图格子大小定义
        private Vector2Int mapSize;

        enum GridType
        {
            None,
            Land,
            Wall,
            Size
        }

        //对应地图


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
                TitleBar(titleRect);
            }
            manualUpdate();
            GUILayout.EndHorizontal();
        }

        private void InitIfNeeded()
        {
            throw new NotImplementedException();
        }

        void inspectorUpdate()
        {
            throw new NotImplementedException();
        }

        void manualUpdate()
        {
            throw new NotImplementedException();
        }

        Rect titleRect
        {
            get { return new Rect(300, 0, position.width - 340, 30); }
        }

        void TitleBar(Rect rect)
        {
            GUILayout.BeginArea(rect);
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("当前表现效果包含了以下效果: ");
                if (GUILayout.Button("保存", GUILayout.Width(80)))
                {
                    UnityEditor.EditorUtility.DisplayDialog("保存成功", "战斗xml保存完成!", "OK");
                }
            }
            GUILayout.EndArea();
        }
    }

}
