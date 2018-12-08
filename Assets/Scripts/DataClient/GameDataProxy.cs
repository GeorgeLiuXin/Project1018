using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XWorld.GameData;

namespace XWorld
{
    public class GameDataProxy
    {

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
        public void Clear()
        {
            ConfigDataTableManager.Instance.Clear();
        }

        /// <summary>
        /// 重新加载某张刚刚修改的表格
        /// </summary>
        /// <param name="tableName">表格名称</param>
        public static void ReloadTable(string tableName)
        {
            ConfigDataTableManager.Instance.ReloadTable(tableName);
        }
        
    }

}