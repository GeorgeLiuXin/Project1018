namespace XWorld.AssetPipeline
{
    using CusEncoding;

    using System.Collections.Generic;
    using System.Text;
    using System;
    using UnityEngine;

    internal class OriginalResourcesLoader : ResourceLoader
    {
        public override void DestroyAsset(UnityEngine.Object obj)
        {
            //Resources资源暂时不提供卸载
        }

        public override void DestroyAsset(string path)
        {
            //Resources资源暂时不提供卸载
        }

        public override void DestroyAssetImmediately(UnityEngine.Object obj)
        {
            //Resources资源暂时不提供卸载
        }

        public override void DestroyAssetImmediately(string path)
        {
            //Resources资源暂时不提供卸载
        }

        public override void Init()
        {

        }

        public override int LoadAsset(string rc, LoadAssetCallback callback, object userdata = null)
        {
            string path = GetNameWithoutExtension(rc);
            ResourceRequest rr = Resources.LoadAsync(path);
            rr.completed += (AsyncOperation ao) =>
            {
                LoadResult lr = new LoadResult();
                lr.isSuccess = ao.isDone;
                lr.assets = new UnityEngine.Object[1] { rr.asset };
                lr.assetNames = new string[1] { rc };
                lr.userData = userdata;

                if (ResourceCallBack != null)
                    ResourceCallBack.Invoke(lr);
                if (callback != null)
                    callback.Invoke(lr);
            };

            // Resources 读取暂时一律按照1处理
            return 1;
        }

        public override int LoadAsset(string rc, int priority, LoadAssetCallback callback, object userdata = null)
        {
            return LoadAsset(rc, callback, userdata);
        }

        public override UnityEngine.Object LoadAssetImmediate(string rc)
        {
            throw new NotImplementedException();
        }

        public override int[] LoadAssets(string[] paths, LoadAssetCallback callback, LoadAssetCallback singleCallback, object userdata = null)
        {
            if (paths == null || paths.Length == 0)
            {
                return null;
            }

            int length = paths.Length;
            int[] refCounts = new int[length];

            List<string> errorList = new List<string>();

            LoadResult lr = new LoadResult();
            lr.assets = new UnityEngine.Object[length];
            lr.assetNames = new string[length];
            lr.userData = userdata;

            int sign = 0;
            for (int i = 0; i < length; i++)
            {
                // Resources 读取暂时一律按照1处理
                refCounts[i] = 1;
                string originPath = paths[i];
                string path = GetNameWithoutExtension(originPath);
                GameLogger.DebugLog(LOG_CHANNEL.ASSET, "Load Asset " + path);
                ResourceRequest rr = Resources.LoadAsync(path);

                rr.completed += (AsyncOperation ao) =>
                {
                    sign++;
                    int currentIndex = sign - 1;

                    bool isSuccess = ao.isDone;
                    GameLogger.DebugLog(LOG_CHANNEL.ASSET, string.Format("Asset: [{0}] Loaded, Success: [{1}]", originPath, isSuccess));
                    if (lr.isSuccess)
                    {
                        lr.isSuccess = isSuccess;
                    }
                    if (!isSuccess)
                    {
                        errorList.Add(originPath);
                    }
                    else
                    {
                        lr.assets[currentIndex] = rr.asset;
                        lr.assetNames[currentIndex] = originPath;
                        if (ResourceCallBack != null)
                            ResourceCallBack.Invoke(lr);
                    }

                    LoadResult lrSingle = new LoadResult();
                    lrSingle.isSuccess = isSuccess;
                    lrSingle.assets = new UnityEngine.Object[1] { rr.asset };
                    lrSingle.assetNames = new string[1] { originPath };
                    lrSingle.userData = userdata;
                    if (singleCallback != null)
                        singleCallback.Invoke(lrSingle);
                    if (sign == length)
                    {
                        lr.errorAssetNames = errorList.ToArray();
                        if (callback != null)
                            callback.Invoke(lr);
                    }
                };
            }
            return refCounts;
        }

        public override int[] LoadAssets(string[] rc, int[] priorities, LoadAssetCallback callback, LoadAssetCallback singleCallback, object userdata = null)
        {
            return LoadAssets(rc, callback, singleCallback, userdata);
        }

        public override byte[] LoadByteBuffer(string mainPath)
        {
            return m_StreamingAssetsLoader.Load(mainPath);
        }

        public override byte[] LoadFileInStreamingAssets(string fileName)
        {
            return LoadByteBuffer(fileName);
        }

        public override void LoadImagePack(string relativePath, LoadAssetCallback callback, LoadAssetCallback singleCallback, object userdata = null)
        {
            // TODO 暂时模拟Bundle肯定有卡顿，还没有好的解决方式
            List<string> assetPaths = new List<string>();
            UnityEngine.Object[] packs = Resources.LoadAll(relativePath);
            foreach (UnityEngine.Object pack in packs)
            {
                assetPaths.Add(relativePath + "/" + pack.name);
            }
            LoadAssets(assetPaths.ToArray(), callback, singleCallback, userdata);
        }

        public override byte[] LoadTextBuffer(string mainPath, bool decode)
        {
            return EncodingUtil.FileByteToLocal(this.LoadByteBuffer(mainPath));
        }

        public override string LoadTextString(string mainPath, bool decode)
        {
            return new string(Encoding.UTF8.GetChars(EncodingUtil.FileByteToLocal(this.LoadByteBuffer(mainPath))));
        }

        public override void PrintBundleList()
        {
        }
        public override void Release()
        {

        }

        public override void Tick(uint uDeltaTimeMS)
        {

        }

        private string GetNameWithoutExtension(string fullname)
        {
            string nameWithoutExtension = fullname;
            int lastIndex = fullname.LastIndexOf(".");
            if (lastIndex != -1)
            {
                nameWithoutExtension = fullname.Substring(0, lastIndex);
            }
            return nameWithoutExtension;
        }
    }
}

