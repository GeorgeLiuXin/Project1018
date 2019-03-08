using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public class TriggerEditor_NpcAnimation : TriggerEditor_Base
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_NPC_ANIMATION;
            }
        }

        [Tooltip("动画Trigger名称")]
        public string animatorId;

        [Tooltip("朝向 角度")]
        public float angle;

        private void Start()
        {

        }

        public override string DoTriggerParam()
        {
            string strParam = string.Format("{0};{1}", animatorId, angle);
            return strParam;
        }

        public override void NeedUpdate()
        {
            if (action != "-1")
            {
                string[] arr = action.Split(';');
                animatorId = arr[0];
                angle = float.Parse(arr[1]);
                action = "-1";
            }
        }
    }
}