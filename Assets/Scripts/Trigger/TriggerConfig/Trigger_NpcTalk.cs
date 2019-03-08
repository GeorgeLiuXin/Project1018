using UnityEngine;

namespace Galaxy
{
    public class Trigger_NpcTalk : GalaxyTrigger
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_NPC_TALK;
            }
        }

        public override void OnTriggerEnterCallBack(GameObject target)
        {
            //
            //GalaxyGameModule.GetGameManager<>
        }

        public override void OnTriggerLeaveCallBack(GameObject target)
        {

        }

        public override void OnBeforeDestroy()
        {
            base.OnBeforeDestroy();

        }
    }
}