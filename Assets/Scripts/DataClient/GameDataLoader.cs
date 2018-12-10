using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XWorld.GameData;

namespace XWorld
{
    /// <summary>
    /// 面向加载   连接了资源层与数据层
    /// </summary>
    public class GameDataLoader
    {

        private Dictionary<string, string> relativePathToNameDict;

        public GameDataLoader()
        {
            relativePathToNameDict = new Dictionary<string, string>();
        }

        public void LoadDefineTableList()
        {
            string path = StaticParam.CONFIG_DEFINE_PATH;
            ResourcesProxy.LoadAsset(path, OnLoadDefineTableComplete);
        }

        private void OnLoadDefineTableComplete(LoadResult result)
        {
            if (result.isSuccess)
            {
                bool isSuccess = true;
                string relativePath = result.assetNames[0];
                try
                {
                    TextAsset asset = result.assets[0] as TextAsset;
                    if (asset == null)
                    {
                        GameLogger.Error(LOG_CHANNEL.ASSET, relativePath + " ERROR! load table asset is NULL,暂时跳过");
                        return;
                    }

                    string content = asset.text;
                    if (string.IsNullOrEmpty(content))
                    {
                        GameLogger.Error(LOG_CHANNEL.ASSET, relativePath + " ERROR! load table content is NULL or empty,暂时跳过");
                        return;
                    }
                    
                    ResourcesProxy.DestroyAsset(relativePath);
                }
                catch (Exception e)
                {
                    isSuccess = false;
                    GameLogger.Error(LOG_CHANNEL.ASSET, relativePath + " Parse Failed : " + e.ToString());
                }
                finally
                {
                    if (isSuccess)
                    {
                        
                    }
                    else
                    {
                        //TODO 留给之后对应加载的job使用
                    }
                }
            }
            else
            {
                //TODO 留给之后对应加载的job使用
                GameLogger.Error(LOG_CHANNEL.ASSET, result.assetNames[0] + " Load Failed!");
            }
        }

        public void LoadAllTable()
        {

        }

        private void OnLoadTableComplete(LoadResult result)
        {
            if (result.isSuccess)
            {
                bool isSuccess = true;
                string relativePath = result.assetNames[0];
                try
                {
                    TextAsset asset = result.assets[0] as TextAsset;
                    if (asset == null)
                    {
                        GameLogger.Error(LOG_CHANNEL.ASSET, relativePath + " ERROR! load table asset is NULL,暂时跳过");
                        return;
                    }

                    string content = asset.text;
                    if (string.IsNullOrEmpty(content))
                    {
                        GameLogger.Error(LOG_CHANNEL.ASSET, relativePath + " ERROR! load table content is NULL or empty,暂时跳过");
                        return;
                    }

                    ResourcesProxy.DestroyAsset(relativePath);
                }
                catch (Exception e)
                {
                    isSuccess = false;
                    GameLogger.Error(LOG_CHANNEL.ASSET, relativePath + " Parse Failed : " + e.ToString());
                }
                finally
                {
                    if (isSuccess)
                    {
                        ConfigDataTableManager.Instance.LoadTable()
                    }
                    else
                    {
                        //TODO 留给之后对应加载的job使用
                    }
                }
            }
            else
            {
                //TODO 留给之后对应加载的job使用
                GameLogger.Error(LOG_CHANNEL.ASSET, result.assetNames[0] + " Load Failed!");
            }
        }
    }

}