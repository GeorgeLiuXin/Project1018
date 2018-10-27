using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XWorld.AssetPipeline
{
    public class TAssetManagerTest : MonoBehaviour
    {
        AssetManager assetManager;
        BundleManager bundleManager;
        private void Start()
        {
          //  GameMain.OnCheckComplete.AddListener(OnCheckComplete);
        }

        private void OnCheckComplete()
        {
            assetManager = XWorldGameModule.GetGameManager<AssetManager>();
            assetManager.AssetLoadFail += AssetLoadFail;
            assetManager.AssetLoadStart += AssetLoadStart;
            assetManager.AssetLoadSuccess += AssetLoadSuccess;
            assetManager.AssetLoadUpdate += AssetLoadUpdate;

            bundleManager = XWorldGameModule.GetGameManager<BundleManager>();
            bundleManager.BundleLoadFail += BundleLoadFail;
            bundleManager.BundleLoadStart += BundleLoadStart;
            bundleManager.BundleLoadSuccess += BundleLoadSuccess;
            bundleManager.BundleLoadUpdate += BundleLoadUpdate;

            assetManager.StartLoad("Assets/AssetDatas/Prefabs/Lamp/table_lamp.prefab", this, 1);
        }

        private void BundleLoadUpdate(object sender, BundleLoadUpdateEventArgs e)
        {
            GameLogger.DebugLog(LOG_CHANNEL.LOGIC, "update bundle : " + e.BundlePath + "  " + e.Progress + "  " + Time.frameCount);
        }

        private void BundleLoadSuccess(object sender, BundleLoadSuccessEventArgs e)
        {
            GameLogger.DebugLog(LOG_CHANNEL.NETWORK, "success bundle : " + e.BundlePath + "  " + Time.frameCount);
        }

        private void BundleLoadStart(object sender, BundleLoadStartEventArgs e)
        {
            GameLogger.DebugLog(LOG_CHANNEL.NETWORK, "start bundle : " + e.BundlePath + "  " + Time.frameCount);
        }

        private void BundleLoadFail(object sender, BundleLoadFailEventArgs e)
        {
            GameLogger.DebugLog(LOG_CHANNEL.NETWORK, "fail bundle : " + e.BundlePath + "  " + e.ErrorContent + "  " + Time.frameCount);
        }

        private void AssetLoadUpdate(object sender, AssetLoadUpdateEventArgs e)
        {
            GameLogger.DebugLog(LOG_CHANNEL.LOGIC, "update asset : " + e.AssetPath + "  " + e.Progress + "  " + Time.frameCount);
        }

        private void AssetLoadSuccess(object sender, AssetLoadSuccessEventArgs e)
        {
            GameLogger.DebugLog(LOG_CHANNEL.LOGIC, "success asset : " + e.AssetPath + "  " + Time.frameCount);

            UnityEngine.Object.Instantiate(e.Asset);
        }

        private void AssetLoadStart(object sender, AssetLoadStartEventArgs e)
        {
            GameLogger.DebugLog(LOG_CHANNEL.LOGIC, "start asset : " + e.AssetPath + "  " + Time.frameCount);
        }

        private void AssetLoadFail(object sender, AssetLoadFailureEventArgs e)
        {
            GameLogger.DebugLog(LOG_CHANNEL.LOGIC, "fail asset : " + e.AssetPath + "  " + e.ErrorContent + "  " + Time.frameCount);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                assetManager.StartLoad("Assets/AssetDatas/Prefabs/Lamp/table_lamp.prefab", this, 1);
                ShowCache();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                assetManager.Unload("Assets/AssetDatas/Prefabs/Lamp/table_lamp.prefab", this, false);
                ShowCache();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                assetManager.Unload("Assets/AssetDatas/Prefabs/Lamp/table_lamp.prefab", this, true);
                ShowCache();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
             //   bundleManager.ForceUnload("normal/material/material0001_d8099726e312b0d9709669779659b400.dat", this, true);
                ShowCache();
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
           //     bundleManager.Collect();
                ShowCache();
            }
        }

        void ShowCache()
        {
            AssetRefCache cache = assetManager.Cache;
            Dictionary<string, AssetReference> assetRefDict = cache.AssetRefDict;

            foreach (AssetReference b in assetRefDict.Values)
            {
                Debug.Log("CACHE   " + b.AssetPath + "  " + b.RefCount + "  " + b.Progress);
            }
        }
    }
}
