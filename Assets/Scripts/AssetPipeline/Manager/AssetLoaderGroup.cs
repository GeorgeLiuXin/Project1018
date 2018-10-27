using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    internal class AssetLoaderGroup
    {
        public Dictionary<string, List<AssetLoader>> m_AssetLoaderDict;
        public List<AssetLoader> m_WaitingLoaders;

        public object UserData;
        public LoadAssetCallback Callback;

        private LoadResult m_SummaryResult;

        public AssetLoaderGroup(string[] paths, int[] prioritys, LoadAssetCallback singleCallback, LoadAssetCallback callBack, object userdata)
        {
            int length = paths.Length;
            m_WaitingLoaders = new List<AssetLoader>();
            m_AssetLoaderDict = new Dictionary<string, List<AssetLoader>>();
            Callback = callBack;
            UserData = userdata;

            for (int i = 0; i < length; i++)
            {
                string path = paths[i];
                int priority = prioritys[i];
                AssetLoader singleLoader = new AssetLoader(path, priority, singleCallback, null);
                singleLoader.Index = i;

                m_AssetLoaderDict.ForceListAdd(path, singleLoader);
                m_WaitingLoaders.Add(singleLoader);
            }

            m_SummaryResult = new LoadResult();
            m_SummaryResult.assetNames = paths;
            m_SummaryResult.assets = new UnityEngine.Object[length];
            m_SummaryResult.userData = userdata;
        }

        public bool FireCallback(string path, bool isSuccess, bool isScene, UnityEngine.Object target)
        {
            bool isGroupComplete = false;
            LoadResult singleResult = null;

            if (m_AssetLoaderDict.ContainsKey(path))
            {
                List<AssetLoader> loaders = m_AssetLoaderDict[path];

                for (int i = 0; i < loaders.Count; i++)
                {
                    AssetLoader loader = loaders[i];
                    singleResult = loader.FireCallback(isSuccess, isScene, target);

                    m_WaitingLoaders.Remove(loader);

                    int index = loader.Index;
                    m_SummaryResult.assets[index] = target;

                    if (false == isSuccess)
                    {
                        m_SummaryResult.isSuccess = false;
                        int len = 0;
                        string[] oldErrorAssetNames = m_SummaryResult.errorAssetNames;
                        if (oldErrorAssetNames != null)
                        {
                            len = oldErrorAssetNames.Length;
                            string[] newErrorAssetNames = new string[len + 1];
                            Array.Copy(oldErrorAssetNames, newErrorAssetNames, len);
                            newErrorAssetNames[len] = path;
                            m_SummaryResult.errorAssetNames = newErrorAssetNames;
                        }
                        else
                        {
                            string[] newErrorAssetNames = new string[1] { path };
                            m_SummaryResult.errorAssetNames = newErrorAssetNames;
                        }
                    }
                }

                if (m_WaitingLoaders.Count == 0)
                {
                    isGroupComplete = true;
                    Callback(m_SummaryResult);
                }
            }

            return isGroupComplete;
        }

        public bool ContainsPath(string assetPath)
        {
            return m_AssetLoaderDict.ContainsKey(assetPath);
        }
    }
}
