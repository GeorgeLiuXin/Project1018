using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public class TriggerEditor_StopCar : TriggerEditor_Base
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_STOP_CAR;
            }
        }


    }
}
