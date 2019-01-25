using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    /// <summary>
    /// 技能逻辑管理器
    /// </summary>
    public class SkillLogicManager : MonoBehaviour
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
//#include "GNodeSkillLogic.h"
//#include "GNodeSkillSpell.h"
//#include "GNodeSkillPassive.h"
//#include "GNodeSkillTrigger.h"
//#include "GNodeSkillTarget.h"
//#include "GNodeSkillLauncher.h"
//#include "GNodeSkillProjectile.h"
//#include "GNodeSkillArea.h"
//#include "GNodeSkillEffect.h"
//#include "GSkillData.h"
//#include "NodeAvatar/NodeAvatar.h"
//#include "NodeLogic/NodeCondition.h"
//#include "NodeThreat/GNodeThreatComponent.h"

//namespace Galaxy
//{
//	GSkillLogicManager::GSkillLogicManager()
//	{
//		///////////////////////////////////////////////////////
//		//技能目标逻辑
//		m_vTargetSelect[TargetSelect_CurTarget]		= new GSkillTargetSelect_Target();
//    m_vTargetSelect[TargetSelect_NearestTarget]	= new GSkillTargetSelect_NearestTarget();
//    //技能目标逻辑
//    ///////////////////////////////////////////////////////

//    ///////////////////////////////////////////////////////
//    //技能发动逻辑
//    m_vLauncherLogic[SkillLauncher_Direct]			= new GSkillLauncher_Direct();
//    m_vLauncherLogic[SkillLauncher_Bullet]			= new GSkillLauncher_Bullet();
//    m_vLauncherLogic[SkillLauncher_Barrage]			= new GSkillLauncher_Barrage();
//    m_vLauncherLogic[SkillLauncher_List]			= new GSKillLaucher_List();
//    //技能发动逻辑
//    ///////////////////////////////////////////////////////

//    ///////////////////////////////////////////////////////
//    //技能范围逻辑
//    m_vAreaLogic[SkillArea_Singleton]	= new GSkillAreaSingelton();
//    m_vAreaLogic[SkillArea_Sector]		= new GSkillAreaSector();
//    m_vAreaLogic[SkillArea_Sphere]		= new GSkillAreaSphere();
//    m_vAreaLogic[SkillArea_Rect]		= new GSkillAreaRect();
//    m_vAreaLogic[SkillArea_Ring]		= new GSkillAreaRing();
//    //技能范围逻辑
//    ///////////////////////////////////////////////////////

//    ///////////////////////////////////////////////////////
//    //技能效果逻辑
//    m_vEffectLogic[SkillEffect_Damage]			= new GSkillEffect_Damage();
//    m_vEffectLogic[SkillEffect_Heal]			= new GSkillEffect_Heal();
//    m_vEffectLogic[SkillEffect_Lua]				= new GSkillEffect_Lua();
//    m_vEffectLogic[SkillEffect_AddBuff]			= new GSkillEffect_AddBuff();
//    m_vEffectLogic[SkillEffect_RemoveBuff]		= new GSkillEffect_RemoveBuff();
//    m_vEffectLogic[SkillEffect_Dispel]			= new GSkillEffect_Dispel();
//    m_vEffectLogic[SkillEffect_Taunt]			= new GSkillEffect_Taunt();
//    m_vEffectLogic[SkillEffect_Repel]			= new GSkillEffect_Repel();
//    m_vEffectLogic[SkillEffect_Relive]			= new GSkillEffect_Relive();
//    m_vEffectLogic[SkillEffect_Recover]			= new GSkillEffect_Recover();
//    m_vEffectLogic[SkillEffect_CDReduce]		= new GSkillEffect_CDReduce();
//    m_vEffectLogic[SkillEffect_Loot]			= new GSkillEffect_Loot();
//    m_vEffectLogic[SkillEffect_RecoverEp]		= new GSkillEffect_RecoverEp();
//    m_vEffectLogic[SkillEffect_DispelByGroup]	= new GSkillEffect_DispelByGroup();
//    m_vEffectLogic[SkillEffect_Shield]			= new GSkillEffect_Shield();
//    //技能效果逻辑
//    ///////////////////////////////////////////////////////
//}

//GSkillLogicManager::~GSkillLogicManager()
//{

//}

//GSkillTargetSelect* GSkillLogicManager::GetTargetSelect(int32 nSelectType)
//{
//    return (m_vTargetSelect.Find(nSelectType)) ? m_vTargetSelect.Get() : NULL;
//}

//GSkillLauncherLogic* GSkillLogicManager::GetLauncherLogic(int32 nLogicType)
//{
//    return (m_vLauncherLogic.Find(nLogicType)) ? m_vLauncherLogic.Get() : NULL;
//}

//GSkillAreaLogic* GSkillLogicManager::GetAreaLogic(int32 nLogicType)
//{
//    return (m_vAreaLogic.Find(nLogicType)) ? m_vAreaLogic.Get() : NULL;
//}

//GSkillEffectLogic* GSkillLogicManager::GetEffectLogic(int32 nLogicType)
//{
//    return (m_vEffectLogic.Find(nLogicType)) ? m_vEffectLogic.Get() : NULL;
//}

