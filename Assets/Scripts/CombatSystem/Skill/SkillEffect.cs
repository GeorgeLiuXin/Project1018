using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    /// <summary>
    /// 技能效果
    /// </summary>
    public class SkillEffect : MonoBehaviour
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
//#include "GNodeSkillEffect.h"
//#include "GSkillData.h"
//#include "GNodeTriggerNotify.h"
//#include "GNodeSkillCalculation.h"
//#include "GNodeSkillComponent.h"
//#include "NodeAvatar/NodeAvatar.h"
//#include "NodeAvatar/NodePlayer.h"
//#include "NodeBuff/GBuffComponent.h"
//#include "NodeThreat/GNodeThreatComponent.h"
//#include "GalaxyRandom.h"
//#include "FSMStateInstance.h"
//#include "GSkillDisplacementLogic.h"
//#include "GSkillDisDefine.h"
//#include "GCDComponent.h"
//#include "NodeAI/GAIComponent.h"
//#include "NodeCard/GNodeSkillExComponent.h"
//#include "GNodePlayerControlProtect.h"

//namespace Galaxy
//{
//	GSkillEffectLogic::GSkillEffectLogic()
//	{
//		///////////////////////////////////////////////////////
//		//技能击退逻辑
//		m_vDisplacementLogic[DisType_Sphere] = new GSkillDisplacementLogic_Sphere();
//    m_vDisplacementLogic[DisType_Self] = new GSkillDisplacementLogic_Self();
//    m_vDisplacementLogic[DisType_Caster] = new GSkillDisplacementLogic_Caster();
//    //技能击退逻辑
//    ///////////////////////////////////////////////////////
//}

//int32 GSkillEffectLogic::GetThreatValue(RoleAValue& sCasterAValue, bool bHit)
//{
//    int32 nThreat = sCasterAValue.Values[AValue::threat] * (1 + sCasterAValue.Values[AValue::threat_r]);
//    nThreat = MAX(nThreat * ((bHit) ? 1 : 0.1), 1);
//    return nThreat;
//}

//void GSkillEffectLogic::ProcessHurtThreat(RoleAValue& sCasterAValue, GNodeAvatar* pCaster, GNodeAvatar* pTarget, bool bHit)
//{
//    if (!pCaster || !pTarget)
//        return;

//    int32 nThreat = GetThreatValue(sCasterAValue, bHit);
//    if (pTarget->GetThreatComponent())
//    {
//        pTarget->GetThreatComponent()->OnHurt(pCaster, nThreat);
//    }
//}

//void GSkillEffectLogic::ProcessHealThreat(RoleAValue& sCasterAValue, GNodeAvatar* pCaster, GNodeAvatar* pTarget)
//{
//    if (!pCaster || !pTarget)
//        return;

//    int32 nThreat = GetThreatValue(sCasterAValue, true);
//    if (pTarget->GetThreatComponent())
//    {
//        pTarget->GetThreatComponent()->OnHeal(pCaster, nThreat);
//    }
//}

//void GSkillEffectLogic::SetStrongControlled(GSkillData* pSkillData, GSkillTargetInfo& sTarInfo, GNodeAvatar* pCaster, GNodeAvatar* pTarget)
//{
//    if (!pSkillData || !pTarget || !pTarget->GetFSM())
//        return;

//    //霸体状态免疫击退
//    if (pTarget->CheckState(GAS_SuperArmor))
//        return;

//    //硬直等级免疫击退
//    if (!pTarget->GetSkillComponent())
//        return;

//    int32 nCasterEndure = pSkillData->GetIntValue(MSV_EndureLevel);
//    int32 nTargetEndure = pTarget->GetSkillComponent()->GetEndureLevel();
//    if (nCasterEndure < nTargetEndure)
//        return;

//    int32 nLogicType = pSkillData->GetIntValue(MSV_EffectBeatBack);
//    GSkillDisplacementLogic* pLogic = GetDisplacementLogic(nLogicType);
//    if (pLogic == NULL)
//        return;

//    if (pTarget->GetAIComponent())
//    {
//        pTarget->GetAIComponent()->OnStrongControl();
//    }

