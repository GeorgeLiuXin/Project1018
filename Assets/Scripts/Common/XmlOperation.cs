using UnityEngine;
using System.Xml;
using System.IO;
using System;
using System.Collections.Generic;

namespace XWorld
{
	public class XmlOperation : IXmlOperation
	{
		public string m_XmlFilePath
		{
			get;
			set;
		}
		
		public XmlOperation(string filePath)
		{
			if (filePath.IsNE())
			{
				GameLogger.DebugLog(LOG_CHANNEL.ERROR, "当前XML路径为空");
				return;
			}

			m_XmlFilePath = filePath;
			CheckXml();
			m_xmlLayout = new Dictionary<string, ConfigData>();
		}

		public void CheckXml()
		{
			string path = m_XmlFilePath;
			if (!File.Exists(path))
			{
				GameLogger.DebugLog(LOG_CHANNEL.ERROR, "当前路径下没有对应的XML文件: " + path);
			}
		}

		//ParamID name    AValue des type defaultvalue    max min flag
		//int32   char int32   char char char char char int32
		//0	TotalTime	0	总计游戏时长 uint32  0	4294967294	0	0
		//1	gold	0	金币 uint32  0	4294967294	0	0
		//2	accountname	0	玩家用户名 Char64      default	default	0
		//3	PlayerHeroList	0	玩家英雄队列 List        default	default	0
		//4	CurHeroList	0	当前出战英雄队列 List        default	default	0

		//ParamID name	AValue	des	 type defaultvalue  max	  min   flag
		//int32   char	int32   char char char			char  char	int32
		protected Dictionary<string, ConfigData> m_xmlLayout;
		public void InitXmlLayout(ref ConfigData[] datas)
		{
			foreach (ConfigData data in datas)
			{
				m_xmlLayout.Add(data.GetString("name"), data);
			}
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

		public virtual void CreateXml()
		{

		}

		public virtual void AddXml()
		{

		}

		public virtual void DeleteXml(string id)
		{

		}

		public virtual void UpdateXml(string id)
		{

		}

		public virtual void ReadXml()
		{

		}

	}

	public interface IXmlOperation
	{
		string m_XmlFilePath
		{
			get;
			set;
		}

		/// <summary>
		/// 初始化Xml的格式
		/// </summary>
		void InitXmlLayout(ref ConfigData[] datas);

		/// <summary>
		/// 检查xml
		/// </summary>
		void CheckXml();
		/// <summary>
		/// 创建XML文件
		/// </summary>
		void CreateXml();
		/// <summary>
		/// 更新指定id的xml数据
		/// </summary>
		/// <param name="id">对应ID</param>
		void UpdateXml(string id);
		/// <summary>
		/// 添加一条数据
		/// </summary>
		void AddXml();
		/// <summary>
		/// 删除数据
		/// </summary>
		/// <param name="id"></param>
		void DeleteXml(string id);
		/// <summary>
		/// 读取xml
		/// </summary>
		void ReadXml();
	}
}