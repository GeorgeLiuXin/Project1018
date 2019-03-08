using UnityEngine;

namespace Galaxy
{
    public class TriggerEditor_EnableTriggerGroup : TriggerEditor_Base
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_VISABLE_GROUP;
            }
        }

        public int EnableGroupId;

        private void Start()
        {

        }

        public override string DoTriggerParam()
        {
            return EnableGroupId.ToString();
        }
        public override void ParseTriggerParam(string action)
        {
            this.EnableGroupId = int.Parse(action);
        }
    }
}