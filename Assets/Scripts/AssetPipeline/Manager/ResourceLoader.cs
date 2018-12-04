using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld.AssetPipeline
{
    public abstract class ResourceLoader
    {
        public static PersistentAssetsLoader m_PersistentAssetsLoader = null;
        public static StreamingAssetsLoader m_StreamingAssetsLoader = null;

        static ResourceLoader()
        {
            if (!Application.isEditor)
                if (Application.platform == RuntimePlatform.Android)
                {
                    m_StreamingAssetsLoader = new AndroidStreamingLoader();
                }
                else
                {
                    m_StreamingAssetsLoader = new IOSStreamingAssetsLoader();
                }
            else
                m_StreamingAssetsLoader = new PCStreamingAssetsLoader();
            m_PersistentAssetsLoader = new PersistentAssetsLoader();
        }

        public abstract void DestroyAssetImmediately(string path);
        public abstract void DestroyAssetImmediately(Object obj);

        public abstract void DestroyAsset(string path);
        public abstract void DestroyAsset(Object obj);

        public abstract void Init();

        public abstract byte[] LoadByteBuffer(string mainPath);
        public abstract byte[] LoadFileInStreamingAssets(string fileName);
        public abstract void LoadImagePack(string relativePath, LoadAssetCallback callback, LoadAssetCallback singleCallback, object userdata = null);
        public abstract int LoadAsset(string rc, LoadAssetCallback callback, object userdata = null);
        public abstract int LoadAsset(string rc, int priority, LoadAssetCallback callback, object userdata = null);
        public abstract int[] LoadAssets(string[] rc, LoadAssetCallback callback, LoadAssetCallback singleCallback, object userdata = null);
        public abstract int[] LoadAssets(string[] rc, int[] priorities, LoadAssetCallback callback, LoadAssetCallback singleCallback, object userdata = null);
        public abstract byte[] LoadTextBuffer(string mainPath, bool decode);
        public abstract string LoadTextString(string mainPath, bool decode);

        public abstract UnityEngine.Object LoadAssetImmediate(string rc);

        public abstract void PrintBundleList();
        public abstract void Release();
        public abstract void Tick(uint uDeltaTimeMS);

        public LoadAssetCallback ResourceCallBack { get; set; }

    }

}
