using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    /// <summary>
    /// 触发技能
    /// </summary>
    public class SkillTrigger : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
//#include "PreComp.h"
//#include "GNodeSkillTrigger.h"
//#include "GNodeTriggerNotify.h"
//#include "GNodeSkillTarget.h"
//#include "GNodeSkillLogic.h"
//#include "GSkillData.h"
//#include "SkillDefine.h"
//#include "GNodeSkillComponent.h"
//#include "NodeBuff/GBuffComponent.h"
//#include "NodeAvatar/NodeAvatar.h"
//#include "CDManager.h"
//#include "GCDComponent.h"

//namespace Galaxy
//{

//    FINISH_FACTORY_Arg0(GSkillSpellLogic_Trigger);

//    GSkillSpellLogic_Trigger::GSkillSpellLogic_Trigger()
//		: m_nTriggerCD(0)
//    {
//        m_bActive = false;
//        m_nEffectTime = 0;
//        m_nEffectCount = 0;
//        m_nCurTickTime = 0;
//        m_nCurEffectCount = 0;
//    }

//    GSkillSpellLogic_Trigger::~GSkillSpellLogic_Trigger()
//    {

//    }

//    bool GSkillSpellLogic_Trigger::Init(GSkillInitParam& param)
//    {
//        bool res = GSkillSpellLogic_Passive::Init(param);
//        if (!param.pSkillData || !res)
//        {
//            return false;
//        }
//        m_nEffectTime = param.pSkillData->GetIntValue(MSV_EffectTime);
//        m_nEffectCount = param.pSkillData->GetIntValue(MSV_EffectCount);
//        return true;
//    }

//    void GSkillSpellLogic_Trigger::Tick(int32 nFrameTime)
//    {
//        if (m_nTriggerCD > 0)
//        {
//            m_nTriggerCD -= nFrameTime;
//        }
//        if (m_bActive && m_nEffectCount > 0)
//        {
//            m_nCurTickTime += nFrameTime;
//            if (m_nCurTickTime < m_nEffectTime)
//            {
//                return;
//            }
//            m_nCurTickTime -= nFrameTime;
//            ProcessEffect();
//            m_nCurEffectCount++;
//            if (m_nCurEffectCount >= m_nEffectCount)
//            {
//                m_bActive = false;
//            }
//        }
//    }

//    bool GSkillSpellLogic_Trigger::ProcessTrigger(GTriggerNotify* pNotify)
//    {
//        //检查触发条件
//        if (!CheckNotify(pNotify))
//            return false;



//        GalaxyLog::debug("Skill Trigger Succ %d", m_pSkillData->m_nDataID);
//        //设置触发CD
//        SetTriggerCD();
//        //设置触发消耗
//        SetTriggerCost();
//        //转化技能效果
//        TransfromEffectNotify(pNotify);
//        //产生触发效果
//        OnTrigger(pNotify);
//        //产生触发事件
//        if (m_pSkillData && m_pSkillData->IsTriggerTriggerNotify())
//        {
//            GNodeAvatar* pCaster = GetCaster();
//            if (pCaster && pCaster->GetSkillComponent())
//            {
//                GetTarget();
//                pCaster->GetSkillComponent()->PushTriggerNotify(m_pSkillData->m_nDataID, m_TargetInfo.m_nTargetID, NotifyType_Trigger, 0, 0, &pNotify->m_vSrcPos, &pNotify->m_vTarPos, &pNotify->m_vDir);
//            }
//        }

//        Reset();
//        ResetRoleAValue();
//        return true;
//    }

//    bool GSkillSpellLogic_Trigger::CheckNotify(GTriggerNotify* pNotify)
//    {
//        if (!m_pOwner || !m_pSkillData || !pNotify)
//            return false;

//        //检查CD
//        if (m_pSkillData->IsTriggerCommonCD())
//        {
//            int32 nCDGroup = m_pSkillData->GetIntValue(MSV_CDGroup);
//            GCDComponent* pCDComponent = m_pOwner->GetCDComponent();
//            if (!pCDComponent || pCDComponent->CheckCD(nCDGroup))
//            {
//                return false;
//            }
//        }
//        else if (m_nTriggerCD > 0)
//        {
//            return false;
//        }

//        GNodeAvatar* pCaster = GetCaster();
//        if (!pCaster || !pCaster->GetSkillComponent())
//            return false;

