using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    /// <summary>
    /// 技能效果结算
    /// </summary>
    public class SkillCalculation : MonoBehaviour
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
//#include "GNodeSkillCalculation.h"
//#include "GSkillData.h"
//#include "SkillDefine.h"
//#include "GNodeSkillComponent.h"
//#include "NodeCard/GNodeSkillExComponent.h"
//#include "GNodeTriggerNotify.h"
//#include "NodeAvatar/NodeAvatar.h"
//#include "GalaxyRandom.h"
//#include "GalaxyConfigValue.h"
//#include "NodeAvatar/NodePlayer_Component.h"
//#include "GNodePlayerControlProtect.h"

//namespace Galaxy
//{
//    //命中判断
//    void GSkillEffectCalculation::DecisionOnHit(int32& nEffectType)
//    {
//        if (!m_pSkillData || !m_pCaster || !m_pTarget)
//            return;

//        //检查闪避
//        if (m_pTarget->CheckState(GAS_Dodge))
//            return;

//        //检查无敌
//        if (m_pTarget->CheckState(GAS_God))
//            return;

//        //检查免疫状态
//        nEffectType |= TriggerNotify_Hit;
//    }

//    //伤害会心判断
//    void GSkillEffectCalculation::DecisionOnDamageCrit(int32& nEffectType)
//    {
//        if (!m_pSkillData || !m_pCaster || !m_pTarget)
//            return;

//        if (!m_pSkillData->IsCalculationCrit())
//            return;

//        using namespace AValue;
//		f32 C0 = 0.6f;
//    f32 C1 = 200.0f;
//    f32 C2 = 0.04f;

//    f32 fDHR = MIN(C0, (m_CasterAValue.Values[dhp] + C1) / (m_TargetAValue.Values[be_dhp] + C1) * C2);
//    fDHR += m_CasterAValue.Values[dhr] - m_TargetAValue.Values[be_dhr];

//		f32 fRand = GALAXY_RANDOM.RandFloat();
//		if (fDHR >= fRand) nEffectType |= TriggerNotify_Crit;
//	}

//    //伤害结算
//    void GSkillEffectCalculation::CalculationDamage(int32& nEffectType, int32& nEffectValue, uint8 damageReduction)
//    {
//        if (!m_pSkillData || !m_pCaster || !m_pTarget)
//            return;

//        //命中判断
//        DecisionOnHit(nEffectType);
//        if ((nEffectType & TriggerNotify_Hit) <= 0)
//            return;

//        //打断判断
//        if (m_pSkillData->IsBreakSkill() && m_pTarget->GetSkillComponent())
//        {
//            if (m_pTarget->GetSkillComponent()->BreakSkill())
//                nEffectType |= TriggerNotify_Break;
//        }

//        //会心判断
//        DecisionOnDamageCrit(nEffectType);

//        //计算攻击
//        using namespace AValue;
//		f32 fAtkPower = GetPower();

//		//计算护甲
//		if (m_pSkillData->IsCalculationAC())
//		{
//			f32 fAtkParam = ConfigValueManager::Instance().CombatAtkParam;
//    f32 fStarParam = ConfigValueManager::Instance().CombatStarParam;
//    f32 fCustomParam = ConfigValueManager::Instance().CombatCustomParam;

//    f32 fAc = m_TargetAValue.Values[d_ac] * (1 + m_TargetAValue.Values[d_ac_r]);
//			if (fAc > 0 && fAtkPower > 0)
//			{
//				int32 nCount = m_pCaster->GetCardStarCount();
//    fAtkPower = fAtkPower* (1 - fAc / (fAc + fAtkPower* fAtkParam + nCount* fStarParam + fCustomParam));
//			}
//}

//		if ((nEffectType & TriggerNotify_Crit) > 0)
//		{
//			f32 MIN_K_DHCR = 0;
//f32 MAX_K_DHCR = 1;
//f32 fDHCR = MIN(MAX(m_CasterAValue.Values[dhcr] - m_TargetAValue.Values[be_dhcr], MIN_K_DHCR), MAX_K_DHCR);
//fAtkPower *= (1.5 + fDHCR);
//		}

//		f32 fDDR = 0;
//		//计算伤增/伤减
//		if (m_pSkillData->IsCalculationDR())
//		{
//			f32 MIN_K_DDR = -1.0f;
//f32 MAX_K_DDR = 1.0f;
//fDDR = m_CasterAValue.Values[dr] - m_TargetAValue.Values[cdr];
//			fDDR = MIN(MAX(fDDR, MIN_K_DDR), MAX_K_DDR);
//		}

