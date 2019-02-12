using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
	public interface IInstanceDataManager
	{
		/// <summary>
		/// 初始化的格式数据托管
		/// </summary>
		/// <param name="datas"></param>
		void OnLoadPlayerData(ref ConfigData[] datas);
	}

}