//        GNodeAvatar* pTarget = NULL;

//        if (m_pSkillData->IsTargetSelfOnly())
//        {
//            pTarget = pCaster;
//            m_TargetInfo.m_nTargetID = pCaster->GetAvatarID();
//            m_TargetInfo.m_vSrcPos = pCaster->GetPos();
//            m_TargetInfo.m_vTarPos = pCaster->GetPos();
//            m_TargetInfo.m_vAimDir = pCaster->GetDir();
//        }
//        else
//        {
//            pTarget = m_pOwner->GetSceneAvatar(pNotify->m_nTargetID);
//            m_TargetInfo.m_nTargetID = pNotify->m_nTargetID;
//            m_TargetInfo.m_vTarPos = pNotify->m_vTarPos;
//            m_TargetInfo.m_vSrcPos = pNotify->m_vSrcPos;
//            if (pNotify->m_vDir.IsZeroFast())
//            {
//                m_TargetInfo.m_vAimDir = m_TargetInfo.m_vTarPos - m_TargetInfo.m_vSrcPos;
//            }
//            else
//            {
//                m_TargetInfo.m_vAimDir = pNotify->m_vDir;
//            }
//            m_TargetInfo.m_vAimDir.z = 0;
//            m_TargetInfo.m_vAimDir.normalize();
//        }

//        //检查消耗
//        if (!pCaster->GetSkillComponent()->CheckCost(m_pSkillData))
//            return false;

//        if (!pNotify->CheckTrigger(pCaster, pTarget, m_pSkillData))
//            return false;

//        if (m_pBuff && m_pBuff->IsInvalid())
//            return false;

//        if (!PassiveProcessCheck())
//            return false;

//        return true;
//    }

//    void GSkillSpellLogic_Trigger::SetTriggerCD()
//    {
//        if (!m_pOwner || !m_pSkillData)
//            return;

//        int32 nCDTime = m_pSkillData->GetIntValue(MSV_CDTime);
//        int32 nCDGroup = m_pSkillData->GetIntValue(MSV_CDGroup);
//        if (m_pSkillData->IsTriggerCommonCD())
//        {
//            GCDComponent* pCDComponent = m_pOwner->GetCDComponent();
//            if (pCDComponent)
//            {
//                pCDComponent->StartCD(nCDGroup, nCDTime, true);
//            }
//        }
//        else if (nCDTime > 0)
//        {
//            m_nTriggerCD = nCDTime;
//        }
//        else
//        {
//            CDTime* pCDTime = GCDManager::Instance().GetCDTime(nCDGroup);
//            if (pCDTime)
//            {
//                m_nTriggerCD = pCDTime->m_nCDTime;
//            }
//        }
//    }

//    void GSkillSpellLogic_Trigger::SetTriggerCost()
//    {
//        if (!m_pOwner || !m_pSkillData)
//            return;

//        if (m_pBuff && m_pOwner->GetBuffComponent())
//        {
//            if (m_pSkillData->IsTriggerRemoveBuff())
//            {
//                m_pBuff->SetInvalid();
//            }
//            else if (m_pSkillData->IsTriggerRemoveLayer())
//            {
//                m_pOwner->GetBuffComponent()->RemoveBuffByLayerCnt(m_pBuff, 1);
//            }
//        }

//        GNodeAvatar* pCaster = GetCaster();
//        if (pCaster && pCaster->GetSkillComponent())
//        {
//            pCaster->GetSkillComponent()->DoCost(m_pSkillData);
//        }
//    }

//    void GSkillSpellLogic_Trigger::TransfromEffectNotify(GTriggerNotify* pNotify)
//    {
//        if (!pNotify || !m_pTransform)
//            return;

//        if (m_pTransform->m_nTransformType == SkillEffectTransform_Trigger_Value)
//        {
//            m_AValue.Values[AValue::ad_skill_p] += pNotify->m_nValue * m_pTransform->m_fTransformPrecent;
//        }
//    }

//    void GSkillSpellLogic_Trigger::OnTrigger(GTriggerNotify* pNotify)
//    {
//        m_bActive = true;
//        m_nCurTickTime = 0;
//        m_nCurEffectCount = 1;
//        if (m_nEffectTime <= 0)
//        {
//            for (int32 i = 0; i < m_nEffectCount; ++i)
//            {
//                ProcessEffect();
//            }
//            m_bActive = false;
//        }
//        else
//        {
//            ProcessEffect();
//        }
//    }

