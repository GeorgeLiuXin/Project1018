//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.IO;
//using System.Xml;
//using System;

//namespace XWorld.GameData
//{
//	/// <summary>
//	/// 1、数据标脏	XTODO
//	/// 2、根据标脏数据动态更新xml
//	/// 3、根据xml更新数据
//	/// </summary>
//	public class PlayerDataManager : Singleton<PlayerDataManager>, IInstanceDataManager
//	{
//		public ParamXmlBase m_dataXml
//		{
//			get;
//			set;
//		}
//		public Dictionary<string, InstanceData> m_instanceDataDict
//		{
//			get;
//			set;
//		}


//		public PlayerDataManager()
//		{
//			m_dataXml = new PlayerDataXml();
//			m_instanceDataDict = new Dictionary<string, InstanceData>();
//		}

//		/// <summary>
//		/// 根据xml读取数据
//		/// </summary>
//		public void OnLoadInstanceDatas(ref InstanceData[] datas)
//		{

//		}

//		public void OnSaveInstanceDatas()
//		{
//			m_dataXml.UpdateXml(m_instanceDataDict);
//		}

//		//XTODO	添加数据托管
//		public void OnLoadInstanceLayout(ref ConfigData[] datas)
//		{
//			m_dataXml.InitXmlLayout(ref datas);
//		}
//	}

//	//ParamID name    AValue des type defaultvalue    max min flag
//	//int32   char int32   char char char char char int32
//	//0	TotalTime	0	总计游戏时长 uint32  0	4294967294	0	0
//	//1	gold	0	金币 uint32  0	4294967294	0	0
//	//2	accountname	0	玩家用户名 Char64      default	default	0
//	//3	PlayerHeroList	0	玩家英雄队列 List        default	default	0
//	//4	CurHeroList	0	当前出战英雄队列 List        default	default	0

//	//ParamID name	AValue	des	 type defaultvalue  max	  min   flag
//	//int32   char	int32   char char char			char  char	int32
//	public class PlayerDataXml : ParamXmlBase
//	{
//		private static string path = StaticParam.PATH_XML_FILES + "/PlayerDataXml/test.xml";
//		public PlayerDataXml() : base(path, StaticParam.XMLPre_PlayerData)
//		{

//		}

//		public override void CreateXml()
//		{
//			base.CreateXml();
//		}

//		public override void DeleteXml(string id)
//		{
//			base.DeleteXml(id);
//		}
//		public override void DeleteAllXml()
//		{
//			base.DeleteAllXml();
//		}

//		public override void UpdateXml()
//		{
//			if (!File.Exists(m_XmlFilePath))
//			{
//				GameLogger.DebugLog(LOG_CHANNEL.ASSET, "当前路径下XML不存在!");
//				return;
//			}

//			XmlDocument xmlDoc = new XmlDocument();
//			xmlDoc.Load(m_XmlFilePath);

//			List<XmlElement> removeList = new List<XmlElement>();

//			XmlNode rootNode = xmlDoc.SelectSingleNode("root");
//			XmlNodeList nodes = rootNode.ChildNodes;
//			foreach (XmlElement node in nodes)
//			{
//				if (!node.HasChildNodes)
//				{
//					removeList.Add(node);
//					continue;
//				}

//				XmlNode preChildNode = node.FirstChild;
//				foreach (ConfigData layout in m_xmlLayout.Values)
//				{
//					XmlNode curChildNode = node.SelectSingleNode(layout.GetString("name"));
//					if (curChildNode == null)
//					{
//						XmlElement curChildNodeEle = xmlDoc.CreateElement(layout.GetString("name"));
//						// 设置节点属性
//						XmlElementSetDefaultValue(layout, ref curChildNodeEle);
//						node.InsertBefore(curChildNodeEle, preChildNode);
//						curChildNode = curChildNodeEle;
//					}
//				}
//			}

//			foreach (XmlElement xmlEle in removeList)
//			{
//				rootNode.RemoveChild(xmlEle);
//			}
//			xmlDoc.Save(m_XmlFilePath);
//		}
		
//		public override void UpdateXml(Dictionary<string, InstanceData> dict)
//		{
//			if (!File.Exists(m_XmlFilePath))
//			{
//				GameLogger.DebugLog(LOG_CHANNEL.ASSET, "当前路径下XML不存在!");
//				return;
//			}

//			XmlDocument xmlDoc = new XmlDocument();
//			xmlDoc.Load(m_XmlFilePath);

//			List<XmlElement> removeList = new List<XmlElement>();

//			XmlNode rootNode = xmlDoc.SelectSingleNode("root");
//			//XmlNodeList nodes = rootNode.ChildNodes;
//			//foreach (XmlElement node in nodes)
//			//{
//			//	if (!node.HasChildNodes)
//			//	{
//			//		removeList.Add(node);
//			//		continue;
//			//	}

//			//	XmlNode preChildNode = node.FirstChild;
//			//	foreach (ConfigData layout in m_xmlLayout.Values)
//			//	{
//			//		XmlNode curChildNode = node.SelectSingleNode(layout.GetString("name"));
//			//		if (curChildNode == null)
//			//		{
//			//			XmlElement curChildNodeEle = xmlDoc.CreateElement(layout.GetString("name"));
//			//			// 设置节点属性
//			//			XmlElementSetDefaultValue(layout, ref curChildNodeEle);
//			//			node.InsertBefore(curChildNodeEle, preChildNode);
//			//			curChildNode = curChildNodeEle;
//			//		}
//			//	}
//			//}

//			//foreach (XmlElement xmlEle in removeList)
//			//{
//			//	rootNode.RemoveChild(xmlEle);
//			//}
//			xmlDoc.Save(m_XmlFilePath);
//		}

//		/// <summary>
//		/// 读取xml
//		/// </summary>
//		public override void ReadXml()
//		{
//			if (File.Exists(m_XmlFilePath))
//			{
//				XmlDocument xmlDoc = new XmlDocument();
//				xmlDoc = new XmlDocument();
//				xmlDoc.Load(m_XmlFilePath);
//				XmlNodeList nodes = xmlDoc.SelectSingleNode("transforms").ChildNodes;
//				foreach (XmlElement xe in nodes)
//				{
//					Debug.Log("ID: " + xe.GetAttribute("id"));
//					Debug.Log("Name: " + xe.GetAttribute("name"));

//					string element = "";
//					foreach (XmlElement x in xe)
//					{
//						element = element + "," + x.InnerText;
//					}
//					Debug.Log(element);
//				}
//			}
//		}

//	}

//}
