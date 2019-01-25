using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{

    /// <summary>
    /// 被动技能
    /// </summary>
    public class SkillPassive : MonoBehaviour
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
//#include "GNodeSkillPassive.h"
//#include "GNodeSkillTarget.h"
//#include "GNodeSkillLogic.h"
//#include "GSkillData.h"
//#include "NodeBuff/GBuffComponent.h"
//#include "GBuffData.h"
//#include "NodeAvatar/NodeAvatar.h"

//namespace Galaxy
//{
//	GSkillSpellLogic_Passive::GSkillSpellLogic_Passive()
//		: m_pBuff(NULL)
//    {

//    }

//    GSkillSpellLogic_Passive::~GSkillSpellLogic_Passive()
//    {

//    }

//    bool GSkillSpellLogic_Passive::Init(GSkillInitParam& param)
//    {
//        if (!GSkillSpellLogic::Init(param))
//            return false;

//        m_pBuff = param.pBuff;
//        return true;
//    }

//    bool GSkillSpellLogic_Passive::PassiveProcessCheck()
//    {
//        if (!m_pOwner || !m_pSkillData)
//            return false;

//        GNodeAvatar* pCaster = GetCaster();
//        if (!pCaster)
//            return false;

//        //自身检查
//        int32 nSrvCheck = m_pSkillData->GetIntValue(MSV_SrcCheck);
//        if (nSrvCheck > 0)
//        {
//            if (!GSKillConditionCheckManager::Instance().Check(nSrvCheck, pCaster))
//                return false;
//        }

//        //目标检查
//        if (!GSkillLogicManager::Instance().CheckTarget(m_pSkillData, pCaster, m_TargetInfo))
//        {
//            return false;
//        }
//        return true;
//    }
//    //////////////////////////////////////////////////////////////////////////
//    //Buff技能 Dot
//    FINISH_FACTORY_Arg0(GSkillSpellLogic_Eot);
//    GSkillSpellLogic_Eot::GSkillSpellLogic_Eot()
//		: m_bTick(true)
//    {

//    }

//    GSkillSpellLogic_Eot::~GSkillSpellLogic_Eot()
//    {

//    }

//    void GSkillSpellLogic_Eot::Tick(int32 nFrameTime)
//    {
//        if (!m_pSkillData || !m_bTick)
//            return;

//        m_nCurTime += nFrameTime;
//        if (m_nCurTime >= m_nEffectTime)
//        {
//            m_nCurTime -= m_nEffectTime;
//            if (PassiveProcessCheck())
//            {
//                m_TargetInfo.m_vSrcPos = m_pOwner->GetPos();
//                m_TargetInfo.m_vAimDir = m_pOwner->GetDir();
//                ProcessEffect();
//            }

//            if (m_pSkillData->GetIntValue(MSV_EffectCount) > 0)
//            {
//                ++m_nCurCount;
//                m_bTick = (m_nEffectCount > m_nCurCount);
//            }
//        }
//    }

//    //////////////////////////////////////////////////////////////////////////
//    FINISH_FACTORY_Arg0(GSkillSpellLogic_Stand);
//    GSkillSpellLogic_Stand::GSkillSpellLogic_Stand()
//		: m_bKeepStand(true)
//		, m_nKeepTime(0)
//    {

//    }


//    GSkillSpellLogic_Stand::~GSkillSpellLogic_Stand()
//    {

//    }

//    bool GSkillSpellLogic_Stand::Init(GSkillInitParam& param)
//    {
//        if (!GSkillSpellLogic_Passive::Init(param))
//            return false;

//        m_bKeepStand = m_pSkillData->GetIntValue(MSV_SpellParam1);
//        m_nKeepTime = m_pSkillData->GetIntValue(MSV_SpellParam2);
//        m_nLaunchTime = m_pSkillData->GetIntValue(MSV_SpellParam3);
//        m_eState = Launching;

//        return true;
//    }

//    void GSkillSpellLogic_Stand::Tick(int32 nFrameTime)
//    {
//        if (!m_pOwner)
//            return;

//        Vector3 vCurPos = m_pOwner->GetPos();
//        Vector3 vOldPos = m_pOwner->GetOldPos();
//        bool bStand = (vOldPos.GetDistance(vCurPos) <= 0.1f);

//        //保持静止
//        if (m_bKeepStand)
//        {
//            m_nCurTime = (bStand) ? (m_nCurTime + nFrameTime) : 0;
//        }
//        //保持移动
//        else
//        {
//            m_nCurTime = (bStand) ? 0 : (m_nCurTime + nFrameTime);
//        }

//        if (m_eState == Launching)
//        {
//            if (m_nCurTime >= m_nLaunchTime)
//            {
//                m_nCurTime -= m_nLaunchTime;
//                m_eState = Normal;
//                return;
//            }
//        }
//        else if (m_eState == Normal)
//        {
//            if (bStand != m_bKeepStand)
//            {
//                m_nCurTime = 0;
//                m_eState = Launching;
//                return;
//            }

//            if (m_nCurTime >= m_nKeepTime)
//            {
//                m_nCurTime -= m_nKeepTime;

//                m_TargetInfo.m_vSrcPos = m_pOwner->GetPos();
//                if (PassiveProcessCheck())
//                {
//                    ProcessEffect();
//                }
//            }
//        }
//    }

//    //////////////////////////////////////////////////////////////////////////
//    //属性集
//    FINISH_FACTORY_Arg0(GSkillSpellLogic_AValue);
//    GSkillSpellLogic_AValue::GSkillSpellLogic_AValue()
//	{

//	}

//GSkillSpellLogic_AValue::~GSkillSpellLogic_AValue()
//{

//}

//    //////////////////////////////////////////////////////////////////////////
//    //满足第一条件则做一次某事，之后满足第二条件则重置
//    FINISH_FACTORY_Arg0(GSkillSpellLogic_Condition);
//GSkillSpellLogic_Condition::GSkillSpellLogic_Condition()
//		: m_bEffect(true)
//		, m_nConditionAdd(0)
//		, m_nConditionRemove(0)
//{

//}

//GSkillSpellLogic_Condition::~GSkillSpellLogic_Condition()
//{

//}

//bool GSkillSpellLogic_Condition::Init(GSkillInitParam& param)
//{
//    if (!GSkillSpellLogic_Passive::Init(param))
//        return false;

//    m_bEffect = false;
//    m_nConditionAdd = m_pSkillData->GetIntValue(MSV_SpellParam1);
//    m_nConditionRemove = m_pSkillData->GetIntValue(MSV_SpellParam2);
//    return true;
//}

//void GSkillSpellLogic_Condition::Tick(int32 nFrameTime)
//{
//    if (!m_pOwner)
//        return;

//    if (!m_bEffect)
//    {
//        if (CheckCondition(m_nConditionAdd))
//        {
//            if (PassiveProcessCheck())
//            {
//                ProcessEffect();
//                m_bEffect = true;
//            }
//        }
//    }
//    else
//    {
//        if (CheckCondition(m_nConditionRemove))
//        {
//            m_bEffect = false;
//        }
//    }
//}

//bool GSkillSpellLogic_Condition::CheckCondition(int nConditionID)
//{
//    if (!m_pOwner || !m_pSkillData)
//        return false;

//    GNodeAvatar* pCaster = GetCaster();
//    if (!pCaster)
//        return false;

//    if (nConditionID > 0)
//    {
//        if (!GSKillConditionCheckManager::Instance().Check(nConditionID, pCaster))
//            return false;
//    }
//    return true;
//}
//}