//GSkillSpellLogic* GSkillLogicManager::CreateSpellLogic(int32 nLogicID)
//{
//    switch (nLogicID)
//    {
//        case SkillSpell_Branch:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_Branch);
//        case SkillSpell_Sing:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_Sing);
//        case SkillSpell_Channel:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_Channel);
//        case SkillSpell_Dash:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_Dash);
//        case SkillSpell_Charger:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_Charger);
//        case SkillSpell_MultiStep:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_MultiStep);
//        case SkillSpell_Teleportation:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_Teleportation);
//        case SkillSpell_AimAt:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_AimAt);
//        case SkillSpell_AttackQueue:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_AttackQueue);
//        case SkillSpell_Eot:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_Eot);
//        case SkillSpell_Stand:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_Stand);
//        case SkillSpell_AValue:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_AValue);
//        case SkillSpell_Condition:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_Condition);
//        case SkillSpell_Trigger:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_Trigger);
//        case SkillSpell_Shield:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_Shield);
//        case SkillSpell_Effect:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_Effect);
//        case SkillSpell_Combine:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_Combine);
//        case SkillSpell_TriggerScript:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_TriggerScript);
//        case SkillSpell_TriggerSkill:
//            return FACTORY_NEWOBJ(GSkillSpellLogic_TriggerSkill);
//        default:
//            return NULL;
//    }
//}

//GSkillProjectile* GSkillLogicManager::CreateProjectile(int32 nType)
//{
//    switch (nType)
//    {
//        case Projectile_Track:
//            return FACTORY_NEWOBJ(GSkillProjectile_Track);
//        case Projectile_Parabola:
//            return FACTORY_NEWOBJ(GSkillProjectile_Parabola);
//        case Projectile_Trap:
//            return FACTORY_NEWOBJ(GSkillProjectile_Trap);
//        case Projectile_Missile:
//            return FACTORY_NEWOBJ(GSkillProjectile_Missile);
//        case Projectile_Base:
//            return FACTORY_NEWOBJ(GSkillProjectile);
//    }
//    return NULL;
//}

////检查目标
//bool GSkillLogicManager::CheckTarget(GSkillData* pSkillData, GNodeAvatar* pCaster, GSkillTargetInfo& sTarInfo)
//{
//    if (!pSkillData || !pCaster)
//        return false;

//    //检查目标条件
//    if (pSkillData->IsTargetAvatar())
//    {
//        GNodeThreatComponent* pThreatCom = pCaster->GetThreatComponent();
//        if (pThreatCom && pThreatCom->IsTaunt())
//        {
//            // 被嘲讽了
//            if (pThreatCom->GetTargetID() != sTarInfo.m_nTargetID)
//            {
//                return false; // 嘲讽的目标和当前技能释放的目标不一致
//            }
//        }

//        if (!CheckTarget(pSkillData, pCaster, sTarInfo.m_nTargetID))
//            return false;
//    }
//    else if (pSkillData->IsTargetPos())
//    {
//        f32 fRange = pSkillData->GetFloatValue(MSV_Range);
//        if (fRange > 0)
//        {
//            f32 fDistance = pCaster->GetPos().GetDistance(sTarInfo.m_vTarPos);
//            if (fDistance > fRange + OffestRange)
//                return false;
//        }
//    }

//    return true;
//}

//bool GSkillLogicManager::CheckTargetList(GSkillData* pSkillData, GNodeAvatar* pCaster, GSpellTargetList& vTargetList)
//{
//    if (!pSkillData || !pCaster)
//        return false;

//    bool bSuccess = false;
//    int32 nCount = MIN(pSkillData->TargetListSize(), MAX_SPELL_TARGET);

//    GSpellTargetList vSuccessList;
//    GSpellTargetList::iterator iter = vTargetList.begin();
//    GSpellTargetList::iterator iter_end = vTargetList.end();
//    for (; iter != iter_end; ++iter)
//    {
//        if (nCount <= 0)
//            break;

//        if (CheckTarget(pSkillData, pCaster, *iter))
//        {
//            vSuccessList.insert(*iter);
//            bSuccess = true;
//        }

//        --nCount;
//    }
//    vTargetList = vSuccessList;
//    return bSuccess;
//}

//bool GSkillLogicManager::CheckTarget(GSkillData* pSkillData, GNodeAvatar* pCaster, int32 nTargetID)
//{
//    GNodeAvatar* pTarget = pCaster->GetSceneAvatar(nTargetID);
//    if (!pTarget)
//        return false;

//    //当前目标类型隐含的条件检查
//    if (pSkillData->IsTargetOhterFriend())
//    {
//        if (!pCaster->CheckRelation(pTarget, ToFriend))
//            return false;
//    }
//    if (pSkillData->IsTargetOhterEnemy())
//    {
//        if (!pCaster->CheckRelation(pTarget, ToEnemy))
//            return false;
//    }

//    int32 nTarCheck = pSkillData->GetIntValue(MSV_TarCheck);
//    if (nTarCheck > 0)
//    {
//        SDConditionParamAvatar sParam;
//        sParam.ParamAvatar = pCaster;
//        if (!GSKillConditionCheckManager::Instance().Check(nTarCheck, pTarget, &sParam))
//            return false;
//    }

//    f32 fRange = pSkillData->GetFloatValue(MSV_Range);
//    if (fRange > 0)
//    {
//        f32 fDistance = pCaster->GetPos().GetDistance(pTarget->GetPos());
//        if (fDistance > fRange + OffestRange)
//            return false;
//    }

//    return true;
//}
//}