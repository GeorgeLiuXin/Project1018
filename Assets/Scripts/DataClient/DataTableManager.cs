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
    public class ConfigData : IDisposable
    {
        public interface IConfigData
        {
            bool GetBool();
            int GetInt();
            uint GetUint();
            long GetInt64();
            ulong GetUint64();
            float GetFloat();
            string GetString();
        }
        class Data<TValue> : IConfigData
        {
            public TValue Value;
            public bool GetBool()
            {
                return Convert.ToBoolean(Value);
            }
            public int GetInt()
            { 
                return Convert.ToInt32(Value);
            }
            public uint GetUint()
            {
                return Convert.ToUInt32(Value);
            }
            public long GetInt64()
            {
                return Convert.ToInt64(Value);
            }
            public ulong GetUint64()
            {
                return Convert.ToUInt64(Value);
            }
            public float GetFloat()
            {
                return Convert.ToSingle(Value);
            }
            public string GetString()
            {
                return Convert.ToString(Value);
            }

            public void SetValue(TValue tvalue)
            {
                Value = tvalue;
            }
        }
        public ConfigData()
        {
        }
        public bool GetBool(string sName)
        {
            IConfigData iData = null;
            if (DataMap.TryGetValue(sName, out iData))
            {
                return iData.GetBool();
            }
            return false;
        }
        public int GetInt(string sName)
        {
            IConfigData iData = null;
            if (DataMap.TryGetValue(sName, out iData))
            {
                return iData.GetInt();
            }
            return 0;
        }
        public uint GetUint(string sName)
        {
            IConfigData iData = null;
            if (DataMap.TryGetValue(sName, out iData))
            {
                return iData.GetUint();
            }
            return 0;
        }

        public long GetInt64(string sName)
        {
            IConfigData iData = null;
            if (DataMap.TryGetValue(sName, out iData))
            {
                return iData.GetInt64();
            }
            return 0;
        }
        public ulong GetUint64(string sName)
        {
            IConfigData iData = null;
            if (DataMap.TryGetValue(sName, out iData))
            {
                return iData.GetUint64();
            }
            return 0;
        }

        public float GetFloat(string sName)
        {
            IConfigData iData = null;
            if (DataMap.TryGetValue(sName, out iData))
            {
                return iData.GetFloat();
            }
            return 0.0f;
        }
        public string GetString(string sName)
        {
            IConfigData iData = null;
            if (DataMap.TryGetValue(sName, out iData))
            {
                return iData.GetString();
            }
            return "";
        }

        public void AddValue(string sTypes, string sName, string sValue)
        {
            if (sTypes == "int8" || 
                sTypes == "int16" ||
                sTypes == "int32"
                )
            {
                Data<int> iData = new Data<int>();
                int iv = 0;
                if (int.TryParse(sValue, out iv))
                {
                    iData.SetValue(iv);
                }
                else
                {
                    iData.SetValue(0);
                }
                DataMap.Add(sName, iData);
            } 
            else if(sTypes == "uint8" ||
                sTypes == "uint16" ||
                sTypes == "uint32")
            {
                Data<uint> uiData = new Data<uint>();
                uint uiv = 0;
                if (uint.TryParse(sValue, out uiv))
                {
                    uiData.SetValue(uiv);
                }
                else
                {
                    uiData.SetValue(0);
                }
                DataMap.Add(sName, uiData);
            }
            else if (sTypes == "int64")
            {
                Data<long> i64Data = new Data<long>();
                long i64v = 0;
                if (long.TryParse(sValue, out i64v))
                {
                    i64Data.SetValue(i64v);
                }
                else
                {
                    i64Data.SetValue(0);
                }
                DataMap.Add(sName, i64Data);
            }
            else if (sTypes == "uint64")
            {
                Data<ulong> ui64Data = new Data<ulong>();
                ulong ui64v = 0;
                if (ulong.TryParse(sValue, out ui64v))
                {
                    ui64Data.SetValue(ui64v);
                }
                else
                {
                    ui64Data.SetValue(0);
                }
                DataMap.Add(sName, ui64Data);
            }
            else if (sTypes == "f32" || sTypes == "f64")
            {
                Data<float> fData = new Data<float>();
                float fv = 0;
                if (float.TryParse(sValue, out fv))
                {
                    fData.SetValue(fv);
                }
                else
                {
                    fData.SetValue(0);
                }
                DataMap.Add(sName, fData);
            }
            else if (sTypes == "char")
            {
                Data<string> sData = new Data<string>();
                sData.SetValue(sValue);
                DataMap.Add(sName, sData);
            }
            else if (sTypes == "bool")
            {
                Data<bool> boolData = new Data<bool>();
                bool b = false;
                if (bool.TryParse(sValue, out b))
                {
                    boolData.SetValue(b);
                }
                else
                {
                    boolData.SetValue(false);
                }
                DataMap.Add(sName, boolData);
            }
        }       
        public void Dispose()
        {
            DataMap.Clear();
            DataMap = null;
        }
      
        Dictionary<string, IConfigData> DataMap = new Dictionary<string, IConfigData>();
    }
    public class ConfigDataTable : IDisposable
    {       
        public void AddData(int id, int id1, ConfigData data)
        {
            long rid = id;
            if (id1>=0)
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
            foreach(KeyValuePair<long, ConfigData> data in DataMap)
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
    
    public class ConfigDataTableManager : Singleton<ConfigDataTableManager>
    {
        //public void Clear()
        //{
        //    foreach (KeyValuePair<string, ConfigDataTable> def in m_TableMap)
        //    {
        //        def.Value.Dispose();
        //    }
        //    m_TableMap.Clear();
        //    m_TableMap = null;
        //    m_DefineMap.Clear();
        //    m_DefineMap = null;
        //}
        
        //public int GetGroupCount(){return m_TableMap.Count;}
        //public ConfigDataTable GeTable(string table)
        //{
        //    ConfigDataTable info = null;
        //    if (m_TableMap.TryGetValue(table, out info))
        //    {
        //        return info;
        //    }
        //    return null;
        //}

        //public void AddConfigDefine(client_config_define def)
        //{
        //    m_DefineMap.Add(def.tableName, def);
        //}

        //public void ClearAllTable()
        //{
        //    foreach (KeyValuePair<string, client_config_define> def in m_DefineMap)
        //    {
        //        ClearTable(def.Key);
        //    }
        //}
        //public void ClearTable(string tableName)
        //{
        //    ConfigDataTable info = null;
        //    if (m_TableMap.TryGetValue(tableName, out info))
        //    {
        //        info.Dispose();
        //    }
        //}
        //public void LoadTable(string tableName, string sContent)
        //{
        //    client_config_define def = null;
        //    if (!m_DefineMap.TryGetValue(tableName, out def))
        //    {
        //        //log;
        //        return;
        //    }
           
        //    ConfigDataTable table = new ConfigDataTable();
        //    table.mTableName = tableName;
        //    m_TableMap.Add(tableName, table);
        //    //pass content
        //    int idxkey1 = -1;
        //    string[] values = sContent.Split("\r"[0]);
        //    string[] lables = values[0].TrimStart('\n').Split(ClientConfigManager.CMD_STRING, StringSplitOptions.None);
        //    string[] types = values[1].TrimStart('\n').Split(ClientConfigManager.CMD_STRING, StringSplitOptions.None);
        //    for (int i = 0; i < lables.Length; ++i)
        //    {
        //        if (lables[i] == def.tableKey2)
        //        {
        //            idxkey1 = i;
        //        }
        //    }
        //    List<ConfigData> dataList = new List<ConfigData>();
        //    for (int i = 2; (i < values.Length); i = (i + 1))
        //    {
        //        string[] subValues = values[i].TrimStart('\n').Split(ClientConfigManager.CMD_STRING, StringSplitOptions.None);
        //        if(subValues.Length != lables.Length)
        //            continue;
        //        ConfigData data = new ConfigData();
        //        int kV2 = -1;
        //        if (idxkey1 >= 0)
        //        {
        //            kV2 = Convert.ToInt32(subValues[idxkey1]);
        //        }
        //        for (int a = 0; a < subValues.Length; a++)
        //        {
        //            data.AddValue(types[a], lables[a], subValues[a]);
        //        }                
        //        LoadOberserver ob = null;
        //        if (m_LoadOberserver.TryGetValue(tableName, out ob))
        //        {
        //            ob(data);
        //            data = null;
        //        }
        //        else
        //        {
        //            if(!m_LoadAllOberserver.ContainsKey(tableName))
        //            {
        //                table.AddData(Convert.ToInt32(subValues[0]), kV2, data);
        //            }
        //            else
        //            {
        //                dataList.Add(data);
        //            }
        //        }
        //    }

        //    LoadAllOberserver obAll = null;
        //    if (m_LoadAllOberserver.TryGetValue(tableName, out obAll))
        //    {
        //        ConfigData[] arr = dataList.ToArray();
        //        obAll(ref arr);               
        //        dataList.Clear();
        //        dataList = null;
        //    }
        //}
        //public void AddLoadOberserver(string tableName, LoadOberserver ob)
        //{
        //    if (ob != null)
        //    {
        //        if(m_LoadOberserver.ContainsKey(tableName))
        //            m_LoadOberserver[tableName] += ob;
        //        else
        //            m_LoadOberserver[tableName] = ob;
        //    }
        //}
        //public void AddLoadAllOberserver(string tableName, LoadAllOberserver ob)
        //{
        //    if (ob != null)
        //    {
        //        m_LoadAllOberserver[tableName] = ob;
        //    }
        //}
        //public ConfigData[] GetAllData(string tableName)
        //{
        //    ConfigDataTable table = GeTable(tableName);
        //    if (table == null)
        //    {
        //        return null;
        //    }
        //    return table.GetAllData();
        //}
        //public ConfigData GetData(string tableName, int key)
        //{
        //    ConfigDataTable table = GeTable(tableName);
        //    if(table != null)
        //    {
        //        return table.GetData(key);
        //    }
        //    return null;
        //}

        //public ConfigData GetData(string tableName, int key, int key1)
        //{
        //    ConfigDataTable table = GeTable(tableName);
        //    if (table != null)
        //    {
        //        return table.GetData(key,key1);
        //    }
        //    return null;
        //}
       
        //public delegate void LoadOberserver(ConfigData data);
        //public delegate void LoadAllOberserver(ref ConfigData[] data);
        //Dictionary<string, client_config_define> m_DefineMap = new Dictionary<string, client_config_define>();
        //Dictionary<string, ConfigDataTable> m_TableMap = new Dictionary<string, ConfigDataTable>();
        //Dictionary<string, LoadOberserver> m_LoadOberserver = new Dictionary<string, LoadOberserver>();
        //Dictionary<string, LoadAllOberserver> m_LoadAllOberserver = new Dictionary<string, LoadAllOberserver>();
    }
   


}