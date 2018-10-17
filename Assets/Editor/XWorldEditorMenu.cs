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

        ///*********************     AssetPipeline    ************************/

        //[MenuItem("XWorld Tools/AssetPipeline/AssetBundle管理面板", false, 51)]
        //public static void ShowAssetBundleWindow()
        //{
        //    AssetBundleBrowserMain.instance.titleContent = new GUIContent("AssetBundles");
        //    AssetBundleBrowserMain.instance.Show();
        //}

        //[MenuItem("XWorld Tools/AssetPipeline/Build AssetBundles Immediately", false, 52)]
        //public static void BuildFromUI()
        //{
        //    XWorld.Builder.BuildFromUI();
        //}

        //[MenuItem("XWorld Tools/AssetPipeline/Auto AssetBundle管理面板", false, 53)]
        //public static void ShowAutoAssetBundleWindow()
        //{
        //    XWorld.AssetPipeline.XWorldBundleBrowser.instance.Show();
        //}

        ///*********************     Checker    ************************/
        //[MenuItem("XWorld Tools/Checker/Check UI Label", false, 101)]
        //public static void CheckUILabel()
        //{
        //    UILabelChecker.CheckSceneSetting();
        //}
        //[MenuItem("XWorld Tools/Checker/Check UI Prefab", false, 102)]
        //public static void CheckUIPrefab()
        //{
        //    UINormalizedTransformChecker.CheckSceneSetting();
        //}
        
        ///*********************     Record    ************************/
        //[MenuItem("XWorld Tools/Record/Delete All", false, 111)]
        //public static void DeleteAllRecord()
        //{
        //    LocalRecordEditor.DeleteAll();
        //}

        //[MenuItem("XWorld Tools/Record/Delete Local Player Account", false, 112)]
        //public static void DeleteLocalPlayerAccount()
        //{
        //    LocalRecordEditor.DeleteLocalPlayerAccount();
        //}

        //[MenuItem("XWorld Tools/Record/Delete Custom IP", false, 113)]
        //public static void DeleteCustomIp()
        //{
        //    LocalRecordEditor.DeleteCustomIp();
        //}
        
        ///*********************     Scenes    ************************/
        //[MenuItem("XWorld Tools/Scenes/Select Scene", false, 121)]
        //public static void ShowQuickSelectScene()
        //{
        //    QuickSelectScene.Show();
        //}

        ///*********************     Map    ************************/
        //[MenuItem("XWorld Tools/自定义场景工具",false,130)]
        //public static void ShowMapEditor()
        //{
        //    MapEditor.ShowEditor();
        //}
    }
}
