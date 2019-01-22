using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
	public struct sCDInfo
	{
		public float fCurTime;
		public float fTotalTime;
		public void UpdateCD(float _fCurTime)
		{
			fCurTime = _fCurTime;
			fTotalTime = Mathf.Max(fCurTime, fTotalTime);
		}
	}

	/// <summary>
	/// 5000以后为技能CD组，前为技能CD
	/// </summary>
	public class CDComponent : ComponentBase
	{
		private string m_TableName = "cddefine";

		private Dictionary<int, sCDInfo> m_CDDict;
		private Dictionary<int, int> m_CDCountDict;
		private Dictionary<int, sCDInfo> m_CDCommonDict;

		private List<int> m_RemoveList;

		protected override void InitComponent()
		{
			m_CDDict = new Dictionary<int, sCDInfo>();
			m_CDCountDict = new Dictionary<int, int>();
			m_CDCommonDict = new Dictionary<int, sCDInfo>();

			m_RemoveList = new List<int>();
		}

		public override void OnPreDestroy()
		{
			m_CDDict.Clear();
			m_CDCountDict.Clear();
			m_CDCommonDict.Clear();

			m_CDDict = null;
			m_CDCountDict = null;
			m_CDCommonDict = null;

			m_RemoveList.Clear();
			m_RemoveList = null;
		}

		public void Update()
		{
			if (!Owner)
				return;
			foreach (KeyValuePair<int,sCDInfo> item in m_CDDict)
			{
				float fTime = item.Value.fCurTime;
				fTime = fTime - Time.deltaTime;
				if (!RecoverCDCount(item.Key, fTime))
				{
					if (fTime <= 0)
					{
						m_RemoveList.Add(item.Key);
					}
					else
					{
						item.Value.UpdateCD(fTime - Time.deltaTime);
					}
				}
			}
			foreach (int index in m_RemoveList)
			{
				m_CDDict.Remove(index);
			}

			foreach (KeyValuePair<int, sCDInfo> item in m_CDCommonDict)
			{
				float fTime = item.Value.fCurTime;
				fTime = fTime - Time.deltaTime;
				if (fTime <= 0)
				{
					m_RemoveList.Add(item.Key);
				}
				else
				{
					item.Value.UpdateCD(fTime - Time.deltaTime);
				}
			}
			foreach (int index in m_RemoveList)
			{
				m_CDCommonDict.Remove(index);
			}
		}

		public void StartCD(int nSkillID, float fCDtime)
		{
			if (!Owner)
				return;

			ConfigData pCDData = GameDataProxy.GetData(m_TableName, nSkillID);
			if (pCDData != null)
			{
				GameLogger.DebugLog(LOG_CHANNEL.COMBAT, "技能当前存在该技能CD组!");
				StartCD(nSkillID);
				return;
			}

			sCDInfo info = new sCDInfo();
			info.fCurTime = fCDtime;
			info.fTotalTime = fCDtime;

			if (m_CDDict.ContainsKey(nSkillID))
			{
				float fTime = m_CDDict[nSkillID].fCurTime;
				fTime = Mathf.Max(fTime, info.fCurTime);
				m_CDDict[nSkillID].UpdateCD(fTime);
			}
			else
			{
				AddCDToDict(nSkillID, info.fCurTime);
			}
		}
		public void StartCD(int nGroupID)
		{
			if (!Owner)
				return;
			ConfigData pCDData = GameDataProxy.GetData(m_TableName, nGroupID);
			if (pCDData == null)
				return;

			if (StartCountCD(nGroupID, pCDData))
			{
				return;
			}
			else
			{
				if (m_CDDict.ContainsKey(nGroupID))
				{
					float fTime = m_CDDict[nGroupID].fCurTime;
					fTime = Mathf.Max(fTime, pCDData.GetFloat("CDTime"));
					m_CDDict[nGroupID].UpdateCD(fTime);
				}
				else
				{
					AddCDToDict(nGroupID, pCDData.GetFloat("CDTime"));
				}
			}

			StartCommonCD(pCDData.GetInt("CDCommon"));
		}

		private bool StartCountCD(int nGroupID, ConfigData pCDData)
		{
			if (!Owner)
				return false;
			//充能计数
			int nCDCount = 0;
			int nConfigCount = pCDData.GetInt("CDCount");
			if (nConfigCount > 0)
			{
				if (m_CDCountDict.ContainsKey(nGroupID))
				{
					nCDCount = m_CDCountDict[nGroupID];
					m_CDCountDict[nGroupID] = (--nCDCount);
				}
				else
				{
					nCDCount = nConfigCount;
					m_CDCountDict.Add(nGroupID, --nCDCount);
				}

				if (!m_CDDict.ContainsKey(nGroupID))
				{
					AddCDToDict(nGroupID, pCDData.GetFloat("CDTime"));
				}
				return true;
			}
			return false;
		}

		private void AddCDToDict(int nCDindex, float fCDtime)
		{
			sCDInfo info = new sCDInfo();
			info.fCurTime = fCDtime;
			info.fTotalTime = fCDtime;
			m_CDDict.Add(nCDindex, info);
		}

		public void StartCommonCD(int nCommonCD)
		{
			if (!Owner)
				return;
			ConfigData pCommonCDData = GameDataProxy.GetData(m_TableName, nCommonCD);
			if (pCommonCDData == null)
				return;

			sCDInfo info = new sCDInfo();
			info.fCurTime = pCommonCDData.GetInt("CDTime");
			info.fTotalTime = pCommonCDData.GetInt("CDTime");
			m_CDCommonDict[pCommonCDData.GetInt("CDGroup")] = info;
		}

		public void StopCD(int nCDindex)
		{
			if (!Owner)
				return;

			m_CDDict.Remove(nCDindex);

			ConfigData pCDData = GameDataProxy.GetData(m_TableName, nCDindex);
			if (pCDData == null)
				return;
			m_CDCountDict[nCDindex] = pCDData.GetInt("CDCount");
		}

		public void StopAllCD()
		{
			m_CDDict.Clear();
			foreach (KeyValuePair<int, int> item in m_CDCountDict)
			{
				ConfigData pCDData = GameDataProxy.GetData(m_TableName, item.Key);
				if (pCDData == null)
					return;
				m_CDCountDict[item.Key] = pCDData.GetInt("CDCount");
			}
		}

		public void AddCD(int nCDindex, float fCDTime)
		{
			if (!Owner)
				return;

			if (m_CDDict.ContainsKey(nCDindex))
			{
				float fTime = m_CDDict[nCDindex].fCurTime + fCDTime;
				m_CDDict[nCDindex].UpdateCD(fTime);
			}
			else
			{
				StartCD(nCDindex, fCDTime);
			}
		}

		public void ReduceCD(int nCDindex, float fCDTime)
		{
			if (!Owner)
				return;

			if (!m_CDDict.ContainsKey(nCDindex))
				return;

			float fTime = m_CDDict[nCDindex].fCurTime - fCDTime;
			if (!RecoverCDCount(nCDindex, fTime))
			{
				m_CDDict.Remove(nCDindex);
			}
			else if (fTime > 0)
			{
				m_CDDict[nCDindex].UpdateCD(fTime);
			}
		}
		private bool RecoverCDCount(int nCDindex, float fTime)
		{
			ConfigData pCDData = GameDataProxy.GetData(m_TableName, nCDindex);
			if (pCDData == null)
				return false;

			//当且仅当充能技能在CD中且CD会被减为0以下时回复充能
			if (m_CDCountDict.ContainsKey(nCDindex) && fTime < 0)
			{
				int nCDCount = m_CDCountDict[nCDindex] + 1;
				nCDCount = Mathf.Min(pCDData.GetInt("CDCount"), nCDCount);
				m_CDCountDict[nCDindex] = nCDCount;
				return true;
			}
			return false;
		}

		/// <summary>
		/// 返回为true则正在CD中
		/// </summary>
		public bool CheckCD(int nCDindex)
		{
			if (!Owner)
				return false;

			ConfigData pCDData = GameDataProxy.GetData(m_TableName, nCDindex);
			if (pCDData == null)
				return false;

			if (m_CDCommonDict.ContainsKey(pCDData.GetInt("CDCommon")))
				return true;

			if (m_CDCountDict.ContainsKey(nCDindex) && m_CDCountDict[nCDindex] > 0)
				return false;

			if (m_CDDict.ContainsKey(nCDindex))
				return true;

			return false;
		}
	}

}