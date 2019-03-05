using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{

    public class BuffEffectLogic : PerformanceLogic
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
        public int m_nPartOfModel;

        private static Dictionary<int, string> m_modelPoint = new Dictionary<int, string>
        {
            { 0, StaticParam.POINT_BLOOD_NAME},
            { 1, StaticParam.POINT_HIT_NAME},
            { 2, StaticParam.POINT_FOOT_NAME},
            { 3, StaticParam.POINT_WAIST_NAME},
        };
        private GameObject m_buffObj;

        public override void Init(params object[] values)
        {
            m_buffObj = GalaxyGameModule.GetGameManager<EffectManager>().GetCommonEffectByID(m_nEffectID);
            ActorObj actor = ActorMgr.GetByServerID(m_OwenrID) as ActorObj;
            Transform point;
            actor.GetEngineObj().TryGetPrefabPos(m_modelPoint[m_nPartOfModel], out point);
            m_buffObj.SetParent(point, true);
        }

        public override bool Tick(float fTime)
        {
            return base.Tick(fTime);
        }

        public override void Reset()
        {
            GalaxyGameModule.GetGameManager<EffectManager>().ReturnEffect(m_buffObj);
        }

    }

}