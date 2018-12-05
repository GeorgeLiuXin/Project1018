using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XWorld
{
    //一张表
    public class ConfigDataTable : IDisposable
    {
        public string TableName;
        public Dictionary<long, ConfigData> DataMap;
        public ConfigDataTable()
        {
            TableName = "";
            DataMap = new Dictionary<long, ConfigData>();
        }

        public void Dispose()
        {
            foreach (KeyValuePair<long, ConfigData> data in DataMap)
            {
                data.Value.Dispose();
            }
            DataMap.Clear();
            DataMap = null;
        }

        public ConfigData GetData(int pk)
        {
            if (!DataMap.ContainsKey(pk))
                return null;
            return DataMap[pk];
        }
        public ConfigData GetData(int firstPK, int secondPK)
        {
            long longPK = ((long)firstPK << 32) + (long)secondPK;
            ConfigData iData = null;
            if (DataMap.TryGetValue(longPK, out iData))
            {
                return iData;
            }
            return null;
        }

        public ConfigData[] GetAllData()
        {
            return DataMap.Values.ToArray();
        }

        public void AddData(int firstPK, int secondPK, ConfigData data)
        {
            long longID = firstPK;
            if (secondPK >= 0)
            {
                longID = ((long)firstPK << 32) + (long)secondPK;
            }
            if (DataMap.ContainsKey(longID))
            {
                GameLogger.Error(LOG_CHANNEL.ERROR, "Table:" + TableName + ",Has Same Key:" + firstPK + "/" + secondPK);
            }
            DataMap.Add(longID, data);
        }
    }
}