//    //////////////////////////////////////////////////////////////////////////
//    //护盾技能
//    FINISH_FACTORY_Arg0(GSkillSpellLogic_Shield);
//    GSkillSpellLogic_Shield::GSkillSpellLogic_Shield()
//	{

//	}

//GSkillSpellLogic_Shield::~GSkillSpellLogic_Shield()
//{

//}

//    //////////////////////////////////////////////////////////////////////////
//    //效果加成技能
//    FINISH_FACTORY_Arg0(GSkillSpellLogic_Effect);
//GSkillSpellLogic_Effect::GSkillSpellLogic_Effect()
//	{

//	}
//	//产生触发效果
//	void GSkillSpellLogic_Effect::OnTrigger(GTriggerNotify* pNotify)
//{
//    if (!m_pOwner || !m_pSkillData)
//        return;

//    if (!pNotify || pNotify->GetType() != NotifyObject_Effect)
//        return;

//    GTriggerNotifyEffect* pNotifyEffect = (GTriggerNotifyEffect*)pNotify;
//    if (!pNotifyEffect->m_pValue)
//        return;

//    int32 nCorrectType = m_pSkillData->GetIntValue(MSV_EffectParam1);
//    switch (nCorrectType)
//    {
//        case GSkillSpellLogic_Effect::Normal:
//            CorrectNormal(pNotifyEffect);
//            break;
//        case GSkillSpellLogic_Effect::DistanceNear:
//            CorrectDistanceNear(pNotifyEffect);
//            break;
//        case GSkillSpellLogic_Effect::DistanceFar:
//            CorrectDistanceFar(pNotifyEffect);
//            break;
//    }

//}

//void GSkillSpellLogic_Effect::CorrectNormal(GTriggerNotifyEffect* pNotifyEffect)
//{
//    int32 nCorrect = m_pSkillData->GetIntValue(MSV_EffectParam2);
//    f32 fPercent = m_pSkillData->GetIntValue(MSV_EffectParam3) / f32(100);
//    f32 & fValue = *pNotifyEffect->m_pValue;
//    fValue = nCorrect + fValue * (1 + fPercent);
//}

//void GSkillSpellLogic_Effect::CorrectDistanceNear(GTriggerNotifyEffect* pNotifyEffect)
//{
//    GNodeAvatar* pTarget = m_pOwner->GetSceneAvatar(pNotifyEffect->m_nTargetID);
//    if (pTarget)
//    {
//        f32 fDistance = m_pOwner->GetPos().GetDistance(pTarget->GetPos());
//        f32 fMaxRange = MIN(1.0, m_pSkillData->GetFloatValue(MSV_Range));
//        f32 fMinRange = (f32)m_pSkillData->GetIntValue(MSV_EffectParam2);

//        fDistance = MAX(0, fDistance - fMinRange);
//        fMaxRange = MIN(1.0, fMaxRange - fMinRange);

//        f32 fPercent = m_pSkillData->GetIntValue(MSV_EffectParam3) / f32(100);
//        f32 & fValue = *pNotifyEffect->m_pValue;
//        fValue *= (1 + fPercent * MAX(0, 1 - MIN(fDistance / fMaxRange, 1.0)));
//    }
//}

//void GSkillSpellLogic_Effect::CorrectDistanceFar(GTriggerNotifyEffect* pNotifyEffect)
//{
//    GNodeAvatar* pTarget = m_pOwner->GetSceneAvatar(pNotifyEffect->m_nTargetID);
//    if (pTarget)
//    {
//        f32 fDistance = m_pOwner->GetPos().GetDistance(pTarget->GetPos());
//        f32 fMaxRange = MIN(1.0, m_pSkillData->GetFloatValue(MSV_Range));
//        f32 fMinRange = (f32)m_pSkillData->GetIntValue(MSV_EffectParam2);

//        fDistance = MAX(0, fDistance - fMinRange);
//        fMaxRange = MIN(1.0, fMaxRange - fMinRange);

