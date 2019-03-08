using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public class Trigger_NpcAnimation : GalaxyTrigger
    {
        //多组动作内容 留着扩展
        protected int m_animationId;
        protected float m_fAngel;

        public override void OnTriggerEnterCallBack(GameObject target)
        {
            if (target == null)
                return;

            Vector3 vEuler = target.transform.eulerAngles;
            vEuler.y = m_fAngel;
            target.transform.eulerAngles = vEuler;

            LosePlayerContorl(target, m_animationId);
        }

        protected override void LosePlayerContorl(GameObject actorObject, int animID)
        {
            base.LosePlayerContorl(actorObject,animID);

            ActorObj actorObj = GalaxyGameModule.GetGameManager<GalaxyActorManager>().GetByClientID(actorObject.GetInstanceID()) as ActorObj;
            if(actorObj != null)
            {
                actorObj.aniCom.SetDynamicAnimGroup(animID);
            }
        }

        public override void OnTriggerLeaveCallBack(GameObject target)
        {
            ResetPlayerControl(target);

            ActorObj actorObj = GalaxyGameModule.GetGameManager<GalaxyActorManager>().GetByClientID(target.GetInstanceID()) as ActorObj;
            if (actorObj != null)
            {
                actorObj.aniCom.SetDynamicAnimGroup(0);
            }
        }

        protected override void ParseAction()
        {
            if (ConfigData.triggerAction == "-1")
                return;

            string[] arrPos = ConfigData.triggerAction.Split(';');
            if (arrPos == null)
                return;

            m_animationId = int.Parse(arrPos[0]);
            m_fAngel = float.Parse(arrPos[1]);
        }

        public override void OnBeforeDestroy()
        {
            base.OnBeforeDestroy();
        }
    }
}
