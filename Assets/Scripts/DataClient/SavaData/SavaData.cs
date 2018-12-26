using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public class SavaData : IDisposable
    {

        //一行数据
        public Dictionary<string, IConfigData> RowDataMap;

        public SavaData()
        {
            RowDataMap = new Dictionary<string, IConfigData>();
        }

        public void Dispose()
        {
            RowDataMap.Clear();
            RowDataMap = null;
        }

        public bool GetBool(string sName)
        {
            IConfigData iData = null;
            if (RowDataMap.TryGetValue(sName, out iData))
            {
                return iData.GetBool();
            }
            return false;
        }
        public int GetInt(string sName)
        {
            IConfigData iData = null;
            if (RowDataMap.TryGetValue(sName, out iData))
            {
                return iData.GetInt();
            }
            return 0;
        }
        public uint GetUint(string sName)
        {
            IConfigData iData = null;
            if (RowDataMap.TryGetValue(sName, out iData))
            {
                return iData.GetUint();
            }
            return 0;
        }
        public long GetInt64(string sName)
        {
            IConfigData iData = null;
            if (RowDataMap.TryGetValue(sName, out iData))
            {
                return iData.GetInt64();
            }
            return 0;
        }
        public ulong GetUint64(string sName)
        {
            IConfigData iData = null;
            if (RowDataMap.TryGetValue(sName, out iData))
            {
                return iData.GetUint64();
            }
            return 0;
        }
        public float GetFloat(string sName)
        {
            IConfigData iData = null;
            if (RowDataMap.TryGetValue(sName, out iData))
            {
                return iData.GetFloat();
            }
            return 0.0f;
        }
        public string GetString(string sName)
        {
            IConfigData iData = null;
            if (RowDataMap.TryGetValue(sName, out iData))
            {
                return iData.GetString();
            }
            return "";
        }

        public void AddValue(string sTypes, string sName, string sValue)
        {
            if (sTypes == "int8" || sTypes == "int16" || sTypes == "int32")
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
                RowDataMap.Add(sName, iData);
            }
            else if (sTypes == "uint8" || sTypes == "uint16" || sTypes == "uint32")
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
                RowDataMap.Add(sName, uiData);
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
                RowDataMap.Add(sName, i64Data);
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
                RowDataMap.Add(sName, ui64Data);
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
                RowDataMap.Add(sName, fData);
            }
            else if (sTypes == "char")
            {
                Data<string> sData = new Data<string>();
                sData.SetValue(sValue);
                RowDataMap.Add(sName, sData);
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
                RowDataMap.Add(sName, boolData);
            }
        }
    }

}
