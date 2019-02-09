using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    /// <summary>
    /// 触发技能相关检查
    /// </summary>
    public class TriggerNotify : MonoBehaviour
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
//#include "GNodeTriggerNotify.h"
//#include "GNodeSkillComponent.h"
//#include "GSkillData.h"
//#include "NodeAvatar/NodeAvatar.h"
//#include "GalaxyRandom.h"

//namespace Galaxy
//{
//    bool GTriggerNotify::CheckTrigger(GNodeAvatar* pCaster, GNodeAvatar* pTarget, GSkillData* pSkillData)
//    {
//        if (!pCaster || !pSkillData)
//            return false;

//        int32 nType = pSkillData->GetIntValue(MSV_TriggerType);
//        if (m_nType != nType)
//            return false;

//        int32 nCheck = pSkillData->GetIntValue(MSV_TriggerCheck);

//        //检查消息DataID
//        if ((nCheck & TriggerCheck_Data_UnChecked) <= 0)
//        {
//            int32 nDataID = pSkillData->GetIntValue(MSV_TriggerDataID);
//            if ((nCheck & TriggerCheck_Data_ID) > 0)
//            {
//                if (m_nDataID != nDataID) return false;
//            }
//            else if ((nCheck & TriggerCheck_Data_SkillGroup) > 0)
//            {
//                GSkillData* pSkillData_ = GSkillDataManager::Instance().GetSkillData(m_nDataID); //TODO 模板数据不可修正
//                if (!pSkillData_) return false;

//                int32 nSkillGroup = pSkillData_->GetIntValue(MSV_SkillGroup);
//                if ((nCheck & TriggerCheck_Data_SkillGroup_And) > 0)
//                {
//                    if ((nSkillGroup & nDataID) != nDataID) return false;
//                }
//                else if ((nCheck & TriggerCheck_Data_SkillGroup_Or) > 0)
//                {
//                    if ((nSkillGroup & nDataID) <= 0) return false;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//        }
//        //检查消息参数
//        if ((nCheck & TriggerCheck_Flag_UnChecked) <= 0)
//        {
//            int32 nFlag = pSkillData->GetIntValue(MSV_TriggerNotify);
//            if ((nCheck & TriggerCheck_Flag_Equal) > 0)
//            {
//                if (m_nFlag != nFlag) return false;
//            }
//            else if ((nCheck & TriggerCheck_Flag_And) > 0)
//            {
//                if ((m_nFlag & nFlag) != nFlag) return false;
//            }
//            else if ((nCheck & TriggerCheck_Flag_Or) > 0)
//            {
//                if ((m_nFlag & nFlag) <= 0) return false;
//            }
//        }
//        //检查消息数值
//        if ((nCheck & TriggerCheck_Value_UnChecked) <= 0)
//        {
//            int32 nValue = pSkillData->GetIntValue(MSV_TriggerValue);
//            if ((nCheck & TriggerCheck_Value_Greater) > 0)
//            {
//                if (m_nValue <= nValue) return false;
//            }
//            else if ((nCheck & TriggerCheck_Value_GreaterAndEqual) > 0)
//            {
//                if (m_nValue < nValue) return false;
//            }
//            else if ((nCheck & TriggerCheck_Value_Less) > 0)
//            {
//                if (m_nValue >= nValue) return false;
//            }
//            else if ((nCheck & TriggerCheck_Value_LessAndEqual) > 0)
//            {
//                if (m_nValue > nValue) return false;
//            }
//            else if ((nCheck & TriggerCheck_Value_Equal) > 0)
//            {
//                if (m_nValue != nValue) return false;
//            }
//            else if ((nCheck & TriggerCheck_Value_NotEqual) > 0)
//            {
//                if (m_nValue == nValue) return false;
//            }
//            else if ((nCheck & TriggerCheck_Value_And) > 0)
//            {
//                if ((m_nValue & nValue) != nValue) return false;
//            }
//            else if ((nCheck & TriggerCheck_Value_Or) > 0)
//            {
//                if ((m_nValue & nValue) <= 0) return false;
//            }
//        }

