using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    [PerformanceLogicDes("指定播放动画且为强制播放，但会被其他逻辑打断")]
    public class PlayAnimLogic : PerformanceLogic
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

        [PerformanceLogicItemDes("动画id")]
        public int m_nAnimID;
        
        public override void Init(params object[] values)
        {
            ActorObj actor = ActorMgr.GetByServerID(m_OwenrID) as ActorObj;
            if (actor == null)
            {
                return;
            }
            FSMParam_Idle idleParam = new FSMParam_Idle();
            idleParam.vCurPos = actor.GetPos();
            idleParam.vCurDir = actor.GetDir();
            idleParam.nAnimID = m_nAnimID;
            actor.SetTargetState(idleParam,true);
        }

        public override bool Tick(float fTime)
        {
            return base.Tick(fTime);
        }

        public override void Reset()
        {
            ActorObj actor = ActorMgr.GetByServerID(m_OwenrID) as ActorObj;
            if (actor == null)
            {
                return;
            }
            FSMParam_Idle idleParam = new FSMParam_Idle();
            idleParam.vCurPos = actor.GetPos();
            idleParam.vCurDir = actor.GetDir();
            actor.SetTargetState(idleParam, true);
        }
    }

}