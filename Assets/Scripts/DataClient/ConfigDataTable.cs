using System;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public class ConfigDataTable : IDisposable
    {
        public void AddData(int id, int id1, ConfigData data)
        {
            long rid = id;
            if (id1 >= 0)
            {
                rid = ((long)id << 32) + (long)id1;
            }
            if (DataMap.ContainsKey(rid))
            {
                GameLogger.Error(LOG_CHANNEL.ERROR, "Table:" + mTableName + ",Has Same Key:" + id + "/" + id1);
            }
            DataMap.Add(rid, data);
        }
        public ConfigData GetData(int id)
        {
            ConfigData iData = null;
            if (DataMap.TryGetValue(id, out iData))
            {
                return iData;
            }
            return null;
        }

        public ConfigData[] GetAllData()
        {
            return DataMap.Values.ToArray();
        }

        public ConfigData GetData(int id, int id2)
        {
            long rID = ((long)id << 32) + (long)id2;
            ConfigData iData = null;
            if (DataMap.TryGetValue(rID, out iData))
            {
                return iData;
            }
            return null;
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

        // TODO 以后去掉该接口
        public List<ConfigData> GetConfigs()
        {
            return new List<ConfigData>(DataMap.Values);
        }
        public string mTableName = "";
        Dictionary<long, ConfigData> DataMap = new Dictionary<long, ConfigData>();
    }

}