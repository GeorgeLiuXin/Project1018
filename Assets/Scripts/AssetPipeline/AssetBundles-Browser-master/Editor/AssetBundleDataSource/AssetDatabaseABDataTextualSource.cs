using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using System.IO;
using LitJson;
using Galaxy.AssetBundleBrowser;
using Galaxy;

namespace Galaxy.AssetBundleBrowser.AssetBundleDataSource
{
    internal class AssetDatabaseABDataTextualSource : ABDataSource
    {
        public static TextualSourcesLoad m_loader;

        public static List<ABDataSource> CreateDataSources()
        {
            var op = new AssetDatabaseABDataTextualSource();
            var retList = new List<ABDataSource>();
            retList.Add(op);
            Init();
            return retList;
        }

        public static void Init()
        {
            m_loader = new TextualSourcesLoad();
            m_loader.OnEnable();
        }

        public static void Over()
        {
            if (m_loader != null)
            {
                m_loader.OnDisable();
            }
        }

        public string Name
        {
            get
            {
                return "Default";
            }
        }

        public string ProviderName
        {
            get
            {
                return "Built-in";
            }
        }
        
        public string GetAssetBundleName(string assetPath)
        {
            return m_loader.m_buildAssetBundleMap.GetAssetBundleName(assetPath);
            //var importer = AssetImporter.GetAtPath(assetPath);
            //if (importer == null)
            //{
            //    return string.Empty;
            //}
            //var bundleName = importer.assetBundleName;
            //if (importer.assetBundleVariant.Length > 0)
            //{
            //    bundleName = bundleName + "." + importer.assetBundleVariant;
            //}
            //return bundleName;
        }

        public string GetImplicitAssetBundleName(string assetPath)
        {
            // 暂时不能理解
            return m_loader.m_buildAssetBundleMap.GetAssetBundleName(assetPath);
            //return AssetDatabase.GetImplicitAssetBundleName(assetPath);
        }

        public string[] GetAllAssetBundleNames()
        {
            return m_loader.m_buildAssetBundleMap.GetAllAssetBundleNames();
            //return AssetDatabase.GetAllAssetBundleNames();
        }

        public bool IsReadOnly()
        {
            return false;
        }

        public void SetAssetBundleNameAndVariant(string assetPath, string bundleName, string variantName)
        {
            m_loader.m_buildAssetBundleMap.SetAssetBundleNameAndVariant(assetPath, bundleName, variantName);
        }

        public void RemoveUnusedAssetBundleNames()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }

        public string[] GetAssetPathsFromAssetBundle(string assetBundleName) {
           return m_loader.m_buildAssetBundleMap.GetAssetPathsFromAssetBundle(assetBundleName);
        }

        public bool CanSpecifyBuildTarget
        {
            get { return true; }
        }
        public bool CanSpecifyBuildOutputDirectory
        {
            get { return true; }
        }

        public bool CanSpecifyBuildOptions
        {
            get { return true; }
        }

        public bool BuildAssetBundles(ABBuildInfo info)
        {
            Builder.outputPath = info.outputDirectory;
            Builder.options = info.options;
            Builder.target = info.buildTarget;
            AssetBundleBuild[] builds = m_loader.m_buildAssetBundleMap.GetBuilders();
            Builder.Build(builds);
        
            return true;
        }
    }
}

public static class AssetBundleManifestExtensions
{
    public static AssetBundleMap ToMap(this AssetBundleManifest assetBundleManifest, string outputDirectory)
    {
        var assetBundleMap = new AssetBundleMap();

        assetBundleMap.allAssetBundles = assetBundleManifest.GetAllAssetBundles();
        //  assetBundleMap.allAssetBundlesWithVariant = assetBundleManifest.GetAllAssetBundlesWithVariant();

        var list = new List<AssetBundleMap.AssetBundleItem>();
        foreach (var assetBundlePath in assetBundleManifest.GetAllAssetBundles())
        {
            var item = new AssetBundleMap.AssetBundleItem();
            string assetBundlePathWithoutHash = assetBundlePath;
            //#pragma warning disable 0429
            //                string assetBundlePathWithoutHash = AssetBundleUtility.GetAssetBundlePathWithoutHash(assetBundlePath,
            //                    GlobalAssetSetting.EnableAppendHashToAssetBundlePath);
            //#pragma warning restore 0429

            item.assetBundlePath = assetBundlePath;
            //item.assetBundleVariant = Utility.GetVariant(assetBundlePath);
            item.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundlePathWithoutHash);

            item.allDependencies = assetBundleManifest.GetAllDependencies(item.assetBundlePath);
            item.directDependencies = assetBundleManifest.GetDirectDependencies(item.assetBundlePath);
            item.assetBundleHash = assetBundleManifest.GetAssetBundleHash(item.assetBundlePath).ToString();

            //Set CRC
            string assetBundleManifestFullPath = Path.GetFullPath(outputDirectory + "/" + assetBundlePathWithoutHash + ".manifest");
            var manifest = BundleChecker.Deserialize(assetBundleManifestFullPath);
            item.CRC = manifest.CRC;

            // Set MD5
            string assetBundleFullPath = Path.GetFullPath(outputDirectory + "/" + assetBundlePathWithoutHash);
            string md5 = BundleChecker.GetFileMD5(assetBundleFullPath);

            item.md5 = md5;

            // Set file length
            var fi = new FileInfo(assetBundleFullPath);
            item.length = (int)fi.Length;

            list.Add(item);
        }
        assetBundleMap.assetBundleBuilds = list.ToArray();

        return assetBundleMap;
    }

    public static void ToJson(this AssetBundleManifest assetBundleManifest, string outputDirectory, string assetBundleMapPath,bool PrettyPrint = false)
    {
        AssetBundleMap assetBundleMap = assetBundleManifest.ToMap(outputDirectory);
        using (var sw = new StreamWriter(assetBundleMapPath))
        {
            JsonWriter writer = new JsonWriter(sw);
            writer.PrettyPrint = PrettyPrint;
            JsonMapper.ToJson(assetBundleMap, writer);
        }
    }
}
