using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public class TriggerEditor_OS : TriggerEditor_Base
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_OS;
            }
        }

        [Tooltip("OS系统配置ID")]
        public int OS_ID;

        private void Start()
        {

        }

        public override string DoTriggerParam()
        {
            return OS_ID.ToString();
        }

        public override void NeedUpdate()
        {
            if (action != "-1")
            {
                OS_ID = int.Parse(action);
                action = "-1";
            }
        }
    }
}