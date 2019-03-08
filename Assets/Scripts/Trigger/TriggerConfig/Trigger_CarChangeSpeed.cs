using UnityEngine;

namespace Galaxy
{
    public class Trigger_CarChangeSpeed : GalaxyTrigger
    {
        public float m_fTargetSpeed;

        public override void OnTriggerEnterCallBack(GameObject target)
        {
            if (target == null)
                return;

            while(target.transform.parent != null)
            {
                target = target.transform.parent.gameObject;
            }

            RCC_CarControllerV3 control = target.GetComponent<RCC_CarControllerV3>();
            if (control == null)
                return;

            if (m_fTargetSpeed < 0f)
                m_fTargetSpeed = 0f;

            control.speed = m_fTargetSpeed;
            control.maxspeed = m_fTargetSpeed;
            control.orgMaxSpeed = m_fTargetSpeed;
        }

        protected override void ParseAction()
        {
            if (ConfigData.triggerAction == "-1")
                return;

            float.TryParse(ConfigData.triggerAction, out m_fTargetSpeed);
        }
    }
}