//        f32 fPercent = m_pSkillData->GetIntValue(MSV_EffectParam3) / f32(100);
//        f32 & fValue = *pNotifyEffect->m_pValue;
//        fValue *= 1 + fPercent * MIN(fDistance / fMaxRange, 1.0);
//    }
//}

//    //////////////////////////////////////////////////////////////////////////
//    //结算技能
//    FINISH_FACTORY_Arg0(GSkillSpellLogic_Combine);
//GSkillSpellLogic_Combine::GSkillSpellLogic_Combine()
//	{

//	}

//	void GSkillSpellLogic_Combine::OnTrigger(GTriggerNotify* pNotify)
//{
//    if (!m_pSkillData)
//        return;

//    if (!pNotify)
//        return;

//    if (pNotify->GetType() != NotifyObject_Combine)
//        return;

//    GTriggerNotifyCombine* pNotifyCalculation = (GTriggerNotifyCombine*)pNotify;
//    if (!pNotifyCalculation->m_pRoleAValue)
//        return;

//    pNotifyCalculation->m_pRoleAValue->Combine(m_AValue);
//    pNotifyCalculation->m_pRoleAValue->Combine(m_pSkillData->m_RoleValue);
//}

//    //////////////////////////////////////////////////////////////////////////
//    //触发脚本技能
//    FINISH_FACTORY_Arg0(GSkillSpellLogic_TriggerScript);
//GSkillSpellLogic_TriggerScript::GSkillSpellLogic_TriggerScript()
//	{

//	}

//	void GSkillSpellLogic_TriggerScript::OnTrigger(GTriggerNotify* pNotify)
//{
//    if (!pNotify)
//        return;

//    GNodeAvatar* pCaster = GetCaster();
//    if (!pCaster)
//        return;

//    if (!m_pSkillData)
//        return;

//    char buff[64] = { 0 };
//    sprintf(buff, "OnSkillTrigger_%d", m_pSkillData->GetSkillID());
//    luabridge::LuaRef ref = GLuaVM::GetMainLuaVM().getGlobal(buff);
//    if (!ref.isNil())
//		{
//        try
//        {
//				ref(m_pSkillData, pCaster, pNotify->m_nDataID, pNotify->m_nTargetID, pNotify->m_nValue, pNotify->m_vSrcPos, pNotify->m_vTarPos, pNotify->m_vDir);
//        }
//        catch (luabridge::LuaException const&e)
//			{
//            GalaxyLog::error("call GalaxyLuaOnStart :%s", e.what());
//        }
//        }

//    }

//    ///////////////////////////////////////////////////////////////////////
//    //触发服务器技能
//    FINISH_FACTORY_Arg0(GSkillSpellLogic_TriggerSkill);
//    GSkillSpellLogic_TriggerSkill::GSkillSpellLogic_TriggerSkill()
//		: m_nSkillID(0)

//    {

//    }

//    bool GSkillSpellLogic_TriggerSkill::Init(GSkillInitParam & param)

//    {
//        if (!GSkillSpellLogic_Trigger::Init(param))
//            return false;

//        m_nSkillID = m_pSkillData->GetIntValue(MSV_SpellParam1);
//        return true;
//    }

//    void GSkillSpellLogic_TriggerSkill::OnTrigger(GTriggerNotify * pNotify)

//    {
//        if (!pNotify)
//            return;

//        if (!m_pSkillData)
//            return;

//        GNodeAvatar* pCaster = GetCaster();
//        if (!pCaster || !pCaster->GetSkillComponent())
//            return;

//        pCaster->GetSkillComponent()->FinishSkill();

//        //GSpellParam spellParam;
//        //pCaster->GetSkillComponent()->SpellSkill(m_nSkillID, m_TargetInfo);

//        GPacketServerSkillSpell pkt;
//        pkt.SkillID = m_nSkillID;
//        pkt.SkillTarget = m_TargetInfo.m_nTargetID;
//        pkt.SetPos(pNotify->m_vSrcPos.x, pNotify->m_vSrcPos.y, pNotify->m_vSrcPos.z);
//        pkt.SetAimDir(pNotify->m_vDir.x, pNotify->m_vDir.y, pNotify->m_vDir.z);
//        pkt.SetTarPos(pNotify->m_vTarPos.x, pNotify->m_vTarPos.y, pNotify->m_vTarPos.z);

//        pCaster->SendPacket(&pkt);
//    }
//}