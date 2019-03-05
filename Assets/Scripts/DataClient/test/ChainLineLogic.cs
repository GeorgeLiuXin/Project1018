using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    /// <summary>
    /// buff连线效果逻辑
    /// eg:莫甘娜大招，小精灵连线
    /// </summary>
    public class ChainLineLogic : PerformanceLogic
    {
        protected GalaxyActorManager m_actorMgr;
        protected GalaxyActorManager ActorMgr
        {
            get
            {
                if (m_actorMgr == null)
                {
                    m_actorMgr = GalaxyGameModule.GetGameManager<GalaxyActorManager>();
                }
                return m_actorMgr;
            }
        }

        public int m_nEffectID;
        public float m_yOffest;

        private GameObject m_effectObj;

        private Transform m_pCaster;
        private Transform m_pTarget;

        public ChainLineLogic()
        {
        }

        /// <summary>
        /// 1、自己的Transform
        /// 2、目标的Transform
        /// </summary>
        public override void Init(params object[] values)
        {
            if (values == null)
                return;
            
            m_effectObj = GalaxyGameModule.GetGameManager<EffectManager>().GetCommonEffectByID(m_nEffectID);

            int nCaster = Convert.ToInt32(values[0]);
            int nTarget = Convert.ToInt32(values[1]);
            ActorObj actor;
            actor = ActorMgr.GetByServerID(nCaster) as ActorObj;
            if (actor != null)
            {
                actor.GetEngineObj().TryGetPrefabPos(StaticParam.POINT_WAIST_NAME, out m_pCaster);
            }
            actor = ActorMgr.GetByServerID(nTarget) as ActorObj;
            if (actor != null)
            {
                actor.GetEngineObj().TryGetPrefabPos(StaticParam.POINT_WAIST_NAME, out m_pTarget);
            }
            UVChainLine uvLogic = m_effectObj.AddComponent<UVChainLine>();
            uvLogic.start = m_pCaster;
            uvLogic.target = m_pTarget;
            uvLogic.yOffset = m_yOffest;
        }

        public override bool Tick(float fTime)
        {
            return base.Tick(fTime);
        }

        public override void Reset()
        {

        }
    }

}