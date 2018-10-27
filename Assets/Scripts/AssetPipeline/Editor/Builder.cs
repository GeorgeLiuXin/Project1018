using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using LitJson;

namespace Galaxy
{
    public class Builder
    {
        public static string outputPath = "";
        public static BuildTarget target;

        private const string META_EXTENSION = ".meta";
        private const string MANIFEST_EXTENSION = ".manifest";
        private const string CACHE_EXTENSION = ".cache";
        private const string ASSETBUNDLE_DIR = "Bundle";
        private const string BASE_ASSETBUNDLE_DIR = "BundleBase";
        public static BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression;

        public static void BuildFromUI()
        {
            BuildInternal(false);
        }

        public static void Build(AssetBundleBuild[] builds)
        {
            BuildInternal(builds, true);
        }
        public static void BuildInternal(AssetBundleBuild[] builds, bool error_quit)
        {
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            var assetBundleManifest = BuildPipeline.BuildAssetBundles(outputPath, builds, options,
               target);

            try
            {
                //    CheckIsDAG(assetBundleManifest);

                //    CheckTypeTreeChange();

                WriteAssetBundleMap(assetBundleManifest);

                //   CopyToStreamingAssets(assetBundleManifest);
            }
            catch (Exception e)
            {
                Debug.LogError("Build AssetBundles failed!\n" + e.ToString());

                if (error_quit)
                {
                    //      EditorApplication.Exit(20);
                }
            }
        }

        public static void BuildInternal(bool error_quit)
        {
            string outputPath = GetOutputPath();
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

#pragma warning disable 0162
            if (VersionManager.EnableAppendHashToAssetBundlePath)
            {
                options |= BuildAssetBundleOptions.AppendHashToAssetBundleName;
            }
            else
            {
                options &= ~BuildAssetBundleOptions.AppendHashToAssetBundleName;
            }
#pragma warning restore 0162

            var assetBundleManifest = BuildPipeline.BuildAssetBundles(outputPath, options,
                EditorUserBuildSettings.activeBuildTarget);

            try
            {
                CheckIsDAG(assetBundleManifest);

                CheckTypeTreeChange();

                WriteAssetBundleMap(assetBundleManifest);

                //   CopyToStreamingAssets(assetBundleManifest);
            }
            catch (Exception e)
            {
                Debug.LogError("Build AssetBundles failed!\n" + e.ToString());

                if (error_quit)
                {
                    EditorApplication.Exit(20);
                }
            }
        }

        private static void WriteAssetBundleMap(AssetBundleManifest assetBundleManifest)
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            var assetBundleMap = new AssetBundleMap();

            assetBundleMap.allAssetBundles = assetBundleManifest.GetAllAssetBundles();
            assetBundleMap.allAssetBundlesWithVariant = assetBundleManifest.GetAllAssetBundlesWithVariant();

            var list = new List<AssetBundleMap.AssetBundleItem>();
            foreach (var assetBundlePath in assetBundleManifest.GetAllAssetBundles())
            {
                var item = new AssetBundleMap.AssetBundleItem();

#pragma warning disable 0429
                string assetBundlePathWithoutHash = Utility.GetAssetBundlePathWithoutHash(assetBundlePath,
                    VersionManager.EnableAppendHashToAssetBundlePath);
#pragma warning restore 0429

                item.assetBundlePath = assetBundlePath;
                item.assetBundleVariant = Utility.GetVariant(assetBundlePath);
                item.assetNames = Galaxy.AssetBundleBrowser.AssetBundleModel.Model.DataSource.GetAssetPathsFromAssetBundle(assetBundlePathWithoutHash);

                item.allDependencies = assetBundleManifest.GetAllDependencies(item.assetBundlePath);
                item.directDependencies = assetBundleManifest.GetDirectDependencies(item.assetBundlePath);
                item.assetBundleHash = assetBundleManifest.GetAssetBundleHash(item.assetBundlePath).ToString();

                // Set CRC
                string assetBundleManifestFullPath = Path.Combine(GetOutputPath(),
                    assetBundlePathWithoutHash + MANIFEST_EXTENSION);
                var manifest = Checker.Deserialize(assetBundleManifestFullPath);
                item.CRC = manifest.CRC;

                // Set MD5
                string assetBundleCachePath = GetAssetBundleCachePath(Path.Combine(GetOutputPath(),
                    assetBundlePathWithoutHash));
                string md5 = Checker.GetCachedMd5(assetBundleCachePath, item.CRC);
                if (string.IsNullOrEmpty(md5))
                {
                    md5 = Utility.ComputeMd5Hash(Path.Combine(GetOutputPath(), assetBundlePath));
                    Checker.SetCacheMd5(assetBundleCachePath, md5, item.CRC);
                }
                item.md5 = md5;

                // Set file length
                var fi = new FileInfo(Path.Combine(GetOutputPath(), assetBundlePath));
                item.length = (int)fi.Length;

                list.Add(item);
            }
            assetBundleMap.assetBundleBuilds = list.ToArray();

