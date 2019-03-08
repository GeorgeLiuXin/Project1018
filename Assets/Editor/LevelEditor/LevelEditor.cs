using System.Collections.Generic;

namespace Galaxy
{
    public class LevelEditor : UnityEditor.Editor
    {
        private static string LEVEL_CONFIG_PATH = UnityEngine.Application.dataPath + "../../Server/bin32/Data/DataTable";
      
        public static void ShowWindow()
        {
            LevelEditorFrameWnd frameWnd = UnityEditor.EditorWindow.GetWindow<LevelEditorFrameWnd>();
            frameWnd.minSize = new UnityEngine.Vector2(600f, 300f);
            frameWnd.maxSize = frameWnd.minSize;
            frameWnd.wantsMouseMove = true;
            frameWnd.autoRepaintOnSceneChange = true;
            frameWnd.titleContent.text = "关卡编辑器";
            UnityEngine.Rect wndPos = frameWnd.position;
            wndPos.center = new UnityEngine.Rect(
                    0f, 0f, UnityEngine.Screen.currentResolution.width, UnityEngine.Screen.currentResolution.height).center;

            frameWnd.position = wndPos;
            frameWnd.Show();
        }
    }
}
