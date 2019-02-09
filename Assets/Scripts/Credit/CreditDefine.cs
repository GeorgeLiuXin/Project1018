using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
	public enum TargetType
	{
		ToEnemy = CreditLevelId.Credit_Hostile,
		ToNeutral = CreditLevelId.Credit_Neutrality,
		ToFriend = CreditLevelId.Credit_Friendly | CreditLevelId.Credit_Worship,
		ToEnemy_Neutral = ToEnemy | ToNeutral,
		ToFriend_Neutral = ToFriend | ToNeutral,
		ToAll = ToEnemy | ToNeutral | ToFriend,
	};
	public enum CreditLevelId
	{
		Credit_Safe = 0,			// 安全
		Credit_Hostile = 1,			// 敌方
		Credit_Neutrality = 2,		// 中立
		Credit_Friendly = 3,		// 友方
		Credit_Worship = 4,			// 崇拜
	};

	public enum CampType
	{
		Default,                    //默认
		Player = 1,                 //玩家标准
		Enemy = 11,                 //怪物
		Neutrality = 12,            //中立
		AbsolutelyFriendly = 20,    //绝对友好
	}

}
