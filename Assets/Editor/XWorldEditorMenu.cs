using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// 将Editor自定义功能分类，统一放在目录XWorld Tools
/// </summary>
namespace XWorld
{
    public class XWorldEditorMenu
    {
        /*********************     CSharp Tools    ************************/

        [MenuItem("XWorld Tools/CSharp Tools/自动生成工具", false, 1)]
        public static void ShowCSharpWindow()
        {
            OneKeyBuild thisWindow = (OneKeyBuild)EditorWindow.GetWindow(typeof(OneKeyBuild));
            thisWindow.titleContent = new GUIContent("工具");
        }

		/*********************     Map Tools    ************************/

		[MenuItem("XWorld Tools/LevelEditor/关卡编辑器", false, 1)]
		public static void ShowLevelEditorWindow()
		{

		}

		[MenuItem("XWorld Tools/LevelEditor/地形编辑器", false, 2)]
		public static void ShowMapEditorWindow()
		{

		}

		[MenuItem("XWorld Tools/LevelEditor/寻路工具", false, 3)]
		public static void ShowNavGridToolWindow()
		{

		}

		/*********************     CombatTools    ************************/
		[MenuItem("XWorld Tools/CombatTools/特效编辑器播放", false, 100)]
		public static void ShowEffectConfigEditor()
		{
			EffectConfigEditor.ShowEditor();
		}
		[MenuItem("XWorld Tools/CombatTools/技能伤害数值监视", false, 100)]
		public static void ShowCombatCalculationEditor()
		{
			CombatCalculationEditor.ShowEditor();
		}
		[MenuItem("XWorld Tools/CombatTools/客户端表现XML配置", false, 100)]
		public static void ShowEffectLogicXMLEditor()
		{
			EffectLogicXMLEditor.ShowEditor();
		}

		///*********************     Data    ************************/

		//[MenuItem("XWorld Tools/SystemData/SystemData管理面板", false, 51)]
		//public static void ShowAssetBundleWindow()
		//{
		//	EffectLogicXMLEditor.ShowEditor();
		//}

	}
}