//		f32 fDamage = fAtkPower * m_CasterAValue.Values[ad_skill_r] * (1 + fDDR);
//f32 fDamageDM = 0.0f; //计算真实伤害
//		if (m_pSkillData->IsCalculationDM())
//		{
//			fDamageDM = MAX(m_CasterAValue.Values[dm] - m_TargetAValue.Values[da], 0);
//		}

//		f32 fDFR = ConfigValueManager::Instance().CombatDFRParam;
//f32 fRandom = GALAXY_RANDOM.RandFloat(1 - fDFR, 1 + fDFR);
//f32 fCounter = GetCounterEffect(m_pCaster->GetCounter(), m_pTarget->GetCounter());
//f32 fTotalDamage = MAX(1, (fDamage + fDamageDM) * fRandom * fCounter);
//		//造成的伤害
//		if (m_pCaster->GetSkillComponent())
//		{
//			m_pCaster->GetSkillComponent()->PushTriggerNotifyEffect(m_pSkillData->m_nDataID, m_pTarget->GetAvatarID(), NotifyType_MakeDamage, nEffectType, &fTotalDamage);		
//		}
//        //受障碍物减免的伤害
//        if (m_pSkillData->IsCaclObstacle())
//        {
//            uint8 damagePierced = MIN(100, MAX(0, m_pSkillData->GetIntValue(MSV_EffectParam3)));  //EffectDamageLogic的参数3作为伤害穿透系数
//            if (damagePierced< 100 - damageReduction)
//            {
//                //目前不清楚NavNode初始化流程，所以damageReduction暂时是反着算的
//                f32 daPercent = (damageReduction + damagePierced) / 100.0f;
//fTotalDamage *= daPercent;    //目前障碍物减伤100%,即damageReduction为0
//            }
//        }

//		if (fTotalDamage >= m_pTarget->GetHp())
//		{
//			nEffectType |= TriggerNotify_NearDeath;
//		}

//		//承受的伤害
//		if (m_pTarget->GetSkillComponent())
//		{
//			m_pTarget->GetSkillComponent()->PushTriggerNotifyEffect(m_pSkillData->m_nDataID, m_pCaster->GetAvatarID(), NotifyType_TakeDamage, nEffectType, &fTotalDamage);
//		}
        
//		//if (fTotalDamage < fDamage + fDamageDM)
//		//{
//		//	nEffectType |= TriggerNotify_Absorbs;
//		//}
//		if (m_pSkillData->IsCalculationHD())
//		{
//			if (m_pTarget->GetSkillExComponent())
//			{
//				m_pTarget->GetSkillExComponent()->HandleDamage(m_CasterAValue.Values[hdr], &fTotalDamage);
//			}
//		}
		
//		if (fTotalDamage >= 1.0f)
//		{
//			if (m_pTarget->IsPlayer())
//			{
//				GNodePlayer_Component* pPlayer = static_cast<GNodePlayer_Component*>(m_pTarget);
//GNodePlayerControlProtect* pCom = pPlayer->GetControlProtectComponent();
//				if (pCom)
//				{
//					pCom->PushDamageData(fTotalDamage);
//				}
//			}
//			nEffectValue = (int32)fTotalDamage;
//			m_pTarget->SetHurt(m_pCaster->GetAvatarID(), nEffectValue, TRUE);
//		}
//	}

//	//治疗会心判断
//	void GSkillEffectCalculation::DecisionOnHealCrit(int32& nEffectType)
//{
//    if (!m_pSkillData || !m_pCaster || !m_pTarget)
//        return;

//    if (!m_pSkillData->IsCalculationCrit())
//        return;

//    using namespace AValue;
//		float fCrit = MAX(m_CasterAValue.Values[dhr], 0);
//f32 fRand = GALAXY_RANDOM.RandFloat();
//		if (fCrit >= fRand) nEffectType |= TriggerNotify_Crit;
//	}

////治疗结算
//void GSkillEffectCalculation::CalculationHeal(int32& nEffectType, int32& nEffectValue)
//{
//    if (!m_pSkillData || !m_pCaster || !m_pTarget)
//        return;

//    //会心判断
//    DecisionOnHealCrit(nEffectType);

//    //计算治疗
//    f32 fHeal = GetPower();

//    using namespace AValue;
//		if ( (nEffectType & TriggerNotify_Crit) > 0 )
//		{
//			fHeal *= (1.5 + m_CasterAValue.Values[dhcr]);
//		}

