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
        public static void ShowWindow()
        {
            OneKeyBuild thisWindow = (OneKeyBuild)EditorWindow.GetWindow(typeof(OneKeyBuild));
            thisWindow.titleContent = new GUIContent("工具");
        }

		///*********************     Data    ************************/

		[MenuItem("XWorld Tools/SystemData/SystemData管理面板", false, 51)]
		public static void ShowAssetBundleWindow()
		{
			EffectLogicXMLEditor.ShowEditor();
		}

	}
}
