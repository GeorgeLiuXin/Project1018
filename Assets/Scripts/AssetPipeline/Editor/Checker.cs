using UnityEngine;
using UnityEditor;
using System.IO;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.Text;
using System.Collections.Generic;

namespace Galaxy
{
    public class Checker
    {
        public static ICollection<string> GetTypeTreeChange(string baseDir, string targetDir)
        {
            var changeFiles = new List<string>();

            baseDir = Path.GetFullPath(baseDir);
            if (Directory.Exists(baseDir))
            {
                string baseDirName = Path.GetFileName(baseDir);
                var baseFiles = Directory.GetFiles(baseDir, "*", SearchOption.AllDirectories);

                for (int i = 0; i < baseFiles.Length; i++)
                {
                    if (!baseFiles[i].EndsWith(".manifest"))
                    {
                        continue;
                    }

                    // Escape AssetBundleManifest
                    string baseFileName = Path.GetFileNameWithoutExtension(baseFiles[i]);
                    if (baseDirName == baseFileName)
                    {
                        continue;
                    }

                    string relativePath = Utility.GetRelativePath(baseFiles[i], baseDir);
                    string targetPath = Path.Combine(targetDir, relativePath);

                    if (!IsTypeTreeEqual(baseFiles[i], targetPath))
                    {
                        changeFiles.Add(targetPath);
                    }
                }
            }

            return changeFiles;
        }

        private static bool IsTypeTreeEqual(string leftFile, string rightFile)
        {
            if (File.Exists(leftFile) && File.Exists(rightFile))
            {
                var leftManifest = Deserialize(leftFile);
                var rightManifest = Deserialize(rightFile);

                var leftTypeTreeHash = leftManifest.Hashes.TypeTreeHash;
                var rightTypeTreeHash = rightManifest.Hashes.TypeTreeHash;

                if (leftTypeTreeHash.serializedVersion == rightTypeTreeHash.serializedVersion &&
                    leftTypeTreeHash.Hash == rightTypeTreeHash.Hash)
                {
                    return true;
                }
            }

            return false;
        }

        public static AssetBundleManifestYaml Deserialize(string filePath)
        {
            var deserializer = new Deserializer();
            var input = new StreamReader(filePath);
            return deserializer.Deserialize<AssetBundleManifestYaml>(input);
        }

        public static string GetCachedMd5(string assetBundleCachePath, uint CRC)
        {
            if (File.Exists(assetBundleCachePath))
            {
                var content = File.ReadAllLines(assetBundleCachePath);
                uint cachedCRC = uint.Parse(content[0]);

                if (cachedCRC == CRC)
                {
                    return content[1];
                }
            }

            return string.Empty;
        }

        public static void SetCacheMd5(string assetBundleCachePath, string md5, uint CRC)
        {
            using (var sw = new StreamWriter(assetBundleCachePath))
            {
                sw.WriteLine(CRC);
                sw.WriteLine(md5);
            }
        }
    }
}
