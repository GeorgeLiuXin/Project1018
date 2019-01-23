using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{

    public enum TargetType
    {
        ToEnemy = 1 << 0,
        ToNeutral = 1 << 1,
        ToFriend = 1 << 2,
        ToEnemy_Neutral = ToEnemy | ToNeutral,
        ToFriend_Neutral = ToFriend | ToNeutral,
        ToAll = ToEnemy | ToNeutral | ToFriend,
    };
    public enum CreditLevelId
    {
        Credit_Safe = 0, // 安全
        Credit_Hostile = 1, // 敌方
        Credit_Neutrality = 2, // 冷漠
        Credit_Friendly = 3, // 中立
        Credit_Friendly1 = 4, // 友方
        Credit_Friendly2 = 5, // 敬重
        Credit_Friendly3 = 6, // 尊敬
        Credit_Friendly4 = 7, // 崇敬
        Credit_Worship = 8, // 崇拜
    };

    public enum CampType
    {
        Default,
        Player,
        Enemy,
        Neutrality,
    }

}
