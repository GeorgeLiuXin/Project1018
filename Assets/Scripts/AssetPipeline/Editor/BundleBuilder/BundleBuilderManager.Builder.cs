/**************************************************
 *  创建人   : 夏佳文
 *  创建时间 : 2018.6.15
 *  说明     : 新的打包类，采用自动分析与打包
 * ************************************************/

using System.Collections.Generic;
using UnityEngine;
using Galaxy;
using Galaxy.DataNode;
using System.IO;
using System;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;

using Galaxy.AssetBundleBrowser.AssetBundleDataSource;
using LitJson;

namespace Galaxy.AssetPipeline
{
    internal partial class BundleBuilderManager
    {
        private class Builder
        {
            private const string META_EXTENSION = ".meta";
            private const string MANIFEST_EXTENSION = ".manifest";
            private const string CACHE_EXTENSION = ".cache";
            private const string ASSETBUNDLE_DIR = "Bundle";
            private const string BASE_ASSETBUNDLE_DIR = "BundleBase";

            private string m_OutputPath = "";
            private RuntimePlatform m_Platform;
            private BuildTarget m_Target;
            private EnumChannelType m_ChannelType;

            private IABDataSource m_DataSource;

            public static BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.AppendHashToAssetBundleName;

            public void Build(AssetBundleBuild[] builds, VersionInfo version, RuntimePlatform platform, EnumChannelType channel)
            {
                m_ChannelType = channel;
                m_Platform = platform;
                m_Target = BuildHelper.CustomTargetToBuildTarget(platform);
                m_OutputPath = BuildHelper.CheckBundlePath(
                BuildHelper.ChannelToName(channel),
                BuildHelper.GameVersionStr(version.GameVersion, version.First, version.Second),
                BuildHelper.CustomTargetToName(platform)
                );
                BuildInternal(builds, true);
            }

            internal void Build(IABDataSource dataSource, VersionInfo versionInfo, RuntimePlatform platform, EnumChannelType channel)
            {
                m_DataSource = dataSource;
                Build(dataSource.GetAssetBundleBuilds(), versionInfo, platform, channel);
            }

            public void BuildInternal(AssetBundleBuild[] builds, bool error_quit = false)
            {
                if (!Directory.Exists(m_OutputPath))
                {
                    Directory.CreateDirectory(m_OutputPath);
                }

                var assetBundleManifest = BuildPipeline.BuildAssetBundles(m_OutputPath, builds, options,
                   m_Target);

                try
                {
                    CheckIsDAG(assetBundleManifest);

                    //    CheckTypeTreeChange();

                    WriteAssetBundleMap(assetBundleManifest);

                    CheckOutputFolder(assetBundleManifest);

                    CopyToStreamingAssets(assetBundleManifest);

                    CopyMd5ToCFG();

                    Debug.Log("<color=#FFA500>----- Build Bundle Success! -----</color>");
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

            private void WriteAssetBundleMap(AssetBundleManifest assetBundleManifest)
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
                    item.assetNames = m_DataSource.GetAssetPathsFromAssetBundle(assetBundlePathWithoutHash);

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
                string assetBundleMapPathPretty = Path.Combine(GetOutputPath(), "assetbundlemapPrettyPrint.json");
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

            private string GetOutputPath()
            {
                return m_OutputPath;
            }

            private string GetBasePath()
            {
                return Path.GetFullPath(GetBundlePath(BASE_ASSETBUNDLE_DIR));
            }

            private string GetBundlePath(string root)
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

            private bool CheckIsDAG(AssetBundleManifest assetBundleManifest)
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
                else
                {
                    Debug.Log("CheckIsDAG success. 无循环依赖.");
                }

                return true;
            }

            private void CheckTypeTreeChange()
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

                        string warningContent = "TypeTree changed: " + assetBundleRelativePath +
                            "\n\nAssets:\n" + string.Join("\n", assetPaths) +
                            "\n\nDependencies:\n" + string.Join("\n", dependencies);
                        BundleLog.Warning(warningContent);
                    }
                    throw new System.Exception("Build AssetBundles failed! TypeTree changed!");
                }
            }

