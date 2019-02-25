//using UnityEngine;
//using System.Xml;
//using System.IO;
//using System;
//using System.Collections.Generic;

//namespace XWorld
//{
//	public class XmlOperation : IXmlOperation
//	{
//		public string m_XmlFilePath
//		{
//			get;
//			set;
//		}
		
//		public XmlOperation(string filePath)
//		{
//			if (filePath.IsNE())
//			{
//				GameLogger.DebugLog(LOG_CHANNEL.ERROR, "当前XML路径为空");
//				return;
//			}

//			m_XmlFilePath = filePath;
//			CheckXml();
//			m_xmlLayout = new Dictionary<string, ConfigData>();
//		}

//		public void CheckXml()
//		{
//			string path = m_XmlFilePath;
//			if (!File.Exists(path))
//			{
//				GameLogger.DebugLog(LOG_CHANNEL.ERROR, "当前路径下没有对应的XML文件: " + path);
//			}
//		}

//		protected Dictionary<string, ConfigData> m_xmlLayout;
//		public void InitXmlLayout(ref ConfigData[] datas)
//		{
//			foreach (ConfigData data in datas)
//			{
//				m_xmlLayout.Add(data.GetString("name"), data);
//			}
//		}

//		/// <summary>
//		/// 根据路径来创建xml文件
//		/// </summary>
//		public virtual void CreateXml()
//		{

//		}

//		/// <summary>
//		/// 删除数据
//		/// </summary>
//		/// <param name="id"></param>
//		public virtual void DeleteXml(string id)
//		{

//		}
//		/// <summary>
//		/// 删除所有数据
//		/// </summary>
//		public virtual void DeleteAllXml()
//		{

//		}

//		/// <summary>
//		/// 更新整个xml数据
//		/// </summary>
//		public virtual void UpdateXml()
//		{

//		}

//		/// <summary>
//		/// 根据系统数据更新整个xml数据，保存当前游戏的系统数据
//		/// </summary>
//		/// <param name="dict">系统数据</param>
//		public virtual void UpdateXml(Dictionary<string, InstanceData> dict)
//		{

//		}

//		/// <summary>
//		/// 读取xml
//		/// </summary>
//		public virtual void ReadXml()
//		{

//		}

//	}

//	public interface IXmlOperation
//	{
//		string m_XmlFilePath
//		{
//			get;
//			set;
//		}

//		/// <summary>
//		/// 初始化Xml的格式
//		/// </summary>
//		void InitXmlLayout(ref ConfigData[] datas);

//		/// <summary>
//		/// 检查xml
//		/// </summary>
//		void CheckXml();
//		/// <summary>
//		/// 创建XML文件
//		/// </summary>
//		void CreateXml();
//		/// <summary>
//		/// 根据配表更新整个xml数据，填充当前没有的配表节点
//		/// </summary>
//		void UpdateXml();
//		/// <summary>
//		/// 根据系统数据更新整个xml数据，保存当前游戏的系统数据
//		/// </summary>
//		/// <param name="dict">系统数据</param>
//		void UpdateXml(Dictionary<string, InstanceData> dict);
//		/// <summary>
//		/// 删除数据
//		/// </summary>
//		/// <param name="id"></param>
//		void DeleteXml(string id);
//		/// <summary>
//		/// 删除所有数据
//		/// </summary>
//		void DeleteAllXml();
//		/// <summary>
//		/// 读取xml
//		/// </summary>
//		void ReadXml();
//	}
//}