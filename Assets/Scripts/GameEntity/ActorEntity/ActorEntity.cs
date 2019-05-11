using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
	//所有游戏物体的基本单位
	public class ActorEntity : EntityComponent, GameBaseEntity
	{
		protected int m_nAvatarID;
		public int AvatarID
		{
			get
			{
				return m_nAvatarID;
			}
		}
		protected int InstanceID;

		public ActorEntity(int nAvatarID)
		{
			m_nAvatarID = nAvatarID;
		}

		public void Init()
		{
			m_bFight = true;
		}

		public void Tick(float fElapseTimes)
		{

		}

		public virtual int GetCamp()
		{
			return (int)CampType.Default;
		}

		public int GetAvatarID()
		{
			return m_nAvatarID;
		}

		public virtual GameEntityType GetEntityType()
		{
			return GameEntityType.Empty;
		}

		public bool CreateEngineObj()
		{
			if(m_EngineObj != null)
				return true;

			int nModelID = GetModelID();
			ConfigData modelData = GameDataProxy.GetData("modelresdefine", nModelID);
			if(modelData == null)
				return false;

			string strName = modelData.GetString("ModelName");
			ResourcesProxy.LoadAsset(strName, OnEngineObjectLoadEnd, strName);
			return true;
		}

		public void OnEngineObjectLoadEnd(LoadResult result)
		{
			if(!result.isSuccess)
			{
				GameLogger.Error(LOG_CHANNEL.ERROR, "Load Engine Object Failed!  modelName : " + result.userData);
				return;
			}

			m_EngineObj = GameObject.Instantiate(result.assets[0] as GameObject);
			if(m_EngineObj == null)
			{
				GameLogger.Error(LOG_CHANNEL.ERROR, "Create Engine Object Failed!  modelName : " + result.userData);
				return;
			}

			InstanceID = m_EngineObj.gameObject.GetInstanceID();
			AfterCreateEngineObj();
		}

		/// <summary>
		/// 此接口下创建游戏物体的脚本
		/// </summary>
		public override void AfterCreateEngineObj()
		{
			base.AfterCreateEngineObj();
			//GalaxyGameModule.GetGameManager<GalaxyActorManager>().UpdateClientID(ServerID, ClientID);
		}

		public void DestroyEngineObj()
		{
			if(m_EngineObj == null)
				return;

			BeforeDestroy();
			GameObject.Destroy(m_EngineObj);
			m_EngineObj = null;
		}

		public override void BeforeDestroy()
		{
			base.BeforeDestroy();
		}

		public GameObject GetEngineObj()
		{
			if(m_EngineObj == null)
				return null;
			return m_EngineObj;
		}

		public void SetPos(Vector3 vPos)
		{
			if(m_EngineObj == null)
				return;
			m_EngineObj.transform.position = vPos;
		}

		public void SetDir(Vector3 vForward)
		{
			if(m_EngineObj == null)
				return;
			m_EngineObj.transform.forward = vForward;
		}

		public Vector3 GetPos()
		{
			if(m_EngineObj == null)
				return Vector3.zero;
			return m_EngineObj.transform.position;
		}

		public Vector3 GetDir()
		{
			if(m_EngineObj == null)
				return Vector3.zero;
			return m_EngineObj.transform.forward;
		}

		public void RegEngineObjEvent()
		{

		}

		public void UnRegEngineObjEvent()
		{

		}



		protected bool m_bFight;
		public bool IsFight()
		{
			return m_bFight;
		}
		public void EnterCombat()
		{
			m_bFight = true;
		}
		public void LeaveCombat()
		{
			m_bFight = false;
		}

	}
}
