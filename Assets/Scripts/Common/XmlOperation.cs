using UnityEngine;
using System.Xml;
using System.IO;
using System;

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
		}

		public void CheckXml()
		{
			string path = m_XmlFilePath;
			if (!File.Exists(path))
			{
				GameLogger.DebugLog(LOG_CHANNEL.ERROR, "当前路径下没有对应的XML文件: " + path);
			}
		}

		public virtual void AddXml()
		{

		}

		public virtual void CreateXml()
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