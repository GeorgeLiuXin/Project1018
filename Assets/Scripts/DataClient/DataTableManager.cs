using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

namespace XWorld
{
	/// <summary>
	/// 修改读表方式
	/// 修改之前的生成
	/// 
	/// 添加表格的重读方法，提升改表效率
	/// </summary>
	public class ConfigDataTableManager : Singleton<ConfigDataTableManager>
	{
		private Dictionary<string, ConfigData> m_TableDefineList;
		private Dictionary<string, ConfigDataTable> m_TableMap;

		public ConfigDataTableManager()
		{
			m_TableMap = new Dictionary<string, ConfigDataTable>();
			m_TableDefineList = new Dictionary<string, ConfigData>();
		}

		public void LoadTableDefineList(string sContent)
		{
			string[] values = sContent.Split('\n');
			foreach (string value in values)
			{
				string[] subValues = value.TrimStart('\n').Split('\t');
			}
		}

		public void LoadAllTable()
		{

		}

		public void LoadTable()
		{

		}

		public ConfigDataTable GetTable(string tableName)
		{
			if (!m_TableMap.ContainsKey(tableName))
				return null;
			return m_TableMap[tableName];
		}

		public ConfigData GetData(string tableName, int key)
		{
			ConfigDataTable table = GetTable(tableName);
			if (table == null)
				return null;
			return table.GetData(key);
		}

		public ConfigData GetData(string tableName, int pk1, int pk2)
		{
			ConfigDataTable table = GetTable(tableName);
			if (table == null)
				return null;
			return table.GetData(pk1, pk2);
		}

		public ConfigData[] GetAllData(string tableName)
		{
			ConfigDataTable table = GetTable(tableName);
			if (table == null)
				return null;
			return table.GetAllData();
		}

		public void ClearTable(string tableName)
		{
			ConfigDataTable table = null;
			if (m_TableMap.TryGetValue(tableName, out table))
			{
				table.Dispose();
			}
		}

		public void ClearAllTable()
		{
			foreach (KeyValuePair<string, ConfigDataTable> table in m_TableMap)
			{
				table.Value.Dispose();
			}
		}

		public void Clear()
		{
			foreach (KeyValuePair<string, ConfigDataTable> table in m_TableMap)
			{
				table.Value.Dispose();
			}
			m_TableMap.Clear();
			m_TableMap = null;
		}

		public void ReloadTable()
		{

		}

	}

}