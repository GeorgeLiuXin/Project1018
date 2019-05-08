using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
	//玩家实体
	public class PlayerEntity : ActorEntity
	{
		public PlayerEntity(int nAvatarID) : base(nAvatarID)
		{
		}

		public override int GetCamp()
		{
			return (int)CampType.Player;
		}

		public override GameEntityType GetEntityType()
		{
			return GameEntityType.Player;
		}
	}

}