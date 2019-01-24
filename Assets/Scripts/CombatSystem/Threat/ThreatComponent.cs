using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public struct Threat
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
                if (pThreat.fValidTime > 0)
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
                Threat threat = item.Value;
                if (threat.nThreat >= m_nTargetThreat * 1.1f)
                {
                    SetTarget(threat.nAvatarID, threat.nThreat);
                }
            }
            //当前没有目标, 清空仇恨, 脱战
            if (m_nTargetID == 0)
            {
                ResetTarget();
            }
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

        public void AddThreat(ActorObj pAvatar)
        {
            if (Owner == null || pAvatar == null)
                return;
            if (Owner == pAvatar)
                return;
            
            GThreat* pThreat = GetThreat(pAvatar->GetAvatarID(), true);
            if (!pThreat)
                return;

            //当前有仇恨值(>=0)时,可以成为目标,仇恨值只能增加
            if (nValue >= 0 || pThreat->m_nThreat <= 0)
            {
                pThreat->m_nThreat += nValue;
            }
            pThreat->m_nThreat = MAX(-1, pThreat->m_nThreat);
            if (m_nTargetID == 0 || (!m_bTauntState && pThreat->m_nThreat >= m_nTargetThreat * 1.1f))
            {
                SetTarget(pThreat->m_nAvatarID, pThreat->m_nThreat);
            }

            //设置战斗状态
            if (!m_pOwnerNode->IsFight())
            {
                m_pOwnerNode->EnterCombat();
            }

            //仇恨链接
            if (bLink)
            {
                if (m_pOwnerNode->CheckRelation(pAvatar, ToEnemy))
                {
                    pAvatar->AddThreat(m_pOwnerNode, 0, false);
                }
                else
                {
                    pAvatar->AddThreat(m_pOwnerNode, -1, false);
                }
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

        public void GetThreat()
        {

        }

        public void OnHurt()
        {

        }

        public void OnHeal()
        {

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


//    //伤害
//    void GNodeThreatComponent::OnHurt(GNodeAvatar* pAvatar, int32 nValue)
//    {
//        if (m_pOwnerNode)
//        {
//            m_pOwnerNode->AddThreat(pAvatar, nValue, true);
//        }
//    }
//    //治疗
//    void GNodeThreatComponent::OnHeal(GNodeAvatar* pAvatar, int32 nValue)
//    {
//        if (!m_pOwnerNode)
//            return;

//        m_vThreatList.Begin();
//        while (!m_vThreatList.IsEnd())
//        {
//            GThreat* pThreat = m_vThreatList.Get();
//            if (pThreat)
//            {
//                GNodeAvatar* pTAvatar = m_pOwnerNode->GetSceneAvatar(pThreat->m_nAvatarID);
//                if (pTAvatar)
//                {
//                    pTAvatar->AddThreat(pAvatar, nValue, true);
//                }
//            }
//            m_vThreatList.Next();
//        }
//    }
//    //受到嘲讽
//    void GNodeThreatComponent::OnTaunt(GNodeAvatar* pAvatar)
//    {
//        if (!pAvatar)
//            return;

//        GThreat* pThreat = GetThreat(pAvatar->GetAvatarID());
//        if (pThreat)
//        {
//            pThreat->m_nThreat = MAX(m_nTargetThreat, pThreat->m_nThreat);
//        }
//        else
//        {
//            OnHurt(pAvatar, m_nTargetThreat);
//            pThreat = GetThreat(pAvatar->GetAvatarID());
//        }

//        m_bTauntState = true; // 受到了嘲讽		
//        if (pThreat)
//        {
//            SetTarget(pAvatar->GetAvatarID(), pThreat->m_nThreat, true);
//        }
//    }

//    //获取仇恨
//    GThreat* GNodeThreatComponent::GetThreat(int32 nAvatarID, bool bCreate)
//    {
//        GThreat* pThreat = NULL;
//        if (m_vThreatList.Find(nAvatarID))
//        {
//            pThreat = m_vThreatList.Get();
//        }
//        else if (bCreate)
//        {
//            pThreat = FACTORY_NEWOBJ(GThreat);
//            if (pThreat)
//            {
//                pThreat->m_nAvatarID = nAvatarID;
//                pThreat->m_nThreat = 0;
//                m_vThreatList[nAvatarID] = pThreat;
//            }
//        }
//        return pThreat;
//    }