            private void CheckOutputFolder(AssetBundleManifest assetBundleManifest)
            {
                var cacheBundleFiles = new List<string>(Directory.GetFiles(m_OutputPath, "*", SearchOption.AllDirectories));

                var outputReserveFiles = new List<string>();
                outputReserveFiles.Add(VersionManager.ASSETBUNDLE_MAP_FILENAME);
                outputReserveFiles.Add(VersionManager.ASSETBUNDLE_MAP_FILENAME + META_EXTENSION);
                outputReserveFiles.Add(VersionManager.ASSETBUNDLE_MAP_MD5_FILENAME);
                outputReserveFiles.Add(VersionManager.ASSETBUNDLE_MAP_MD5_FILENAME + META_EXTENSION);
                outputReserveFiles.Add("assetbundlemapPrettyPrint.json");
                outputReserveFiles.Add(BuildHelper.CustomTargetToName(m_Platform));
                outputReserveFiles.Add(BuildHelper.CustomTargetToName(m_Platform) + ".manifest");

                foreach (var assetBundlePath in assetBundleManifest.GetAllAssetBundles())
                {
                    string src = assetBundlePath;

                    // 防止文件操作符不统一
                    src = src.Replace('/', Path.DirectorySeparatorChar);
                    outputReserveFiles.Add(src);
                    string manifestPath = Utility.GetAssetBundlePathWithoutHash(src,
                        VersionManager.EnableAppendHashToAssetBundlePath) + MANIFEST_EXTENSION;
                    outputReserveFiles.Add(manifestPath);

                    string cachePath = GetAssetBundleCachePath(Utility.GetAssetBundlePathWithoutHash(src,
                        VersionManager.EnableAppendHashToAssetBundlePath));
                    outputReserveFiles.Add(cachePath);
                }

                DeleteUselessFilesAndDirectories(GetOutputPath(), outputReserveFiles.ToArray());
            }
            
            private void CopyToStreamingAssets(AssetBundleManifest assetBundleManifest)
            {
                string cacheBundleDir = Path.Combine(Application.streamingAssetsPath,
                    VersionManager.ASSETBUNDLE_STREAMING_ASSETS_DIR);

                if (!Directory.Exists(cacheBundleDir))
                {
                    Directory.CreateDirectory(cacheBundleDir);
                }

                var cacheBundleFiles = new List<string>(Directory.GetFiles(cacheBundleDir, "*", SearchOption.AllDirectories));
                
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
                    // 防止文件操作符不统一
                    dst = dst.Replace('/', Path.DirectorySeparatorChar);
#pragma warning restore 0162

                    string targetFilePath = Path.Combine(cacheBundleDir, dst);
                    if (!cacheBundleFiles.Contains(targetFilePath))
                    {
                        CopyFromAssetBundleToStreamingAssets(src, dst);
                    }
                    else
                    {
                        // Hash没发生变化，但是文件发生了变化
                        string localMd5 = BuildHelper.GetFileMD5(targetFilePath);
                        long localLength = BuildHelper.GetFileSize(targetFilePath);

                        string srcFilePath = Path.Combine(GetOutputPath(), src);
                        string bundleMd5 = BuildHelper.GetFileMD5(srcFilePath);
                        long bundleLength = BuildHelper.GetFileSize(srcFilePath);
                        if (localMd5 != bundleMd5 || localLength != bundleLength)
                        {
                            Debug.LogWarning(targetFilePath + " is Corrupt");
                            CopyFromAssetBundleToStreamingAssets(src, dst);
                        }
                    }
                    
                    streamingAssetsReserveFiles.Add(dst);
                }
                
                DeleteUselessFilesAndDirectories(cacheBundleDir, streamingAssetsReserveFiles.ToArray());

                CopyFromAssetBundleToStreamingAssets(VersionManager.ASSETBUNDLE_MAP_FILENAME);
                CopyFromAssetBundleToStreamingAssets(VersionManager.ASSETBUNDLE_MAP_MD5_FILENAME);