//    if (pTarget->IsPlayer())
//    {
//        GNodePlayer_Component* pPlayer = static_cast<GNodePlayer_Component*>(pTarget);
//        GNodePlayerControlProtect* pProtect = pPlayer->GetControlProtectComponent();
//        if (pProtect)
//        {
//            pProtect->PushControlData(pSkillData);
//        }
//    }

//    FSMParam_StrongControlled param;
//    if (pSkillData->GetIntValue(MSV_BeatBackMoveTime) == 0 && pSkillData->GetIntValue(MSV_BeatBackLifeTime) == 0)
//    {
//        //暂时 在之后添加了系统配置表后修改为开放配置的击退时间参数
//        param.nControlledType = BREAK_EFFECT_BEATBACKTYPE;
//        param.fMoveTime = BREAK_EFFECT_MOVETIME;
//        param.fLifeTime = BREAK_EFFECT_LIFETIME;
//        param.nBeatDir = BREAK_EFFECT_BEATBACKDIR;
//        param.vStartPos = pTarget->GetPos();
//        //2018-09-30 jasondong 伤害源坐标若在墙中会把人打上天，注掉
//        //param.vStartPos.z = pTarget->GetSceneHeight(pTarget->GetPos());
//        param.vEndPos = param.vStartPos;
//        pTarget->SetTargetState(&param, SPRI_Equal);
//    }
//    else
//    {
//        param.nControlledType = pSkillData->GetIntValue(MSV_BeatBackType);
//        param.fMoveTime = pSkillData->GetIntValue(MSV_BeatBackMoveTime) / 1000.0;
//        param.fLifeTime = pSkillData->GetIntValue(MSV_BeatBackLifeTime) / 1000.0;
//        param.nBeatDir = pSkillData->GetIntValue(MSV_BeatBackDir);
//        param.vStartPos = pTarget->GetPos();
//        //2018-09-30 jasondong 伤害源坐标若在墙中会把人打上天，注掉
//        //param.vStartPos.z = pTarget->GetSceneHeight(pTarget->GetPos());
//        param.vEndPos = pLogic->Process(sTarInfo, pSkillData, pSkillData->IsAreaUseTarPos(), pCaster, pTarget);
//        pTarget->SetTargetState(&param, SPRI_Equal);
//    }

//}

//GSkillDisplacementLogic* GSkillEffectLogic::GetDisplacementLogic(int32 nLogicType)
//{
//    return (m_vDisplacementLogic.Find(nLogicType)) ? m_vDisplacementLogic.Get() : NULL;
//}
//uint8 GSkillEffectLogic::GetDamageReduction(GNodeAvatar* pCast, Vector3 srcPos, Vector3 tarPos, GSkillData* pSkillData)
//{
//    if (!pCast || !pSkillData)
//        return 100;
//    if (srcPos.GetDistance(tarPos) <= 0.3f)
//    {
//        return 100;
//    }
//    if (pSkillData->IsCaclObstacle())
//    {
//        const NavNode* pNode = NULL;
//        NavRayCastInfo info;
//        if (pCast->RayCast2D(srcPos, tarPos, 0.5, &info)) //需要一个接口，接口功能：计算startpos到endpos中所有障碍物的总减伤系数。
//        {
//            if (60102 == pSkillData->GetSkillID())
//            {
//                if (!pCast->RayCast(srcPos, tarPos, &info))
//                {
//                    //演示版本, 临时处理 2019-01-18
//                    //菊花怪导弹穿护盾bug,后续需要处理子弹和伤害的RayCast接口 
//                    info.damageInfo = 100.f;
//                }
//            }
//            return info.damageInfo;
//        }
//    }
//    return 100;
//}
////////////////////////////////////////////////////////////////////////////
//bool GSkillEffect_Damage::Process(GSkillEffectCalculation& sCalculation, GSkillTargetInfo& sTarInfo, RoleAValue& sSkillAValue)
//{
//    GNodeAvatar* pCaster = sCalculation.m_pCaster;
//    GNodeAvatar* pTarget = sCalculation.m_pTarget;
//    GSkillData* pSkillData = sCalculation.m_pSkillData;
//    if (!pSkillData || !pCaster || !pTarget)
//        return false;

