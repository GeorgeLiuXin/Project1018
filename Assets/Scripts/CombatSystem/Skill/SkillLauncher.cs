using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    /// <summary>
    /// 技能发射器
    /// 1、直接伤害帧
    /// 2、子弹
    /// </summary>
    public class SkillLauncher : MonoBehaviour
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
//#include "GNodeSkillLauncher.h"
//#include "GNodeSkillComponent.h"
//#include "GNodeSkillSpell.h"
//#include "GSkillData.h"
//#include "NodeAvatar/NodeAvatar.h"

//namespace Galaxy
//{
//    void GSkillLauncher_Direct::Process(GSkillSpellLogic* pSpellLogic, GNodeAvatar* pCaster)
//    {
//        if (pCaster && pCaster->GetSkillComponent() && pSpellLogic->m_pSkillData)
//        {
//            pCaster->GetSkillComponent()->ProcessSkillEffect(pSpellLogic->m_pSkillData, pSpellLogic->m_TargetInfo, pSpellLogic->m_AValue);
//        }
//    }

//    void GSkillLauncher_Bullet::Process(GSkillSpellLogic* pSpellLogic, GNodeAvatar* pCaster)
//    {
//        if (pCaster && pCaster->GetSkillComponent() && pSpellLogic->m_pSkillData)
//        {
//            int32 count = pSpellLogic->m_pSkillData->GetIntValue(MSV_ProjectileParam2);
//            if (count <= 0)
//            {
//                count = 1;
//            }
//            for (int32 i = 0; i < count; ++i)
//            {
//                pCaster->GetSkillComponent()->CreateSkillProjectile(pSpellLogic->m_pSkillData, pSpellLogic->m_TargetInfo, pSpellLogic->m_AValue);
//            }
//        }
//    }

//    void GSkillLauncher_Barrage::Process(GSkillSpellLogic* pSpellLogic, GNodeAvatar* pCaster)
//    {
//        if (!pCaster || !pSpellLogic->m_pSkillData && pSpellLogic->m_pSkillData)
//            return;

//        GSkillData* pSkillData = pSpellLogic->m_pSkillData;

//        //位置偏移
//        int32 nDistance = pSkillData->GetIntValue(MSV_LauncherParam1);
//        f32 fOffset = (nDistance / 10000) / 10.0f;
//        f32 fChildOffset = (nDistance % 10000) / 10.0f;
//        //子弹数量
//        int32 nCount = pSkillData->GetIntValue(MSV_LauncherParam2);
//        //间隔角度
//        int32 nAngle = pSkillData->GetIntValue(MSV_LauncherParam3);
//        f32 fDistance = pSkillData->GetFloatValue(MSV_ProjectileSpeed) * pSkillData->GetIntValue(MSV_ProjectileTime) / 1000;

//        if (nCount <= 0)
//            return;

//        GNodeSkillComponent* pSkillComponent = pCaster->GetSkillComponent();
//        if (!pSkillComponent)
//            return;

//        GSkillTargetInfo tempInfo = pSpellLogic->m_TargetInfo;
//        tempInfo.m_vSrcPos = tempInfo.m_vSrcPos + tempInfo.m_vAimDir * fOffset;

//        Vector3 vTempSrcPos = tempInfo.m_vSrcPos;
//        f32 fStartAngle = -(nAngle * (nCount - 1)) / 2.0f - nAngle;
//        tempInfo.m_vAimDir = tempInfo.m_vAimDir.GetRotated(Vector3(0, 0, 1), DEG2RAD(fStartAngle));
//        for (int32 i = 0; i < nCount; ++i)
//        {
//            tempInfo.m_vAimDir = tempInfo.m_vAimDir.GetRotated(Vector3(0, 0, 1), DEG2RAD(nAngle));
//            tempInfo.m_vAimDir.Normalize();
//            tempInfo.m_vSrcPos = tempInfo.m_vSrcPos + tempInfo.m_vAimDir * fChildOffset;
//            tempInfo.m_vTarPos = tempInfo.m_vSrcPos + tempInfo.m_vAimDir * fDistance;
//            pSkillComponent->CreateSkillProjectile(pSkillData, tempInfo, pSpellLogic->m_AValue);
//            tempInfo.m_vSrcPos = vTempSrcPos;
//        }
//    }

//    void GSKillLaucher_List::Process(GSkillSpellLogic* pSpellLogic, GNodeAvatar* pCaster)
//    {
//        if (pCaster && pCaster->GetSkillComponent())
//        {
//            int32 nTargetID = pSpellLogic->m_TargetInfo.m_nTargetID;
//            GSpellTargetList::iterator iter = pSpellLogic->m_vTargetList.begin();
//            GSpellTargetList::iterator iter_end = pSpellLogic->m_vTargetList.end();
//            for (; iter != iter_end; ++iter)
//            {
//                pSpellLogic->m_TargetInfo.m_nTargetID = *iter;
//                pCaster->GetSkillComponent()->ProcessSkillEffect(pSpellLogic->m_pSkillData, pSpellLogic->m_TargetInfo, pSpellLogic->m_AValue);
//            }
//            pSpellLogic->m_TargetInfo.m_nTargetID = nTargetID;
//        }
//    }
//}