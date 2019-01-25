using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{

    /// <summary>
    /// 技能基础逻辑
    /// </summary>
    public class SkillSpellLogic : MonoBehaviour
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
//#include "GNodeSkillLauncher.h"
//#include "GNodeSkillComponent.h"
//#include "GCollisionComponent.h"
//#include "NodeAvatar/NodeAvatar.h"
//#include "FSMStateInstance.h"
//#include "GSkillData.h"
//#include "AValueDefine.h"
//#include "GServerCollider.h"
//#include "GCDComponent.h"
//#include "GalaxyConfigValue.h"
//#include "NodeLogic/NodeCondition.h"
//#include "NodeManager/NodeConditionCheckManager.h"
//#include "GalaxyMath.h"

//namespace Galaxy
//{
//	GSkillSpellLogic::GSkillSpellLogic()
//		: m_bFinished(false)
//		, m_bCosted(false)
//		, m_nCasterID(0)
//		, m_nCurTime(0)
//		, m_nCurCount(0)
//		, m_nEffectTime(0)
//		, m_nEffectCount(0)
//		, m_pSkillData(NULL)
//		, m_pTransform(NULL)
//		, m_pOwner(NULL)
//    {

//    }

//    GSkillSpellLogic::~GSkillSpellLogic()
//    {
//        FACTORY_DELOBJ(m_pSkillData);
//    }

//    bool GSkillSpellLogic::Init(GSkillInitParam& param)
//    {
//        if (!param.pOwner || !param.pCaster || !param.pSkillData)
//            return false;

//        m_pSkillData = param.pSkillData->Clone(param.nSlots);
//        if (!m_pSkillData)
//            return false;

//        m_pOwner = param.pOwner;
//        m_nCasterID = param.pCaster->GetAvatarID();
//        m_nEffectTime = m_pSkillData->GetIntValue(MSV_EffectTime);
//        m_nEffectCount = m_pSkillData->GetIntValue(MSV_EffectCount);

//        //属性转换
//        int32 nTransfromID = m_pSkillData->GetIntValue(MSV_EffectTransform);
//        m_pTransform = GSkillDataManager::Instance().GetSkillEffectTransform(nTransfromID);
//        return true;
//    }

//    void GSkillSpellLogic::Reset()
//    {
//        m_bFinished = false;
//        m_bCosted = false;
//        m_nCurTime = 0;
//        m_nCurCount = 0;
//        m_TargetInfo.Reset();
//        m_vTargetList.clear();
//    }

//    void GSkillSpellLogic::ResetRoleAValue()
//    {
//        m_AValue.Reset(NULL);
//        //转化技能效果
//        TransfromRoleAValue();
//    }

//    void GSkillSpellLogic::TransfromRoleAValue()
//    {
//        if (!m_pTransform)
//            return;

//        GNodeAvatar* pCaster = GetCaster();
//        if (!pCaster)
//            return;

//        f32 fValue = 0.0f;
//        switch (m_pTransform->m_nTransformType)
//        {
//            case SkillEffectTransform_Caster_AValue:            //施法属性集
//                {
//                    int32 nValueID = AValueDefine::Instance().GetValueID(m_pTransform->m_sTransformName);
//                    if (nValueID >= 0 && nValueID < AValue::Count)
//                    {
//                        fValue = pCaster->GetRoleAValue().Values[nValueID];
//                    }
//                    break;
//                }
//            case SkillEffectTransform_Caster_Param:         //施法参数集
//                {
//                    if (pCaster->GetParamPool())
//                    {
//                        fValue = pCaster->GetParamPool()->GetValue(m_pTransform->m_sTransformName.c_str(), 0.0f);
//                    }
//                    break;
//                }
//            case SkillEffectTransform_Caster_Script:
//                {
//                    char buff[64] = { 0 };
//                    sprintf(buff, "OnSkillEffectTransfrom%d", GetSkillID());
//                    luabridge::LuaRef ref = GLuaVM::GetMainLuaVM().getGlobal(buff);
//                    if (!ref.isNil())
//			{
//                        try
//                        {
//                            fValue = ref(pCaster, NULL).cast<f32>();
//                        }
//                        catch (luabridge::LuaException const&e)
//				{
//                            GalaxyLog::error("call OnSkillEffectTransfrom%d :%s", GetSkillID(), e.what());
//                        }
//                        }
//                        break;
//                    }
//                }
//                m_AValue.Values[AValue::ad_skill_p] += fValue * m_pTransform->m_fTransformPrecent;
//        }

//        void GSkillSpellLogic::ProcessEffect()

//    {
//            int32 nLauncherType = m_pSkillData->GetIntValue(MSV_LauncherLogic);
//            GSkillLauncherLogic* pLauncher = GSkillLogicManager::Instance().GetLauncherLogic(nLauncherType);
//            if (pLauncher)
//            {
//                pLauncher->Process(this, GetCaster());
//            }
//        }

//        GNodeAvatar* GSkillSpellLogic::GetCaster()

//    {
//            if (m_pOwner)
//            {
//                return (m_pOwner->GetAvatarID() == m_nCasterID) ? m_pOwner : m_pOwner->GetSceneAvatar(m_nCasterID);
//            }
//            return NULL;
//        }

//        GNodeAvatar* GSkillSpellLogic::GetTarget()

//    {
//            if (m_pOwner)
//            {
//                return (m_pOwner->GetAvatarID() == m_TargetInfo.m_nTargetID) ? m_pOwner : m_pOwner->GetSceneAvatar(m_TargetInfo.m_nTargetID);
//            }
//            return NULL;
//        }

//        Galaxy::int32 GSkillSpellLogic::GetSkillID()

//    {
//            return (m_pSkillData) ? m_pSkillData->m_nDataID : 0;
//        }

//        Galaxy::int32 GSkillSpellLogic::GetSkillLevel()

//    {
//            return (m_pSkillData) ? m_pSkillData->m_nLevel : 0;
//        }

//        void GSkillSpellLogic::SetSkillSlots(int32 nSlots)

//    {
//            if (m_pSkillData)
//            {
//                m_pSkillData->UpdateSlots(nSlots, true);
//            }
//        }

//        Galaxy::int32 GSkillSpellLogic::GetSkillSlots()

//    {
//            return (m_pSkillData) ? m_pSkillData->m_nSlots : 0;
//        }

//        Galaxy::int32 GSkillSpellLogic::GetSkillPriority()

//    {
//            return (m_pSkillData) ? m_pSkillData->GetValue(MSV_Priority) : 0;
//        }

//        RoleAValue* GSkillSpellLogic::GetSkillAValue()

//    {
//            return (m_pSkillData) ? m_pSkillData->GetAvalue() : NULL;
//        }

//        //////////////////////////////////////////////////////////////////////////
//        //主动技能
//        GSkillSpellLogic_Active::GSkillSpellLogic_Active()
//		: m_nLockTime(0)
//		, m_nLastTime(0)
//		, m_nFirstEffectTime(0)
//		, m_nMoveStep(MoveStep_End)
//		, m_fMoveSpeed(0)
//		, m_nStartTime(0)
//		, m_nStopTime(0)

//    {

//        }

//        bool GSkillSpellLogic_Active::Init(GSkillInitParam & param)

//    {
//            if (!GSkillSpellLogic::Init(param))
//                return false;

//            m_nFirstEffectTime = m_pSkillData->GetIntValue(MSV_FirstEffectTime);
//            m_nLockTime = m_pSkillData->GetIntValue(MSV_LockTime);
//            m_nLastTime = m_pSkillData->GetIntValue(MSV_LastTime);

//            return true;
//        }
//        void GSkillSpellLogic_Active::ReInit()

//    {
//            GSkillSpellLogic::ReInit();

//            if (!m_pOwner)
//                return;

//            if (m_pOwner->IsPlayer())
//            {
//                if (m_pSkillData->IsMoveSkill() == false)
//                {
//                    Vector3 vDir = m_TargetInfo.m_vAimDir;
//                    vDir.z = 0;
//                    vDir.Normalize();
//                    m_pOwner->SetDir(vDir);
//                }
//            }

//            m_nMoveStep = MoveStep_End;
//            float fDis = m_pSkillData->GetFloatValue(MSV_SkillDis);
//            m_nStartTime = m_pSkillData->GetIntValue(MSV_SkillMoveStartTime);
//            m_nStopTime = m_pSkillData->GetIntValue(MSV_SkillMoveEndTime);
//            if (m_TargetInfo.m_vSrcPos.GetDistance(m_TargetInfo.m_vMoveTarPos) > 0.1f)
//            {
//                if (fDis > 0 && m_nStopTime > m_nStartTime)
//                {
//                    m_nMoveStep = MoveStep_NotStart;
//                    m_fMoveSpeed = fDis / ((float)m_nStopTime - (float)m_nStartTime);
//                }
//            }
//        }
//        void GSkillSpellLogic_Active::TickMove(int32 nFrameTime)

//    {
//            if (!m_pOwner)
//                return;

//            //暂时先不考虑玩家，玩家由客户端发送
//            if (m_pOwner->IsPlayer())
//            {
//                return;
//            }

//            if (MoveStep_End == m_nMoveStep)
//            {
//                return;
//            }

//            m_nStartTime -= nFrameTime;
//            m_nStopTime -= nFrameTime;

//            if (m_nStartTime < 0 && m_nStopTime > 0)
//            {
//                m_nMoveStep = MoveStep_Moving;
//                Vector3 vSrcPos = m_pOwner->GetPos();
//                f32 dis = vSrcPos.GetDistance(m_TargetInfo.m_vMoveTarPos);
//                if (dis < 0.1f)
//                {
//                    return;
//                }
//                f32 len = m_fMoveSpeed * nFrameTime;
//                if (dis < len)
//                {
//                    m_TargetInfo.m_vMoveTarPos.z = m_pOwner->GetSceneHeight(m_TargetInfo.m_vMoveTarPos);
//                    m_pOwner->SetPos(m_TargetInfo.m_vMoveTarPos);
//                }
//                else
//                {
//                    Vector3 vMoveDir = m_TargetInfo.m_vMoveTarPos - m_TargetInfo.m_vSrcPos;
//                    vMoveDir.z = 0.0f;
//                    vMoveDir.Normalize();
//                    vMoveDir.SetLength(len);
//                    Vector3 vTarPos = vSrcPos + vMoveDir;
//                    m_pOwner->SetPos(vTarPos);
//                }

//            }
//            else if (m_nStopTime <= 0)
//            {
//                m_nMoveStep = MoveStep_End;
//                Vector3 vTarPos = m_TargetInfo.m_vMoveTarPos;
//                vTarPos.z = m_pOwner->GetSceneHeight(vTarPos);
//                m_pOwner->SetPos(vTarPos);
//            }
//        }
//        void GSkillSpellLogic_Active::Reset()

//    {
//            GSkillSpellLogic::Reset();
//            m_nFirstEffectTime = m_pSkillData->GetIntValue(MSV_FirstEffectTime);
//            m_nLockTime = m_pSkillData->GetIntValue(MSV_LockTime);
//            m_nLastTime = m_pSkillData->GetIntValue(MSV_LastTime);
//            m_nCurTime = m_nEffectTime - m_nFirstEffectTime;
//        }

//        void GSkillSpellLogic_Active::ProcessEffect()

//    {
//            if (EffectCost())
//            {
//                GSkillSpellLogic::ProcessEffect();
//            }
//        }

//        bool GSkillSpellLogic_Active::EffectCost()

//    {
//            if (!m_pSkillData)
//                return false;

//            //效果阶段消耗逻辑
//            if (!m_bCosted && m_pSkillData->IsEffectStateCost())
//            {
//                GNodeAvatar* pCaster = GetCaster();
//                if (!pCaster)
//                {
//                    Finish();
//                    return false;
//                }
//                GNodeSkillComponent* pComp = pCaster->GetSkillComponent();
//                if (!pComp || !pComp->CheckCost(m_pSkillData) || !pComp->CheckCD(m_pSkillData))
//                {
//                    Finish();
//                    return false;
//                }

//                pComp->DoCost(m_pSkillData);
//                pComp->StartCD(m_pSkillData, false);
//                SetCosted();
//            }

//            return true;
//        }

//        void GSkillSpellLogic_Active::SetFSMState()
//    {
//            auto pCaster = GetCaster();
//            if (!pCaster)
//                return;
//            FSMParam_Skill sParam;
//            sParam.fTime = m_nLastTime;
//            sParam.fBreakableTime = m_nLockTime;
//            int nStateMach = m_pSkillData->IsMoveSkill() ? (int)ExStateMach_Up : (int)BaseStateMach;
//            pCaster->SetTargetState(&sParam, SPRI_Equal, (eStateMach)nStateMach);
//        }

//        void GSkillSpellLogic_Active::Tick(int32 nFrameTime)

//    {
//            m_nLockTime -= nFrameTime;
//            m_nLastTime -= nFrameTime;
//            if (m_nLastTime <= 0)
//            {
//                Finish();
//            }
//            TickMove(nFrameTime);
//        }

//        void GSkillSpellLogic_Active::TickEffect(int32 nFrameTime)

//    {
//            if (m_nCurCount >= m_nEffectCount)
//                return;

//            m_nCurTime += nFrameTime;
//            if (m_nCurTime >= m_nEffectTime)
//            {
//                ++m_nCurCount;
//                m_nCurTime -= m_nEffectTime;
//                ProcessEffect();
//            }
//        }

//        //////////////////////////////////////////////////////////////////////////
//        //技能帧
//        FINISH_FACTORY_Arg0(GSkillSpellLogic_Branch);
//        GSkillSpellLogic_Branch::GSkillSpellLogic_Branch()

//    {

//        }
//        void GSkillSpellLogic_Branch::ReInit()

//    {
//            if (m_pOwner == nullptr)
//            {
//                return;
//            }
//            if (m_pOwner->IsPlayer() == false)
//            {
//                if (m_pSkillData->IsTargetPos() == false)
//                {
//                    Vector3 vGoalPos = m_TargetInfo.m_vMoveTarPos;
//                    float fDis = m_pSkillData->GetFloatValue(MSV_SkillDis);
//                    if (m_pSkillData->GetIntValue(MSV_SkillMoveDir) == 0)
//                    {
//                        vGoalPos = m_TargetInfo.m_vSrcPos + m_TargetInfo.m_vAimDir * fDis;
//                    }
//                    else
//                    {
//                        vGoalPos = m_TargetInfo.m_vSrcPos - m_TargetInfo.m_vAimDir * fDis;
//                    }
//                    m_pOwner->RayCast2D(m_TargetInfo.m_vSrcPos, vGoalPos, 0.5f);
//                    m_TargetInfo.m_vMoveTarPos = vGoalPos;
//                }
//            }
//            GSkillSpellLogic_Active::ReInit();
//        }
//        void GSkillSpellLogic_Branch::Tick(int32 nFrameTime)

//    {
//            GSkillSpellLogic_Active::Tick(nFrameTime);
//            TickEffect(nFrameTime);
//        }


//        //////////////////////////////////////////////////////////////////////////
//        //吟唱
//        FINISH_FACTORY_Arg0(GSkillSpellLogic_Sing);
//        GSkillSpellLogic_Sing::GSkillSpellLogic_Sing()
//    {

//        }

//        bool GSkillSpellLogic_Sing::Init(GSkillInitParam & param)

//    {
//            if (!GSkillSpellLogic::Init(param))
//                return false;

//            m_nCurTime = 0;
//            m_nAnimID = m_pSkillData->GetIntValue(MSV_SpellParam1);
//            m_nSingTime = m_pSkillData->GetIntValue(MSV_SpellParam2);
//            m_nEffectTime += m_nSingTime;
//            return true;
//        }

//        void GSkillSpellLogic_Sing::Tick(int32 nFrameTime)

//    {
//            GSkillSpellLogic_Active::Tick(nFrameTime);

//            TickEffect(nFrameTime);
//        }

//        void GSkillSpellLogic_Sing::Reset()

//    {
//            GSkillSpellLogic_Active::Reset();
//            m_nCurTime = 0;
//        }

//        //////////////////////////////////////////////////////////////////////////
//        //引导
//        FINISH_FACTORY_Arg0(GSkillSpellLogic_Channel);
//        GSkillSpellLogic_Channel::GSkillSpellLogic_Channel()
//    {

//        }

//        bool GSkillSpellLogic_Channel::Init(GSkillInitParam & param)

//    {
//            if (!GSkillSpellLogic::Init(param))
//                return false;

//            m_nAnimID = m_pSkillData->GetIntValue(MSV_SpellParam1);
//            return true;
//        }

//        void GSkillSpellLogic_Channel::Tick(int32 nFrameTime)
//    {
//            GSkillSpellLogic_Active::Tick(nFrameTime);
//            TickEffect(nFrameTime);
//        }

//        void GSkillSpellLogic_Channel::Reset()

//    {
//            GSkillSpellLogic_Active::Reset();
//        }

//        //////////////////////////////////////////////////////////////////////////
//        //位移
//        GSkillSpellLogic_Move::GSkillSpellLogic_Move()
//		: m_bServerMove(false)
//		, m_bStartMove(false)
//		, m_bArrived(false)
//		, m_fMoveSpeed(0.0f)
//		, m_fMoveSpeedA(0.0f)
//		, m_fCurSpeed(0.0f)
//		, m_nStartTime(0)
//		, m_fDistance(0.0f)

//    {

//        }

//        bool GSkillSpellLogic_Move::Init(GSkillInitParam & param)

//    {
//            if (!GSkillSpellLogic_Active::Init(param))
//                return false;

//            m_bServerMove = param.pCaster->IsPlayer(); //玩家并且不处于托管状态
//            m_fMoveSpeed = m_pSkillData->GetIntValue(MSV_SpellParam1);
//            m_fMoveSpeedA = m_pSkillData->GetIntValue(MSV_SpellParam2);
//            m_nStartTime = m_pSkillData->GetIntValue(MSV_SpellParam3);
//            return true;
//        }

//        void GSkillSpellLogic_Move::ReInit()

//    {
//            if (m_pSkillData->IsTargetAvatar())
//            {
//                float fTime = (m_nLastTime - m_nStartTime) / 1000;
//                m_fDistance = m_fMoveSpeed * fTime + 0.5f * m_fMoveSpeedA * fTime * fTime;
//            }
//            if (m_pSkillData->IsTargetPos())
//            {
//                m_pOwner->RayCast2D(m_pOwner->GetPos(), m_TargetInfo.m_vTarPos, 0.5f);
//                m_fDistance = m_TargetInfo.m_vTarPos.GetDistance(m_TargetInfo.m_vSrcPos);
//            }
//            if (m_pSkillData->IsTargetDir())
//            {
//                f32 fTime = (m_nLastTime - m_nStartTime) / 1000;
//                f32 fDis = m_fMoveSpeed * fTime + 0.5 * m_fMoveSpeedA * fTime * fTime;
//                m_TargetInfo.m_vSrcPos = m_pOwner->GetPos();
//                m_TargetInfo.m_vTarPos = m_TargetInfo.m_vAimDir * fDis + m_TargetInfo.m_vSrcPos;
//                m_pOwner->RayCast2D(m_pOwner->GetPos(), m_TargetInfo.m_vTarPos, 0.5f);
//                m_fDistance = m_TargetInfo.m_vTarPos.GetDistance(m_TargetInfo.m_vSrcPos);
//            }
//        }

//        void GSkillSpellLogic_Move::Reset()

//    {
//            GSkillSpellLogic_Active::Reset();

//            m_bStartMove = false;
//            m_bArrived = false;
//            m_fCurSpeed = m_fMoveSpeed;
//            m_nStartTime = m_pSkillData->GetIntValue(MSV_SpellParam3);
//        }

//        void GSkillSpellLogic_Move::Tick(int32 nFrameTime)

//    {
//            GSkillSpellLogic_Active::Tick(nFrameTime);
//            GSkillSpellLogic_Active::TickEffect(nFrameTime);

//            //移动阶段
//            if (m_bStartMove)
//            {
//                Move(nFrameTime);
//            }
//            else
//            {
//                m_nCurTime += nFrameTime;
//                if (m_nCurTime >= m_nStartTime)
//                {
//                    StartMove();
//                    Move(m_nCurTime - m_nStartTime);
//                    //产生位移事件
//                }
//            }
//        }
//        void GSkillSpellLogic_Move::TickMove(int32 nFrameTime)

//    {
//            return;
//        }

//        void GSkillSpellLogic_Move::StartMove()

//    {
//            m_bStartMove = true;
//        }

//        void GSkillSpellLogic_Move::Arrived()

//    {
//            if (m_bArrived)
//                return;

//            m_bArrived = true;
//            //产生到达事件
//        }

//        //////////////////////////////////////////////////////////////////////////
//        //冲锋
//        FINISH_FACTORY_Arg0(GSkillSpellLogic_Dash);
//        void GSkillSpellLogic_Dash::StartMove()

//    {
//            GSkillSpellLogic_Move::StartMove();
//        }

//        void GSkillSpellLogic_Dash::Move(int32 nFrameTime)

//    {
//            if (!m_pOwner)
//                return;

//            if (m_bServerMove)
//                return;

//            Vector3 vSrcPos = m_TargetInfo.m_vSrcPos;
//            Vector3 vTarPos = m_TargetInfo.m_vTarPos;

//            if (m_pSkillData->IsTargetAvatar())
//            {
//                GNodeAvatar* pTarget = m_pOwner->GetSceneAvatar(m_TargetInfo.m_nTargetID);
//                if (!pTarget)
//                    return;

//                vTarPos = pTarget->GetPos();
//                vTarPos.z = pTarget->GetSceneHeight(vTarPos);
//                //vTarPos.z += 1.0f;

//                //if (pTarget->GetCollisionComponent())
//                //{
//                //	SSphere sph;
//                //	if (pTarget->GetCollisionComponent()->GetShape(0, sph))
//                //	{
//                //		Vector3 sCenter(sph.x, sph.y, sph.z);
//                //		GServerCollider::Local2World(sCenter, pTarget->GetPos(), pTarget->GetDir());
//                //		vTarPos = sCenter;
//                //	}
//                //}
//            }

//            float fTime = nFrameTime / 1000.0f;
//            m_fCurSpeed += fTime * m_fMoveSpeedA;
//            f32 fLength = fTime * m_fCurSpeed;
//            m_fDistance -= fLength;

//            if (m_pSkillData->IsTargetDir())
//            {
//                if (m_fDistance <= 0)
//                {
//                    m_TargetInfo.m_vSrcPos = vTarPos;
//                    m_TargetInfo.m_vTarPos = vTarPos;
//                    Arrived();
//                }
//                else
//                {
//                    Vector3 vDir = m_TargetInfo.m_vAimDir;
//                    vDir.z = 0.0f; //方向类子弹锁定Z轴
//                    vDir.Normalize();
//                    vDir.SetLength(fLength);
//                    m_TargetInfo.m_vSrcPos = vSrcPos + vDir;
//                    m_TargetInfo.m_vTarPos = vTarPos;
//                }
//            }
//            else if (m_pSkillData->IsTargetAvatar() || m_pSkillData->IsTargetPos())
//            {
//                if (m_fDistance <= 0)
//                {
//                    m_TargetInfo.m_vSrcPos = vTarPos;
//                    m_TargetInfo.m_vTarPos = vTarPos;
//                    Arrived();
//                }
//                else
//                {
//                    Vector3 vDir = vTarPos - vSrcPos;
//                    vDir.Normalize();
//                    vDir.SetLength(fLength);
//                    m_TargetInfo.m_vSrcPos = vSrcPos + vDir;
//                    m_TargetInfo.m_vTarPos = vTarPos;
//                }
//            }
//            else
//            {
//                return;
//            }

//            m_pOwner->SetPos(m_TargetInfo.m_vSrcPos);
//        }

//        //////////////////////////////////////////////////////////////////////////
//        // 移动中盯着某个点
//        FINISH_FACTORY_Arg0(GSkillSpellLogic_AimAt);

//        void GSkillSpellLogic_AimAt::TickMove(int32 nFrameTime)

//    {
//            m_nStartTime -= nFrameTime;
//            if (m_nStartTime < 0 && m_nEndTime > 0)
//            {
//                // 此处基于Frame的误差，会多转和少转，但是并不影响整体体验
//                m_pOwner->SetDir(m_pOwner->GetDir() + (m_vTurnByFrame * nFrameTime));
//                m_nEndTime -= nFrameTime;
//                if (m_nEndTime <= 0)
//                {
//                    m_pOwner->SetDir(m_TargetInfo.m_vAimDir);
//                }
//            }
//            GSkillSpellLogic_Branch::TickMove(nFrameTime);
//        }


//        void GSkillSpellLogic_AimAt::Finish()

//    {
//            GSkillSpellLogic_Branch::Finish();
//            //m_pOwner->SetDir(m_TargetInfo.m_vAimDir);
//        }

//        void GSkillSpellLogic_AimAt::ReInit()

//    {
//            m_nStartTime = m_pSkillData->GetIntValue(MSV_SkillMoveStartTime);
//            m_nEndTime = m_pSkillData->GetIntValue(MSV_SkillMoveEndTime);

//            int32 turnTime = m_nEndTime - m_nStartTime;
//            if (turnTime < 500)
//            {
//                turnTime = 500;
//                m_nEndTime = m_nStartTime + 500;
//            }

//            m_TargetInfo.m_vAimDir = m_TargetInfo.m_vTarPos - m_TargetInfo.m_vSrcPos;
//            m_TargetInfo.m_vAimDir.z = 0;
//            m_TargetInfo.m_vAimDir.Normalize();
//            //m_pOwner->SetDir(m_TargetInfo.m_vAimDir);
//            if (turnTime > 0)
//            {
//                m_vTurnByFrame = (m_TargetInfo.m_vAimDir - m_pOwner->GetDir()) / turnTime;
//            }
//            else
//            {
//                m_vTurnByFrame = Vector3(0, 0, 0);
//            }
//        }

//        //////////////////////////////////////////////////////////////////////////
//        //蓄力
//        FINISH_FACTORY_Arg0(GSkillSpellLogic_Charger);

//        GSkillSpellLogic_Charger::GSkillSpellLogic_Charger()
//		: m_bChargerCast(false)
//		, m_nChargerTime(0)
//		, m_nChargerCnt(0)
//		, m_nChargerMaxCnt(0)

//    {

//        }

//        bool GSkillSpellLogic_Charger::Init(GSkillInitParam & param)

//    {
//            if (!GSkillSpellLogic_Active::Init(param))
//                return false;

//            m_nChargerTime = m_pSkillData->GetIntValue(MSV_SpellParam1);
//            m_nChargerMaxCnt = m_pSkillData->GetIntValue(MSV_SpellParam2);
//            return true;
//        }
//        void GSkillSpellLogic_Charger::SetFSMState()
//    {
//            auto pCaster = GetCaster();
//            if (!pCaster)
//                return;
//            FSMParam_Skill sParam;
//            //2018-07-30 策划需求-蓄力无限憋
//            sParam.fTime = FLT_MAX;//m_pSkillData->GetIntValue(MSV_SpellParam1) * (m_pSkillData->GetIntValue(MSV_SpellParam2) + 1);
//            sParam.fBreakableTime = 0;
//            int nStateMach = m_pSkillData->IsMoveSkill() ? (int)ExStateMach_Up : (int)BaseStateMach;
//            pCaster->SetTargetState(&sParam, SPRI_Equal, (eStateMach)nStateMach);
//        }
//        void GSkillSpellLogic_Charger::Reset()

//    {
//            GSkillSpellLogic_Active::Reset();

//            m_bChargerCast = false;
//            m_nChargerCnt = 0;
//        }

//        void GSkillSpellLogic_Charger::Tick(int32 nFrameTime)

//    {
//            //施放后，进入前/后摇阶段
//            if (m_bChargerCast)
//            {
//                GSkillSpellLogic_Active::Tick(nFrameTime);
//                TickEffect(nFrameTime);
//            }
//            //--- 蓄力完成，超时结束技能
//            //--- 策划需求-蓄力无限憋 2018-07-30
//            //+++ 策划需求-蓄满直接放 2018-10-09
//            else if (m_nChargerCnt >= m_nChargerMaxCnt)
//            {
//                //return;
//                m_nCurTime += nFrameTime;   //等客户端通知
//                if (m_nCurTime >= MAX_SKILL_NETWORK_DELAY)
//                {
//                    Finish();
//                }
//            }
//            //蓄力
//            else
//            {
//                m_nCurTime += nFrameTime;
//                if (m_nCurTime >= m_nChargerTime)
//                {
//                    m_nCurTime -= m_nChargerTime;
//                    m_TargetInfo.m_nUserData = (++m_nChargerCnt);
//                }
//            }
//        }

//        void GSkillSpellLogic_Charger::Cast(int32 chargeLevel/* = -1*/)
//    {
//            if (m_bChargerCast)
//                return;

//            if (!m_pSkillData)
//                return;

//            //if (-1 == chargeLevel)
//            //{
//            //    SkillAbort();
//            //    Finish();
//            //}

//            GNodeAvatar* pCaster = GetCaster();
//            if (!pCaster || !pCaster->GetSkillComponent())
//                return;

//            //服务器加速
//            chargeLevel = MAX(0, chargeLevel);
//            int32 nTimeStamp = chargeLevel * m_nChargerTime;
//            pCaster->GetSkillComponent()->SpeedUpSpell(nTimeStamp);

//            //开始等待计时
//            m_bChargerCast = true;
//            m_nCurTime = 0;

//            FSMParam_Skill sParam;
//            sParam.fTime = m_nLastTime;
//            sParam.fBreakableTime = m_nLockTime;
//            int nStateMach = m_pSkillData->IsMoveSkill() ? (int)ExStateMach_Up : (int)BaseStateMach;
//            m_pOwner->SetTargetState(&sParam, SPRI_Equal, (eStateMach)nStateMach);

//            //通知其他客户端施放技能
//            GPacketSkillCast pkt;
//            pkt.SkillID = m_pSkillData->m_nDataID;
//            pkt.SkillSlots = m_pSkillData->m_nSlots;
//            pkt.SkillState = m_nChargerCnt;
//            pCaster->BroadcastPacket(&pkt);

//            if (m_pSkillData->IsTriggerSkillNotify())
//            {
//                pCaster->GetSkillComponent()->PushTriggerNotify(m_pSkillData->m_nDataID, m_TargetInfo.m_nTargetID, NotifyType_Skill, TriggerNotify_SkillCast, m_nChargerCnt, &m_TargetInfo.m_vSrcPos, &m_TargetInfo.m_vTarPos, &m_TargetInfo.m_vAimDir);
//            }
//        }

//        void GSkillSpellLogic_Charger::Abort(bool bServer)
//    {
//            if (bServer)
//                return;
//            if (!m_pSkillData)
//                return;

//            GCDComponent* pCDComponent = m_pOwner->GetCDComponent();
//            if (!pCDComponent)
//                return;

//            int32 nCDGroup = m_pSkillData->GetIntValue(MSV_CDGroup);
//            int32 nCDTime = m_pSkillData->GetIntValue(MSV_CDTime);
//            f32 fRadio = ConfigValueManager::Instance().GetFloatConfigValue(SkillChargeAbortCDRatio, 0.5f);
//            pCDComponent->StartCD(nCDGroup, nCDTime * fRadio, false);
//        }

//        //////////////////////////////////////////////////////////////////////////
//        //多段效果
//        FINISH_FACTORY_Arg0(GSkillSpellLogic_MultiStep);
//        GSkillSpellLogic_MultiStep::GSkillSpellLogic_MultiStep()
//		: m_nStepCnt(0)
//		, m_nStepCntMax(0)

//    {

//        }

//        bool GSkillSpellLogic_MultiStep::Init(GSkillInitParam & param)

//    {
//            if (!GSkillSpellLogic_Active::Init(param))
//                return false;

//            m_nStepCntMax = m_pSkillData->GetIntValue(MSV_SpellParam1);
//            return true;
//        }

//        void GSkillSpellLogic_MultiStep::Reset()

//    {
//            GSkillSpellLogic_Active::Reset();

//            m_nStepCnt = 0;
//        }

//        void GSkillSpellLogic_MultiStep::Tick(int32 nFrameTime)

//    {
//            GSkillSpellLogic_Active::Tick(nFrameTime);
//        }

//        void GSkillSpellLogic_MultiStep::Cast(int32 chargeLevel /*= -1*/)

//    {
//            if (!m_pSkillData)
//                return;

//            GNodeAvatar* pCaster = GetCaster();
//            if (!pCaster || !pCaster->GetSkillComponent())
//                return;

//            if (m_nStepCnt >= m_nStepCntMax)
//                return;

//            ++m_nStepCnt;

//            FSMParam_Skill sParam;
//            sParam.fTime = m_nLastTime;
//            sParam.fBreakableTime = m_nLockTime;
//            int nStateMach = m_pSkillData->IsMoveSkill() ? (int)ExStateMach_Up : (int)BaseStateMach;
//            m_pOwner->SetTargetState(&sParam, SPRI_Equal, (eStateMach)nStateMach);

//            //通知其他客户端施放技能
//            GPacketSkillCast pkt;
//            pkt.SkillID = m_pSkillData->m_nDataID;
//            pkt.SkillSlots = m_pSkillData->m_nSlots;
//            pkt.SkillState = m_nStepCnt;
//            pCaster->BroadcastPacket(&pkt);

//            if (m_pSkillData->IsTriggerSkillNotify())
//            {
//                pCaster->GetSkillComponent()->PushTriggerNotify(m_pSkillData->m_nDataID, m_TargetInfo.m_nTargetID, NotifyType_Skill, TriggerNotify_SkillCast, m_nStepCnt, &m_TargetInfo.m_vSrcPos, &m_TargetInfo.m_vTarPos, &m_TargetInfo.m_vAimDir);
//            }
//        }

//        //////////////////////////////////////////////////////////////////////////
//        //瞬移技能
//        FINISH_FACTORY_Arg0(GSkillSpellLogic_Teleportation);
//        GSkillSpellLogic_Teleportation::GSkillSpellLogic_Teleportation()
//    {
//        }
//        void GSkillSpellLogic_Teleportation::Tick(int32 nFrameTime)
//    {
//            GSkillSpellLogic_Active::Tick(nFrameTime);
//            TickEffect(nFrameTime);
//        }


//        //////////////////////////////////////////////////////////////////////////
//        //自动攻击列表目标
//        FINISH_FACTORY_Arg0(GSkillSpellLogic_AttackQueue);
//        GSkillSpellLogic_AttackQueue::GSkillSpellLogic_AttackQueue()
//		: m_fAimRange(0)
//		, m_nQueueCount(0)
//		, m_nAimType(0)

//    {

//        }

//        bool GSkillSpellLogic_AttackQueue::Init(GSkillInitParam & param)

//    {
//            if (!GSkillSpellLogic_Active::Init(param))
//                return false;

//            m_nQueueCount = m_pSkillData->GetIntValue(MSV_SpellParam1);
//            m_fAimRange = m_pSkillData->GetIntValue(MSV_SpellParam2) / 10.0;
//            m_nAimType = m_pSkillData->GetIntValue(MSV_SpellParam3);
//            return true;
//        }

//        void GSkillSpellLogic_AttackQueue::ReInit()

//    {
//            GSkillSpellLogic::ReInit();
//            m_TargetList.clear();

//            GNodeAvatar* curTarget = m_pOwner->GetSceneAvatar(m_TargetInfo.m_nTargetID);
//            if (CheckAvatar(curTarget))
//                m_TargetList.push_back(curTarget);

//            AOIList* pList = m_pOwner->GetArroundAvatar();
//            if (!pList)
//                return;
//            for (int32 i = 0; i < pList->GetCount(); ++i)
//            {
//                int32 avatarID = pList->GetIdByIndex(i);
//                if (avatarID == m_TargetInfo.m_nTargetID)
//                    continue;
//                GNodeAvatar* pTarget = m_pOwner->GetSceneAvatar(avatarID);
//                if (!CheckAvatar(pTarget))
//                    continue;

//                m_TargetList.push_back(pTarget);
//            }
//            m_nIndex = 0;
//        }

//        bool GSkillSpellLogic_AttackQueue::CheckAvatar(GNodeAvatar * pTarget)

//    {
//            if (!pTarget || pTarget->IsDead())
//                return false;

//            int32 nTarCheck = m_pSkillData->GetValue(MSV_AreaTarCheck);
//            if (nTarCheck > 0)
//            {
//                SDConditionParamAvatar sParam;
//                sParam.ParamAvatar = m_pOwner;
//                if (!GSKillConditionCheckManager::Instance().Check(nTarCheck, pTarget, &sParam))
//                    return false;
//            }

//            Vector3 vSrcPos = m_pOwner->GetPos();
//            Vector3 vSrcdir = m_TargetInfo.m_vAimDir;
//            Vector3 vTarPos = pTarget->GetPos();
//            if (vTarPos.GetDistance(vSrcPos) > m_fAimRange)
//                return false;
//            Vector3 offestPos = vTarPos - vSrcPos;
//            f32 fAngle = vSrcdir.Dot(offestPos);
//            if (fAngle > 1)
//            {
//                fAngle = 0.9999f;
//            }
//            else if (fAngle < -1)
//            {
//                fAngle = -0.9999f;
//            }
//            f32 alpha = RAD2DEG(acos(fAngle));
//            switch (m_nAimType)
//            {
//                case Normal:
//                    break;
//                case Front:
//                    if (vSrcdir.Dot(offestPos) < 0)
//                        return false;
//                    break;
//                case Angle90:
//                    if (alpha >= 45)
//                        return false;
//                    break;
//                case Angle120:
//                    if (alpha >= 60)
//                        return false;
//                    break;
//            }
//            return true;
//        }

//        void GSkillSpellLogic_AttackQueue::Reset()

//    {
//            GSkillSpellLogic_Active::Reset();

//            m_nQueueCount = m_pSkillData->GetIntValue(MSV_SpellParam1);
//            m_fAimRange = m_pSkillData->GetIntValue(MSV_SpellParam2) / 10.0;
//            m_nAimType = m_pSkillData->GetIntValue(MSV_SpellParam3);
//        }

//        void GSkillSpellLogic_AttackQueue::Tick(int32 nFrameTime)

//    {
//            GSkillSpellLogic_Active::Tick(nFrameTime);
//            TickEffect(nFrameTime);
//        }

//        void GSkillSpellLogic_AttackQueue::ProcessEffect()

//    {
//            GNodeAvatar* pTarget;

//            for (std::vector<GNodeAvatar*>::iterator iter = m_TargetList.begin(); iter != m_TargetList.end();)
//            {
//                pTarget = *iter;
//                if (!pTarget || pTarget->IsDead())
//                {
//                    iter = m_TargetList.erase(iter);
//                    continue;
//                }
//                else
//                {
//                    m_TargetList.erase(iter);
//                    m_TargetList.push_back(pTarget);

//                    m_TargetInfo.m_nTargetID = pTarget->GetAvatarID();
//                    m_TargetInfo.m_vSrcPos = m_pOwner->GetPos();
//                    m_TargetInfo.m_vSrcPos.z += 1.5f;
//                    m_TargetInfo.m_vTarPos = pTarget->GetPos();
//                    m_TargetInfo.m_vTarPos.z += 1.5f;
//                    m_TargetInfo.m_vAimDir = (m_TargetInfo.m_vTarPos - m_TargetInfo.m_vSrcPos).normalize();
//                    m_TargetInfo.m_nShapeID = 0;

//                    GSkillSpellLogic_Active::ProcessEffect();
//                    return;
//                }
//            }
//        }

//    }