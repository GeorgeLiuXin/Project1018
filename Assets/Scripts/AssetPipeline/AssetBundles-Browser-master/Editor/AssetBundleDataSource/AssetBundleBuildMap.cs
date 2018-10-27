using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;

namespace Galaxy.AssetBundleBrowser.AssetBundleDataSource
{
    [System.Serializable]
    internal class AssetBundleBuildMap
    {
        [SerializeField]
        private List<CustomAssetBundleBuild> m_builders = new List<CustomAssetBundleBuild>();
        private Dictionary<string, CustomAssetBundleBuild> m_buildAssetMap = new Dictionary<string, CustomAssetBundleBuild>();
        private Dictionary<string, CustomAssetBundleBuild> m_buildBundleMap = new Dictionary<string, CustomAssetBundleBuild>();

        public List<CustomAssetBundleBuild> Builders
        {
            get
            {
                return m_builders;
            }

            set
            {
                m_builders = value;
            }
        }

        private bool m_isInit = false;

        public void Init()
        {
            if (!m_isInit)
            {
                m_isInit = true;

                foreach (CustomAssetBundleBuild build in m_builders)
                {
                    m_buildBundleMap.Add(build.AssetBundleName, build);
                    foreach (string assetName in build.AssetNames)
                    {
                        m_buildAssetMap.Add(assetName, build);
                    }
                }
            }
        }
        
        public void Over()
        {
            m_isInit = false;
        }

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
            foreach (CustomAssetBundleBuild build in Builders)
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
            foreach (CustomAssetBundleBuild build in Builders)
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
            foreach (CustomAssetBundleBuild build in Builders)
            {
                names.Add(build.AssetBundleName);
            }
            return names.ToArray();
        }
        
        public void SetAssetBundleNameAndVariant(string assetPath, string bundleName, string variantName = "")
        {
            Debug.Log("<color=#EEE0E5>path: " + assetPath + " bundle: " + bundleName + "</color>");
            Refresh(assetPath, bundleName, variantName);
            //CustomAssetBundleBuild builder = new CustomAssetBundleBuild();
            //bool isContans = false;
            //foreach (CustomAssetBundleBuild build in Builders)
            //{
            //    if (string.Equals(bundleName, build.AssetBundleName))
            //    {
            //        builder = build;
            //        isContans = true;
            //        break;
            //    }
            //}

            //if (!isContans)
            //{
            //    builder.AssetBundleName = bundleName;
            //    Builders.Add(builder);
            //}

            //if (!builder.AssetNames.Contains(assetPath))
            //{
            //    builder.AssetNames.Add(assetPath);
            //}
        }

        public void RemoveUnusedAssetBundleNames()
        {
            // AssetDatabase.RemoveUnusedAssetBundleNames();
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

    [System.Serializable]
    public class CustomAssetBundleBuild
    {
        [SerializeField]
        private List<string> addressableNames = new List<string>();
        [SerializeField]
        private string assetBundleName = string.Empty;
        [SerializeField]
        private string assetBundleVariant = string.Empty;
        [SerializeField]
        private List<string> assetNames = new List<string>();

        public AssetBundleBuild ToStruct()
        {
            AssetBundleBuild build = new AssetBundleBuild();
            build.addressableNames = addressableNames.ToArray();
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