            // Write assetBundleMap to JSON
            string assetBundleMapPath = Path.Combine(GetOutputPath(), VersionManager.ASSETBUNDLE_MAP_FILENAME);
            var sw = new StreamWriter(assetBundleMapPath);

            JsonWriter writer = new JsonWriter(sw);
            writer.PrettyPrint = false;
            JsonMapper.ToJson(assetBundleMap, writer);

            sw.Flush();
            sw.Close();


            // Write assetBundleMap to JSON Pretty
            string assetBundleMapPathPretty = Path.Combine(GetOutputPath(), VersionManager.ASSETBUNDLE_MAP_FILENAME + "PrettyPrint");
            sw = new StreamWriter(assetBundleMapPathPretty);

            writer = new JsonWriter(sw);
            writer.PrettyPrint = true;
            JsonMapper.ToJson(assetBundleMap, writer);

            sw.Flush();
            sw.Close();

            // Calc md5 hash for update check
            string md5hash = Utility.ComputeMd5Hash(assetBundleMapPath);
            string path = Path.Combine(GetOutputPath(), VersionManager.ASSETBUNDLE_MAP_MD5_FILENAME);
            File.WriteAllText(path, md5hash, System.Text.Encoding.UTF8);
        }

        private static string GetOutputPath()
        {
            return outputPath;
            //  return Path.GetFullPath(GetBundlePath(ASSETBUNDLE_DIR));
        }

        private static string GetBasePath()
        {
            return Path.GetFullPath(GetBundlePath(BASE_ASSETBUNDLE_DIR));
        }

