using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Xml;
using Galaxy.XmlData;

namespace Galaxy
{
    //XML 表现效果读取方式
    public class EffectLogicReader : XmlReaderBase
    {
        private static string m_xmlPath;
        public static string XmlPath
        {
            get
            {
#if !UNITY_EDITOR && UNITY_ANDROID
                m_xmlPath = Application.streamingAssetsPath + "/CombatXmlDefine.xml";
#elif !UNITY_EDITOR && UNITY_IPHONE
                m_xmlPath = Application.streamingAssetsPath + "/CombatXmlDefine.xml";
#else
                m_xmlPath = Application.streamingAssetsPath + "/CombatXmlDefine.xml";
#endif
                return m_xmlPath;
            }
        }

        public EffectLogicReader() : base(XmlPath)
        {

        }

    }

}