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
		private Dictionary<int, ActorObj> ActorDict;
		private Dictionary<int, ActorObj> ActorInstanceIDDict;
		private Dictionary<int, float> DelayRemoveList;
		private List<int> RemoveAvatarList = new List<int>();
		
		public int LocalPlayerID { get; set; }
		private LocalPlayer m_LocalPlayer;
		
		public override void InitManager()
		{
			ActorDict = new Dictionary<int, ActorObj>();
			ActorInstanceIDDict = new Dictionary<int, ActorObj>();
			DelayRemoveList = new Dictionary<int, float>();
			RegisterEvents();
		}

		public override void Update(float fElapseTimes)
		{
			List<ActorObj> actorList = new List<ActorObj>(ActorDict.Values);

			for (int i = 0; i < actorList.Count; i++)
			{
				actorList[i].Tick(fElapseTimes);
			}

			foreach (var key in new List<int>(DelayRemoveList.Keys))
			{
				DelayRemoveList[key] -= fElapseTimes;
				if (DelayRemoveList[key] <= 0.0f)
				{
					ActorObj actor = GetActorByID(key);
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
		
		public ActorObj CreateAvatar(int avatarType)
		{
			ActorObj actor = null;
			AvatarType type = (AvatarType) avatarType;
			switch (type)
			{
				case AvatarType.NPC_Player:
					actor = new NPCPlayer();
					break;
				case AvatarType.NPC_Enemy:
					actor = new NPCEnemy();
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
		public LocalPlayer GetLocalPlayer()
		{
			return m_LocalPlayer as LocalPlayer;
		}
		
		public void AddActor(ActorObj actor)
		{
			if (actor.AvatarID == -1)
				return;
			if (ActorDict.ContainsKey(actor.AvatarID))
				return;
			ActorDict[actor.AvatarID] = actor;
		}

		public void RemoveActor(ActorObj actor)
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
			ActorObj act = GetActorByID(nAvatarID);
			if (act != null)
			{
				if (bForceRemove)
				{
					RemoveActor(act);
				}
				else
				{
					ActorObj actObj = act as ActorObj;
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
			List<ActorObj> actorList = new List<ActorObj>(ActorDict.Values);
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

		public ActorObj GetActorByID(int clientID)
		{
			if (!ActorDict.ContainsKey(clientID))
				return null;

			return ActorDict[clientID];
		}

		public ActorObj GetByInstanceID(int avatarDID)
		{
			if (!ActorInstanceIDDict.ContainsKey(avatarDID))
				return null;

			return ActorInstanceIDDict[avatarDID];
		}

		public List<ActorObj> GetAllActor()
		{
			if (ActorDict.Values != null)
			{
				return new List<ActorObj>(ActorDict.Values);
			}
			return null;
		}
		
	}
}
