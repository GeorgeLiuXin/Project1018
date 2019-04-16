using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy.XmlData
{
    /// <summary>
    /// 表现效果相关参数    XMLClassProperty
    /// 单个属性，对应类的参数
    /// </summary>
    public class XmlParamItem
    {
        public string sName;
        public string sType;
        public string sValue;

        #region 获取
        public int GetInt()
        {
            return Convert.ToInt32(sValue);
        }
        public uint GetUint()
        {
            return Convert.ToUInt32(sValue);
        }
        public long GetInt64()
        {
            return Convert.ToInt64(sValue);
        }
        public ulong GetUint64()
        {
            return Convert.ToUInt64(sValue);
        }
        public float GetFloat()
        {
            return Convert.ToSingle(sValue);
        }
        public string GetString()
        {
            return Convert.ToString(sValue);
        }

        public object GetValue()
        {
            object value;
            if (sType == "int8" ||
                sType == "int16" ||
                sType == "int32"
                )
            {
                value = GetInt();
            }
            else if (sType == "uint8" ||
                sType == "uint16" ||
                sType == "uint32")
            {
                value = GetUint();
            }
            else if (sType == "int64")
            {
                value = GetInt64();
            }
            else if (sType == "uint64")
            {
                value = GetUint64();
            }
            else if (sType == "f32" || sType == "f64")
            {
                value = GetFloat();
            }
            else if (sType == "char")
            {
                value = GetString();
            }
            else
            {
                value = null;
            }
            return value;
        }
        #endregion
    }

    /// <summary>
    /// 单个表现效果及其相关属性    XMLClass
    /// 单个数据，对应一个类
    /// </summary>
    public class XmlClassData : List<XmlParamItem>
    {
        public int iLogicIndex;
        public string sLogicName;
    }

    /// <summary>
    /// 表现效果集合，对应于一个特效效果ID    XMLClasses
    /// 单个数据项，针对外面系统，内部可以多数据组合
    /// </summary>
    public class XmlDataList : List<XmlClassData>
    {
        //单个数据的唯一id
        public int iIndex;
        //单个数据的描述
        public string sDescribe;
    }

}
