using XWorld;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld.AssetPipeline
{
    internal class AssetLoader
    {
        public string Path;
        public int Priority;
        public LoadAssetCallback Callback;
        public object Userdata;

        // 提供给Array排序
        public int Index;

        public AssetLoader(string path, int priority, LoadAssetCallback callback, object userdata)
        {
            Path = path;
            Priority = priority;
            Callback = callback;
            Userdata = userdata;
        }

        public LoadResult FireCallback(bool isSuccess, bool isScene, UnityEngine.Object target)
        {
#if UNITY_EDITOR
            // TODO 如果是场景需要如何重置shader,让场景模块自己来
            if (target)
            {
                ShaderResetTools.ResetShader(target);
            }
#endif
            LoadResult lr = new LoadResult();
            lr.isSuccess = isSuccess;
            lr.assets = new UnityEngine.Object[1] { target };
            lr.assetNames = new string[1] { Path };
            lr.userData = Userdata;

            if (Callback != null)
                Callback.Invoke(lr);

            return lr;
        }
    }
}