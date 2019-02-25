//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace XWorld
//{
//	public interface IInstanceDataManager
//	{
//		ParamXmlBase m_dataXml
//		{
//			get;
//			set;
//		}
//		Dictionary<string, InstanceData> m_instanceDataDict
//		{
//			get;
//			set;
//		}
//		/// <summary>
//		/// 读取以初始化本系统数据
//		/// </summary>
//		/// <param name="datas">XML数据</param>
//		void OnLoadInstanceDatas(ref InstanceData[] datas);
//		/// <summary>
//		/// 存储以保存本系统当前标脏数据
//		/// </summary>
//		void OnSaveInstanceDatas();
//		/// <summary>
//		/// 托管		初始化数据的XML格式
//		/// </summary>
//		/// <param name="datas">配表格式</param>
//		void OnLoadInstanceLayout(ref ConfigData[] datas);
//	}

//}