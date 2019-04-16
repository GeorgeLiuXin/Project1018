using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    [PerformanceLogicDes("buff特效逻辑，可配置对应的挂点信息")]
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

        [PerformanceLogicItemDes("buff特效对应的特效id")]
        public int m_nEffectID;
        [PerformanceLogicItemDes("buff特效所绑定的位置0头顶1受击点2脚底3腰部")]
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
            if (m_buffObj == null)
            {
                GameLogger.DebugLog(LOG_CHANNEL.ASSET, string.Format("BuffEffectLogic effectID: [{0}] ,effect is null! load failed!", m_nEffectID));
                return;
            }

            ActorObj actor = ActorMgr.GetByServerID(m_OwenrID) as ActorObj;
            Transform point;
            if (actor != null && actor.GetEngineObj().TryGetPrefabPos(m_modelPoint[m_nPartOfModel], out point))
            {
                m_buffObj.SetParent(point, true);
            }
            else
            {
                GameLogger.DebugLog(LOG_CHANNEL.LOGIC, string.Format("actor is null or actor don't have point named [{0}]", m_modelPoint[m_nPartOfModel]));
            }
        }

        public override bool Tick(float fTime)
        {
            return base.Tick(fTime);
        }

        public override void Reset()
        {
            if (m_buffObj)
            {
                GalaxyGameModule.GetGameManager<EffectManager>().ReturnEffect(m_buffObj);
            }
        }
    }

}