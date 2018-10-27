using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Galaxy.AssetPipeline
{
    internal partial class CustomAssetDataSource : IABDataSource
    {
        private class AssetBundleBuildMap
        {
            private List<CustomAssetBundleBuild> m_builders = new List<CustomAssetBundleBuild>();
            private Dictionary<string, CustomAssetBundleBuild> m_buildAssetMap = new Dictionary<string, CustomAssetBundleBuild>();
            private Dictionary<string, CustomAssetBundleBuild> m_buildBundleMap = new Dictionary<string, CustomAssetBundleBuild>();

            private bool BuilderRemove(CustomAssetBundleBuild build)
            {
                if (m_builders.Contains(build))
                {
                    m_builders.Remove(build);
                    return true;
                }
                return false;
            }

            private bool AssetMapRemove(string assetName)
            {
                if (m_buildAssetMap.ContainsKey(assetName))
                {
                    m_buildAssetMap.Remove(assetName);
                    return true;
                }
                return false;
            }

            private bool BundleMapRemove(string bundleName)
            {
                if (m_buildBundleMap.ContainsKey(bundleName))
                {
                    m_buildBundleMap.Remove(bundleName);
                    return true;
                }
                return false;
            }

            private void RemoveAsset(string assetPath)
            {
                if (m_buildAssetMap.ContainsKey(assetPath))
                {
                    CustomAssetBundleBuild builder = m_buildAssetMap[assetPath];
                    AssetMapRemove(assetPath);
                    if (builder.AssetNames.Contains(assetPath))
                    {
                        builder.AssetNames.Remove(assetPath);
                    }
                    if (builder.AssetNames.Count == 0)
                    {
                        BundleMapRemove(builder.AssetBundleName);
                        BuilderRemove(builder);
                    }
                }
            }

            private void Add(string assetPath, string bundleName)
            {
                CustomAssetBundleBuild build = null;

                if (m_buildBundleMap.ContainsKey(bundleName))
                {
                    build = m_buildBundleMap[bundleName];
                }
                else
                {
                    build = new CustomAssetBundleBuild();
                    m_builders.Add(build);
                    build.AssetBundleName = bundleName;
                    m_buildBundleMap.Add(bundleName, build);
                }

                if (build != null)
                {
                    if (!build.AssetNames.Contains(assetPath))
                    {
                        build.AssetNames.Add(assetPath);
                        if (!m_buildAssetMap.ContainsKey(assetPath))
                        {
                            m_buildAssetMap.Add(assetPath, build);
                        }
                    }
                }
            }

            private void Refresh(string assetPath, string bundleName, string variantName)
            {
                bool isAssetContains = m_buildAssetMap.ContainsKey(assetPath);
                bool isBundleContains = m_buildBundleMap.ContainsKey(bundleName);

                bool isDeleteAsset = (string.IsNullOrEmpty(bundleName));
                if (isDeleteAsset)
                {
                    RemoveAsset(assetPath);
                }
                else
                {
                    //正常流程先删除
                    RemoveAsset(assetPath);
                    Add(assetPath, bundleName);
                }
            }

            public string[] GetAssetPathsFromAssetBundle(string name)
            {
                List<string> assets = new List<string>();
                foreach (CustomAssetBundleBuild build in m_builders)
                {
                    if (string.Equals(build.AssetBundleName + ".dat", name))
                    {
                        return build.AssetNames.ToArray();
                    }
                }
                return assets.ToArray();
            }

            public string GetAssetBundleName(string assetPath)
            {
                string bundleName = string.Empty;
                foreach (CustomAssetBundleBuild build in m_builders)
                {
                    foreach (string name in build.AssetNames)
                    {
                        if (string.Equals(name, assetPath))
                        {
                            bundleName = build.AssetBundleName;
                            return bundleName;
                        }
                    }
                }
                return bundleName;
            }

            public string[] GetAllAssetBundleNames()
            {
                List<string> names = new List<string>();
                foreach (CustomAssetBundleBuild build in m_builders)
                {
                    names.Add(build.AssetBundleName);
                }
                return names.ToArray();
            }

            public void SetAssetBundleNameAndVariant(string assetPath, string bundleName, string variantName = "")
            {
                // 模拟Unity行为,转小写
                bundleName = bundleName.ToLower();
                
                UnityEngine.Debug.Log("<color=#EEE0E5>path: " + assetPath + " bundle: " + bundleName + "</color>");
                Refresh(assetPath, bundleName, variantName);
            }

            public AssetBundleBuild[] GetBuilders()
            {
                List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
                foreach (CustomAssetBundleBuild build in m_builders)
                {
                    builds.Add(build.ToStruct());
                }
                return builds.ToArray();
            }
        }

        private class CustomAssetBundleBuild
        {
            private List<string> addressableNames = new List<string>();
            private string assetBundleName = string.Empty;
            private string assetBundleVariant = string.Empty;
            private List<string> assetNames = new List<string>();

            public AssetBundleBuild ToStruct()
            {
                AssetBundleBuild build = new AssetBundleBuild();
                build.addressableNames = assetNames.ToArray();
                build.assetBundleName = assetBundleName + ".dat";
                build.assetBundleVariant = assetBundleVariant;
                build.assetNames = assetNames.ToArray();
                return build;
            }

            public List<string> AddressableNames
            {
                get
                {
                    return addressableNames;
                }

                set
                {
                    addressableNames = value;
                }
            }

            public string AssetBundleName
            {
                get
                {
                    return assetBundleName;
                }

                set
                {
                    assetBundleName = value;
                }
            }

            public string AssetBundleVariant
            {
                get
                {
                    return assetBundleVariant;
                }

                set
                {
                    assetBundleVariant = value;
                }
            }

            public List<string> AssetNames
            {
                get
                {
                    return assetNames;
                }

                set
                {
                    assetNames = value;
                }
            }
        }
    }
}
