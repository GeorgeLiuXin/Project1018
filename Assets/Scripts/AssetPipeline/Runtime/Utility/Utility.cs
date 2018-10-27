using UnityEngine;
using System;
using System.IO;

namespace XWorld
{
    public class Utility
    {
        public static string GetHierarchyPath(GameObject go)
        {
            return GetHierarchyPath(go.transform);
        }

        public static string GetHierarchyPath(Transform t)
        {
            var sb = new System.Text.StringBuilder(t.name);

            while (t.parent != null)
            {
                t = t.parent;
                sb.Insert(0, "/");
                sb.Insert(0, t.name);
            }

            return sb.ToString();
        }

        public static string GetRelativePath(string filespec, string folder)
        {
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        public static string GetAssetBundlePathWithoutHash(string assetBundlePath, bool hasHash)
        {
            var fileName = assetBundlePath.Contains(".") ?
                assetBundlePath.Substring(0, assetBundlePath.LastIndexOf('.')) : assetBundlePath;

            var ext = assetBundlePath.Contains(".") ?
                assetBundlePath.Substring(assetBundlePath.LastIndexOf('.')) : string.Empty;

            var fileNameWithoutHash = hasHash ? fileName.Substring(0, fileName.LastIndexOf('_')) : fileName;

            return fileNameWithoutHash + ext;
        }

        public static string GetVariant(string assetBundlePath)
        {
            if (assetBundlePath.Contains(".") && assetBundlePath.LastIndexOf('.') != assetBundlePath.Length - 1)
            {
                return assetBundlePath.Substring(assetBundlePath.LastIndexOf('.') + 1);
            }

            return string.Empty;
        }

        public static string InsertHashToAssetBundlePath(string assetBundlePath, string hash)
        {
            if (!assetBundlePath.Contains(".") || assetBundlePath.LastIndexOf('.') == assetBundlePath.Length - 1)
            {
                return assetBundlePath + "_" + hash;
            }

            string assetBundlePathWithoutVariant = assetBundlePath.Substring(0, assetBundlePath.LastIndexOf('.'));
            string variant = assetBundlePath.Substring(assetBundlePath.LastIndexOf('.'));

            return assetBundlePathWithoutVariant + "_" + hash + variant;
        }

        public static string ReadLine(string path)
        {
            using (var sr = new StreamReader(path))
            {
                return sr.ReadLine();
            }
        }

        public static void WriteAllText(string path, string contents)
        {
            using (var sw = new StreamWriter(path))
            {
                sw.Write(contents);
            }
        }

        public static string ComputeMd5Hash(string path)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            using (var src = File.OpenRead(path))
            {
                return System.BitConverter.ToString(md5.ComputeHash(src))
                    .Replace("-", string.Empty)
                    .ToLower();
            }
        }

        public static long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }
    }
}
