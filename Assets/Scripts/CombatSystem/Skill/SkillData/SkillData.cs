using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace XWorld
{

    public class SkillData : ModifyObject<SkillData>
    {
        public static void InitSkillDefine()
        {
            if (m_ModifyDefine == null)
                m_ModifyDefine = new Dictionary<string, int>();

            if (m_ModifyValue == null)
                m_ModifyValue = new Dictionary<int, int>();

            if (m_ModifyLogic == null)
                m_ModifyLogic = new Dictionary<int, eModifyLogic>();

            foreach (eModifySkill _enum in Enum.GetValues(typeof(eModifySkill)))
            {
                m_ModifyDefine.Add(_enum.ToString(), (int)_enum);
            }

            SkillData data = new SkillData();
            m_DataFields = data.GetType().GetFields();
            for (int i = 0; i < m_DataFields.Length; ++i)
            {
                int _enum;
                if (m_ModifyDefine.TryGetValue(m_DataFields[i].Name, out _enum))
                {
                    m_ModifyValue[_enum] = i;
                }
            }

            m_ModifyLogic.Add((int)eModifySkill.MSV_BaseSkillID, eModifyLogic.Null); //父技能
            m_ModifyLogic.Add((int)eModifySkill.MSV_SkillType, eModifyLogic.NOR); //技能类型
            m_ModifyLogic.Add((int)eModifySkill.MSV_SkillAttr, eModifyLogic.NOR); //技能属性
            m_ModifyLogic.Add((int)eModifySkill.MSV_SpellLogic, eModifyLogic.Null); //施法逻辑
            m_ModifyLogic.Add((int)eModifySkill.MSV_SpellParam1, eModifyLogic.Sum); //施法参数
            m_ModifyLogic.Add((int)eModifySkill.MSV_SpellParam2, eModifyLogic.Sum); //施法参数
            m_ModifyLogic.Add((int)eModifySkill.MSV_SpellParam3, eModifyLogic.Sum); //施法参数
            m_ModifyLogic.Add((int)eModifySkill.MSV_TarSeclect, eModifyLogic.REP); //目标选择
            m_ModifyLogic.Add((int)eModifySkill.MSV_TarType, eModifyLogic.REP); //目标类型
            m_ModifyLogic.Add((int)eModifySkill.MSV_SrcCheck, eModifyLogic.REP); //自身检查
            m_ModifyLogic.Add((int)eModifySkill.MSV_TarCheck, eModifyLogic.REP); //目标检查
            m_ModifyLogic.Add((int)eModifySkill.MSV_SlipDis, eModifyLogic.REP); //滑步距离
            m_ModifyLogic.Add((int)eModifySkill.MSV_Range, eModifyLogic.Sum); //技能范围
            m_ModifyLogic.Add((int)eModifySkill.MSV_Angle, eModifyLogic.Sum); //技能角度
            m_ModifyLogic.Add((int)eModifySkill.MSV_CDGroup, eModifyLogic.REP); //CD组
            m_ModifyLogic.Add((int)eModifySkill.MSV_CDTime, eModifyLogic.Sum); //CD时间
            m_ModifyLogic.Add((int)eModifySkill.MSV_CostType1, eModifyLogic.REP); //消耗类型
            m_ModifyLogic.Add((int)eModifySkill.MSV_CostValue1, eModifyLogic.Sum); //消耗值
            m_ModifyLogic.Add((int)eModifySkill.MSV_CostType2, eModifyLogic.REP); //消耗类型
            m_ModifyLogic.Add((int)eModifySkill.MSV_CostValue2, eModifyLogic.Sum); //消耗值
            m_ModifyLogic.Add((int)eModifySkill.MSV_LockTime, eModifyLogic.Sum); //可打断时间
            m_ModifyLogic.Add((int)eModifySkill.MSV_LastTime, eModifyLogic.Sum); //技能持续时间
            m_ModifyLogic.Add((int)eModifySkill.MSV_EffectLogic, eModifyLogic.Null); //效果逻辑
            m_ModifyLogic.Add((int)eModifySkill.MSV_EffectParam1, eModifyLogic.Sum); //效果参数1
            m_ModifyLogic.Add((int)eModifySkill.MSV_EffectParam2, eModifyLogic.Sum); //效果参数2
            m_ModifyLogic.Add((int)eModifySkill.MSV_EffectParam3, eModifyLogic.Sum); //效果参数3
            m_ModifyLogic.Add((int)eModifySkill.MSV_EffectType, eModifyLogic.NOR); //效果类型
            m_ModifyLogic.Add((int)eModifySkill.MSV_FirstEffectTime, eModifyLogic.Sum); //首次生效时间
            m_ModifyLogic.Add((int)eModifySkill.MSV_EffectTime, eModifyLogic.Sum); //效果时间
            m_ModifyLogic.Add((int)eModifySkill.MSV_EffectCount, eModifyLogic.Sum); //效果次数	
            m_ModifyLogic.Add((int)eModifySkill.MSV_EffectCalculation, eModifyLogic.NOR); //效果结算
            m_ModifyLogic.Add((int)eModifySkill.MSV_EffectTransform, eModifyLogic.REP); //效果转换
            m_ModifyLogic.Add((int)eModifySkill.MSV_EffectBeatBack, eModifyLogic.REP); //击退效果
            m_ModifyLogic.Add((int)eModifySkill.MSV_BeatBackSrcCond, eModifyLogic.REP); //击退自身条件
            m_ModifyLogic.Add((int)eModifySkill.MSV_BeatBackTarCond, eModifyLogic.REP); //击退目标条件
            m_ModifyLogic.Add((int)eModifySkill.MSV_BeatBackType, eModifyLogic.REP); //击退类型
            m_ModifyLogic.Add((int)eModifySkill.MSV_BeatBackDir, eModifyLogic.REP); //击退方向
            m_ModifyLogic.Add((int)eModifySkill.MSV_BeatBackOffsetDis, eModifyLogic.Sum); //移动距离
            m_ModifyLogic.Add((int)eModifySkill.MSV_BeatBackFixedDis, eModifyLogic.Sum); //保险距离
            m_ModifyLogic.Add((int)eModifySkill.MSV_BeatBackMoveTime, eModifyLogic.Sum); //位移时间
            m_ModifyLogic.Add((int)eModifySkill.MSV_BeatBackLifeTime, eModifyLogic.Sum); //动画时间
            m_ModifyLogic.Add((int)eModifySkill.MSV_LauncherLogic, eModifyLogic.REP); //发射逻辑
            m_ModifyLogic.Add((int)eModifySkill.MSV_LauncherParam1, eModifyLogic.Sum); //发射参数
            m_ModifyLogic.Add((int)eModifySkill.MSV_LauncherParam2, eModifyLogic.Sum); //发射参数
            m_ModifyLogic.Add((int)eModifySkill.MSV_LauncherParam3, eModifyLogic.Sum); //发射参数
            m_ModifyLogic.Add((int)eModifySkill.MSV_ProjectileOffset, eModifyLogic.REP); //发射参数
            m_ModifyLogic.Add((int)eModifySkill.MSV_AreaLogic, eModifyLogic.REP); //范围逻辑
            m_ModifyLogic.Add((int)eModifySkill.MSV_AreaParam1, eModifyLogic.Sum); //范围参数
            m_ModifyLogic.Add((int)eModifySkill.MSV_AreaParam2, eModifyLogic.Sum); //范围参数
            m_ModifyLogic.Add((int)eModifySkill.MSV_AreaParam3, eModifyLogic.Sum); //范围参数
            m_ModifyLogic.Add((int)eModifySkill.MSV_AreaTarCheck, eModifyLogic.REP); //范围目标检查
            m_ModifyLogic.Add((int)eModifySkill.MSV_AreaTarCnt, eModifyLogic.Sum); //范围目标数量
            m_ModifyLogic.Add((int)eModifySkill.MSV_AreaFilter, eModifyLogic.REP); //范围筛选
            m_ModifyLogic.Add((int)eModifySkill.MSV_ProjectileLogic, eModifyLogic.Null); //子弹逻辑
            m_ModifyLogic.Add((int)eModifySkill.MSV_ProjectileParam1, eModifyLogic.Sum); //子弹参数
            m_ModifyLogic.Add((int)eModifySkill.MSV_ProjectileParam2, eModifyLogic.Sum); //子弹参数
            m_ModifyLogic.Add((int)eModifySkill.MSV_ProjectileParam3, eModifyLogic.Sum); //子弹参数
            m_ModifyLogic.Add((int)eModifySkill.MSV_ProjectileTime, eModifyLogic.Sum); //子弹持续时间
            m_ModifyLogic.Add((int)eModifySkill.MSV_ProjectileSpeed, eModifyLogic.Sum); //子弹速度
            m_ModifyLogic.Add((int)eModifySkill.MSV_ProjectileFirstEffectTime, eModifyLogic.Sum); //子弹首次生效时间
            m_ModifyLogic.Add((int)eModifySkill.MSV_ProjectileEffectTime, eModifyLogic.Sum); //子弹效果时间
            m_ModifyLogic.Add((int)eModifySkill.MSV_ProjectileEffectCount, eModifyLogic.Sum); //子弹影响目标数量
            m_ModifyLogic.Add((int)eModifySkill.MSV_ProjectileEffectID, eModifyLogic.REP); //子弹特效ID
            m_ModifyLogic.Add((int)eModifySkill.MSV_ProjectileDieEffectID, eModifyLogic.REP); //子弹死亡特效ID
            m_ModifyLogic.Add((int)eModifySkill.MSV_ProjectileTimeOutEffectID, eModifyLogic.REP); //子弹死亡特效ID
            m_ModifyLogic.Add((int)eModifySkill.MSV_TriggerType, eModifyLogic.Null); //触发类型
            m_ModifyLogic.Add((int)eModifySkill.MSV_TriggerNotify, eModifyLogic.REP); //触发消息
            m_ModifyLogic.Add((int)eModifySkill.MSV_TriggerCheck, eModifyLogic.REP); //触发检查
            m_ModifyLogic.Add((int)eModifySkill.MSV_TriggerDataID, eModifyLogic.REP); //触发DataID
            m_ModifyLogic.Add((int)eModifySkill.MSV_TriggerValue, eModifyLogic.Sum); //触发Value
            m_ModifyLogic.Add((int)eModifySkill.MSV_TriggerProbability, eModifyLogic.Sum); //触发几率
            m_ModifyLogic.Add((int)eModifySkill.MSV_AnimID, eModifyLogic.REP); //动画ID
            m_ModifyLogic.Add((int)eModifySkill.MSV_Priority, eModifyLogic.REP); //优先级
            m_ModifyLogic.Add((int)eModifySkill.MSV_CombatPerformanceID, eModifyLogic.REP); //客户端战斗表现
            m_ModifyLogic.Add((int)eModifySkill.MSV_CombatPerformanceTimes, eModifyLogic.REP); //客户端假伤害帧次数
            m_ModifyLogic.Add((int)eModifySkill.MSV_EndureLevel, eModifyLogic.REP); //硬直等级
            m_ModifyLogic.Add((int)eModifySkill.MSV_CombatWarningID, eModifyLogic.REP); //技能攻击预警ID
            m_ModifyLogic.Add((int)eModifySkill.MSV_SkillMoveStartTime, eModifyLogic.REP); //技能移动开始时间
            m_ModifyLogic.Add((int)eModifySkill.MSV_SkillMoveEndTime, eModifyLogic.REP); //技能移动结束时间
        }

        public override void OnLoadData(ConfigData data)
        {
            base.OnLoadData(data);
            SkillID = data.GetInt("SkillID");
            SkillNameID = data.GetInt("SkillNameID");
            MSV_WeaponID = data.GetInt("MSV_WeaponID");
            MSV_ProjectileFly1EffectID = data.GetInt("MSV_ProjectileFly1EffectID");
            MSV_ProjectileFly2EffectID = data.GetInt("MSV_ProjectileFly2EffectID");
        }

        public override int DataID
        {
            get { return SkillID; }
        }

        public void UpdateSlots(int nSlots, bool bReset)
        {
            if (m_nSlots == nSlots)
                return;

            if (bReset)
            {
                SkillData skillData = XWorldGameModule.GetGameManager<ModifyDataManager>().GetSkillData(DataID);
                if (skillData != null)
                    Clone(skillData, this);
                else
                    return;
            }

            m_nSlots = nSlots;
            if ((m_nSlotsMask & m_nSlots) <= 0)
                return;

            ModifyData modifyData = XWorldGameModule.GetGameManager<ModifyDataManager>().GetSkillSlotsData(DataID);
            if (modifyData == null)
                return;

            for (int i = 0; i < 32; ++i)
            {
                int _slot = 1 << i;
                if (_slot > m_nSlots)
                    break;

                if ((m_nSlots & _slot) <= 0)
                    continue;

                ModifyList modifyList = modifyData.GetModifyList(_slot);
                if (modifyList != null)
                {
                    Combine(modifyList);
                }
            }
        }

        public SkillData Clone(int nSlots)
        {
            SkillData skillData = new SkillData();
            if (skillData != null)
                skillData = this.Clone() as SkillData;
            else
                return null;

            skillData.UpdateSlots(nSlots, false);
            return skillData;
        }

        public static void Clone(SkillData srcData, SkillData dirData)
        {
            if (srcData != null && dirData != null)
            {
                dirData = srcData.Clone() as SkillData;
            }
        }
        //技能类型检查
        public bool CheckSkillType(int nSkillType) { return (MSV_SkillType & nSkillType) > 0; }
        public bool IsTemplateSkill() { return CheckSkillType((int)eSkillType.SkillType_Template); }
        public bool IsActiveSkill() { return CheckSkillType((int)eSkillType.SkillType_Active); }
        public bool IsTriggerSkill() { return CheckSkillType((int)eSkillType.SkillType_Trigger); }
        public bool IsBuffSkill() { return CheckSkillType((int)eSkillType.SkillType_Buff); }
        public bool IsPassiveSkill() { return CheckSkillType((int)eSkillType.SkillType_Passive) || IsTriggerSkill() || IsBuffSkill(); }

        //技能属性检查
        public bool CheckSkillAttr(int nSkillAttr) { return (MSV_SkillAttr & nSkillAttr) > 0; }

        public bool IsEffectStateCost() { return CheckSkillAttr((int)eSkillAttr.SkillAttr_EffectStateCost); } //效果阶段产生消耗
        public bool IsAreaUseTarPos() { return CheckSkillAttr((int)eSkillAttr.SkillAttr_AreaUseTarPos); } //范围效果使用目标坐标
        public bool IsAreaIncludeSelf() { return CheckSkillAttr((int)eSkillAttr.SkillAttr_AreaIncludeSelf); } //范围效果包含自身
        public bool IsAreaAddExclude() { return CheckSkillAttr((int)eSkillAttr.SkillAttr_AreaAddExclude); } //范围效果排除重复目标
        public bool IsTriggerCommonCD() { return CheckSkillAttr((int)eSkillAttr.SkillAttr_TriggerCommonCD); } //触发共CD
        public bool IsTriggerRemoveBuff() { return CheckSkillAttr((int)eSkillAttr.SkillAttr_TriggerRemoveBuff); } //触发后移除Buff
        public bool IsTriggerRemoveLayer() { return CheckSkillAttr((int)eSkillAttr.SkillAttr_TriggerRemoveLayer); } //触发后减少Buff层数
        public bool IsTriggerTriggerNotify() { return CheckSkillAttr((int)eSkillAttr.SkillAttr_TriggerNotify); } //触发后产生触发事件
        public bool IsTriggerSkillNotify() { return CheckSkillAttr((int)eSkillAttr.SkillAttr_SkillNotify); } //产生技能施放事件
        public bool IsBulletPeriodEffect() { return CheckSkillAttr((int)eSkillAttr.SkillAttr_BulletPeriodEffect); } //子弹产生周期效果
        public bool IsBulletHitEffect() { return CheckSkillAttr((int)eSkillAttr.SkillAttr_BulletHitEffect); } //子弹产生命中效果
        public bool IsBulletHitNoRemove() { return CheckSkillAttr((int)eSkillAttr.SkillAttr_BulletHitNoRemove); } //子弹命中移除
        public bool IsBulletNotify() { return CheckSkillAttr((int)eSkillAttr.SkillAttr_BulletNotify); } //子弹产生事件
        public bool IsBulletBornTarPos() { return CheckSkillAttr((int)eSkillAttr.SkillAttr_BulletBornTarPos); } //子弹出生在目标点
        public bool IsMoveSkill() { return CheckSkillAttr((int)eSkillAttr.SkillAttr_MoveSkill); } //使用时可以移动

        //技能目标
        public bool CheckTarget(int nType) { return (MSV_TarType & nType) > 0; }
        public bool IsTargetSelf() { return CheckTarget((int)eSkillTargetType.TargetType_Self); } //是否对自己使用
        public bool IsTargetOhterFriend() { return CheckTarget((int)eSkillTargetType.TargetType_OtherFriend); } //是否对其他友方使用
        public bool IsTargetOhterEnemy() { return CheckTarget((int)eSkillTargetType.TargetType_OtherEnemy); } //是否对其他敌人使用
        public bool IsTargetOther() { return IsTargetOhterFriend() || IsTargetOhterEnemy(); } //是否对其他使用
        public bool IsTargetAvatar() { return IsTargetSelf() || IsTargetOther(); } //是否对角色使用
        public bool IsTargetSelfOnly() { return IsTargetSelf() && !IsTargetOther(); } //是否仅对自己使用
        public bool IsTargetPos() { return CheckTarget((int)eSkillTargetType.TargetType_Pos); }//对坐标使用
        public bool IsTargetDir() { return CheckTarget((int)eSkillTargetType.TargetType_Dir); }//对朝向使用

        //技能结算检查
        public bool CheckSkillCalculation(int nState) { return (MSV_EffectCalculation & nState) > 0; }
        public bool IsCalculationHit() { return CheckSkillCalculation((int)eSkillCalculation.SkillCalculation_Hit); } //计算命中
        public bool IsCalculationAtk() { return CheckSkillCalculation((int)eSkillCalculation.SkillCalculation_Atk); } //计算攻击
        public bool IsCalculationAC() { return CheckSkillCalculation((int)eSkillCalculation.SkillCalculation_AC); } //计算护甲
        public bool IsCalculationCrit() { return CheckSkillCalculation((int)eSkillCalculation.SkillCalculation_Crit); } //计算暴击
        public bool IsCalculationDR() { return CheckSkillCalculation((int)eSkillCalculation.SkillCalculation_DR); } //计算伤增/伤减
        public bool IsCalculationDM() { return CheckSkillCalculation((int)eSkillCalculation.SkillCalculation_DM); } //计算附加/吸收
        public bool IsCalculationHHR() { return CheckSkillCalculation((int)eSkillCalculation.SkillCalculation_HHR); } //计算治疗/承受治疗
        public bool IsCalculationEx() { return CheckSkillCalculation((int)eSkillCalculation.SkillCalculation_Ex); } //计算特殊效果

        #region Property
        public int SkillID = 0;

        public int SkillNameID = 0;

        public int MSV_SrcCheck = 0;

        public int MSV_TarCheck = 0;

        public int MSV_SkillType = 0;

        public int MSV_SkillAttr = 0;

        public int MSV_BaseSkillID = 0;

        public int MSV_TriggerType = 0;

        public int MSV_TriggerCheck = 0;

        public int MSV_TriggerNotify = 0;

        public int MSV_TriggerDataID = 0;

        public int MSV_TriggerValue = 0;

        public int MSV_TriggerProbability = 0;

        public int MSV_SpellLogic = 0;

        public int MSV_SpellParam1 = 0;

        public int MSV_SpellParam2 = 0;

        public int MSV_SpellParam3 = 0;

        public int MSV_CastType = 0;

        public int MSV_TarType = 0;

        public float MSV_Range = 0;

        public float MSV_SlipDis = 0;

        public int MSV_DodgeTime = 0;

        public int MSV_LockTime = 0;

        public int MSV_LastTime = 0;

        public int MSV_CDGroup = 0;

        public int MSV_CDTime = 0;

        public int MSV_CostType1 = 0;

        public int MSV_CostValue1 = 0;

        public int MSV_CostType2 = 0;

        public int MSV_CostValue2 = 0;

        public int MSV_LauncherLogic = 0;

        public int MSV_LauncherParam1 = 0;

        public int MSV_LauncherParam2 = 0;

        public int MSV_LauncherParam3 = 0;

        public int MSV_ProjectileLogic = 0;

        public int MSV_ProjectileTime = 0;

        public float MSV_ProjectileSpeed = 0;

        public int MSV_ProjectileFirstEffectTime = 0;

        public int MSV_ProjectileEffectTime = 0;

        public int MSV_ProjectileEffectCount = 0;

        public int MSV_ProjectileParam1 = 0;

        public int MSV_ProjectileParam2 = 0;

        public int MSV_ProjectileParam3 = 0;

        public int MSV_ProjectileOffset = 0;

        public int MSV_ProjectileEffectID = 0;

        public int MSV_ProjectileDieEffectID = 0;

        public int MSV_ProjectileTimeOutEffectID = 0;

        public int MSV_ProjectileFly1EffectID = 0;

        public int MSV_ProjectileFly2EffectID = 0;

        public int MSV_AreaLogic = 0;

        public int MSV_AreaTarCheck = 0;

        public int MSV_AreaFilter = 0;

        public int MSV_AreaTarCnt = 0;

        public int MSV_AreaParam1 = 0;

        public int MSV_AreaParam2 = 0;

        public int MSV_AreaParam3 = 0;

        public int MSV_FirstEffectTime = 0;

        public int MSV_EffectTime = 0;

        public int MSV_EffectCount = 0;

        public int MSV_EffectType = 0;

        public int MSV_EffectLogic = 0;

        public int MSV_EffectParam1 = 0;

        public int MSV_EffectParam2 = 0;

        public int MSV_EffectParam3 = 0;

        public int MSV_EffectCalculation = 0;

        public int MSV_EffectTransform = 0;

        public int MSV_EffectBeatBack = 0;

        public int MSV_BeaBackSrcCond = 0;

        public int MSV_BeatBackTarCond = 0;

        public int MSV_BeatBackType = 0;

        public int MSV_BeatBackDir = 0;

        public float MSV_BeatBackOffsetDis = 0;

        public float MSV_BeatBackFixedDis = 0;

        public int MSV_BeatBackMoveTime = 0;

        public int MSV_BeatBackLifeTime = 0;

        public float MSV_SkillDis = 0;

        public int MSV_SkillMoveDir = 0;

        public int MSV_SkillMoveStartTime = 0;

        public int MSV_SkillMoveEndTime = 0;

        public int MSV_AnimID = 0;

        public int MSV_WeaponID = 0;

        public int MSV_CombatPerformanceID = 0;

        public int MSV_CombatPerformanceTimes = 0;

        public int MSV_EndureLevel = 0;

        public int MSV_CombatWarningID = 0;
        #endregion
    }

}
