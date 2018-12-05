using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System;
using System.Linq;

namespace XWorld
{
    /// <summary>
    /// 修改读表方式
    /// 修改之前的生成
    /// 
    /// 添加表格的重读方法，提升改表效率
    /// </summary>
    public class ConfigDataTableManager : Singleton<ConfigDataTableManager>
    {
        public void Clear()
        {
            foreach (KeyValuePair<string, ConfigDataTable> def in m_TableMap)
            {
                def.Value.Dispose();
            }
            m_TableMap.Clear();
            m_TableMap = null;
            m_DefineMap.Clear();
            m_DefineMap = null;
        }

        public int GetGroupCount() { return m_TableMap.Count; }
        public ConfigDataTable GeTable(string table)
        {
            ConfigDataTable info = null;
            if (m_TableMap.TryGetValue(table, out info))
            {
                return info;
            }
            return null;
        }

        public void AddConfigDefine(client_config_define def)
        {
            m_DefineMap.Add(def.tableName, def);
        }

        public void ClearAllTable()
        {
            foreach (KeyValuePair<string, client_config_define> def in m_DefineMap)
            {
                ClearTable(def.Key);
            }
        }
        public void ClearTable(string tableName)
        {
            ConfigDataTable info = null;
            if (m_TableMap.TryGetValue(tableName, out info))
            {
                info.Dispose();
            }
        }
        public void LoadTable(string tableName, string sContent)
        {
            client_config_define def = null;
            if (!m_DefineMap.TryGetValue(tableName, out def))
            {
                //log;
                return;
            }

            ConfigDataTable table = new ConfigDataTable();
            table.mTableName = tableName;
            m_TableMap.Add(tableName, table);
            //pass content
            int idxkey1 = -1;
            string[] values = sContent.Split("\r"[0]);
            string[] lables = values[0].TrimStart('\n').Split(ClientConfigManager.CMD_STRING, StringSplitOptions.None);
            string[] types = values[1].TrimStart('\n').Split(ClientConfigManager.CMD_STRING, StringSplitOptions.None);
            for (int i = 0; i < lables.Length; ++i)
            {
                if (lables[i] == def.tableKey2)
                {
                    idxkey1 = i;
                }
            }
            List<ConfigData> dataList = new List<ConfigData>();
            for (int i = 2; (i < values.Length); i = (i + 1))
            {
                string[] subValues = values[i].TrimStart('\n').Split(ClientConfigManager.CMD_STRING, StringSplitOptions.None);
                if (subValues.Length != lables.Length)
                    continue;
                ConfigData data = new ConfigData();
                int kV2 = -1;
                if (idxkey1 >= 0)
                {
                    kV2 = Convert.ToInt32(subValues[idxkey1]);
                }
                for (int a = 0; a < subValues.Length; a++)
                {
                    data.AddValue(types[a], lables[a], subValues[a]);
                }
                LoadOberserver ob = null;
                if (m_LoadOberserver.TryGetValue(tableName, out ob))
                {
                    ob(data);
                    data = null;
                }
                else
                {
                    if (!m_LoadAllOberserver.ContainsKey(tableName))
                    {
                        table.AddData(Convert.ToInt32(subValues[0]), kV2, data);
                    }
                    else
                    {
                        dataList.Add(data);
                    }
                }
            }

            LoadAllOberserver obAll = null;
            if (m_LoadAllOberserver.TryGetValue(tableName, out obAll))
            {
                ConfigData[] arr = dataList.ToArray();
                obAll(ref arr);
                dataList.Clear();
                dataList = null;
            }
        }
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
        public ConfigData[] GetAllData(string tableName)
        {
            ConfigDataTable table = GeTable(tableName);
            if (table == null)
            {
                return null;
            }
            return table.GetAllData();
        }
        public ConfigData GetData(string tableName, int key)
        {
            ConfigDataTable table = GeTable(tableName);
            if (table != null)
            {
                return table.GetData(key);
            }
            return null;
        }

        public ConfigData GetData(string tableName, int key, int key1)
        {
            ConfigDataTable table = GeTable(tableName);
            if (table != null)
            {
                return table.GetData(key, key1);
            }
            return null;
        }

        public delegate void LoadOberserver(ConfigData data);
        public delegate void LoadAllOberserver(ref ConfigData[] data);
        Dictionary<string, client_config_define> m_DefineMap = new Dictionary<string, client_config_define>();
        Dictionary<string, ConfigDataTable> m_TableMap = new Dictionary<string, ConfigDataTable>();
        Dictionary<string, LoadOberserver> m_LoadOberserver = new Dictionary<string, LoadOberserver>();
        Dictionary<string, LoadAllOberserver> m_LoadAllOberserver = new Dictionary<string, LoadAllOberserver>();
    }
   


}