//f32 fHHP_R = 0;
//		if (m_pSkillData->IsCalculationHHR())
//		{
//			f32 MIN_K_HHP_R = -1;
//f32 MAX_K_HHP_R = 2;
//fHHP_R = MIN(MAX((m_CasterAValue.Values[hhp_r] + m_TargetAValue.Values[be_hhp_r]), MIN_K_HHP_R), MAX_K_HHP_R);
//		}

//		fHeal *= m_CasterAValue.Values[ad_skill_r] * (1 + fHHP_R);

//		if (fHeal >= 1.0f)
//		{
//			nEffectValue = (int32)fHeal;
//			m_pTarget->SetHeal(m_pCaster->GetAvatarID(), nEffectValue, TRUE);
//		}
//	}

//	void GSkillEffectCalculation::CalculationShield(int32& nEffectType, int32& nEffectValue)
//{
//    if (!m_pSkillData || !m_pCaster || !m_pTarget)
//        return;

//    //计算护盾
//    using namespace AValue;
//		f32 fValue = m_CasterAValue.Values[ad_skill_p];

//fValue = fValue* MAX(0, 1 + m_CasterAValue.Values[hd_r] + m_TargetAValue.Values[be_hd_r]);

//		if (fValue >= 1.0f)
//		{
//			nEffectValue = (int32)fValue;
//		}
//	}

//	f32 GSkillEffectCalculation::GetPower()
//{
//    if (!m_pSkillData)
//        return 0;

//    using namespace AValue;
//		f32 fValue = m_CasterAValue.Values[ad_skill_p];
//		//计算攻击
//		if (m_pSkillData->IsCalculationAtk())
//		{
//			fValue += MAX(m_CasterAValue.Values[atk_d] * (1 + m_CasterAValue.Values[atk_d_r]), 1);
//		}
//		return fValue;
//	}

//	Galaxy::f32 GSkillEffectCalculation::GetCounterEffect(int32 nCasterCounter, int32 nTargetCounter)
//{
//    using namespace AValue;
//		int32 nCounterP = 0.3;
//int32 nCounterQ = 0.2;
//int32 nCounter = GSkillDataManager::Instance().GetCounter(nCasterCounter, nTargetCounter);
//		switch (nCounter)
//		{
//		case CounterEffect_P:				//克制
//			return 1.0 + nCounterP + m_CasterAValue.Values[counter_p];
//		case CounterEffect_Q:			//被克制
//			return 1.0 - nCounterQ - m_CasterAValue.Values[counter_q];
//		default: return 1.0;
//		}
//	}

//	void GSkillEffectCalculation::TransfromEffectTarget()
//{
//    if (!m_pSkillData || !m_pCaster || !m_pTarget)
//        return;

//    //转换技能效果
//    int32 nTransfromID = m_pSkillData->GetIntValue(MSV_EffectTransform);
//    GSkillEffectTransform* pTransform = GSkillDataManager::Instance().GetSkillEffectTransform(nTransfromID);
//    if (!pTransform)
//        return;

//    f32 fValue = 0.0f;
//    switch (pTransform->m_nTransformType)
//    {
//        case SkillEffectTransform_Target_AValue:            //施法属性集
//            {
//                int32 nValueID = AValueDefine::Instance().GetValueID(pTransform->m_sTransformName);
//                if (nValueID >= 0 && nValueID < AValue::Count)
//                {
//                    fValue = m_pTarget->GetRoleAValue().Values[nValueID];
//                }
//                break;
//            }
//        case SkillEffectTransform_Target_Param:         //施法参数集
//            {
//                if (m_pTarget->GetParamPool())
//                {
//                    fValue = m_pTarget->GetParamPool()->GetValue(pTransform->m_sTransformName.c_str(), 0.0f);
//                }
//                break;
//            }
//        case SkillEffectTransform_Target_Script:
//            {
//                char buff[64] = { 0 };
//                sprintf(buff, "OnSkillEffectTransfrom%d", m_pSkillData->GetSkillID());
//                luabridge::LuaRef ref = GLuaVM::GetMainLuaVM().getGlobal(buff);
//                if (!ref.isNil())
//			{
//                    try
//                    {
//                        fValue = ref(m_pCaster, m_pTarget).cast<f32>();
//                    }
//                    catch (luabridge::LuaException const&e)
//				{
//                        GalaxyLog::error("call OnSkillEffectTransfrom%d :%s", m_pSkillData->GetSkillID(), e.what());
//                    }
//                    }
//                    break;
//                }
//            }
//            m_CasterAValue.Values[AValue::ad_skill_p] += fValue * pTransform->m_fTransformPrecent;
//    }
//}