using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace Galaxy
{
    public class BuilderTest
    {
        public static void BuildTest()
        {
            string outputPath = "Bundle/" + Application.platform.ToString();
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            AssetBundleBuild[] assetBundleBuilds = AssetDatabase.GetAllAssetBundleNames().Select
                (
                    x => new AssetBundleBuild()
                    {
                        assetBundleName = x,
                        assetBundleVariant = "",
                        assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(x)
                    }
                ).ToArray();

            var assetBundleManifest = BuildPipeline.BuildAssetBundles(outputPath, assetBundleBuilds,
                BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

            if (assetBundleManifest != null)
            {
                Debug.Log("GetAllAssetBundles: " +
                    string.Join("\n", assetBundleManifest.GetAllAssetBundles()));
                Debug.Log("GetAllAssetBundlesWithVariant: " +
                    string.Join("\n", assetBundleManifest.GetAllAssetBundlesWithVariant()));

                var assetBundles = assetBundleManifest.GetAllAssetBundles();

                foreach (var assetBundle in assetBundles)
                {
                    string result = assetBundle + "\n";
                    result += "GetAllDependencies: " +
                        string.Join("\n", assetBundleManifest.GetAllDependencies(assetBundle)) + "\n";
                    result += "GetAssetBundleHash: " + assetBundleManifest.GetAssetBundleHash(assetBundle) + "\n";
                    result += "GetDirectDependencies: " +
                        string.Join("\n", assetBundleManifest.GetDirectDependencies(assetBundle)) + "\n";
                    Debug.Log(result);
                }
            }
        }

        static void GetNames()
        {
            var names = AssetDatabase.GetAllAssetBundleNames();
            foreach (var name in names)
            {
                Debug.Log("Asset Bundle: " + name + "\n\n" +
                    string.Join("\n", AssetDatabase.GetAssetPathsFromAssetBundle(name)));
            }
        }
    }
}