//    if (pTarget->IsDead())
//        return false;
//    uint8 damageReduction = GetDamageReduction(pCaster, sTarInfo.m_vSrcPos, sTarInfo.m_vTarPos, pSkillData);
//    GPacketEffect pkt;
//    pkt.skillID = pSkillData->m_nDataID;
//    pkt.casterID = pCaster->GetAvatarID();
//    pkt.targetID = pTarget->GetAvatarID();
//    pkt.notifyType = NotifyType_Damage;
//    pkt.effectType = pSkillData->GetIntValue(MSV_EffectType);
//    pkt.effectValue = 0;

//    sCalculation.CalculationDamage(pkt.effectType, pkt.effectValue, damageReduction);
//    pCaster->BroadcastPacket(&pkt);

//    //////////////////////////////////////////////////////////////////////////
//    //产生效果事件
//    if (pCaster->GetSkillComponent())
//    {
//        pCaster->GetSkillComponent()->PushTriggerNotify(pSkillData->m_nDataID, pTarget->GetAvatarID(), NotifyType_Damage, pkt.effectType, pkt.effectValue, &sTarInfo.m_vSrcPos, &sTarInfo.m_vTarPos, &sTarInfo.m_vAimDir);
//    }

//    if (pTarget->GetSkillComponent())
//    {
//        pTarget->GetSkillComponent()->PushTriggerNotify(pSkillData->m_nDataID, pCaster->GetAvatarID(), NotifyType_OnDamage, pkt.effectType, pkt.effectValue, &sTarInfo.m_vSrcPos, &sTarInfo.m_vTarPos, &sTarInfo.m_vAimDir);
//    }
//    //产生效果事件
//    //////////////////////////////////////////////////////////////////////////

//    bool bHit = ((pkt.effectType & TriggerNotify_Hit) > 0);

//    // JTODO: 扩展配置
//    if (bHit && 100 == damageReduction)
//    {
//        SetStrongControlled(pSkillData, sTarInfo, pCaster, pTarget);
//    }

//    //产生仇恨
//    ProcessHurtThreat(sCalculation.m_CasterAValue, pCaster, pTarget, bHit);
//    return bHit;
//}

//bool GSkillEffect_Heal::Process(GSkillEffectCalculation& sCalculation, GSkillTargetInfo& sTarInfo, RoleAValue& sSkillAValue)
//{
//    GNodeAvatar* pCaster = sCalculation.m_pCaster;
//    GNodeAvatar* pTarget = sCalculation.m_pTarget;
//    GSkillData* pSkillData = sCalculation.m_pSkillData;
//    if (!pSkillData || !pCaster || !pTarget)
//        return false;

//    if (pTarget->IsDead())
//        return false;

//    GPacketEffect pkt;
//    pkt.skillID = pSkillData->m_nDataID;
//    pkt.casterID = pCaster->GetAvatarID();
//    pkt.targetID = pTarget->GetAvatarID();
//    pkt.notifyType = NotifyType_Heal;
//    pkt.effectType = pSkillData->GetIntValue(MSV_EffectType);
//    pkt.effectValue = 0;

//    sCalculation.CalculationHeal(pkt.effectType, pkt.effectValue);
//    pCaster->BroadcastPacket(&pkt);

//    //////////////////////////////////////////////////////////////////////////
//    //产生效果事件
//    if (pCaster->GetSkillComponent())
//    {
//        pCaster->GetSkillComponent()->PushTriggerNotify(pSkillData->m_nDataID, pTarget->GetAvatarID(), NotifyType_Heal, pkt.effectType, pkt.effectValue, &sTarInfo.m_vSrcPos, &sTarInfo.m_vTarPos, &sTarInfo.m_vAimDir);
//    }
//    if (pTarget->GetSkillComponent())
//    {
//        pTarget->GetSkillComponent()->PushTriggerNotify(pSkillData->m_nDataID, pCaster->GetAvatarID(), NotifyType_OnHeal, pkt.effectType, pkt.effectValue, &sTarInfo.m_vSrcPos, &sTarInfo.m_vTarPos, &sTarInfo.m_vAimDir);
//    }
//    //产生效果事件
//    //////////////////////////////////////////////////////////////////////////

