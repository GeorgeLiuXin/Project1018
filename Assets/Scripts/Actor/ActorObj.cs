using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
	//所有游戏物体的基本单位
	public class ActorObj : MonoBehaviour
	{
        protected bool m_bFight;
        public int m_nAvatarID;
        protected Vector3 m_vPos;

		// Use this for initialization
		void Start()
		{
            m_bFight = true;
        }

		// Update is called once per frame
		void Update()
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