        private static string GetBundlePath(string root)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                    return Path.Combine(root, "OSXEditor");
                case RuntimePlatform.WindowsEditor:
                    return Path.Combine(root, "WindowsEditor");
                case RuntimePlatform.Android:
                    return Path.Combine(root, "Android");
                case RuntimePlatform.IPhonePlayer:
                    return Path.Combine(root, "iOS");
            }

            return string.Empty;
        }

        private static bool CheckIsDAG(AssetBundleManifest assetBundleManifest)
        {
            var assetBundles = assetBundleManifest.GetAllAssetBundles();
            var duplicateBundles = new List<string>();

            foreach (var assetBundle in assetBundles)
            {
                var dependencies = assetBundleManifest.GetAllDependencies(assetBundle);

                if (ArrayUtility.Contains(dependencies, assetBundle))
                {
                    duplicateBundles.Add(assetBundle);
                }
            }

            if (duplicateBundles.Count > 0)
            {
                var sb = new System.Text.StringBuilder();

                foreach (var bundle in duplicateBundles)
                {
                    sb.AppendLine(bundle);
                    sb.Append(string.Join("\n", AssetDatabase.GetAssetPathsFromAssetBundle(bundle)));
                    sb.AppendLine();
                }

                Debug.LogError(sb);

                throw new System.Exception("Build AssetBundles failed!\nAssetBundle has cyclic reference");
            }

            return true;
        }

        private static void CheckTypeTreeChange()
        {
            var changes = Checker.GetTypeTreeChange(GetBasePath(), GetOutputPath());
            if (changes.Count > 0)
            {
                foreach (var change in changes)
                {
                    string assetBundlePath = GetAssetBundlePathFromManifest(change);
                    string assetBundleRelativePath = Utility.GetRelativePath(assetBundlePath, GetOutputPath());

                    var assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleRelativePath);
                    var dependencies = AssetDatabase.GetDependencies(assetPaths, true);

                    Debug.LogError("TypeTree changed: " + assetBundleRelativePath +
                        "\n\nAssets:\n" + string.Join("\n", assetPaths) +
                        "\n\nDependencies:\n" + string.Join("\n", dependencies));
                }

                throw new System.Exception("Build AssetBundles failed! TypeTree changed!");
            }
        }

        private static void CopyToStreamingAssets(AssetBundleManifest assetBundleManifest)
        {
            string cacheBundleDir = Path.Combine(Application.streamingAssetsPath,
                VersionManager.ASSETBUNDLE_STREAMING_ASSETS_DIR);

            if (!Directory.Exists(cacheBundleDir))
            {
                Directory.CreateDirectory(cacheBundleDir);
            }

            var cacheBundleFiles = new List<string>(Directory.GetFiles(cacheBundleDir, "*", SearchOption.AllDirectories));

            var outputReserveFiles = new List<string>();
            outputReserveFiles.Add(VersionManager.ASSETBUNDLE_MAP_FILENAME);
            outputReserveFiles.Add(VersionManager.ASSETBUNDLE_MAP_MD5_FILENAME);
            outputReserveFiles.Add(Path.GetFileName(GetOutputPath()));
            outputReserveFiles.Add(Path.GetFileName(GetOutputPath()) + MANIFEST_EXTENSION);

            var streamingAssetsReserveFiles = new List<string>();
            streamingAssetsReserveFiles.Add(VersionManager.ASSETBUNDLE_MAP_FILENAME);
            streamingAssetsReserveFiles.Add(VersionManager.ASSETBUNDLE_MAP_FILENAME + META_EXTENSION);
            streamingAssetsReserveFiles.Add(VersionManager.ASSETBUNDLE_MAP_MD5_FILENAME);
            streamingAssetsReserveFiles.Add(VersionManager.ASSETBUNDLE_MAP_MD5_FILENAME + META_EXTENSION);

            foreach (var assetBundlePath in assetBundleManifest.GetAllAssetBundles())
            {
                string src = assetBundlePath;
                string dst = null;

#pragma warning disable 0162
                if (VersionManager.EnableAppendHashToAssetBundlePath)
                {
                    dst = assetBundlePath;
                }
                else
                {
                    string hash = assetBundleManifest.GetAssetBundleHash(assetBundlePath).ToString();
                    dst = Utility.InsertHashToAssetBundlePath(assetBundlePath, hash);
                }
#pragma warning restore 0162

                if (!cacheBundleFiles.Contains(Path.Combine(cacheBundleDir, dst)))
                {
                    CopyFromAssetBundleToStreamingAssets(src, dst);
                }

                outputReserveFiles.Add(src);
#pragma warning disable 0429
                string manifestPath = Utility.GetAssetBundlePathWithoutHash(src,
                    VersionManager.EnableAppendHashToAssetBundlePath) + MANIFEST_EXTENSION;
                outputReserveFiles.Add(manifestPath);

                string cachePath = GetAssetBundleCachePath(Utility.GetAssetBundlePathWithoutHash(src,
                    VersionManager.EnableAppendHashToAssetBundlePath));
                outputReserveFiles.Add(cachePath);
#pragma warning restore 0429

                streamingAssetsReserveFiles.Add(dst);
            }

            DeleteUselessFilesAndDirectories(GetOutputPath(), outputReserveFiles.ToArray());
            DeleteUselessFilesAndDirectories(cacheBundleDir, streamingAssetsReserveFiles.ToArray());

            CopyFromAssetBundleToStreamingAssets(VersionManager.ASSETBUNDLE_MAP_FILENAME);
            CopyFromAssetBundleToStreamingAssets(VersionManager.ASSETBUNDLE_MAP_MD5_FILENAME);

            AssetDatabase.Refresh();
        }

        private static void CopyFromAssetBundleToStreamingAssets(string fileName)
        {
            CopyFromAssetBundleToStreamingAssets(fileName, fileName);
        }

        private static void CopyFromAssetBundleToStreamingAssets(string srcFile, string dstFile)
        {
            string src = Path.Combine(GetOutputPath(), srcFile);
            string dst = GetCacheBundlePath(dstFile);

            if (File.Exists(dst))
            {
                File.Delete(dst);
            }

            string dir = Path.GetDirectoryName(dst);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.Copy(src, dst);
        }

        private static string GetCacheBundlePath(string fileName)
        {
            string cacheBundleDir = Path.Combine(Application.streamingAssetsPath,
                VersionManager.ASSETBUNDLE_STREAMING_ASSETS_DIR);
            return Path.Combine(cacheBundleDir, fileName);
        }

        private static void DeleteUselessFilesAndDirectories(string directory, string[] reserveFiles)
        {
            var uselessFiles = new List<string>(Directory.GetFiles(directory, "*", SearchOption.AllDirectories));
            var uselessDirs = new List<string>(Directory.GetDirectories(directory, "*", SearchOption.AllDirectories));

            for (int i = 0; i < reserveFiles.Length; i++)
            {
                var file = Path.Combine(directory, reserveFiles[i]);
                uselessFiles.Remove(file);

                string fileMeta = file + META_EXTENSION;
                uselessFiles.Remove(fileMeta);

                string dir = Path.GetDirectoryName(file);
                uselessDirs.Remove(dir);

                string dirMeta = dir + META_EXTENSION;
                uselessFiles.Remove(dirMeta);
            }

            for (int i = 0; i < uselessFiles.Count; i++)
            {
                File.Delete(uselessFiles[i]);
            }

            for (int i = 0; i < uselessDirs.Count; i++)
            {
                Directory.Delete(uselessDirs[i]);
            }
        }

        private static string GetAssetBundlePathFromManifest(string assetBundleManifestPath)
        {
            if (assetBundleManifestPath.EndsWith(MANIFEST_EXTENSION))
            {
                return assetBundleManifestPath.Substring(0, assetBundleManifestPath.Length - MANIFEST_EXTENSION.Length);
            }

            return assetBundleManifestPath;
        }

        private static string GetAssetBundleCachePath(string assetBundlePath)
        {
            return assetBundlePath + CACHE_EXTENSION;
        }
    }
}
