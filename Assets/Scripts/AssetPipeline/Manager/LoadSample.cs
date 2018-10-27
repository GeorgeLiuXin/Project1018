using XWorld;
using XWorld;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadSample : MonoBehaviour
{
    string atlasPath1 = "Models/cube.prefab";
    string atlasPath2 = "Models/EventSystem.prefab";
    string atlasExtension = ".prefab";
    GameObject atlas1;
    GameObject atlas2;

    void Start()
    {
        Invoke("DoStart",0.5f);
    }

    void DoStart()
    {
        ResourcesProxy.LoadImagePack("Test", OnSingle, OnLoad);
        // ResourcesProxy.LoadAssets(new string[2] { atlasPath1, atlasPath2 }, OnLoadComplete, (int)1);
    }

    private void OnLoad(LoadResult result)
    {
        Debug.Log("OnLoad  " + result.isSuccess);
        Debug.Log("OnLoad  " + string.Join(", ", result.assetNames));
    }

    private void OnSingle(LoadResult result)
    {
        Texture2D tex = result.assets[0] as Texture2D;

        Debug.Log("OnSingle  " + result.isSuccess);
        Debug.Log("OnSingle  " + string.Join(", ",result.assetNames));
    }

    //传统Resources读法
    IEnumerator OldResourcesLoad()
    {
        ResourceRequest rq = Resources.LoadAsync(atlasPath1);
        yield return rq;
        if (rq.isDone)
        {
            atlas1 = rq.asset as GameObject;
        }

        rq = Resources.LoadAsync(atlasPath2);
        yield return rq;
        if (rq.isDone)
        {
            atlas2 = rq.asset as GameObject;
        }
    }
    //传统Resources卸载
    void OldResourcesDestroy()
    {
        atlas1 = null;
        atlas2 = null;

        //这个API只能卸载mesh，audio，texture等源文件素材，卸载GameObject会报错
        //Resources.UnloadAsset();

        //便利整个内存，卸载无人引用的资源，效率类似于System.GC.Collect();，会出现卡顿
        Resources.UnloadUnusedAssets();
    }

    //ResourcesProxy读法
    void ResourcesProxyLoad()
    {
        //开启Editor模式请先修改配置文件，目前默认是开启的Editor模式

        //自己随便测试的话，打开这个选项才能在Editor下开启读取
        GlobalAssetSetting.EnableEditorSimulation = true;
        //走正式（带本地Config加载）流程的话修改配置文件 PreInit.cfg中的Debug 数值要修改为1


        ResourcesProxy.LoadAsset(atlasPath1 + atlasExtension, OnLoadComplete, (int)1);
        ResourcesProxy.LoadAsset(atlasPath2 + atlasExtension, OnLoadComplete, (int)2);
    }

    //ResourcesProxy卸载
    void ResourcesProxyDestroy()
    {
        ResourcesProxy.DestroyAsset(atlasPath1 + atlasExtension);
        //or
        ResourcesProxy.DestroyAsset(atlas1);
    }

    private void OnLoadComplete(LoadResult result)
    {
        if (result.isSuccess)
        {
            if ((int)result.userData == 1)
            {
                atlas1 = result.assets[0] as GameObject;
            }
            else if ((int)result.userData == 2)
            {
                atlas2 = result.assets[0] as GameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
