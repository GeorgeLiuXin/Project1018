using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace XWorld
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

    //一个数据
    public class Data<TValue> : IConfigData
    {
        public TValue value;
        public bool GetBool()
        {
            return Convert.ToBoolean(value);
        }
        public int GetInt()
        {
            return Convert.ToInt32(value);
        }
        public uint GetUint()
        {
            return Convert.ToUInt32(value);
        }
        public long GetInt64()
        {
            return Convert.ToInt64(value);
        }
        public ulong GetUint64()
        {
            return Convert.ToUInt64(value);
        }
        public float GetFloat()
        {
            return Convert.ToSingle(value);
        }
        public string GetString()
        {
            return Convert.ToString(value);
        }

        public void SetValue(TValue _value)
        {
            value = _value;
        }
    }

}
