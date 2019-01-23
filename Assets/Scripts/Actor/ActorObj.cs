using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
	//所有游戏物体的基本单位
	public class ActorObj : MonoBehaviour
	{
        protected bool m_bFight;

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

        public void LeaveCombat()
        {
            m_bFight = false;
        }

        public int GetCamp()
        {
            return (int)CampType.Default;
        }

    }
}
