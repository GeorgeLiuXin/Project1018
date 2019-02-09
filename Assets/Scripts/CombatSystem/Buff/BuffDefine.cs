using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld
{
    public enum eBuffTypeFlag
    {
        BTF_SystemBuff = 1 << 0, //系统Buff
        BTF_TemplateBuff = 1 << 1, //模版Buff
        BTF_PublicBuff = 1 << 2, //公共Buff 不区分施放者
        BTF_PlusBuff = 1 << 3, //增益Buff
        BTF_DeBuff = 1 << 4, //减益Buff
        BTF_SkillBuff = 1 << 5, //技能附加Buff
        BTF_HaloBuff = 1 << 6, //光环Buff
    };
    //Buff规则
    public enum eBuffAttrFlag
    {
        BAF_SyncToClient = 1 << 0,  //同步客户端
        BAF_ReplaceAble = 1 << 1,   //可以替换
        BAF_StacksAble = 1 << 2,    //可以叠加
        BAF_StacksAddTime = 1 << 3, //叠加增加持续时间
        BAF_StacksResetTime = 1 << 4,   //叠加重置持续时间
        BAF_StacksUseDuration = 1 << 5, //叠加使用持续时间, 到时移除一层
        BAF_StacksAValue = 1 << 6,  //叠加属性集当前层
        BAF_StacksLayerAValue = 1 << 7, //叠加属性集所有层
        BAF_TickDuration = 1 << 8,  //TICK使用持续时间
        BAF_TickOffline = 1 << 9,   //TICK下线计时
        BAF_CancelAble = 1 << 10,   //可取消
        BAF_DispelAble = 1 << 11,   //可驱散
        BAF_CalculateAValue = 1 << 12,  //参与计算属性集
        BAF_CalculateAtOnce = 1 << 13, //立即计算属性集
        BAF_NotifyAdd = 1 << 14, //产生添加消息
        BAF_NotifyEnd = 1 << 15, //产生结束消息
        BAF_NotifyDispel = 1 << 16, //产生驱散消息
        BAF_NotifyStacks = 1 << 17, //产生叠加消息
        BAF_CalculateRoleValue = 1 << 18, //参与计算战斗属性集
        BAF_ScriptAdd = 1 << 19, //脚本调用添加Buff	
        BAF_ScriptStacks = 1 << 20, //脚本调用叠加Buff
        BAF_ScriptDispel = 1 << 21, //脚本调用驱散Buff
        BAF_ScriptRemove = 1 << 22, //脚本调用移除Buff
    };

    public enum eBuffRemoveFlag
    {
        BRF_RemoveOnLogin = 1 << 0, //下线则移除
        BRF_RemoveOnChangeScene = 1 << 1,   //跳转则移除
        BRF_RemoveOnDead = 1 << 2,  //死亡则移除
        BRF_RemoveOnAttack = 1 << 3,    //攻击则移除
        BRF_RemoveOnHurt = 1 << 4,  //受击则移除
        BRF_RemoveOnSpell = 1 << 5, //施法则移除
        BRF_RemoveOnEnterCombat = 1 << 6,  //进战则移除
        BRF_RemoveOnLeaveCombat = 1 << 7,  //脱战则移除
    };

    public enum eBuffState
    {
        BuffState_Stun = 1 << 0, //眩晕
        BuffState_Entangle = 1 << 1, //缠绕
        BuffState_Sleep = 1 << 2, //睡眠
        BuffState_Root = 1 << 3, //定身
        BuffState_Frozen = 1 << 4, //冰冻
        BuffState_FB_Move = 1 << 5, //禁用移动
        BuffState_FB_Skill = 1 << 6, //禁用技能
        BuffState_FB_Item = 1 << 7, //禁用物品
        BuffState_Taunt = 1 << 8, //嘲讽
    };

    public enum eBuffRemoveReason
    {

    };

}
