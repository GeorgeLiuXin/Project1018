using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XWorld;
using XWorld.AssetPipeline;

public class TBundleManagerTest : MonoBehaviour
{
    BundleManager bundleManager;
    private void Start()
    {
     //   GameMain.OnCheckComplete.AddListener(OnCheckComplete);
    }

    private void OnCheckComplete()
    {
        bundleManager = XWorldGameModule.GetGameManager<BundleManager>();
        bundleManager.BundleLoadFail += BundleLoadFail;
        bundleManager.BundleLoadStart += BundleLoadStart;
        bundleManager.BundleLoadSuccess += BundleLoadSuccess;
        bundleManager.BundleLoadUpdate += BundleLoadUpdate;
        bundleManager.StartLoad("normal/material/material0001_d8099726e312b0d9709669779659b400.dat", this, 1);
        bundleManager.Unload("normal/material/material0001_d8099726e312b0d9709669779659b400.dat", this, true);
        bundleManager.StartLoad("normal/material/material0001_d8099726e312b0d9709669779659b400.dat", this, 1);
        bundleManager.StartLoad("normal/bytes/static/bytes0001_7bd11b3b628cafc28375477702935f00.dat", this, 2);
        bundleManager.StartLoad("normal/prefab/prefab0001_550787115b94a9a9d459b2cf8a81e732.dat", this, 3);
        bundleManager.StartLoad("normal/prefab/lamp/prefab0001_35f352ba51ab66a9bbfb17fab08fe8f0.dat", this, 4);
        bundleManager.StartLoad("normal/texture/texture0001_2b757ad45c3288eb130c039661482931.dat", this, 1);
        bundleManager.StartLoad("normal/texture/texture0001_2b757ad45c3288eb130c039661482931.dat", this, 1);

        Invoke("ShowCache", 2f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            bundleManager.StartLoad("normal/material/material0001_d8099726e312b0d9709669779659b400.dat", this, 1);
            ShowCache();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            bundleManager.Unload("normal/material/material0001_d8099726e312b0d9709669779659b400.dat", this, false);
            ShowCache();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            bundleManager.Unload("normal/material/material0001_d8099726e312b0d9709669779659b400.dat", this, true);
            ShowCache();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            bundleManager.ForceUnload("normal/material/material0001_d8099726e312b0d9709669779659b400.dat", this, true);
            ShowCache();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            bundleManager.Collect();
            ShowCache();
        }
    }

    void ShowCache()
    {
        BundleRefCache cache = bundleManager.Cache;
        Dictionary<string, BundleReference> bundleRefDict = cache.BundleRefDict;

        foreach (BundleReference b in bundleRefDict.Values)
        {
            //Debug.Log("CACHE   " + b.BundlePath + "  " + b.RefCount + "  " + b.Progress);
        }
    }

    private void BundleLoadUpdate(object sender, BundleLoadUpdateEventArgs e)
    {
       // Debug.Log("update  " + e.BundlePath + "  " + e.Progress);
    }

    private void BundleLoadSuccess(object sender, BundleLoadSuccessEventArgs e)
    {
     //   Debug.Log("success  " + e.BundlePath + "  " + e.Bundle);
    }

    private void BundleLoadStart(object sender, BundleLoadStartEventArgs e)
    {
        Debug.Log("start  " + e.BundlePath + "  " + Time.frameCount);
    }

    private void BundleLoadFail(object sender, BundleLoadFailEventArgs e)
    {
        Debug.Log("fail  " + e.BundlePath + "  " + e.ErrorContent);
    }
}

