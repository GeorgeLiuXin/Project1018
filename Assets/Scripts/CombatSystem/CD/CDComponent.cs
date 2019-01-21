using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{

    public struct sCDInfo
    {
        public float fCurTime;
        public float fTotalTime;
        public void UpdateCD(float fTime)
        {
            fCurTime = fTime;
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

        protected override void InitComponent()
        {
            m_CDDict = new Dictionary<int, sCDInfo>();
            m_CDCountDict = new Dictionary<int, int>();
            m_CDCommonDict = new Dictionary<int, sCDInfo>();
        }

        public override void OnPreDestroy()
        {
            m_CDDict.Clear();
            m_CDCountDict.Clear();
            m_CDCommonDict.Clear();
            m_CDDict = null;
            m_CDCountDict = null;
            m_CDCommonDict = null;
        }

        public void Update()
        {

        }

        private void AddCDToDict(int nGroupID,float fTime)
        {
            sCDInfo info = new sCDInfo();
            info.fCurTime = fTime;
            info.fTotalTime = fTime;
            m_CDDict.Add(nGroupID, info);
        }

        public void StartCD(int nSkillID, int nCDtime)
        {
            if (!Owner)
                return;
            sCDInfo info = new sCDInfo();
            info.fCurTime = nCDtime;
            info.fTotalTime = nCDtime;
            
            if (StartCountCD(nSkillID, pCDGroup))
            {
                return;
            }
            else
            {
                if (m_CDDict.ContainsKey(nGroupID))
                {
                    float fTime = m_CDDict[nGroupID].fCurTime;
                    fTime = Mathf.Max(fTime, pCDGroup.GetFloat("CDTime"));
                    m_CDDict[nGroupID].UpdateCD(fTime);
                }
                else
                {
                    AddCDToDict(nGroupID, pCDGroup.GetFloat("CDTime"));
                }
            }

            StartCommonCD(pCDGroup.GetInt("CDCommon"));
        }

        public void StartCD(int nGroupID)
        {
            if (!Owner)
                return;
            ConfigData pCDGroup = GameDataProxy.GetData(m_TableName, nGroupID);
            if (pCDGroup == null)
                return;
            
            if (StartCountCD(nGroupID,pCDGroup))
            {
                return;
            }
            else
            {
                if (m_CDDict.ContainsKey(nGroupID))
                {
                    float fTime = m_CDDict[nGroupID].fCurTime;
                    fTime = Mathf.Max(fTime, pCDGroup.GetFloat("CDTime"));
                    m_CDDict[nGroupID].UpdateCD(fTime);
                }
                else
                {
                    AddCDToDict(nGroupID, pCDGroup.GetFloat("CDTime"));
                }
            }

            StartCommonCD(pCDGroup.GetInt("CDCommon"));
        }

        private bool StartCountCD(int nGroupID, ConfigData pCDGroup)
        {
            //充能计数
            int nCDCount = 0;
            int nConfigCount = pCDGroup.GetInt("CDCount");
            if (nConfigCount > 0)
            {
                if (m_CDCountDict.ContainsKey(nGroupID))
                {
                    nCDCount = m_CDCountDict[nGroupID];
                }
                else
                {
                    nCDCount = pCDGroup.GetInt("CDCount");
                }
                m_CDCountDict[nGroupID] = (--nCDCount);

                if (!m_CDDict.ContainsKey(nGroupID))
                {
                    AddCDToDict(nGroupID, pCDGroup.GetFloat("CDTime"));
                }
                return true;
            }
            return false;
        }

        public void StartCommonCD(int nCommonCD)
        {

        }

        //void GCDComponent::StartCDCommon(int32 nGroupID)
        //{
        //    CDTime* pCommonCD = GCDManager::Instance().GetCDTime(nGroupID);
        //    if (pCommonCD && pCommonCD->IsCommonCD())
        //    {
        //        m_CDCommonMap[pCommonCD->m_nCDGroup] = pCommonCD->m_nCDTime;
        //    }
        //}

        //void GCDComponent::StopCD(int32 nGroupID)
        //{
        //    if (!m_pAvatar)
        //        return;

        //    if (nGroupID == -1)
        //    {
        //        m_CDMap.clear();
        //        m_CDCountMap.clear();
        //    }
        //    else
        //    {
        //        m_CDMap.Remove(nGroupID);
        //        m_CDCountMap.Remove(nGroupID);
        //    }

        //    GPacketCDFinish pkt;
        //    pkt.nCDGroup = nGroupID;
        //    m_pAvatar->SendPacket(&pkt);
        //}

        //void GCDComponent::AddCD(int32 nGroupID, int32 nCDTime)
        //{
        //    if (!m_pAvatar)
        //        return;

        //    if (m_CDMap.Find(nGroupID))
        //    {
        //        int32 nTime = m_CDMap.Get() + nCDTime;
        //        m_CDMap.Set(nTime);

        //        GPacketCDUpdate pkt;
        //        pkt.nCDGroup = nGroupID;
        //        pkt.nCDTime = nTime;
        //        m_pAvatar->SendPacket(&pkt);
        //    }
        //    else
        //    {
        //        StartCD(nGroupID, nCDTime, false);
        //    }
        //}

        //void GCDComponent::ReduceCD(int32 nGroupID, int32 nCDTime)
        //{
        //    if (!m_pAvatar)
        //        return;

        //    if (!m_CDMap.Find(nGroupID))
        //        return;

        //    int32 nTime = m_CDMap.Get() - nCDTime;
        //    if (nTime > 0)
        //    {
        //        m_CDMap.Set(nTime);

        //        GPacketCDUpdate pkt;
        //        pkt.nCDGroup = nGroupID;
        //        pkt.nCDTime = nTime;
        //        m_pAvatar->SendPacket(&pkt);
        //    }
        //    else if (!RecoverCDCount(nGroupID))
        //    {
        //        GPacketCDFinish pkt;
        //        pkt.nCDGroup = nGroupID;
        //        m_pAvatar->SendPacket(&pkt);
        //        m_CDMap.Remove();
        //    }
        //}

        //bool GCDComponent::CheckCD(int32 nGroupID)
        //{
        //    CDTime* pCDTime = GCDManager::Instance().GetCDTime(nGroupID);
        //    if (!pCDTime)
        //        return false;

        //    if (m_CDCommonMap.Find(pCDTime->m_nCDCommon) && m_CDCommonMap.Get() > 200)
        //        return true;

        //    if (m_CDCountMap.Find(nGroupID) && m_CDCountMap.Get() > 0)
        //        return false;

        //    if (m_CDMap.Find(nGroupID))
        //        return true;

        //    return false;
        //}

        //bool GCDComponent::RecoverCDCount(int32 nGroupID)
        //{
        //    CDTime* pCDTime = GCDManager::Instance().GetCDTime(nGroupID);
        //    if (!pCDTime || pCDTime->m_nCDCount <= 0)
        //        return false;

        //    if (m_CDCountMap.Find(nGroupID))
        //    {
        //        int32 & nCDCount = m_CDCountMap.Get();
        //        if ((++nCDCount) >= pCDTime->m_nCDCount)
        //        {
        //            m_CDCountMap.Remove();
        //            return false;
        //        }
        //        else
        //        {
        //            m_CDMap[nGroupID] = pCDTime->m_nCDTime;

        //            GPacketCDStart pkt;
        //            pkt.nCDGroup = nGroupID;
        //            pkt.nCDTime = pCDTime->m_nCDTime;
        //            pkt.nCDCount = nCDCount;
        //            m_pAvatar->SendPacket(&pkt);
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //void GCDComponent::NodeTick(int32 nFrameTime)
        //{
        //    if (!m_pAvatar)
        //        return;

        //    m_CDMap.Begin();
        //    while (!m_CDMap.IsEnd())
        //    {
        //        int32 & nTime = m_CDMap.Get();
        //        nTime -= nFrameTime;
        //        if (nTime <= 0 && !RecoverCDCount(m_CDMap.GetKey()))
        //        {
        //            GPacketCDFinish pkt;
        //            pkt.nCDGroup = m_CDMap.GetKey();
        //            m_pAvatar->SendPacket(&pkt);
        //            m_CDMap.Remove();
        //        }
        //        else
        //        {
        //            m_CDMap.Next();
        //        }
        //    }

        //    m_CDCommonMap.Begin();
        //    while (!m_CDCommonMap.IsEnd())
        //    {
        //        int32 & nCDTime = m_CDCommonMap.Get();
        //        nCDTime -= nFrameTime;
        //        (nCDTime > 0) ? m_CDCommonMap.Next() : m_CDCommonMap.Remove();
        //    }
        //}

        //void GCDComponent::WorldTick(int32 nFrameTime)
        //{
        //    if (m_CDMap.Count() <= 0)
        //        return;

        //    m_CDMap.Begin();
        //    while (!m_CDMap.IsEnd())
        //    {
        //        int32 & nTime = m_CDMap.Get();
        //        nTime -= nFrameTime;
        //        (nTime > 0) ? m_CDMap.Next() : m_CDMap.Remove();
        //    }
        //}
        //void GCDComponent::UpdateCD(GPacketCDUpdate* pPkt)
        //{
        //    if (pPkt)
        //    {
        //        StartCD(pPkt->nCDGroup, pPkt->nCDTime);
        //    }
        //}

    }

}