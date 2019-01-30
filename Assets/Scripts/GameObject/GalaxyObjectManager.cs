using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace XWorld
{
    public class ActorManager : XWorldGameManagerBase
    {
		private Dictionary<int, ActorObj> ActorDict;
		private Dictionary<int, ActorObj> ActorInstanceIDDict;
		private Dictionary<int, float> DelayRemoveList;
		private List<int> RemoveAvatarList = new List<int>();
		
		public int LocalPlayerDID { get; set; }
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
			GPacketBase pkt = values[0] as GPacketBase;
			if (pkt == null)
				return;

			InitAvatarData(pkt);
		}

		private void OnUpdateAvatarRes(object[] values)
		{
			GPacketBase pkt = values[0] as GPacketBase;
			if (pkt == null)
				return;

			UpdateAvatarData(pkt);
		}

		private void OnRemoveAvatarRes(object[] values)
		{
			GPacketBase pkt = values[0] as GPacketBase;
			if (pkt == null)
				return;

			PlayerUserData(pkt);
		}

		public void InitAvatarData(GPacketBase pkt)
		{
			int avatarID = pkt.GetInt32("SrcAvatarID");
			int paramType = pkt.GetInt32("paramType");
			GalaxyActor actor = GetByServerID(avatarID);
			if (actor == null)
			{
				actor = CreateAvatar(paramType);
				actor.ServerID = avatarID;
				AddActor(actor);
			}

			byte[] buffer = new byte[pkt.mBuffSize];
			pkt.ReadBuffer(buffer, pkt.mBuffSize);
			bool bFinish = false;
			byte paramFlag = pkt.GetByte("m_ParamFlag");
			if ((paramFlag & (byte) eParamPool.LastPacket) == (byte) eParamPool.LastPacket)
			{
				bFinish = true;
			}
			actor.UpdataFromPacketBuffer(buffer, bFinish, true);
		}

		public void UpdateAvatarData(GPacketBase pkt)
		{
			int avatarID = pkt.GetInt32("SrcAvatarID");
			int paramType = pkt.GetInt32("paramType");
			GalaxyActor actor = GetByServerID(avatarID);
			if (actor == null)
			{
				//actor = CreateAvatar(paramType);
				//actor.ServerID = avatarID;
				//AddActor(actor);
				return;
			}

			byte[] buffer = new byte[pkt.mBuffSize];
			pkt.ReadBuffer(buffer, pkt.mBuffSize);
			bool bFinish = false;
			byte paramFlag = pkt.GetByte("m_ParamFlag");
			if ((paramFlag & (byte) eParamPool.LastPacket) == (byte) eParamPool.LastPacket)
			{
				bFinish = true;
			}
			actor.UpdataFromPacketBuffer(buffer, bFinish, false);
		}

		public GalaxyActor CreateAvatar(int paramType)
		{
			ParamType paramId = ParamPool.GetParamType(paramType);
			GalaxyActor actor = null;
			if (paramId == ParamType.Param_avatar)
			{
				actor = new NetPlayer();
			}
			else if (paramId == ParamType.Param_monster)
			{
				actor = new NetNPC();
			}
			if (paramType != -1)
			{
				ParamPool pool = ParamPoolDefine.CreateParamPool(paramType);
				actor.SetParamPool(pool);
			}
			return actor;
		}

		public bool IsLocalPlayer(int avatarID)
		{
			return (m_LocalPlayer.m_AvatarID == avatarID) ? true : false;
		}
		public LocalPlayer GetLocalPlayer()
		{
			return m_LocalPlayer as LocalPlayer;
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

		public void UpdateClientID(int serverID, int clientID)
		{
			if (ActorDict.ContainsKey(clientID))
				return;
			ActorObj actor = GetActorByID(serverID);
			if (actor == null)
				return;
			if (ActorDict.ContainsKey(actor.ClientID))
			{
				ActorDict.Remove(actor.ClientID);
			}
			actor.ClientID = clientID;
			ActorDict[clientID] = actor;
		}

		public void AddActor(ActorObj actor)
		{
			if (actor.m_AvatarID == -1)
				return;
			if (ActorDict.ContainsKey(actor.m_AvatarID))
				return;
			ActorDict[actor.m_AvatarID] = actor;
		}

		public void RemoveActor(ActorObj actor)
		{
			if (actor.m_AvatarID == -1)
				return;
			if (!ActorDict.ContainsKey(actor.m_AvatarID))
				return;

			actor.UnloadRes();
			if (!ActorDict.ContainsKey(actor.m_AvatarID))
				return;
			ActorDict.Remove(actor.m_AvatarID);
		}

		public void RemoveActorWithDelay(int nServerID, float fDelayTime)
		{
			float fTime;
			if (!DelayRemoveList.TryGetValue(nServerID, out fTime))
			{
				DelayRemoveList.Add(nServerID, fDelayTime);
			}
		}

		public void RemoveByServerID(int ServerID, bool bForceRemove)
		{
			ActorObj act = GetByServerID(ServerID);
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
						if (actObj.CheckState((int) eState.State_Dead) == false)
						{
							RemoveActor(act);
						}
						else
						{
							RemoveActorWithDelay(ServerID, actObj.GetRemoveDelayTime());
						}
					}
				}
			}
		}
		public void RemoveAll(bool bExceptLocalPlayer)
		{
			List<ActorObj> actorList = new List<ActorObj>(ServerIDActorManager.Values);
			for (int i = 0; i < actorList.Count; i++)
			{
				if (actorList[i] != null)
				{
					if (bExceptLocalPlayer && IsLocalPlayer(actorList[i].ServerID))
					{
						continue;
					}
					RemoveByServerID(actorList[i].ServerID, true);
				}
			}
			DelayRemoveList.Clear();
		}

		public void UpdateAOIObjView(int AvatarID, bool bInView)
		{
			if (AvatarID == m_LocalPlayer.ServerID)
				return;

			if (bInView)
			{
				ActorObj act = GetByServerID(AvatarID);
				if (act == null)
				{
					//向服务器拽玩家数据
					GPacketBase pkt = m_packetDefineMgr.CreatePacket("GPacketClientAvatarDataRequest");
					if (pkt == null)
						return;
					pkt.SetInt32("TarAvatarID", AvatarID);
					GalaxyNetManager.Instance.SendPacket(pkt);
				}
			}
			else
			{
				RemoveAvatarList.Add(AvatarID);
			}
		}
		
		public void PlayerUserData(GPacketBase pkt)
		{
			int idx = pkt.GetInt32("index");
			int paramtype = pkt.GetInt32("m_ParamType");
			LocalPlayer player = null;
			if (LocalPlayerList.ContainsKey(idx))
			{
				player = LocalPlayerList[idx];
			}
			else
			{
				if (idx != GetFirstEmptyIndex())
					return;
				player = new LocalPlayer();
				if (player == null)
					return;
				ParamPool pool = ParamPoolDefine.CreateParamPool((int) ParamType.Param_avatar, ParamPool.GetParamDataID(paramtype));
				player.SetParamPool(pool);
				player.ServerID = LocalPlayerServerID;
				LocalPlayerList.Add(idx, player);
			}
			byte flag = pkt.GetByte("m_ParamFlag");
			int size = pkt.mBuffSize;
			byte[] buff = new byte[size];
			pkt.ReadBuffer(buff, size);
			player.Pool.Read(buff);
		}
		
		public bool ContainsAvatar(int id)
		{
			if (LocalPlayerList.ContainsKey(id))
				return true;
			else
				return false;
		}

		public void SelectAvatar(int id)
		{
			if (ContainsAvatar(id))
			{
				m_LocalPlayer = LocalPlayerList[id];
				AddActor(m_LocalPlayer);
				LocalPlayerList.Clear();
			}
		}
		
		public void ClearActorObjSelected()
		{
			List<ActorObj> actorList = new List<ActorObj>(ServerIDActorManager.Values);

			for (int i = 0; i < actorList.Count; i++)
			{
				if (actorList[i] == null)
				{
					continue;
				}
				ActorObj act = actorList[i] as ActorObj;
				act.m_bSelected = false;
			}
		}

		public List<ActorObj> GetAllActor()
		{
			if (ServerIDActorManager.Values != null)
			{
				return new List<ActorObj>(ServerIDActorManager.Values);
			}
			return null;
		}

		public void UpdateAOIAdd(ref List<int> list)
		{
			for (int i = 0; i < list.Count; ++i)
			{
				int id = list[i];
				if (IsLocalPlayer(id))
				{
					continue;
				}
				if (!ServerIDActorManager.ContainsKey(id))
				{
					//GameLogger.Warning(LOG_CHANNEL.NETWORK, "Aoi Add : " + id);
					UpdateAOIObjView(id, true);
				}
			}

		}
		public void UpdateAOIRemove(ref List<int> list)
		{
			if (ServerIDActorManager.Count == 0)
			{
				return;
			}
			Dictionary<int, ActorObj>.KeyCollection key = ServerIDActorManager.Keys;
			Dictionary<int, ActorObj>.KeyCollection.Enumerator en = key.GetEnumerator();
			while (en.MoveNext())
			{
				int id = en.Current;
				if (IsLocalPlayer(id))
				{
					continue;
				}
				if (!list.Contains(id))
				{
					//GameLogger.Warning(LOG_CHANNEL.NETWORK, "Aoi Remove : " + id);
					UpdateAOIObjView(id, false);
				}
			}

			if (RemoveAvatarList.Count > 0)
			{
				foreach (int avatarId in RemoveAvatarList)
				{
					RemoveByServerID(avatarId, false);
					//ServerIDActorManager.ForceRemove(avatarId);

				}
				RemoveAvatarList.Clear();
			}
		}
		
	}
}