//    //产生仇恨
//    ProcessHealThreat(sCalculation.m_CasterAValue, pCaster, pTarget);

//    return true;
//}

//bool GSkillEffect_Lua::Process(GSkillEffectCalculation& sCalculation, GSkillTargetInfo& sTarInfo, RoleAValue& sSkillAValue)
//{
//    GNodeAvatar* pCaster = sCalculation.m_pCaster;
//    GNodeAvatar* pTarget = sCalculation.m_pTarget;
//    GSkillData* pSkillData = sCalculation.m_pSkillData;
//    if (!pSkillData || !pCaster || !pTarget)
//        return false;

//    char buff[64] = { 0 };
//    sprintf(buff, "OnSkillEffect_%d", pSkillData->GetSkillID());
//    luabridge::LuaRef ref = GLuaVM::GetMainLuaVM().getGlobal(buff);
//    if (!ref.isNil())
//		{
//        try
//        {
//				ref(pSkillData, pCaster, pTarget, sTarInfo.m_nUserData);
//        }
//        catch (luabridge::LuaException const&e)
//			{
//            GalaxyLog::error("call GalaxyLuaOnStart :%s", e.what());
//        }
//        }
//        return true;
//    }

//    bool GSkillEffect_AddBuff::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)

//    {
//        GNodeAvatar* pCaster = sCalculation.m_pCaster;
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        GPacketEffect pkt;
//        pkt.skillID = pSkillData->m_nDataID;
//        pkt.casterID = pCaster->GetAvatarID();
//        pkt.targetID = pTarget->GetAvatarID();
//        pkt.notifyType = NotifyType_Buff;
//        pkt.effectType = pSkillData->GetIntValue(MSV_EffectType);
//        pkt.effectValue = 0;

//        sCalculation.DecisionOnHit(pkt.effectType);
//        bool bHit = ((pkt.effectType & TriggerNotify_Hit) > 0);
//        if (bHit)
//        {
//            GBuffCreateArg arg;
//            arg.m_nBuffID = pSkillData->GetIntValue(MSV_EffectParam1);
//            arg.m_nBuffLevel = pSkillData->GetIntValue(MSV_EffectParam2);
//            arg.m_nBuffLayer = pSkillData->GetIntValue(MSV_EffectParam3);
//            arg.m_fUserData = sCalculation.GetPower();
//            pTarget->AddBuff(pCaster, &arg);
//        }
//        pCaster->BroadcastPacket(&pkt);

//        int32 nEffectType = pSkillData->GetIntValue(MSV_EffectType);
//        if ((nEffectType & TriggerNotify_Buff) > 0)
//        {
//            //产生仇恨
//            ProcessHealThreat(sCalculation.m_CasterAValue, pCaster, pTarget);
//        }
//        else if ((nEffectType & TriggerNotify_Debuff) > 0)
//        {
//            //产生仇恨
//            ProcessHurtThreat(sCalculation.m_CasterAValue, pCaster, pTarget, bHit);
//        }

//        return bHit;
//    }

//    bool GSkillEffect_RemoveBuff::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)

//    {
//        GNodeAvatar* pCaster = sCalculation.m_pCaster;
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        if (pTarget->GetBuffComponent())
//        {
//            int32 nBuffID = pSkillData->GetIntValue(MSV_EffectParam1);
//            int32 nBuffLayer = pSkillData->GetIntValue(MSV_EffectParam2);
//            int32 nOwnerType = pSkillData->GetIntValue(MSV_EffectParam3);

//            int64 nCasterDID = -1;
//            if (nOwnerType == 1)
//            {
//                nCasterDID = pCaster->GetAvatarDID();
//            }
//            else if (nOwnerType == 2)
//            {
//                nCasterDID = pTarget->GetAvatarDID();
//            }

//            if (nBuffLayer > 0)
//            {
//                pTarget->GetBuffComponent()->RemoveBuffByLayerCnt(nBuffID, nCasterDID, nBuffLayer);
//            }
//            else
//            {
//                pTarget->GetBuffComponent()->RemoveBuff(nBuffID, nCasterDID);
//            }
//        }

//        return true;
//    }