//        //检查概率
//        int32 nProbalitity = pSkillData->GetIntValue(MSV_TriggerProbability);
//        int32 nRand = GALAXY_RANDOM.RandInt(1, 1000);
//        return (nProbalitity >= nRand);
//    }

//    //////////////////////////////////////////////////////////////////////////
//    //通用触发事件
//    FINISH_FACTORY_Arg0(GTriggerNotifyNormal);

//    //////////////////////////////////////////////////////////////////////////
//    //伤害吸收触发事件
//    FINISH_FACTORY_Arg0(GTriggerNotifyEffect);

//    //////////////////////////////////////////////////////////////////////////
//    //结算触发事件
//    FINISH_FACTORY_Arg0(GTriggerNotifyCombine);
//}



//#ifndef _GALAXY_NODE_TRIGGER_NOTIFY_H_
//#define _GALAXY_NODE_TRIGGER_NOTIFY_H_

//#include "GalaxyFactory.h"
//#include "SkillDefine.h"
//#include "GMap.h"
//#include <vector>

//namespace Galaxy
//{
//    class GSkillData;
//    class GNodeAvatar;

//    enum eNotifyObject
//    {
//        NotifyObject_Base = 0,
//        NotifyObject_Normal,
//        NotifyObject_Effect,
//        NotifyObject_Combine,
//    };

//    class GTriggerNotify
//    {
//        public:
//		GTriggerNotify() :m_nDataID(0),m_nTargetID(0),m_nType(0),m_nFlag(0),m_nValue(0), m_vSrcPos(0,0,0), m_vTarPos(0, 0, 0), m_vDir(0,0,0) { }
//        virtual ~GTriggerNotify() { }
//        virtual int32 GetType() { return NotifyObject_Base; }
//        virtual bool CheckTrigger(GNodeAvatar* pCaster, GNodeAvatar* pTarget, GSkillData* pSkillData);
//        //立即处理触发逻辑, 如护盾/假死
//        virtual bool IsTriggerAtOnce() { return false; }

//        int32 m_nDataID;
//        int32 m_nTargetID;
//        int32 m_nType;
//        int32 m_nFlag;
//        int32 m_nValue;
//        Vector3 m_vSrcPos;
//        Vector3 m_vTarPos;
//        Vector3 m_vDir;
//    };

//    //////////////////////////////////////////////////////////////////////////
//    //通用触发事件
//    class GTriggerNotifyNormal : public GTriggerNotify
//	{

//        DECLARE_FACTORY_Arg0(GTriggerNotifyNormal, -1, new GPoolAllocater)
//	public:

//        GTriggerNotifyNormal()
//    { }
//    virtual ~GTriggerNotifyNormal() { }
//    virtual int32 GetType() { return NotifyObject_Normal; }
//};

////////////////////////////////////////////////////////////////////////////
////伤害治疗触发事件
//class GTriggerNotifyEffect : public GTriggerNotify
//	{

//        DECLARE_FACTORY_Arg0(GTriggerNotifyEffect, -1, new GPoolAllocater)
//	public:

//        GTriggerNotifyEffect():m_pValue(NULL) { }
//virtual ~GTriggerNotifyEffect() { }
//virtual int32 GetType() { return NotifyObject_Effect; }
//virtual bool IsTriggerAtOnce() { return true; }

//f32* m_pValue;
//	};

//	//////////////////////////////////////////////////////////////////////////
//	//合并触发事件
//	class RoleAValue;
//class GTriggerNotifyCombine : public GTriggerNotify
//	{

//        DECLARE_FACTORY_Arg0(GTriggerNotifyCombine, -1, new GPoolAllocater)
//	public:

//        GTriggerNotifyCombine():m_pRoleAValue(NULL) { }
//virtual ~GTriggerNotifyCombine() { }
//virtual int32 GetType() { return NotifyObject_Combine; }
//virtual bool IsTriggerAtOnce() { return true; }
//RoleAValue* m_pRoleAValue;
//	};
//}

//#endif