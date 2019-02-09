using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
	//所有游戏物体的基本单位
	public class ActorObj
	{
        protected bool m_bFight;
        public int m_nAvatarID;
        protected Vector3 m_vPos;
		public int AvatarID;
		public int InstanceID;
		protected GameObject m_EngineObj;

		public void Init()
		{
            m_bFight = true;
			InstanceID = m_EngineObj.gameObject.GetInstanceID();
		}

		public void Tick(float fElapseTimes)
		{

		}

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

        public int GetCamp()
        {
            return (int)CampType.Default;
        }

        public void SetAvatarID(int nAvatarID)
        {
            m_nAvatarID = nAvatarID;
        }

        public int GetAvatarID()
        {
            return m_nAvatarID;
        }

        public void SetPos(Vector3 vPos)
        {
            m_vPos = vPos;
        }

        public Vector3 GetPos()
        {
            return m_vPos;
        }

    }
}