//    bool GSkillEffect_Dispel::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)

//    {
//        GNodeAvatar* pCaster = sCalculation.m_pCaster;
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        GPacketEffect pkt;
//        pkt.skillID = pSkillData->m_nDataID;
//        pkt.casterID = pCaster->GetAvatarID();
//        pkt.targetID = pTarget->GetAvatarID();
//        pkt.notifyType = NotifyType_Dispel;
//        pkt.effectType = pSkillData->GetIntValue(MSV_EffectType);
//        pkt.effectValue = 0;

//        sCalculation.DecisionOnHit(pkt.effectType);
//        bool bHit = ((pkt.effectType & TriggerNotify_Hit) > 0);
//        int32 nDispelCount = 0;
//        if (bHit)
//        {
//            int32 nBuffCount = pSkillData->GetIntValue(MSV_EffectParam1);
//            if ((pkt.effectType & TriggerNotify_DispelBuff) > 0)
//            {
//                nDispelCount = pTarget->DispelBuff(pCaster, BTF_PlusBuff, nBuffCount);
//            }
//            else if ((pkt.effectType & TriggerNotify_DispelDebuff) > 0)
//            {
//                nDispelCount = pTarget->DispelBuff(pCaster, BTF_DeBuff, nBuffCount);
//            }
//        }
//        pCaster->BroadcastPacket(&pkt);

//        if (pCaster->GetSkillComponent())
//        {
//            //Buff添加事件
//            if (pCaster && pCaster->GetSkillComponent())
//            {
//                pCaster->GetSkillComponent()->PushTriggerNotify(pSkillData->m_nDataID, pCaster->GetAvatarID(), NotifyType_Dispel, pkt.effectType, nDispelCount, &sTarInfo.m_vSrcPos, &sTarInfo.m_vTarPos, &sTarInfo.m_vAimDir);
//            }
//        }

//        int32 nEffectType = pSkillData->GetIntValue(MSV_EffectType);
//        if ((nEffectType & TriggerNotify_DispelBuff) > 0)
//        {
//            //产生仇恨
//            ProcessHurtThreat(sCalculation.m_CasterAValue, pCaster, pTarget, bHit);
//        }
//        else if ((nEffectType & TriggerNotify_DispelDebuff) > 0)
//        {
//            //产生仇恨
//            ProcessHealThreat(sCalculation.m_CasterAValue, pCaster, pTarget);
//        }

//        return bHit;
//    }

//    bool GSkillEffect_Taunt::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)

//    {
//        GNodeAvatar* pCaster = sCalculation.m_pCaster;
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        if (pTarget->IsDead())
//            return false;

//        if (pTarget->IsPlayer())
//            return false;

//        GPacketEffect pkt;
//        pkt.skillID = pSkillData->m_nDataID;
//        pkt.casterID = pCaster->GetAvatarID();
//        pkt.targetID = pTarget->GetAvatarID();
//        pkt.notifyType = NotifyType_Damage;
//        pkt.effectType = pSkillData->GetIntValue(MSV_EffectType);
//        pkt.effectValue = 0;

//        sCalculation.DecisionOnHit(pkt.effectType);
//        bool bHit = ((pkt.effectType & TriggerNotify_Hit) > 0);
//        {
//            if (pTarget->GetThreatComponent())
//            {
//                pTarget->GetThreatComponent()->OnTaunt(pCaster);
//            }
//        }
//        pCaster->BroadcastPacket(&pkt);

//        //产生仇恨
//        ProcessHurtThreat(sCalculation.m_CasterAValue, pCaster, pTarget, bHit);
//        return true;
//    }

//    bool GSkillEffect_Repel::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)
//    {
//        GNodeAvatar* pCaster = sCalculation.m_pCaster;
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        if (pTarget->IsDead())
//            return false;

//        //产生效果事件
//        int32 mEffectType = pSkillData->GetIntValue(MSV_EffectType);
//        sCalculation.DecisionOnHit(mEffectType);

//        bool bHit = ((mEffectType & TriggerNotify_Hit) > 0);
//        SetStrongControlled(pSkillData, sTarInfo, pCaster, pTarget);
//        return bHit;
//    }

