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
	/// <summary>
	/// 修改读表方式
	/// 修改之前的生成
    /// 将表格在当前位置读取出来
	/// 
	/// 添加表格的重读方法，提升改表效率
	/// </summary>
	public class ConfigDataTableManager : Singleton<ConfigDataTableManager>
	{
		private ConfigDataTable m_TableDefine;
		private Dictionary<string, ConfigDataTable> m_TableMap;

		public ConfigDataTableManager()
		{
			m_TableMap = new Dictionary<string, ConfigDataTable>();
		}

        public string LoadTxtContentFromPath(string path)
        {
            ResourcesProxy.LoadAsset(path)
            return "";
        }

		public void LoadDefineTableList(string tableName, string sContent)
        {
            if (m_TableDefine != null)
            {
                m_TableDefine.Dispose();
                m_TableDefine = null;
            }
            m_TableDefine = LoadTable(tableName, sContent);
            m_TableDefine.TableName = tableName;
        }
        
        public void LoadAllTable()
        {
            if (m_TableDefine == null)
            {
                GameLogger.DebugLog(LOG_CHANNEL.ERROR, "数据管理表尚未加载完毕");
                return;
            }
            ConfigData[] datas = m_TableDefine.GetAllData();
            foreach (ConfigData data in datas)
            {
                string tableName = data.GetString("tableName");
                string tablePath = data.GetString("tablePath");
                string tablePK2 = data.GetString("extKey");

                string content = LoadTxtContentFromPath(tablePath);
                ConfigDataTable table = LoadTable(tableName, content, tablePK2);
                table.TableName = tableName;
                m_TableMap.Add(tableName, table);
            }
        }

        public ConfigDataTable LoadTable(string tableName, string sContent, string pk2 = default(string))
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
                    return null;
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

                table.AddData(Convert.ToInt32(subValues[0]), pk2Index, data);
            }
            return table;
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

		public void ReloadTable(string tableName)
		{

		}

	}

}