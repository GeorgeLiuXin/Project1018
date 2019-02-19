using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Xml;

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

    }

    /// <summary>
    /// 表现效果id To 效果集合  classDict
    /// </summary>
    public class EffectLogicParamDataManager : XWorldGameManagerBase
    {
        public Dictionary<int, EffectLogicParamData> m_dict;
        private string m_xmlPath;
        private EffectLogicReader reader;

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
        }

        public override void ShutDown()
        {

        }

        public override void Update(float fElapseTimes)
        {

        }
    }
    
    public interface IXmlOperation1
    {
        string m_XmlFilePath
        {
            get;
            set;
        }
		
        /// <summary>
        /// 创建xml文件
        /// </summary>
        void CreateXml();

		/// <summary>
		/// 更新xml数据
		/// </summary>
		/// <param name="data">数据</param>
		void AddXml(EffectLogicParamData data);
		/// <summary>
		/// 更新xml数据
		/// </summary>
		/// <param name="data">数据</param>
		void AddXml(Dictionary<int, EffectLogicParamData> datadict);

		/// <summary>
		/// 更新xml数据
		/// </summary>
		/// <param name="data">数据</param>
		void UpdateXml(EffectLogicParamData data);
		/// <summary>
		/// 更新xml数据
		/// </summary>
		/// <param name="data">数据</param>
		void UpdateXml(Dictionary<int, EffectLogicParamData> datadict);

		/// <summary>
		/// 删除数据
		/// </summary>
		/// <param name="id"></param>
		void DeleteXml(string id);
        /// <summary>
        /// 删除所有数据
        /// </summary>
        void DeleteAllXml();

        /// <summary>
        /// 读取xml
        /// </summary>
        void ReadXml();
		/// <summary>
		/// 读取xml
		/// </summary>
		/// <param name="id"></param>
		void ReadXml(string id);
	}

    public class XmlBase
    {
        public string m_XmlFilePath
        {
            get;
            set;
        }

        public XmlBase(string filePath)
        {
            if (filePath.IsNE())
            {
                GameLogger.DebugLog(LOG_CHANNEL.ERROR, "当前XML路径为空");
                return;
            }

            m_XmlFilePath = filePath;
            CheckXml();
        }

        public void CheckXml()
        {
            string path = m_XmlFilePath;
            if (!File.Exists(path))
            {
                GameLogger.DebugLog(LOG_CHANNEL.ERROR, "当前路径下没有对应的XML文件: " + path);
            }
        }
    }

    //XML 表现效果读取方式
    public class EffectLogicReader : XmlBase, IXmlOperation1
	{

        public EffectLogicReader(string filePath) : base(filePath)
        {

        }

        public string GetPropertyValue(EffectLogicParamItem item)
		{
			return item.sValue;
		}

        public void CreateXml()
        {
            if (!File.Exists(m_XmlFilePath))
            {
                // 创建xml文档实例
                XmlDocument xmlDoc = new XmlDocument();
                // 创建根节点
                XmlElement root = xmlDoc.CreateElement("root");
                root.SetAttribute("curid", "0");
                xmlDoc.AppendChild(root);

                xmlDoc.Save(m_XmlFilePath);
            }
        }
        
        public void AddXml(EffectLogicParamData data)
        {
            if (!File.Exists(m_XmlFilePath))
            {
                // 创建xml文档实例
                XmlDocument xmlDoc = new XmlDocument();
                // 创建根节点
                XmlElement root = xmlDoc.CreateElement("root");
				int id = Convert.ToInt32(root.GetAttribute("curid"));

				id++;
				//创建整个表现节点
				XmlElement curParentElm = xmlDoc.CreateElement(id.ToString());
				foreach (EffectLogicParamList _class in data)
				{
					//创建单个表现类型
					XmlElement elm = xmlDoc.CreateElement(_class.sLogicName);
					foreach (EffectLogicParamItem _property in _class)
					{
						XmlElement elmProperty = xmlDoc.CreateElement(_property.sName);
						elmProperty.SetAttribute("Type", _property.sType);
						elmProperty.InnerText = GetPropertyValue(_property);
						elm.AppendChild(elmProperty);
					}
					curParentElm.AppendChild(elm);
				}
				root.AppendChild(curParentElm);

				root.SetAttribute("curid", id.ToString());
				xmlDoc.AppendChild(root);
				xmlDoc.Save(m_XmlFilePath);
            }
		}
		public void AddXml(Dictionary<int, EffectLogicParamData> datadict)
		{
			if (!File.Exists(m_XmlFilePath))
			{
				// 创建xml文档实例
				XmlDocument xmlDoc = new XmlDocument();
				// 创建根节点
				XmlElement root = xmlDoc.CreateElement("root");
				int id = Convert.ToInt32(root.GetAttribute("curid"));

				foreach (KeyValuePair<int,EffectLogicParamData> pair in datadict)
				{
					id = Mathf.Max(id, pair.Key);
					//创建整个表现节点
					XmlElement curParentElm = xmlDoc.CreateElement(pair.Key.ToString());
					foreach (EffectLogicParamList _class in pair.Value)
					{
						//创建单个表现类型
						XmlElement elm = xmlDoc.CreateElement(_class.sLogicName);
						foreach (EffectLogicParamItem _property in _class)
						{
							XmlElement elmProperty = xmlDoc.CreateElement(_property.sName);
							elmProperty.SetAttribute("Type", _property.sType);
							elmProperty.InnerText = GetPropertyValue(_property);
							elm.AppendChild(elmProperty);
						}
						curParentElm.AppendChild(elm);
					}
					root.AppendChild(curParentElm);
				}
				id++;

				root.SetAttribute("curid", id.ToString());
				xmlDoc.AppendChild(root);
				xmlDoc.Save(m_XmlFilePath);
			}
		}

		public void DeleteXml(string id)
        {
            if (File.Exists(m_XmlFilePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlFilePath);
                XmlNode rootNode = xmlDoc.SelectSingleNode("root");
                XmlNode xe = rootNode.SelectSingleNode("root/" + id);
                rootNode.RemoveChild(xe);

                xmlDoc.Save(m_XmlFilePath);
            }
        }
        public void DeleteAllXml()
        {
            if (File.Exists(m_XmlFilePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlFilePath);
                XmlNode rootNode = xmlDoc.SelectSingleNode("root");
                rootNode.RemoveAll();

                xmlDoc.Save(m_XmlFilePath);
            }
        }

		public void UpdateXml(EffectLogicParamData data)
		{
			throw new NotImplementedException();
		}

		public void UpdateXml(Dictionary<int, EffectLogicParamData> datadict)
		{
			throw new NotImplementedException();
		}
		public void UpdateXml(Dictionary<string, EffectLogicParamItem> dict)
        {
            if (!File.Exists(m_XmlFilePath))
            {
                GameLogger.DebugLog(LOG_CHANNEL.ASSET, "当前路径下XML不存在!");
                return;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(m_XmlFilePath);

            List<XmlElement> removeList = new List<XmlElement>();

            XmlNode rootNode = xmlDoc.SelectSingleNode("root");
            XmlNodeList nodes = rootNode.ChildNodes;
            foreach (XmlElement node in nodes)
            {
                if (!node.HasChildNodes)
                {
                    removeList.Add(node);
                    continue;
                }

                XmlNode preChildNode = node.FirstChild;
                foreach (ConfigData layout in m_xmlLayout.Values)
                {
                    XmlNode curChildNode = node.SelectSingleNode(layout.GetString("name"));
                    if (curChildNode == null)
                    {
                        XmlElement curChildNodeEle = xmlDoc.CreateElement(layout.GetString("name"));
                        // 设置节点属性
                        XmlElementSetDefaultValue(layout, ref curChildNodeEle);
                        node.InsertBefore(curChildNodeEle, preChildNode);
                        curChildNode = curChildNodeEle;
                    }
                }
            }

            foreach (XmlElement xmlEle in removeList)
            {
                rootNode.RemoveChild(xmlEle);
            }
            xmlDoc.Save(m_XmlFilePath);
        }

        public void ReadXml()
        {
            if (File.Exists(m_XmlFilePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlFilePath);
                XmlNodeList nodes = xmlDoc.SelectSingleNode("transforms").ChildNodes;
                foreach (XmlElement xe in nodes)
                {
                    Debug.Log("ID: " + xe.GetAttribute("id"));
                    Debug.Log("Name: " + xe.GetAttribute("name"));

                    string element = "";
                    foreach (XmlElement x in xe)
                    {
                        element = element + "," + x.InnerText;
                    }
                    Debug.Log(element);
                }
            }
		}
		public void ReadXml(string id)
		{
			if (File.Exists(m_XmlFilePath))
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc = new XmlDocument();
				xmlDoc.Load(m_XmlFilePath);
				XmlNodeList nodes = xmlDoc.SelectSingleNode("transforms").ChildNodes;
				foreach (XmlElement xe in nodes)
				{
					Debug.Log("ID: " + xe.GetAttribute("id"));
					Debug.Log("Name: " + xe.GetAttribute("name"));

					string element = "";
					foreach (XmlElement x in xe)
					{
						element = element + "," + x.InnerText;
					}
					Debug.Log(element);
				}
			}
		}

	}

}