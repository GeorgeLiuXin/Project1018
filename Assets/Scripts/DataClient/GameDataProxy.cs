using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XWorld.GameData;

namespace XWorld
{
    /// <summary>
    /// 面向前端逻辑  连接了数据层和前端逻辑
    /// </summary>
    public class GameDataProxy
    {
        private static GameDataLoader m_Loader;
        public static GameDataLoader DataLoader
        {
            get
            {
                if (m_Loader == null)
                {
                    m_Loader = new GameDataLoader();
                }
                return m_Loader;
            }
        }

		#region 数据托管

		public void AddLoadOberserver(string tableName, ConfigDataTableManager.LoadOberserver ob)
		{
			ConfigDataTableManager.Instance.AddLoadOberserver(tableName, ob);
		}

		public void AddLoadAllOberserver(string tableName, ConfigDataTableManager.LoadAllOberserver ob)
		{
			ConfigDataTableManager.Instance.AddLoadAllOberserver(tableName, ob);
		}

		#endregion

		#region 数据获取

		/// <summary>
		/// 获取对应表格的对应数据
		/// </summary>
		/// <param name="tableName">表格名称</param>
		/// <param name="pk1">主键值</param>
		/// <param name="pk2">第二主键</param>
		/// <returns></returns>
		public static ConfigData GetData(string tableName, int pk1, int pk2 = -1)
        {
            if (pk2 == -1)
            {
                return ConfigDataTableManager.Instance.GetData(tableName, pk1);
            }
            else
            {
                return ConfigDataTableManager.Instance.GetData(tableName, pk1, pk2);
            }
        }

        /// <summary>
        /// 获取对应表格的所有数据
        /// </summary>
        /// <param name="tableName">表格名称</param>
        /// <returns></returns>
        public static ConfigData[] GetAllData(string tableName)
        {
            return ConfigDataTableManager.Instance.GetAllData(tableName);
        }

        /// <summary>
        /// 清理所有表格数据
        /// </summary>
        public static void ClearAllTable()
        {
            ConfigDataTableManager.Instance.ClearAllTable();
        }

        /// <summary>
        /// 析构DataTableMgr
        /// </summary>
        public static void Clear()
        {
            ConfigDataTableManager.Instance.Clear();
        }

		#endregion

		#region 玩家实例数据相关方法



		#endregion

		#region 玩家配置数据/通用数据相关方法

		public static void SavePlayerConfigData(string key,int value)
		{
			PlayerPrefs.SetInt(key, value);
		}
		public static void SavePlayerConfigData(string key, float value)
		{
			PlayerPrefs.SetFloat(key, value);
		}
		public static void SavePlayerConfigData(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
		}

		public static void GetPlayerConfigData(string key,ref int value)
		{
			value = PlayerPrefs.GetInt(key, 0);
		}
		public static void GetPlayerConfigData(string key, ref float value)
		{
			value = PlayerPrefs.GetFloat(key, 0);
		}
		public static void GetPlayerConfigData(string key, ref string value)
		{
			value = PlayerPrefs.GetString(key, default(string));
		}

		public static bool HasThisPlayerConfigData(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		public static void DeletePlayerConfigData(string key)
		{
			PlayerPrefs.DeleteKey(key);
		}

		#endregion

		#region 数据层加载方法

		/// <summary>
		/// 加载总表格及相关表格  ConfigData整体加载
		/// </summary>
		public static void LoadDefineTableList()
        {
            DataLoader.StartLoadDefineTableList();
        }

        /// <summary>
        /// 重新加载某张刚刚修改的表格
        /// </summary>
        /// <param name="tableName">表格名称</param>
        public static void ReloadTable(string tableName)
        {
            DataLoader.LoadTable(tableName);
        }

        #endregion
        
    }

}