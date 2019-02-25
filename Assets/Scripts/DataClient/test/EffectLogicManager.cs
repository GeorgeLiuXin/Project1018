using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    /// <summary>
    /// 表现效果相关参数    classXMLproperty
    /// </summary>
    public class EffectLogicParamItem
    {
        public string sName;
        public string sType;
        public string sValue;

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
    }

    /// <summary>
    /// 单个表现效果及其相关属性    classXML
    /// </summary>
    public class EffectLogicParamList : List<EffectLogicParamItem>
    {
        public string sLogicName;
    }

    /// <summary>
    /// 表现效果集合，对应于一个特效效果ID    classesXML
    /// </summary>
    public class EffectLogicParamData : List<EffectLogicParamList>
    {
        //单个数据的唯一id
        public int iIndex;
        //单个数据的描述
        public string sDescribe;
    }

    /// <summary>
    /// 表现效果id To 效果集合  classDict
    /// </summary>
    public class EffectLogicManager : XWorldGameManagerBase
    {
        public EffectLogicReader reader;
        public Dictionary<int, EffectLogicParamData> m_dict;
        private string m_xmlPath;

        public PerformanceLogicFactory factory;

        public override void InitManager()
        {
            InitXMLPath();
            reader = new EffectLogicReader(m_xmlPath);
            InitDataDict();
        }
        private void InitXMLPath()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            m_xmlPath = Application.streamingAssetsPath + "/CombatXmlDefine.xml";
#elif !UNITY_EDITOR && UNITY_IPHONE
            m_xmlPath = Application.streamingAssetsPath + "/CombatXmlDefine.xml";
#else
            m_xmlPath = Application.streamingAssetsPath + "/CombatXmlDefine.xml";
#endif
        }
        private void InitDataDict()
        {
            m_dict = new Dictionary<int, EffectLogicParamData>();
            reader.ReadXml(ref m_dict);
        }

        public override void ShutDown()
        {

        }

        public override void Update(float fElapseTimes)
        {

        }

        public void ReloadDataDict()
        {
            m_dict.Clear();
            m_dict = null;
            InitDataDict();
        }
    }
}