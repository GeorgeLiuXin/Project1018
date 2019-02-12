using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

namespace XWorld
{
	public class ParamXmlBase : XmlOperation
	{
		protected string m_xmlPreName
		{
			get;
			set;
		}

		public ParamXmlBase(string filePath, string xmlPreName) : base(filePath)
		{
			m_xmlPreName = xmlPreName;
		}

		public void XmlElementSetDefaultValue(ConfigData layout, ref XmlElement elmXml)
		{
			// 设置节点默认属性
			elmXml.SetAttribute("type", layout.GetString("type"));
			elmXml.SetAttribute("max", layout.GetString("max"));
			elmXml.SetAttribute("min", layout.GetString("min"));
			elmXml.SetAttribute("flag", layout.GetInt("flag").ToString());
			elmXml.InnerText = layout.GetString("defaultvalue");
		}
		
		public override void CreateXml()
		{
			if (!File.Exists(m_XmlFilePath))
			{
				// 创建xml文档实例
				XmlDocument xmlDoc = new XmlDocument();
				// 创建根节点
				XmlElement root = xmlDoc.CreateElement("root");
				root.SetAttribute("curid", "1");

				//创建单个实例数据节点
				XmlElement defaultElm = xmlDoc.CreateElement("instanceid", m_xmlPreName + "0");
				foreach (ConfigData layout in m_xmlLayout.Values)
				{
					//创建子节点
					XmlElement elmXml = xmlDoc.CreateElement(layout.GetString("name"));
					// 设置节点属性
					XmlElementSetDefaultValue(layout, ref elmXml);
					defaultElm.AppendChild(elmXml);
				}
				root.AppendChild(defaultElm);
				xmlDoc.AppendChild(root);

				xmlDoc.Save(m_XmlFilePath);
			}
		}

		public override void DeleteXml(string id)
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
		public override void DeleteAllXml()
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
		
		public override void UpdateXml()
		{
			GameLogger.DebugLog(LOG_CHANNEL.ERROR, "ParamXmlBase的UpdateXml()被调用!");
		}
		
		public override void UpdateXml(Dictionary<string, InstanceData> dict)
		{
			GameLogger.DebugLog(LOG_CHANNEL.ERROR, "ParamXmlBase的UpdateXml(dict)被调用!");
		}

		public override void ReadXml()
		{
			GameLogger.DebugLog(LOG_CHANNEL.ERROR, "ParamXmlBase的ReadXml被调用!");
		}

	}

}