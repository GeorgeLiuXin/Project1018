using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XWorld
{
    public class TResourcesProxyTest : MonoBehaviour
    {
        void Start()
        {
        //    GameMain.OnCheckComplete.AddListener(OnCheckComplete);
        }

        private void OnCheckComplete()
        {
            Debug.LogError("Start");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                ResourcesProxy.LoadAsset("Prefabs/chair.prefab", OnChairLoadComplete, 1);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                ResourcesProxy.LoadAssets(new string[] { "Prefabs/computer.prefab", "Prefabs/lustre.prefab" }, OnComplete, OnSingleLoadComplete, 10);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                ResourcesProxy.DestroyAsset("Prefabs/computer.prefab");
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                ResourcesProxy.DestroyAsset("Prefabs/lustre.prefab");
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                ResourcesProxy.DestroyAssetImmediately("Prefabs/lustre.prefab");
            }
        }

        private void OnComplete(LoadResult result)
        {
            Debug.Log("OnComplete  " + result.isSuccess + "  " + result.assets.Length + "  " + result.userData);
            if (result.isSuccess)
            {
                foreach (UnityEngine.Object obj in result.assets)
                {
                    GameObject.Instantiate(obj);
                }
            }
        }

        private void OnSingleLoadComplete(LoadResult result)
        {
            Debug.Log("OnSingleLoadComplete  " + result.isSuccess + "  " + result.userData);
            if (result.isSuccess)
            {
                GameObject.Instantiate(result.assets[0]);
                ResourcesProxy.DestroyAssetImmediately("Prefabs/lustre.prefab");
            }
        }

        private void OnChairLoadComplete(LoadResult result)
        {
            Debug.Log("OnChairLoadComplete  " + result.isSuccess + "  " + result.userData);
            if (result.isSuccess)
            {
                GameObject.Instantiate(result.assets[0]);
            }
            ResourcesProxy.DestroyAsset("Prefabs/chair.prefab");
        }
    }
}
