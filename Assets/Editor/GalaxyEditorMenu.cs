using UnityEngine;
using UnityEditor;

/// <summary>
/// 将Editor自定义功能分类，统一放在目录Galaxy Tools
/// </summary>
namespace Galaxy
{
    public class GalaxyEditorMenu
    {
        /*********************     CSharp Tools    ************************/

        [MenuItem("Galaxy Tools/CSharp Tools/自动生成工具", false, 1)]
        public static void ShowWindow()
        {
            OneKeyBuild thisWindow = (OneKeyBuild)EditorWindow.GetWindow(typeof(OneKeyBuild));
            thisWindow.titleContent = new GUIContent("工具");
        }
        
        /*********************     Map    ************************/
        [MenuItem("Galaxy Tools/自定义场景工具", false, 130)]
        public static void ShowMapEditor()
        {
            //MapEditor.ShowEditor();
        }

        /*********************     LevelEditor    ************************/
        [MenuItem("Galaxy Tools/关卡编辑器", false, 150)]
        public static void ShowLevelEditor()
        {
            LevelEditor.ShowWindow();
        }
        
        /*********************     CombatTools    ************************/
        [MenuItem("Galaxy Tools/CombatTools/特效编辑器播放", false, 100)]
        public static void ShowEffectConfigEditor()
        {
            EffectConfigEditor.ShowEditor();
        }
        [MenuItem("Galaxy Tools/CombatTools/技能伤害数值监视", false, 100)]
        public static void ShowCombatCalculationEditor()
        {
            CombatCalculationEditor.ShowEditor();
        }
        [MenuItem("Galaxy Tools/CombatTools/客户端表现XML配置", false, 100)]
        public static void ShowEffectLogicXMLEditor()
        {
            EffectLogicXMLEditor.ShowEditor();
        }
    }
}