                AssetDatabase.Refresh();
            }

            private void CopyMd5ToCFG()
            {
                string md5FilePath = Path.Combine(m_OutputPath, "assetbundlemap.json.md5");
                if (File.Exists(md5FilePath))
                {
                    string md5Content = File.ReadAllText(md5FilePath);
                    if (!string.IsNullOrEmpty(md5Content))
                    {
                        string cfgPath = Path.Combine(Application.streamingAssetsPath, "PreInit.cfg");
                        if (File.Exists(cfgPath))
                        {
                            INICfg cfg = new INICfg();
                            cfg.Parse(File.ReadAllText(cfgPath));
                            cfg.SetString("Global", "ResVersionMd5", md5Content);
                            cfg.Save(cfgPath);
                        }
                        else
                        {
                            Debug.LogError("Can't Get CFG: " + cfgPath);
                        }
                        // TODO: 以后将会被移除
                        string remoteCfgPath = Path.Combine(Application.streamingAssetsPath, "RemoteConfigRelease.cfg");
                        if (File.Exists(remoteCfgPath))
                        {
                            INICfg cfg = new INICfg();
                            //   string section = BuildHelper.CustomTargetToName(m_Platform);
                            cfg.Parse(File.ReadAllText(remoteCfgPath));

                            // TODO 暂时也不分平台
                            cfg.SetString(BuildHelper.CustomTargetToName(RuntimePlatform.Android), "ResVersionMd5", md5Content);
                            cfg.SetString(BuildHelper.CustomTargetToName(RuntimePlatform.IPhonePlayer), "ResVersionMd5", md5Content);
                            cfg.SetString(BuildHelper.CustomTargetToName(RuntimePlatform.WindowsEditor), "ResVersionMd5", md5Content);
                            cfg.Save(remoteCfgPath);
                        }
                        else
                        {
                            Debug.LogError("Can't Get CFG: " + remoteCfgPath);
                        }
                    }
                    else
                    {
                        Debug.LogError("Can't Get Content : " + md5FilePath);
                    }
                }
                else
                {
                    Debug.LogError("Can't Get : " + md5FilePath);
                }
            }

            private void CopyFromAssetBundleToStreamingAssets(string fileName)
            {
                CopyFromAssetBundleToStreamingAssets(fileName, fileName);
            }

            private void CopyFromAssetBundleToStreamingAssets(string srcFile, string dstFile)
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

            private string GetCacheBundlePath(string fileName)
            {
                string cacheBundleDir = Path.Combine(Application.streamingAssetsPath,
                    VersionManager.ASSETBUNDLE_STREAMING_ASSETS_DIR);
                return Path.Combine(cacheBundleDir, fileName);
            }

            private void DeleteAllFiles(string directory)
            {
                var uselessFiles = new List<string>(Directory.GetFiles(directory, "*", SearchOption.AllDirectories));
                var uselessDirs = new List<string>(Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly));

                for (int i = 0; i < uselessFiles.Count; i++)
                {
                    try
                    {
                        File.Delete(uselessFiles[i]);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }

                for (int i = 0; i < uselessDirs.Count; i++)
                {
                    Directory.Delete(uselessDirs[i], true);
                }
            }

            private void DeleteUselessFilesAndDirectories(string directory, string[] reserveFiles)
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
                    try
                    {
                        File.Delete(uselessFiles[i]);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }

                for (int i = 0; i < uselessDirs.Count; i++)
                {
                    DirectoryInfo di= new DirectoryInfo(uselessDirs[i]);
                    if (di.GetFiles().Length == 0 && di.GetDirectories().Length == 0)
                    {
                        Directory.Delete(uselessDirs[i]);
                    }
                }
            }

            private string GetAssetBundlePathFromManifest(string assetBundleManifestPath)
            {
                if (assetBundleManifestPath.EndsWith(MANIFEST_EXTENSION))
                {
                    return assetBundleManifestPath.Substring(0, assetBundleManifestPath.Length - MANIFEST_EXTENSION.Length);
                }

                return assetBundleManifestPath;
            }

            private string GetAssetBundleCachePath(string assetBundlePath)
            {
                return assetBundlePath + CACHE_EXTENSION;
            }
        }
    }
}
