using UnityEngine;

namespace Galaxy
{
    public class Trigger_CineMachine : GalaxyTrigger
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_CINE_MAHCHINE;
            }
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
