using UnityEngine;

namespace Galaxy
{
    public class TriggerEditor_DisableTriggerGroup : TriggerEditor_Base
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_VISABLE_GROUP;
            }
        }

        public int DisableGroupId;

        private void Start()
        {

        }

        public override string DoTriggerParam()
        {
            return DisableGroupId.ToString();
        }

        public override void ParseTriggerParam(string action)
        {
            this.DisableGroupId = int.Parse(action);
        }
    }
}