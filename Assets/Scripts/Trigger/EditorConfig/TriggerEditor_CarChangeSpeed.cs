using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galaxy
{
    public class TriggerEditor_CarChangeSpeed : TriggerEditor_Base
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_CAR_SPEED_CHANGE;
            }
        }

        public float fSpeed = 1f;

        public override void NeedUpdate()
        {
            if(action != "-1")
            {
                fSpeed = float.Parse(action);
                action = "-1";
            }
        }

        public override string DoTriggerParam()
        {
            return fSpeed.ToString();
        }

        public override void ParseTriggerParam(string action)
        {
            fSpeed = float.Parse(action);
        }
    }
}
