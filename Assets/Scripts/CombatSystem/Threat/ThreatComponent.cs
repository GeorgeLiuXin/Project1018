using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public class Threat
    {
        public int nAvatarID;
        public int nThreat;
        public float fValidTime;
    }

    public class ThreatComponent : ComponentBase
    {
        protected int m_nTargetID;
        protected int m_nTargetThreat;

        protected Dictionary<int, Threat> m_ThreatDict;
        private List<int> m_RemoveList;

        protected override void InitComponent()
        {
            m_ThreatDict = new Dictionary<int, Threat>();
            m_RemoveList = new List<int>();
            
            m_nTargetID = 0;
            m_nTargetThreat = 0;
            base.InitComponent();
        }

        public override void OnPreDestroy()
        {
            m_ThreatDict.Clear();
            m_RemoveList.Clear();

            m_nTargetID = 0;
            m_nTargetThreat = 0;
            base.OnPreDestroy();
        }

        public void Update()
        {
            //仇恨计时
            TickThreat(Time.deltaTime);
            //刷新目标
            TickTarget();
        }

        private void TickThreat(float fTime)
        {
            foreach (KeyValuePair<int, Threat> item in m_ThreatDict)
            {
                Threat pThreat = item.Value;
				if (pThreat != null && pThreat.fValidTime > 0)
				{
					pThreat.fValidTime -= fTime;
					if (pThreat.fValidTime <= 0)
					{
						if (pThreat.nAvatarID == m_nTargetID)
						{
							ResetTarget();
                        }
                        m_RemoveList.Add(item.Key);
                        continue;
                    }
                }
            }

            foreach (int key in m_RemoveList)
            {
                m_ThreatDict.Remove(key);
            }
            
            if (m_ThreatDict.Count == 0)
            {
                if (Owner && Owner.IsFight())
                {
                    Owner.LeaveCombat();
                }
            }
        }

        public void TickTarget()
        {
            foreach (KeyValuePair<int, Threat> item in m_ThreatDict)
            {
                Threat pThreat = item.Value;
				if (CompareThreat(pThreat))
				{
					SetTarget(pThreat.nAvatarID, pThreat.nThreat);
                }
            }
            //当前没有目标, 清空仇恨, 脱战
            if (m_nTargetID == 0)
            {
                ResetTarget();
            }
        }
		private bool CompareThreat(Threat pThreat)
		{
			if (m_nTargetID == 0)
				return true;
			if (pThreat != null && pThreat.nThreat >= m_nTargetThreat * 1.1f)
				return true;
			return false;
		}


		public void SetTarget(int nAvatarID, int nThreat)
        {
            m_nTargetID = nAvatarID;
            m_nTargetThreat = nThreat;
        }

        public int GetTarget()
        {
            return m_nTargetID;
        }

        public int GetThreatByIndex(int id)
        {
            if (id < 0 || id >= m_ThreatDict.Count)
            {
                return 0;
            }
            int index = 0;
            foreach (var item in m_ThreatDict)
            {
                if (index == id)
                {
                    return item.Key;
                }
                index++;
            }
            return 0;
        }

        public void ResetTarget()
        {
            m_nTargetID = 0;
            m_nTargetThreat = 0;
		}

		public void OnHurt(ActorObj pAvatar, int nValue)
		{
			if (Owner)
			{
				AddThreat(pAvatar, nValue);
			}
		}

		public void OnHeal(ActorObj pAvatar, int nValue)
		{
			if (!Owner)
				return;

			foreach (KeyValuePair<int, Threat> item in m_ThreatDict)
			{
				Threat pThreat = item.Value;
				if (pThreat == null)
					continue;
				//TODO		当人物管理完成后添加，当治疗时对当前攻击治疗目标的所有人产生仇恨
				//ActorObj pTAvatar = Owner->GetSceneAvatar(pThreat.nAvatarID);
				//if (pTAvatar)
				//{
				//	pTAvatar->AddThreat(pAvatar, nValue, true);
				//}
			}
		}

		public void OnTaunt(ActorObj pAvatar, int nValue)
		{
			if (!Owner)
				return;

			//受到嘲讽时，将第一名的加上去
			Threat pThreat = GetThreat(pAvatar.GetAvatarID(), true);
			pThreat.nThreat += nValue;
			if (!CompareThreat(pThreat))
			{
				int nMaxThreat = Mathf.Max(m_nTargetThreat, pThreat.nThreat);
				//嘲讽是否存在可能越界的情况
				pThreat.nThreat += nMaxThreat;
			}

			if (pThreat != null)
			{
				SetTarget(pAvatar.GetAvatarID(), pThreat.nThreat);
			}
		}

		public void AddThreat(ActorObj pAvatar, int nValue)
		{
			if (Owner == null || pAvatar == null)
				return;
            if (Owner == pAvatar)
                return;

			Threat pThreat = GetThreat(pAvatar.GetAvatarID(), true);
			if (pThreat == null)
				return;
			
            if (nValue >= 0 || pThreat.nThreat <= 0)
            {
                pThreat.nThreat += nValue;
            }
            pThreat.nThreat = Mathf.Max(-1, pThreat.nThreat);
            if (CompareThreat(pThreat))
            {
                SetTarget(pThreat.nAvatarID, pThreat.nThreat);
            }

            //设置战斗状态
            if (!Owner.IsFight())
            {
				Owner.EnterCombat();
            }
        }

        public void RemoveThreat(int nAvatarID)
        {
            if (m_ThreatDict.ContainsKey(nAvatarID))
            {
                m_ThreatDict.Remove(nAvatarID);
            }
            if (m_nTargetID == nAvatarID)
            {
                ResetTarget();
                TickTarget();
            }
        }

        public Threat GetThreat(int nAvatarID, bool bCreate)
		{
			Threat pThreat = null;
			if (m_ThreatDict.ContainsKey(nAvatarID))
			{
				pThreat = m_ThreatDict[nAvatarID];
			}
			else if (bCreate)
			{
				pThreat = new Threat();
				pThreat.nAvatarID = nAvatarID;
				pThreat.nThreat = 0;
				m_ThreatDict.Add(nAvatarID, pThreat);
			}
			return pThreat;
		}

        public bool IsInThreatList(int nAvatarID)
        {
            return m_ThreatDict.ContainsKey(nAvatarID);
        }

        public bool IsThreatListEmpty()
        {
            return m_ThreatDict.Count == 0;
        }
        
        public int GetThreatCount()
        {
            return m_ThreatDict.Count;
        }
    }

}
