using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace XWorld
{
	public enum AvatarType
	{
		NPC_Player,
		NPC_Enemy,
	}

	public class ActorManager : XWorldGameManagerBase
    {
		private Dictionary<int, ActorEntity> ActorDict;
		private Dictionary<int, ActorEntity> ActorInstanceIDDict;
		private Dictionary<int, float> DelayRemoveList;
		private List<int> RemoveAvatarList = new List<int>();
		
		public int LocalPlayerID { get; set; }
		private PlayerEntity m_LocalPlayer;
		
		public override void InitManager()
		{
			ActorDict = new Dictionary<int, ActorEntity>();
			ActorInstanceIDDict = new Dictionary<int, ActorEntity>();
			DelayRemoveList = new Dictionary<int, float>();
			RegisterEvents();
		}

		public override void Update(float fElapseTimes)
		{
			List<ActorEntity> actorList = new List<ActorEntity>(ActorDict.Values);

			for (int i = 0; i < actorList.Count; i++)
			{
				actorList[i].Tick(fElapseTimes);
			}

			foreach (var key in new List<int>(DelayRemoveList.Keys))
			{
				DelayRemoveList[key] -= fElapseTimes;
				if (DelayRemoveList[key] <= 0.0f)
				{
					ActorEntity actor = GetActorByID(key);
					if (actor != null)
					{
						RemoveActor(actor);
						DelayRemoveList.Remove(key);
					}
				}
			}
		}

		public override void ShutDown()
		{
			ActorDict.Clear();
			ActorInstanceIDDict.Clear();
			UnRegisterEvents();
		}

		private void RegisterEvents()
		{
			EventListener.Instance.AddListener(CltEvent.Actor.INIT_AVATAR_RES, OnInitAvatarRes);
			EventListener.Instance.AddListener(CltEvent.Actor.UPDATE_AVATAR_RES, OnUpdateAvatarRes);
			EventListener.Instance.AddListener(CltEvent.Actor.REMOVE_AVATAR_RES, OnRemoveAvatarRes);
		}

		private void UnRegisterEvents()
		{
			EventListener.Instance.RemoveListener(CltEvent.Actor.INIT_AVATAR_RES, OnInitAvatarRes);
			EventListener.Instance.RemoveListener(CltEvent.Actor.UPDATE_AVATAR_RES, OnUpdateAvatarRes);
			EventListener.Instance.RemoveListener(CltEvent.Actor.REMOVE_AVATAR_RES, OnRemoveAvatarRes);
		}
		
		private void OnInitAvatarRes(object[] values)
		{
			//int avatarID = (int)values[0];
			//int avatarType = (int) values[1];
			//ActorObj actor = GetActorByID(avatarID);
			//if (actor == null)
			//{
			//	actor = CreateAvatar(avatarType);
			//	actor.AvatarID = avatarID;
			//	AddActor(actor);
			//}
			
			//actor.UpdataFromPacketBuffer(buffer, bFinish, true);
		}

		private void OnUpdateAvatarRes(object[] values)
		{
			//int avatarID = pkt.GetInt32("SrcAvatarID");
			//int paramType = pkt.GetInt32("paramType");
			//GalaxyActor actor = GetByServerID(avatarID);
			//if (actor == null)
			//{
			//	actor = CreateAvatar(paramType);
			//	actor.ServerID = avatarID;
			//	AddActor(actor);
			//	return;
			//}

			//actor.UpdataFromPacketBuffer(buffer, bFinish, false);
		}

		private void OnRemoveAvatarRes(object[] values)
		{

		}
		
		public ActorEntity CreateAvatar(int avatarType)
		{
			ActorEntity actor = null;
			AvatarType type = (AvatarType) avatarType;
			switch (type)
			{
				case AvatarType.NPC_Player:
					actor = new PlayerEntity();
					break;
				case AvatarType.NPC_Enemy:
					actor = new EnemyEntity();
					break;
				default:
					break;
			}
			return actor;
		}

		public bool IsLocalPlayer(int avatarID)
		{
			return (m_LocalPlayer.AvatarID == avatarID) ? true : false;
		}
		public PlayerEntity GetLocalPlayer()
		{
			return m_LocalPlayer as PlayerEntity;
		}
		
		public void AddActor(ActorEntity actor)
		{
			if (actor.AvatarID == -1)
				return;
			if (ActorDict.ContainsKey(actor.AvatarID))
				return;
			ActorDict[actor.AvatarID] = actor;
		}

		public void RemoveActor(ActorEntity actor)
		{
			if (actor.AvatarID == -1)
				return;
			if (!ActorDict.ContainsKey(actor.AvatarID))
				return;

			//XTODO 丢弃资源
			//actor.UnloadRes();
			if (!ActorDict.ContainsKey(actor.AvatarID))
				return;
			ActorDict.Remove(actor.AvatarID);
		}

		public void RemoveActorWithDelay(int nAvatarID, float fDelayTime)
		{
			float fTime;
			if (!DelayRemoveList.TryGetValue(nAvatarID, out fTime))
			{
				DelayRemoveList.Add(nAvatarID, fDelayTime);
			}
		}

		public void RemoveByServerID(int nAvatarID, bool bForceRemove)
		{
			ActorEntity act = GetActorByID(nAvatarID);
			if (act != null)
			{
				if (bForceRemove)
				{
					RemoveActor(act);
				}
				else
				{
					ActorEntity actObj = act as ActorEntity;
					if (null != actObj)
					{
						//XTODO 死亡移除
						//if (actObj.CheckState((int) eState.State_Dead) == false)
						//{
						//	RemoveActor(act);
						//}
						//else
						//{
						//	RemoveActorWithDelay(nAvatarID, actObj.GetRemoveDelayTime());
						//}
					}
				}
			}
		}
		public void RemoveAll(bool bExceptLocalPlayer)
		{
			List<ActorEntity> actorList = new List<ActorEntity>(ActorDict.Values);
			for (int i = 0; i < actorList.Count; i++)
			{
				if (actorList[i] != null)
				{
					if (bExceptLocalPlayer && IsLocalPlayer(actorList[i].AvatarID))
					{
						continue;
					}
					RemoveByServerID(actorList[i].AvatarID, true);
				}
			}
			DelayRemoveList.Clear();
		}

		public ActorEntity GetActorByID(int clientID)
		{
			if (!ActorDict.ContainsKey(clientID))
				return null;

			return ActorDict[clientID];
		}

		public ActorEntity GetByInstanceID(int avatarDID)
		{
			if (!ActorInstanceIDDict.ContainsKey(avatarDID))
				return null;

			return ActorInstanceIDDict[avatarDID];
		}

		public List<ActorEntity> GetAllActor()
		{
			if (ActorDict.Values != null)
			{
				return new List<ActorEntity>(ActorDict.Values);
			}
			return null;
		}
		
	}
}
