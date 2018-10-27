using UnityEngine;
using UnityEditor;

public class EditorStyleViewer : EditorWindow
{
    private Vector2 scrollPosition = Vector2.zero;
    private string search = string.Empty;

    [MenuItem("Window样式/GUI样式查看器")]
    public static void Init()
    {
        EditorWindow.GetWindow(typeof(EditorStyleViewer));
    }
    int mode = 1;
    void OnGUI()
    {
        GUILayout.BeginHorizontal("HelpBox");
        GUILayout.Label("单击示例将复制其名到剪贴板", "label");
        GUILayout.FlexibleSpace();
        GUILayout.Label("查找:");
        search = EditorGUILayout.TextField(search);
        GUILayout.EndHorizontal();

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        
        string[] labels = new string[4] { "peizhi",
                "打包", "视图", "帮助" };
        mode = GUILayout.Toolbar(mode, labels, "LargeButton", GUILayout.Width(400f));
        foreach (GUIStyle style in GUI.skin)
        {
            if (style.name.ToLower().Contains(search.ToLower()))
            {
                GUILayout.BeginHorizontal("PopupCurveSwatchBackground");
                GUILayout.Space(7);
                if (GUILayout.Button("配置", style))
                {
                    EditorGUIUtility.systemCopyBuffer = "\"" + style.name + "\"";
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.SelectableLabel("\"" + style.name + "\"");
                GUILayout.EndHorizontal();
                GUILayout.Space(11);
            }
        }

        GUILayout.EndScrollView();
    }
}