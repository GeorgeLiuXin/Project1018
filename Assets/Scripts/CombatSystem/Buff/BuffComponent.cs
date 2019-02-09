using System;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public class BuffComponent : ComponentBase
    {


        //private BuffUIManager BuffUI
        //{
        //    get
        //    {
        //        if (m_BuffUI == null)
        //        {
        //            m_BuffUI = XWorldGameModule.GetGameManager<BuffUIManager>();
        //        }
        //        return m_BuffUI;
        //    }
        //}

        //private BuffUIManager m_BuffUI;
        //private BuffEffectManager m_BuffEffectManager = new BuffEffectManager();

        //public override void SetOwner(ActorObj logicObject)
        //{
        //    base.SetOwner(logicObject);
        //    if (this.Owner != null)
        //    {
        //        m_BuffEffectManager.Owner = logicObject;
        //        XWorldCommonDataCont con = this.Owner.CommonDataMgr.GetCont(ParamType.Param_buff);
        //        con.OnInit += new XWorldCommonDataCont.CommonDataCallBack(this.OnBuffInit);
        //        con.OnCreate += new XWorldCommonDataCont.CommonDataCallBack(this.OnBuffCreate);
        //        con.OnUpdate += new XWorldCommonDataCont.CommonDataCallBack(this.OnBuffUpdate);
        //        con.OnDelete += new XWorldCommonDataCont.CommonDataCallBack(this.OnBuffDelete);
        //    }
        //}

        //private void OnBuffDelete(XWorldCommonData obj)
        //{
        //    GBuff buff = obj as GBuff;
        //    if (buff != null)
        //    {
        //        GameLogger.DebugLog(LOG_CHANNEL.NETWORK, string.Format("OnBuffDelete Buff [{1}] Actor[{0}]  ", this.Owner.ServerID, buff.BuffId));
        //        m_BuffEffectManager.RemoveBuff(buff);

        //        if (BuffUI != null && Owner != null)
        //        {
        //            BuffUI.RemoveBuff(Owner.ServerID, buff);
        //        }
        //        if (XWorldGameModule.GetGameManager<XWorldActorManager>().IsLocalPlayer(buff.ServerID))
        //        {
        //            NotifyDeleteBuff(buff);
        //        }
        //    }
        //}

        //private void NotifyDeleteBuff(GBuff buff)
        //{
        //    if (buff != null)
        //    {
        //        GBuffData buffData = GetBuffData(buff.BuffId, buff.BuffLevel);
        //        if (buffData != null)
        //        {
        //            int buffState = buffData.MBV_BuffState;
        //            if ((buffState & 256) != 0)
        //            {
        //                // 这是个嘲讽的buff
        //                EventListener.Instance.Dispatch(CltEvent.Skill.Delete_Taunt_Buff);
        //            }
        //        }
        //    }
        //}

        //private void OnBuffUpdate(XWorldCommonData obj)
        //{
        //    GBuff buff = obj as GBuff;
        //    if (buff != null)
        //    {
        //        GameLogger.DebugLog(LOG_CHANNEL.NETWORK, string.Format("OnBuffUpdate Buff [{1}] Actor[{0}]  ", this.Owner.ServerID, buff.BuffId));
        //    }
        //    if (m_BuffEffectManager != null)
        //    {
        //        m_BuffEffectManager.UpdateBuff(buff);
        //    }

        //    if (BuffUI != null && Owner != null)
        //    {
        //        BuffUI.UpdateBuff(Owner.ServerID, buff);
        //    }
        //}

        //private void OnBuffCreate(XWorldCommonData obj)
        //{
        //    GBuff buff = obj as GBuff;
        //    if (buff != null)
        //    {
        //        GameLogger.DebugLog(LOG_CHANNEL.NETWORK, string.Format("OnBuffCreate Buff [{1}] Actor[{0}]  ", this.Owner.ServerID, buff.BuffId));
        //        OnAddBuff(buff);
        //    }
        //}

        //private void OnBuffInit(XWorldCommonData obj)
        //{
        //    GBuff buff = obj as GBuff;
        //    if (buff != null)
        //    {
        //        GameLogger.DebugLog(LOG_CHANNEL.NETWORK, string.Format("OnBuffInit Buff [{1}] Actor[{0}]  ", this.Owner.ServerID, buff.BuffId));
        //        OnAddBuff(buff);
        //    }
        //}

        //private void OnAddBuff(GBuff buff)
        //{
        //    if (buff != null)
        //    {
        //        if (m_BuffEffectManager != null)
        //        {
        //            m_BuffEffectManager.AddBuff(buff);
        //        }

        //        if (BuffUI != null && Owner != null)
        //        {
        //            BuffUI.AddBuff(Owner.ServerID, buff);
        //        }

        //        if (XWorldGameModule.GetGameManager<XWorldActorManager>().IsLocalPlayer(buff.ServerID))
        //        {
        //            NotifyAddBuff(buff);
        //        }
        //    }
        //}

        //private void NotifyAddBuff(GBuff buff)
        //{
        //    if (buff != null)
        //    {
        //        GBuffData buffData = GetBuffData(buff.BuffId, buff.BuffLevel);
        //        if (buffData != null)
        //        {
        //            int buffState = buffData.MBV_BuffState;
        //            if ((buffState & 256) != 0) 
        //            {
        //                // 这是个嘲讽的buff
        //                EventListener.Instance.Dispatch(CltEvent.Skill.Add_Taunt_Buff, buff.CasterID);
        //            }
        //        }
        //    }
        //}

        //public GBuff GetBuff(int buffID)
        //{
        //    XWorldCommonDataCont con = this.Owner.CommonDataMgr.GetCont(ParamType.Param_buff);
        //    if (con == null)
        //    {
        //        return null;
        //    }
        //    return con.GetCommonDataByDataID(buffID) as GBuff;
        //}

        //public GBuff[] GetBuffs()
        //{
        //    XWorldCommonDataCont con = this.Owner.CommonDataMgr.GetCont(ParamType.Param_buff);
        //    if (con == null)
        //    {
        //        return null;
        //    }
        //    Dictionary<int, XWorldCommonData> dataMap = con.CommonDataMapDataID;
        //    if (dataMap != null && dataMap.Count > 0)
        //    {
        //        GBuff[] buffs = new GBuff[dataMap.Count];

        //        int i = 0;
        //        foreach (XWorldCommonData commonData in dataMap.Values)
        //        {
        //            GBuff buff = commonData as GBuff;
        //            buffs[i] = buff;
        //            i++;
        //        }
        //    }

        //    return null;
        //}

        //public GBuffData GetBuffData(int nDataID, int nLevel = 1)
        //{
        //    GBuffData buffData = XWorldGameModule.GetGameManager<GModifyDataManager>().GetBuffData(nDataID, nLevel);

        //    if (buffData == null)
        //    {
        //        GameLogger.Error(LOG_CHANNEL.LOGIC, "buffData is null " + nDataID + "  " + nLevel);
        //    }
        //    return buffData;
        //}

        //private void Update()
        //{
        //    m_BuffEffectManager.Update(Time.deltaTime);
        //}
    }
}