//    bool GSkillEffect_Relive::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)

//    {
//        GNodeAvatar* pCaster = sCalculation.m_pCaster;
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        if (!pTarget->IsDead())
//            return false;

//        int32 nReliveID = pSkillData->GetIntValue(MSV_EffectParam1);
//        GSkillReliveData* pReliveData = GSkillDataManager::Instance().GetSkillReliveData(nReliveID);
//        if (!pReliveData)
//            return false;

//        //目标是玩家，需要进行复活确认
//        if (pTarget->IsPlayer() && pCaster->GetAvatarID() != pTarget->GetAvatarID())
//        {
//            GNodePlayer* pPlayer = (GNodePlayer*)pTarget;
//            if (pPlayer->CheckReliveCD())
//            {
//                return false;
//            }

//            pPlayer->ReliveAsk(pCaster, nReliveID);
//        }
//        else
//        {
//            pTarget->Relive();
//        }

//        return true;
//    }

//    bool GSkillEffect_Recover::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)

//    {
//        GNodeAvatar* pCaster = sCalculation.m_pCaster;
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        if (pTarget->IsDead())
//            return false;

//        int32 nRecoverID = pSkillData->GetIntValue(MSV_EffectParam1);
//        GSkillRecoverData* pRecoverData = GSkillDataManager::Instance().GetSkillRecoverData(nRecoverID);
//        if (!pRecoverData)
//            return false;

//        int32 nHp = pRecoverData->m_nHp + pTarget->GetHpMax() * pRecoverData->m_fHp_P;

//        if (nHp > 0)
//        {
//            GPacketEffect pkt;
//            pkt.skillID = pSkillData->m_nDataID;
//            pkt.casterID = pCaster->GetAvatarID();
//            pkt.targetID = pTarget->GetAvatarID();
//            pkt.notifyType = NotifyType_OnHeal;
//            pkt.effectType = TriggerNotify_RecoverHp;
//            pkt.effectValue = nHp;
//            pCaster->BroadcastPacket(&pkt);
//        }
//        return true;
//    }

//    bool GSkillEffect_CDReduce::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)

//    {
//        GNodeAvatar* pCaster = sCalculation.m_pCaster;
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        if (pTarget->IsDead())
//            return false;

//        int32 nCDGroup = pSkillData->GetIntValue(MSV_EffectParam1);
//        int32 nCDTime = pSkillData->GetIntValue(MSV_EffectParam2);
//        int32 nCDType = pSkillData->GetIntValue(MSV_EffectParam3);

//        //产生效果事件
//        if (pCaster->GetCDComponent())
//        {
//            if (nCDType > 0)
//                pCaster->GetCDComponent()->AddCD(nCDGroup, nCDTime);
//            else
//                pCaster->GetCDComponent()->ReduceCD(nCDGroup, nCDTime);
//        }
//        return true;
//    }

//    bool GSkillEffect_Loot::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)

//    {
//        GNodeAvatar* pCaster = sCalculation.m_pCaster;
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        GNodeScene* pScene = (GNodeScene*)pCaster->GetScene();
//        if (!IsValidMemory(pScene)) return false;

//        GLootManager* pLootManager = pScene->GetLootManager();
//        if (nullptr == pLootManager) return false;

//        int32 nLootID = sTarInfo.m_nUserData;
//        GLoot* pLoot = pLootManager->GetLoot(pCaster->GetAvatarDID(), nLootID);
//        if (nullptr == pLoot || pLoot->IsDestroy()) return false;

//        Vector3 pos = pCaster->GetPos();
//        if (sqrt(pos.GetSquaredDistance2D(pLoot->GetPos())) > pLoot->GetPickDistance() + Loot_Distance_Offset) return false;

//        pLoot->DoPick((GNodePlayer*)pCaster);

//        return true;
//    }

//    bool GSkillEffect_RecoverEp::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)

//    {
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pTarget)
//            return false;
//        int32 nEnergyId = pSkillData->GetIntValue(MSV_EffectParam1);
//        int32 nEpModify = pSkillData->GetIntValue(MSV_EffectParam2); // 加减值
//        int32 nEpValue = pSkillData->GetIntValue(MSV_EffectParam3);

