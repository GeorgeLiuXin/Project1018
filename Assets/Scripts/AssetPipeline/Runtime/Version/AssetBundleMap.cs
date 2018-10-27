using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace XWorld
{
    public class AssetBundleMap
    {
        public string[] allAssetBundles;
        public string[] allAssetBundlesWithVariant;

        public AssetBundleItem[] assetBundleBuilds;

        public class AssetBundleItem
        {
            public string assetBundlePath
            {
                get
                {
                    return relativePath + "/" + bundleName;
                }
                set
                {
                    Split(value);
                }
            }

            public string bundleName;
            public string relativePath;


            public string assetBundleVariant;
            public string[] assetNames;

            public string[] allDependencies;
            public string[] directDependencies;
            public string assetBundleHash;
            public uint CRC;
            public string md5;
            public int length;

            public void Split(string bundlePath)
            {
                bundlePath = BuildHelper.PathNormalized(bundlePath);
                List<string> parts = new List<string>(bundlePath.Split('/'));

                if (parts.Count > 0)
                {
                    bundleName = parts[parts.Count - 1];
                    parts.RemoveAt(parts.Count - 1);
                    relativePath = string.Join("/", parts.ToArray());
                }
            }
        }
    }
}
