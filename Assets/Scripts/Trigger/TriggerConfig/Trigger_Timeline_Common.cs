using UnityEngine;

namespace Galaxy
{
    public class Trigger_Timeline_Common : GalaxyTrigger
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_TIMELINE;
            }
        }

        protected int m_timelineId = 0;
        protected GameObject m_target;

        public override void OnTriggerEnterCallBack(GameObject target)
        {
            if(target.tag.CompareTo(StaticParam.TAG_LOCAL_PLAYER) == 0)
            {
                LosePlayerContorl(target,0);
            }

            GalaxyGameModule.GetGameManager<TimelineManager>().PlayTimeLine(m_timelineId, OnTimelineEnd);
        }

        protected virtual void OnTimelineEnd(TimelineResult result)
        {
            ResetPlayerControl(m_target);
        }

        public override void OnTriggerLeaveCallBack(GameObject target)
        {

        }

        protected override void ParseAction()
        {
            if (ConfigData == null || ConfigData.triggerAction == "-1")
                return;

            m_timelineId = int.Parse(ConfigData.triggerAction);
            /* string[] arrParam = ConfigData.triggerAction.Split(';');
            if (arrParam.Length != 2)
                return;

            string[] arrFirstParam = arrParam[0].Split(':');
            if(arrFirstParam.Length == 2)
            {
                IsScenePrefab = (arrFirstParam[1] == "False" ? false : true);
            }

            if(IsScenePrefab)
            {
                GameObject sceneObj = GameObject.Find(arrParam[1]);
                if(sceneObj != null)
                {
                    ScenePrefab = sceneObj.transform;
                }
            }
            else
            {
                m_path = arrParam[1];
            } */
        }
    }
}
