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
        protected bool m_bTauntState;
        protected bool m_bNeedUpdate;

        protected int m_nTargetID;
        protected int m_nTargetThreat;

        private List<int> m_RemoveList;
        protected Dictionary<int, Threat> m_ThreatDict;

        protected override void InitComponent()
        {
            m_RemoveList = new List<int>();
            m_ThreatDict = new Dictionary<int, Threat>();
            m_bTauntState = false;
            m_bNeedUpdate = false;
            m_nTargetID = 0;
            m_nTargetThreat = 0;
            base.InitComponent();
        }

        public override void OnPreDestroy()
        {
            m_RemoveList.Clear();
            m_ThreatDict.Clear();
            ClearThreat();
            base.OnPreDestroy();
        }

        public void Update()
        {
            //嘲讽状态
            //TickTaunt(nFrameTime);
            //仇恨计时
            TickThreat(Time.deltaTime);
        }

        private void TickThreat(float fTime)
        {
            foreach (KeyValuePair<int, Threat> item in m_ThreatDict)
            {
                Threat pThreat = item.Value;
                if (pThreat.fValidTime > 0)
                {
                    pThreat.fValidTime -= fTime;
                    if (pThreat.fValidTime<=0)
                    {
                        if (Owner)
                        {
                            GNodeAvatar* pAvatar = m_pOwnerNode->GetSceneAvatar(pThreat->m_nAvatarID);
                            if (pAvatar)
                            {
                                pAvatar->RemoveThreat(m_pOwnerNode->GetAvatarID());
                            }
                        }

                        if (pThreat->m_nAvatarID == m_nTargetID)
                        {
                            ResetTarget();
                            m_bNeedUpdate = true;
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
            
            //刷新仇恨目标
            if (m_bNeedUpdate)
            {
                m_bNeedUpdate = false;
                UpdateTarget();
            }

            if (m_ThreatDict.Count == 0)
            {
                if (Owner && Owner.IsFight())
                {
                    Owner.LeaveCombat();
                }
            }
        }

        private void TickTaunt(int nFrameTime)
        {
            if (m_bTauntState)
            {
                if (Owner && !Owner->CheckState(GAS_Taunt))
                {
                    m_bTauntState = false;
                    m_bNeedUpdate = true;
                }
            }
            else
            {
                if (m_pOwnerNode && m_pOwnerNode->CheckState(GAS_Taunt))
                {
                    m_bTauntState = true;
                }
            }
        }


        public void OnHurt()
        {

        }

        public void OnHeal()
        {

        }

        public void AddThreat()
        {

        }

        public void RemoveThreat()
        {

        }

        public void ClearThreat()
        {

        }

        public void GetThreat()
        {

        }
        public void GetThreatByIndex(int id)
        {

        }

        public int GetTarget()
        {
            return 0;
        }

        public void SetTarget()
        {

        }

        public void ResetTarget()
        {

        }

        public void UpdateTarget()
        {

        }

        public bool IsInThreatList(int nAvatarID)
        {
            return false;
        }

        public bool IsThreatListEmpty()
        {
            return false;
        }

        public bool IsTaunt() { return m_bTauntState; } // 返回值本身就是由GAS_Taunt控制的，因此可提供直接使用的接口。
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
//    //增加仇恨
//    void GNodeThreatComponent::AddThreat(GNodeAvatar* pAvatar, int32 nValue, bool bLink)
//    {
//        if (m_pOwnerNode == NULL || pAvatar == NULL)
//            return;

//        if (!pAvatar->IsShow())
//        {
//            return;
//        }

//        if (m_pOwnerNode->GetAvatarID() == pAvatar->GetAvatarID())
//            return;

//        //不可选中的npc 不计算仇恨
//        if (!pAvatar->IsCalculationThreat())
//        {
//            return;
//        }

//        GThreat* pThreat = GetThreat(pAvatar->GetAvatarID(), true);
//        if (!pThreat)
//            return;

//        //当前有仇恨值(>=0)时,可以成为目标,仇恨值只能增加
//        if (nValue >= 0 || pThreat->m_nThreat <= 0)
//        {
//            pThreat->m_nThreat += nValue;
//        }
//        pThreat->m_nThreat = MAX(-1, pThreat->m_nThreat);
//        if (m_nTargetID == 0 || (!m_bTauntState && pThreat->m_nThreat >= m_nTargetThreat * 1.1f))
//        {
//            SetTarget(pThreat->m_nAvatarID, pThreat->m_nThreat);
//        }

//        //设置战斗状态
//        if (!m_pOwnerNode->IsFight())
//        {
//            m_pOwnerNode->EnterCombat();
//        }

//        //仇恨链接
//        if (bLink)
//        {
//            if (m_pOwnerNode->CheckRelation(pAvatar, ToEnemy))
//            {
//                pAvatar->AddThreat(m_pOwnerNode, 0, false);
//            }
//            else
//            {
//                pAvatar->AddThreat(m_pOwnerNode, -1, false);
//            }
//        }
//    }
//    //移除仇恨
//    void GNodeThreatComponent::RemoveThreat(int32 nAvatarID)
//    {
//        if (m_vThreatList.Find(nAvatarID))
//        {
//            FACTORY_DELOBJ(m_vThreatList.Get());
//            m_vThreatList.Remove();
//        }
//        if (m_nTargetID == nAvatarID)
//        {
//            ResetTarget();
//            UpdateTarget();
//        }
//    }
//    //清空仇恨
//    void GNodeThreatComponent::ClearThreat()
//    {
//        //清空当前仇恨
//        ResetTarget();
//        //广播清除仇恨
//        m_vThreatList.Begin();
//        while (!m_vThreatList.IsEnd())
//        {
//            GThreat* pThreat = m_vThreatList.Get();
//            if (pThreat)
//            {
//                GNodeAvatar* pAvatar = m_pOwnerNode->GetSceneAvatar(pThreat->m_nAvatarID);
//                if (pAvatar)
//                {
//                    pAvatar->RemoveThreat(m_pOwnerNode->GetAvatarID());
//                }
//            }
//            m_vThreatList.Remove();
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

//    bool GNodeThreatComponent::IsInThreatList(int32 nAvatarID)
//    {
//        return (m_vThreatList.Find(nAvatarID));
//    }

//    //获取仇恨对象
//    int32 GNodeThreatComponent::GetTargetID()
//    {
//        return m_nTargetID;
//    }

//    void GNodeThreatComponent::SetTarget(int32 nAvatarID, int32 nThreat, bool bTaunt/* = false*/)
//    {
//        if (nThreat >= 0 || bTaunt)
//        {
//            if (m_pOwnerNode)
//            {
//                m_pOwnerNode->SetTargetID(nAvatarID);
//            }
//            m_nTargetID = nAvatarID;
//            m_nTargetThreat = nThreat;
//        }
//    }

//    void GNodeThreatComponent::ResetTarget()
//    {
//        m_nTargetID = 0;
//        m_nTargetThreat = 0;
//    }

//    void GNodeThreatComponent::UpdateTarget()
//    {
//        m_vThreatList.Begin();
//        while (!m_vThreatList.IsEnd())
//        {
//            GThreat* pThreat = m_vThreatList.Get();
//            if (pThreat && pThreat->m_nThreat >= m_nTargetThreat * 1.1f)
//            {
//                SetTarget(pThreat->m_nAvatarID, pThreat->m_nThreat);
//            }
//            m_vThreatList.Next();
//        }
//        //当前没有目标, 清空仇恨, 脱战
//        if (m_nTargetID == 0)
//        {
//            ClearThreat();
//        }
//    }

//    bool GNodeThreatComponent::IsThreatListEmpty()
//    {
//        return m_vThreatList.empty();
//    }

//    Galaxy::GThreat* GNodeThreatComponent::GetThreatByIndex(int id)
//    {
//        if (id < 0 || id >= m_vThreatList.Count())
//        {
//            return NULL;
//        }
//        GThreatList::iterator iter = m_vThreatList.begin();
//        std::advance(iter, id);
//        return iter->second;
//    }
