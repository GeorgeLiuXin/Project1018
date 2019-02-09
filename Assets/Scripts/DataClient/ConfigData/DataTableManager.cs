using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using XWorld.AssetPipeline;
using XWorld.GameData;

namespace XWorld.GameData
{
	public class ConfigDataTableManager : Singleton<ConfigDataTableManager>
    {
        private Dictionary<string, ConfigDataTable> m_TableMap;

		public ConfigDataTableManager()
		{
			m_TableMap = new Dictionary<string, ConfigDataTable>();
        }
        
        public void OnLoadTableComplete(string tableName, string sContent)
        {

        }
        public void LoadTable(string tableName, string sContent, string pk2 = default(string))
        {
            int pk2Index = -1;
            string[] values = sContent.Split('\r');
            string[] lables = values[0].TrimStart('\n').Split('\t');
            string[] types = values[1].TrimStart('\n').Split('\t');
            if (pk2 != default(string))
            {
                for (int i = 0; i < lables.Length; i++)
                {
                    if (lables[i] == pk2)
                    {
                        pk2Index = i;
                        break;
                    }
                }
                if (pk2Index == -1)
                {
                    GameLogger.Error(LOG_CHANNEL.ERROR, "当前表中原注册的第二主键丢失");
                    return;
                }
            }

            ConfigDataTable table = new ConfigDataTable();
            table.TableName = tableName;
            for (int i = 2; i < values.Length; i++)
            {
                string[] subValues = values[i].TrimStart('\n').Split('\t');
                if (subValues.Length != lables.Length)
                    continue;
                ConfigData data = new ConfigData();
                if (pk2Index != -1)
                {
                    pk2Index = Convert.ToInt32(subValues[pk2Index]);
                }

                for (int j = 0; j < subValues.Length; j++)
                {
                    data.AddValue(types[j], lables[j], subValues[j]);
                }

                if (m_LoadOberserver.ContainsKey(tableName))
                {
                    m_LoadOberserver[tableName](data);
                    data = null;
                }
                else
                {
                    table.AddData(Convert.ToInt32(subValues[0]), pk2Index, data);
                }
            }

            if (m_LoadAllOberserver.ContainsKey(tableName))
            {
                ConfigData[] datas = table.GetAllData();
                m_LoadAllOberserver[tableName](ref datas);
                table.Dispose();
                table = null;
            }
            else
            {
                m_TableMap.Add(tableName, table);
            }
            GameLogger.DebugLog(LOG_CHANNEL.ASSET, tableName + "表格已加载完毕!");
        }

        private ConfigDataTable GetTable(string tableName)
		{
			if (!m_TableMap.ContainsKey(tableName))
				return null;
			return m_TableMap[tableName];
		}

		public ConfigData GetData(string tableName, int key)
		{
			ConfigDataTable table = GetTable(tableName);
			if (table == null)
				return null;
			return table.GetData(key);
		}

		public ConfigData GetData(string tableName, int pk1, int pk2)
		{
			ConfigDataTable table = GetTable(tableName);
			if (table == null)
				return null;
			return table.GetData(pk1, pk2);
		}

		public ConfigData[] GetAllData(string tableName)
		{
			ConfigDataTable table = GetTable(tableName);
			if (table == null)
				return null;
			return table.GetAllData();
		}

		public void ClearTable(string tableName)
		{
			ConfigDataTable table = null;
			if (m_TableMap.TryGetValue(tableName, out table))
			{
				table.Dispose();
			}
		}

		public void ClearAllTable()
		{
			foreach (KeyValuePair<string, ConfigDataTable> table in m_TableMap)
			{
				table.Value.Dispose();
			}
		}

		public void Clear()
		{
			foreach (KeyValuePair<string, ConfigDataTable> table in m_TableMap)
			{
				table.Value.Dispose();
			}
			m_TableMap.Clear();
			m_TableMap = null;
		}

        #region 数据托管给对应自编写管理类

        public delegate void LoadOberserver(ConfigData data);
        public delegate void LoadAllOberserver(ref ConfigData[] data);
        private Dictionary<string, LoadOberserver> m_LoadOberserver = new Dictionary<string, LoadOberserver>();
        private Dictionary<string, LoadAllOberserver> m_LoadAllOberserver = new Dictionary<string, LoadAllOberserver>();

        public void AddLoadOberserver(string tableName, LoadOberserver ob)
        {
            if (ob != null)
            {
                if (m_LoadOberserver.ContainsKey(tableName))
                    m_LoadOberserver[tableName] += ob;
                else
                    m_LoadOberserver[tableName] = ob;
            }
        }
        public void AddLoadAllOberserver(string tableName, LoadAllOberserver ob)
        {
            if (ob != null)
            {
                m_LoadAllOberserver[tableName] = ob;
            }
        }

        #endregion

    }

}