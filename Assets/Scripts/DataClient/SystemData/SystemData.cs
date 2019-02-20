using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{

	/// <summary>
	/// 单个属性数据
	/// </summary>
	public class DataPropertyInXml
	{
		public string sName;
		public string sType;
		public string sValue;

		//标脏
		public bool bDirty;
		public bool IsDirty(int valueID) { return bDirty; }
		public void SetDirty() { bDirty = true; }
		public void Reset() { bDirty = false; }

		public int GetInt()
		{
			return Convert.ToInt32(sValue);
		}
		public uint GetUint()
		{
			return Convert.ToUInt32(sValue);
		}
		public long GetInt64()
		{
			return Convert.ToInt64(sValue);
		}
		public ulong GetUint64()
		{
			return Convert.ToUInt64(sValue);
		}
		public float GetFloat()
		{
			return Convert.ToSingle(sValue);
		}
		public string GetString()
		{
			return Convert.ToString(sValue);
		}

		public object GetValue()
		{
			object value;
			if (sType == "int8" ||
				sType == "int16" ||
				sType == "int32"
				)
			{
				value = GetInt();
			}
			else if (sType == "uint8" ||
				sType == "uint16" ||
				sType == "uint32")
			{
				value = GetUint();
			}
			else if (sType == "int64")
			{
				value = GetInt64();
			}
			else if (sType == "uint64")
			{
				value = GetUint64();
			}
			else if (sType == "f32" || sType == "f64")
			{
				value = GetFloat();
			}
			else if (sType == "char")
			{
				value = GetString();
			}
			else
			{
				value = null;
			}
			return value;
		}
	}

	/// <summary>
	/// 单个数据Class
	/// </summary>
	public class DataClassInXml : List<DataPropertyInXml>
	{
		//XTODO 添加class唯一id

		public string sLogicName;

		//标脏
		public bool bDirty;
		public bool IsDirty(int valueID) { return bDirty; }
		public void SetDirty() { bDirty = true; }
		public void Reset() { bDirty = false; }
	}

	/// <summary>
	/// 某System的SystemData
	/// </summary>
	public class SystemDataInXml : List<DataClassInXml>
	{
		//单个数据的唯一id
		public int iIndex;
		//单个数据的描述
		public string sDescribe;

		//标脏
		public bool bDirty;
		public bool IsDirty(int valueID) { return bDirty; }
		public void SetDirty() { bDirty = true; }
		public void Reset() { bDirty = false; }
	}
	
}