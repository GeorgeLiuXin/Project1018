using UnityEngine;

namespace Galaxy
{
    public class Trigger_LookAtScene : GalaxyTrigger
    {
        private Vector3 m_vLookAtPos;
        private float m_fTimes;

        public override void OnTriggerEnterCallBack(GameObject target)
        {
            if (target == null)
                return;

            GalaxyGameModule.GetGameManager<GalaxyCameraManager>().SetLookAtPoint(m_vLookAtPos,m_fTimes);
        }

        protected override void ParseAction()
        {
            if (ConfigData.triggerAction == "-1")
                return;

            string[] strParam = ConfigData.triggerAction.Split(';');
            if (strParam == null || strParam.Length != 2)
            {
                GameLogger.Error(LOG_CHANNEL.TRIGGER, "Trigger_Animator parse action failed!");
                return;
            }

            string[] arrPos = strParam[0].Split(',');
            if(arrPos.Length == 3)
            {
                m_vLookAtPos.x = float.Parse(arrPos[0]);
                m_vLookAtPos.y = float.Parse(arrPos[1]);
                m_vLookAtPos.z = float.Parse(arrPos[2]);
            }

            m_fTimes = float.Parse(strParam[1]);
        }

    }
}
