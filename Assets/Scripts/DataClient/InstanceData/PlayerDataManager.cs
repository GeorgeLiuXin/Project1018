using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

namespace XWorld.GameData
{
	public class PlayerDataManager : Singleton<PlayerDataManager>, IInstanceDataManager
	{

		private PlayerDataXml m_dataXml;

		public PlayerDataManager()
		{
			m_dataXml = new PlayerDataXml();
		}

		//XTODO	添加数据托管
		public void OnLoadPlayerData(ref ConfigData[] datas)
		{
			m_dataXml.InitXmlLayout(ref datas);
		}

	}

	public class PlayerDataXml : XmlOperation
	{
		private static string path = StaticParam.PATH_XML_FILES + "/PlayerDataXml/test.xml";
		public PlayerDataXml() : base(path)
		{

		}
		
		/// <summary>
		/// 根据路径来创建xml文件
		/// </summary>
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
				XmlElement defaultElm = xmlDoc.CreateElement("defaultElm");
				defaultElm.SetAttribute("instanceid", StaticParam.XMLPre_PlayerData + "0");
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

		/// <summary>
		/// 更新指定id的xml数据
		/// </summary>
		/// <param name="id"></param>
		public override void UpdateXml(string id)
		{
			if (File.Exists(m_XmlFilePath))
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(m_XmlFilePath);

				XmlNodeList nodes = xmlDoc.SelectSingleNode("transforms").ChildNodes;

				foreach (XmlElement xe in nodes)
				{
					if (xe.GetAttribute("id") == id)
					{
						xe.SetAttribute("id", "1001");

						foreach (XmlElement xx1 in xe.ChildNodes)
						{
							if (xx1.Name == "x")
								xx1.InnerText = "1001";
						}
						break;
					}
				}

				xmlDoc.Save(m_XmlFilePath);
			}
		}

		/// <summary>
		/// 添加一条数据
		/// </summary>
		public override void AddXml()
		{
			if (File.Exists(m_XmlFilePath))
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(m_XmlFilePath);
				XmlNode root = xmlDoc.SelectSingleNode("transforms");
				XmlElement elmNew = xmlDoc.CreateElement("rotation");
				elmNew.SetAttribute("id", "1");
				elmNew.SetAttribute("name", "yusong");

				XmlElement rotation_X = xmlDoc.CreateElement("x");
				rotation_X.InnerText = "0";
				XmlElement rotation_Y = xmlDoc.CreateElement("y");
				rotation_Y.InnerText = "1";
				XmlElement rotation_Z = xmlDoc.CreateElement("z");
				rotation_Z.InnerText = "2";

				elmNew.AppendChild(rotation_X);
				elmNew.AppendChild(rotation_Y);
				elmNew.AppendChild(rotation_Z);
				root.AppendChild(elmNew);
				xmlDoc.AppendChild(root);
				xmlDoc.Save(m_XmlFilePath);
			}
		}

		/// <summary>
		/// 删除数据
		/// </summary>
		/// <param name="id"></param>
		public override void DeleteXml(string id)
		{
			if (File.Exists(m_XmlFilePath))
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(m_XmlFilePath);
				XmlNodeList nodeList = xmlDoc.SelectSingleNode("transforms").ChildNodes;
				foreach (XmlElement xe in nodeList)
				{
					if (xe.GetAttribute("id") == id)
					{
						// 移除指定id的属性
						xe.RemoveAttribute("id");
					}

					foreach (XmlElement x1 in xe.ChildNodes)
					{
						// 移除所有z的value
						if (x1.Name == "z")
						{
							x1.RemoveAll();

						}
					}
				}
				xmlDoc.Save(m_XmlFilePath);
			}
		}

		/// <summary>
		/// 读取xml
		/// </summary>
		public override void ReadXml()
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
