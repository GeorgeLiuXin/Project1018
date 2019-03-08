using UnityEngine;

namespace Galaxy
{
    public class TriggerEditor_GuideTrigger : TriggerEditor_Base
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_GUIDE;
            }
        }

        public int guideId;

        public GalaxyTriggerDefine.TRIGGER_GUIDE guideBegin = GalaxyTriggerDefine.TRIGGER_GUIDE.TRIGGER_GUIDE_BEGIN;

        private void Start()
        {

        }

        public override void NeedUpdate()
        {
            if (action != "-1")
            {
                string[] arr = action.Split(';');
                guideId = int.Parse(arr[0]);
                guideBegin = (GalaxyTriggerDefine.TRIGGER_GUIDE)int.Parse(arr[1]);

                action = "-1";
            }
        }

        public override string DoTriggerParam()
        {
            string strFormat = string.Format("{0};{1}", guideId,(int)guideBegin);
            return strFormat;
        }

        public override void ParseTriggerParam(string action)
        {
            this.action = action;
        }
    }
}