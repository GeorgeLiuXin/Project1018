using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld.AssetPipeline
{
    public class ResourcesMgr : Singleton<ResourcesMgr>
    {
        public static string PersistentAssetsPath { get; set; }
        public const string RES_RELATIVE_PATH = "Assets/AssetDatas";
        private ResourceLoader m_ResourceLoader;
        private ResourceLoader m_LocalResourceLoader;
        private Dictionary<string, List<LoadAssetCallback>> m_vResourceCallBackDic;

        public ResourcesMgr()
        {
            this.m_ResourceLoader = null;
            this.m_vResourceCallBackDic = new Dictionary<string, List<LoadAssetCallback>>();
            InitResourceLoader();
        }

        protected void InitResourceLoader()
        {
            //this.m_LocalResourceLoader = new OriginalResourcesLoader();
            //#if DEMO
            //            this.m_ResourceLoader = m_LocalResourceLoader;
            //#else
            this.m_ResourceLoader = new OriginalResourcesLoader();
            this.m_ResourceLoader.Init();
            //this.m_ResourceLoader.OnResourceLoadFailure += OnResourceLoadFailure;
            //#endif
            m_ResourceLoader.ResourceCallBack = new LoadAssetCallback(this.DefaultResourceCallBack);
        }

        private void OnResourceLoadFailure(string assetPath, string content)
        {
            GameLogger.Warning(LOG_CHANNEL.ASSET, assetPath + " : " + content);
        }

        protected void DefaultResourceCallBack(LoadResult result)
        {
            string assetName = string.Empty;
            if (result.assetNames != null && result.assetNames.Length > 0)
                assetName = result.assetNames[0];

            if (!assetName.IsNE() && this.m_vResourceCallBackDic.ContainsKey(assetName))
            {
                List<LoadAssetCallback> callBacks = m_vResourceCallBackDic[assetName];
                if (callBacks != null && callBacks.Count > 0)
                {
                    for (int i = callBacks.Count - 1; i >= 0; i--)
                    {
                        callBacks[i].Invoke(result);
                        callBacks.RemoveAt(i);
                    }
                }
                if (callBacks.Count == 0)
                {
                    this.m_vResourceCallBackDic.ForceRemove(assetName);
                }
            }
        }

        public void DestroyAsset(Object obj)
        {
            this.m_ResourceLoader.DestroyAsset(obj);
        }

        public void DestroyAsset(string path)
        {
            this.m_ResourceLoader.DestroyAsset(path);
        }

        public void DestroyAssetImmediately(Object obj)
        {
            this.m_ResourceLoader.DestroyAssetImmediately(obj);
        }

        public void DestroyAssetImmediately(string path)
        {
            this.m_ResourceLoader.DestroyAssetImmediately(path);
        }

        public void DestroyAssets(string[] paths)
        {
            if (paths != null && paths.Length > 0)
                foreach (string path in paths)
                    DestroyAsset(path);
        }

        public void DestroyAssetsImmediately(string[] paths)
        {
            if (paths != null && paths.Length > 0)
                foreach (string path in paths)
                    DestroyAssetImmediately(path);
        }

        public void DestroyAsset(ref Object obj)
        {
            this.m_ResourceLoader.DestroyAsset(obj);
            obj = null;
        }

        public void Init()
        {
            this.m_ResourceLoader.Init();
        }

        public byte[] LoadByteBuffer(string mainPath)
        {
            return this.m_ResourceLoader.LoadByteBuffer(mainPath);
        }

        public byte[] LoadFileInStreamingAssets(string fileName)
        {
            return this.m_ResourceLoader.LoadFileInStreamingAssets(fileName);
        }


        public void LoadImagePack(string rc, LoadAssetCallback callback, LoadAssetCallback sindleCallback = null, object userdata = null)
        {
            this.m_ResourceLoader.LoadImagePack(rc, callback, sindleCallback, userdata);
        }

        /// <summary>
        /// 加载资源接口
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="callback"></param>
        /// <param name="userdata"></param>
        public int LoadAsset(string rc, LoadAssetCallback callback, object userdata = null)
        {
            return this.m_ResourceLoader.LoadAsset(rc, callback, userdata);
        }

        public int LoadAsset(string relativePath, int priority, LoadAssetCallback callback, object userdata)
        {
            return this.m_ResourceLoader.LoadAsset(relativePath, priority, callback, userdata);
        }

        public int[] LoadAssets(string[] rc, LoadAssetCallback callback, LoadAssetCallback sindleCallback = null, object userdata = null)
        {
            return this.m_ResourceLoader.LoadAssets(rc, callback, sindleCallback, userdata);
        }
        public int[] LoadAssets(string[] rc, int[] priorities, LoadAssetCallback callback, LoadAssetCallback sindleCallback = null, object userdata = null)
        {
            return this.m_ResourceLoader.LoadAssets(rc, priorities, callback, sindleCallback, userdata);
        }

        public byte[] LoadTextBuffer(string mainPath, bool decode = true)
        {
            return this.m_ResourceLoader.LoadTextBuffer(mainPath, decode);
        }

        public string LoadTextString(string mainPath, bool decode = true)
        {
            return this.m_ResourceLoader.LoadTextString(mainPath, decode);
        }

        public UnityEngine.Object LoadAssetImmediate(string path)
        {
            return this.m_ResourceLoader.LoadAssetImmediate(path);
        }

        public void PrintBundleList()
        {
            // TODO: 给开发者提供一个检阅的接口
            this.m_ResourceLoader.PrintBundleList();
        }

        public void RegisterCallBack(string resourcesRelativePath, LoadAssetCallback callBack)
        {
            m_vResourceCallBackDic.ForceAddList(resourcesRelativePath, callBack);
        }

        public void RemoveCallBack(string resourcesRelativePath, LoadAssetCallback callBack)
        {
            if (m_vResourceCallBackDic.ContainsKey(resourcesRelativePath))
            {
                List<LoadAssetCallback> callBacks = m_vResourceCallBackDic[resourcesRelativePath];
                if (callBacks != null && callBacks.Count > 0)
                {
                    if (callBacks.Contains(callBack))
                    {
                        callBacks.Remove(callBack);
                    }
                }
            }
        }

        public void Collect()
        {
            //GalaxyGameModule.GetGameManager<BundleManager>().Collect();
            System.GC.Collect();
        }

        public void Release()
        {
            this.m_vResourceCallBackDic.Clear();
            this.m_ResourceLoader.Release();
        }

        public void Tick(uint uDeltaTimeMS)
        {

        }
    }

}
