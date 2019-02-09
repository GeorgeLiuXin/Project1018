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

        public Dictionary<string, ConfigData> m_TableDefine;

        private Dictionary<string, string> relativePathToNameDict;

        public GameDataLoader()
        {
            relativePathToNameDict = new Dictionary<string, string>();
        }

        public void StartLoadDefineTableList()
        {
            string path = StaticParam.Config_Define_Path;
            ResourcesProxy.LoadAsset(path, OnLoadDefineTableComplete);
        }

        private void OnLoadDefineTableComplete(LoadResult result)
        {
            if (result.isSuccess)
            {
                bool isSuccess = true;
                string relativePath = result.assetNames[0];
                string content = "";
                try
                {
                    TextAsset asset = result.assets[0] as TextAsset;
                    if (asset == null)
                    {
                        GameLogger.Error(LOG_CHANNEL.ASSET, relativePath + " ERROR! load table asset is NULL,暂时跳过");
                        return;
                    }

                    content = asset.text;
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
                        InitConfigData(content);
                        LoadAllTable();
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

        private void InitConfigData(string content)
        {
            if (m_TableDefine != null)
            {
                foreach (ConfigData data in m_TableDefine.Values)
                {
                    data.Dispose();
                }
                m_TableDefine = null;
            }

            InitDefineTable(content);
        }
        private void InitDefineTable(string sContent)
        {
            string[] values = sContent.Split('\r');
            string[] lables = values[0].TrimStart('\n').Split('\t');
            string[] types = values[1].TrimStart('\n').Split('\t');
            if (values.Length - 2 <= 0)
            {
                GameLogger.Error(LOG_CHANNEL.ASSET, "当前表格行数不足三行!");
                return;
            }

            m_TableDefine = new Dictionary<string, ConfigData>();
            for (int i = 2; i < values.Length; i++)
            {
                string[] subValues = values[i].TrimStart('\n').Split('\t');
                if (subValues.Length != lables.Length)
                    continue;
                ConfigData data = new ConfigData();
                for (int j = 0; j < subValues.Length; j++)
                {
                    data.AddValue(types[j], lables[j], subValues[j]);
                }
                m_TableDefine.Add(data.GetString("tableName"), data);
                relativePathToNameDict.Add(data.GetString("tablePath"), data.GetString("tableName"));
            }
        }

        private void LoadAllTable()
        {
            if (m_TableDefine == null || relativePathToNameDict == null || relativePathToNameDict.Count == 0)
            {
                GameLogger.DebugLog(LOG_CHANNEL.ERROR, "数据管理表尚未加载完毕");
                return;
            }
            foreach (string path in relativePathToNameDict.Keys)
            {
                ResourcesProxy.LoadAsset(path, OnLoadTableComplete);
            }
        }
        public void LoadTable(string name)
        {
            ConfigData data = m_TableDefine[name];
            string path = data.GetString("tablePath");
            ResourcesProxy.LoadAsset(path, OnLoadTableComplete);
        }

        private void OnLoadTableComplete(LoadResult result)
        {
            if (result.isSuccess)
            {
                bool isSuccess = true;
                string content = "";
                string relativePath = result.assetNames[0];
                try
                {
                    TextAsset asset = result.assets[0] as TextAsset;
                    if (asset == null)
                    {
                        GameLogger.Error(LOG_CHANNEL.ASSET, relativePath + " ERROR! load table asset is NULL,暂时跳过");
                        return;
                    }

                    content = asset.text;
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
                        string name = relativePathToNameDict[relativePath];
                        ConfigData data = m_TableDefine[name];
                        string tableName = data.GetString("tableName");
                        string tablePath = data.GetString("tablePath");
                        string tablePK2 = data.GetString("extKey");
                        
                        ConfigDataTableManager.Instance.LoadTable(tableName, content, tablePK2);
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