using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public delegate void LoadAssetCallback(LoadResult result);

    public class LoadResult
    {
        public bool isSuccess = true;
        public string[] assetNames;
        public UnityEngine.Object[] assets;
        public string[] errorAssetNames;
        public object userData;

        // 提供给Array排序
        public int index;
    }
}
