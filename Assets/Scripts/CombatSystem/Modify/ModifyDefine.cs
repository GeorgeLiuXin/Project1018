using UnityEngine;
using System.Collections;

namespace XWorld
{
    enum eModifyType
    {
        SkillLevel = 1,
        SkillSlot = 2,
        SkillCustom = 3,
        BuffLevel = 4,
    };

    //修正数值逻辑
    public enum eModifyLogic
    {
        Null = 0,
        Sum, //加减
        REP, //替换
        NOR, //或非
    };

    //修正技能属性
    public enum eModifySkill
    {
        MSV_BaseSkillID = 0,                //父技能
        MSV_SkillType,                  //技能类型
        MSV_SkillAttr,                  //技能属性
        MSV_SpellLogic,                 //施法逻辑
        MSV_SpellParam1,                    //施法参数
        MSV_SpellParam2,                    //施法参数
        MSV_SpellParam3,                    //施法参数
        MSV_TarSeclect,                 //目标选择
        MSV_TarType,                        //目标类型
        MSV_SrcCheck,                   //自身检查
        MSV_TarCheck,                   //目标检查
        MSV_SlipDis,					//滑步距离
        MSV_Range,                      //技能范围
        MSV_Angle,                      //技能角度
        MSV_CDGroup,                    //CD组
        MSV_CDTime,                     //CD时间
        MSV_CostType1,                  //消耗类型
        MSV_CostValue1,                 //消耗值
        MSV_CostType2,                  //消耗类型
        MSV_CostValue2,                 //消耗值
        MSV_LockTime,                   //可打断时间
        MSV_LastTime,                   //技能持续时间	
        MSV_EffectLogic,                    //效果逻辑
        MSV_EffectParam1,               //效果参数1
        MSV_EffectParam2,               //效果参数2
        MSV_EffectParam3,               //效果参数3
        MSV_EffectType,                 //效果类型
        MSV_FirstEffectTime,			//首次生效时间
        MSV_EffectTime,                 //效果时间
        MSV_EffectCount,                    //效果次数
        MSV_EffectCalculation,          //效果结算
        MSV_EffectTransform,                //效果转换
        MSV_EffectBeatBack,             //击退效果
        MSV_BeatBackSrcCond,            //击退自身条件
        MSV_BeatBackTarCond,            //击退目标条件
        MSV_BeatBackType,               //击退类型
        MSV_BeatBackDir,                    //击退方向
        MSV_BeatBackOffsetDis,          //移动距离
        MSV_BeatBackFixedDis,           //保险距离
        MSV_BeatBackMoveTime,           //位移时间
        MSV_BeatBackLifeTime,           //击退时间
        MSV_LauncherLogic,              //发射逻辑
        MSV_LauncherParam1,             //发射参数
        MSV_LauncherParam2,             //发射参数
        MSV_LauncherParam3,             //发射参数
        MSV_AreaLogic,                  //范围逻辑
        MSV_AreaTarCheck,               //范围目标检查
        MSV_AreaTarCnt,                 //范围目标数量
        MSV_AreaFilter,					//范围筛选
        MSV_AreaParam1,                 //范围参数
        MSV_AreaParam2,                 //范围参数
        MSV_AreaParam3,                 //范围参数
        MSV_ProjectileLogic,                //子弹逻辑
        MSV_ProjectileParam1,           //子弹参数
        MSV_ProjectileParam2,           //子弹参数
        MSV_ProjectileParam3,           //子弹参数
        MSV_ProjectileOffset,             //子弹发射位置偏移
        MSV_ProjectileTime,             //子弹持续时间
        MSV_ProjectileSpeed,                //子弹速度
        MSV_ProjectileFirstEffectTime,		//子弹首次生效时间
        MSV_ProjectileEffectTime,       //子弹效果时间
        MSV_ProjectileEffectCount,      //子弹影响目标数量
        MSV_ProjectileEffectID,         //子弹特效ID
        MSV_ProjectileDieEffectID,      //子弹死亡特效ID
        MSV_ProjectileTimeOutEffectID,  //子弹消亡特效ID
        MSV_TriggerType,                    //触发类型
        MSV_TriggerNotify,              //触发消息
        MSV_TriggerCheck,               //触发检查	
        MSV_TriggerDataID,              //触发DataID
        MSV_TriggerValue,               //触发Value
        MSV_TriggerProbability,         //触发几率
        MSV_SkillMoveStartTime,         //位移开始时间
        MSV_SkillMoveEndTime,           //位移结束时间
        MSV_AnimID,                     //动画ID
        MSV_Priority,                       //优先级
        MSV_BindKey,                    //技能绑定按键
        MSV_GroupID,                    //技能组ID
        MSV_GroupOrder,                  //组内顺序
        MSV_CombatPerformanceID,		//技能施放方式
        MSV_CombatPerformanceTimes,		//客户端假伤害帧次数
        MSV_EndureLevel,                //硬直等级
        MSV_SkillDis,                   //技能距离
        MSV_WeaponID,                   //武器ID
        MSV_CombatWarningID,            //技能攻击预警ID
        MSV_Count,
    };

    public enum eModifyBuff
    {
        MBV_BuffType = 0,        //Buff类型
        MBV_BuffAttr,            //Buff属性
        MBV_BuffRemove,         //Buff移除
        MBV_BuffInummue,        //Buff免疫
        MBV_BuffCleanUp,        //Buff清除
        MBV_BuffCleanUpGroup,  //Buff清除组
        MBV_BuffState,           //Buff状态
        MBV_AttrValue,           //角色属性集
        MBV_LayerCnt,            //叠加层数
        MBV_DurationTime,       //持续时间
        MBV_BuffSkill,              //Buff技能
        MBV_BuffSkillLv,         //Buff技能等级
        MBV_BuffSkillUserData,  //Buff技能参数
        MBV_BuffLogic,          //Buff逻辑
        MBV_BuffLogicParam1,    //Buff逻辑参数1
        MBV_BuffLogicParam2,    //Buff逻辑参数2
        MBV_BuffLogicParam3,    //Buff逻辑参数3
        MBV_EffectID,           //特效ID
        MBV_EffectSurface,      //特效材质
        MBV_EffectAddID,        //生成特效ID
        MBV_EffectAddTime,      //生成特效持续时间
        MBV_EffectRemoveID, //消失特效ID
        MBV_EffectRemoveTime, //消失特效持续时间
        MBV_NameID,             //Name字典
        MBV_TipsID,             //Tips字典
        MBV_IconID,             //图标
        MBV_Count,
    };
}