//        GNodeSkillExComponent* pCom = pTarget->GetSkillExComponent();
//        if (pCom)
//        {
//            pCom->SetEpRecover(nEnergyId, nEpModify, nEpValue);
//        }
//        return true;
//    }

//    bool GSkillEffect_DispelByGroup::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)

//    {
//        GNodeAvatar* pCaster = sCalculation.m_pCaster;
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        GPacketEffect pkt;
//        pkt.skillID = pSkillData->m_nDataID;
//        pkt.casterID = pCaster->GetAvatarID();
//        pkt.targetID = pTarget->GetAvatarID();
//        pkt.notifyType = NotifyType_Dispel;
//        pkt.effectType = pSkillData->GetIntValue(MSV_EffectType);
//        pkt.effectValue = 0;

//        sCalculation.DecisionOnHit(pkt.effectType);
//        bool bHit = ((pkt.effectType & TriggerNotify_Hit) > 0);
//        int32 nDispelCount = 0;
//        if (bHit)
//        {
//            int32 nBuffGroup = pSkillData->GetIntValue(MSV_EffectParam1);
//            int32 nBuffCount = pSkillData->GetIntValue(MSV_EffectParam2);
//            int32 nLayoutCount = pSkillData->GetIntValue(MSV_EffectParam3);
//            if ((pkt.effectType & TriggerNotify_DispelBuffGroup) > 0)
//            {
//                nDispelCount = pTarget->DispelBuff(pCaster, nBuffGroup, nBuffCount, nLayoutCount);
//            }
//        }
//        pCaster->BroadcastPacket(&pkt);

//        if (pCaster->GetSkillComponent())
//        {
//            //buff驱散事件
//            if (pCaster && pCaster->GetSkillComponent())
//            {
//                pCaster->GetSkillComponent()->PushTriggerNotify(pSkillData->m_nDataID, pCaster->GetAvatarID(), NotifyType_Dispel, pkt.effectType, nDispelCount, &sTarInfo.m_vSrcPos, &sTarInfo.m_vTarPos, &sTarInfo.m_vAimDir);
//            }
//        }

//        int32 nEffectType = pSkillData->GetIntValue(MSV_EffectType);
//        if ((nEffectType & TriggerNotify_DispelBuff) > 0)
//        {
//            //产生仇恨
//            ProcessHurtThreat(sCalculation.m_CasterAValue, pCaster, pTarget, bHit);
//        }
//        else if ((nEffectType & TriggerNotify_DispelDebuff) > 0)
//        {
//            //产生仇恨
//            ProcessHealThreat(sCalculation.m_CasterAValue, pCaster, pTarget);
//        }

//        return bHit;
//    }
//    bool GSkillEffect_Shield::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)

//    {
//        GNodeAvatar* pCaster = sCalculation.m_pCaster;
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        if (pTarget->IsDead())
//            return false;

//        GPacketEffect pkt;
//        pkt.skillID = pSkillData->m_nDataID;
//        pkt.casterID = pCaster->GetAvatarID();
//        pkt.targetID = pTarget->GetAvatarID();
//        pkt.effectType = pSkillData->GetIntValue(MSV_EffectType);
//        pkt.effectValue = 0;

//        f32 fShieldValue = 0;
//        int32 nShieldAddType = pSkillData->GetIntValue(MSV_EffectParam1);
//        int32 nShieldModify = pSkillData->GetIntValue(MSV_EffectParam2);
//        int32 nShieldValue = pSkillData->GetIntValue(MSV_EffectParam3);

//        sCalculation.CalculationShield(pkt.effectType, pkt.effectValue);
//        fShieldValue = pkt.effectValue;

//        GNodeSkillExComponent* pCom = pTarget->GetSkillExComponent();
//        if (pCom)
//        {
//            pCom->AddShield(&fShieldValue);
//        }
//        pkt.effectValue = fShieldValue;
//        pCaster->BroadcastPacket(&pkt);

//        //int32 nEffectType = pSkillData->GetIntValue(MSV_EffectType);
//        //ProcessHealThreat(sCalculation.m_CasterAValue, pCaster, pTarget);
//        return true;

//    }

//}