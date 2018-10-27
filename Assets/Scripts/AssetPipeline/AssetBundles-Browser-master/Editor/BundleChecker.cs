using System.IO;
using YamlDotNet.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy.AssetBundleBrowser
{
    public class BundleChecker
    {
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

        public static string GetFileMD5(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            try
            {
                FileStream get_file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash_byte = get_md5.ComputeHash(get_file);
                string result = System.BitConverter.ToString(hash_byte).Replace("-", "");
                get_file.Dispose();
                return result;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
                return null;
            }
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
