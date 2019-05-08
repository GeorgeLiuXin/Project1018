using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
	//NPC基类
	public class NonPlayerCharacter : ActorEntity
	{
		public NonPlayerCharacter(int nAvatarID) : base(nAvatarID)
		{
		}

		public override int GetCamp()
		{
			return (int)CampType.Neutrality;
		}
	}

	//怪物/敌人
	public class EnemyEntity : NonPlayerCharacter
	{
		public EnemyEntity(int nAvatarID) : base(nAvatarID)
		{
		}

		public override int GetCamp()
		{
			return (int)CampType.Enemy;
		}

		public override GameEntityType GetEntityType()
		{
			return GameEntityType.Enemy;
		}
	}

}