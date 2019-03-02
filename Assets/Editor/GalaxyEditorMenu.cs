using UnityEngine;
using UnityEditor;
using Galaxy.AssetBundleBrowser;

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

        /*********************     AssetPipeline    ************************/
        [MenuItem("Galaxy Tools/AssetPipeline/AssetBundle监控面板", false, 50)]
        public static void ShowABMonitorMainWindow()
        {
            Galaxy.AssetPipeline.ABMonitorWindowMain.ShowWindow();
        }

        [MenuItem("Galaxy Tools/AssetPipeline/AssetBundle管理面板", false, 51)]
        public static void ShowAssetBundleWindow()
        {
            AssetBundleBrowserMain.instance.titleContent = new GUIContent("AssetBundles");
            AssetBundleBrowserMain.instance.Show();
        }

        [MenuItem("Galaxy Tools/AssetPipeline/Build AssetBundles Immediately", false, 52)]
        public static void BuildFromUI()
        {
            Galaxy.Builder.BuildFromUI();
        }

        [MenuItem("Galaxy Tools/AssetPipeline/Auto AssetBundle管理面板", false, 53)]
        public static void ShowAutoAssetBundleWindow()
        {
            Galaxy.AssetPipeline.GalaxyBundleBrowser.instance.Show();
        }

        /*********************     Checker    ************************/
        [MenuItem("Galaxy Tools/Checker/Check UI Label", false, 101)]
        public static void CheckUILabel()
        {
            UILabelChecker.CheckSceneSetting();
        }
        [MenuItem("Galaxy Tools/Checker/Check UI Prefab", false, 102)]
        public static void CheckUIPrefab()
        {
            UINormalizedTransformChecker.CheckSceneSetting();
        }

        /*********************     Record    ************************/
        [MenuItem("Galaxy Tools/Record/Delete All", false, 111)]
        public static void DeleteAllRecord()
        {
            LocalRecordEditor.DeleteAll();
        }

        [MenuItem("Galaxy Tools/Record/Delete Local Player Account", false, 112)]
        public static void DeleteLocalPlayerAccount()
        {
            LocalRecordEditor.DeleteLocalPlayerAccount();
        }

        [MenuItem("Galaxy Tools/Record/Delete Custom IP", false, 113)]
        public static void DeleteCustomIp()
        {
            LocalRecordEditor.DeleteCustomIp();
        }

        /*********************     Scenes    ************************/
        [MenuItem("Galaxy Tools/Scenes/Select Scene", false, 121)]
        public static void ShowQuickSelectScene()
        {
            QuickSelectScene.Show();
        }

        [MenuItem("Galaxy Tools/Scenes/Back To Launcher (Save)", false, 122)]
        public static void BackToLauncherWithSave()
        {
            QuickSelectScene.OpenLauncherScene(true);
        }

        [MenuItem("Galaxy Tools/Scenes/Back To Launcher (UnSave)", false, 123)]
        public static void BackToLauncherWithUnsave()
        {
            QuickSelectScene.OpenLauncherScene(false);
        }
        
        [MenuItem("Galaxy Tools/Scenes/Reset Shader", false, 10005)]
        public static void ResetSceneShader()
        {
            ShaderResetTools.ResetScene();
        }

        [MenuItem("Galaxy Tools/Scenes/设置BuildSetting - 配表", false, 100001)]
        public static void SceneBuildByCfg()
        {
            AssetPipeline.SceneBuilding builder = new AssetPipeline.SceneBuilding();
            builder.SetBuildSettingScenes(true);
        }

        [MenuItem("Galaxy Tools/Scenes/设置BuildSetting - 所有", false, 100002)]
        public static void SceneBuildAll()
        {
            AssetPipeline.SceneBuilding builder = new AssetPipeline.SceneBuilding();
            builder.SetBuildSettingScenes(false);
        }

        /*********************     Map    ************************/
        [MenuItem("Galaxy Tools/自定义场景工具", false, 130)]
        public static void ShowMapEditor()
        {
            MapEditor.ShowEditor();
        }

        /*********************     LevelEditor    ************************/
        [MenuItem("Galaxy Tools/关卡编辑器", false, 150)]
        public static void ShowLevelEditor()
        {
            LevelEditor.ShowWindow();
        }

        /*********************     Tool    ************************/
        [MenuItem("Galaxy Tools/Tools/属性监控面板", false, 10001)]
        public static void ShowParamView()
        {
            AvatarWindowEditor.OpenWindow();
        }

        [MenuItem("Galaxy Tools/Tools/动画融合", false, 10005)]
        public static void ShowAnimatorMergeView()
        {
            AnimatorMerge.ShowWindow();
        }

        [MenuItem("Galaxy Tools/Tools/拆分光照贴图预制体",false, 1007)]
        public static void ShowSplitLightingMapsTools()
        {
            Galaxy.LightMapEditor.GenLightmap();
        }

        [MenuItem("Galaxy Tools/Tools/生成合并后的光照贴图",false,1008)]
        public static void ShowCombinLightingMaps()
        {
            Galaxy.LightMapEditor.UpdateLightmaps();
        }

        [MenuItem("Galaxy Tools/Tools/生成卡牌预加载资源列表", false, 10006)]
        public static void ShowAnimatorRenameView()
        {
            CardResourceFinder.Generate();
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