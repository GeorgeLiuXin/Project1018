using UnityEngine;

namespace Galaxy
{
    public class Trigger_Animator : GalaxyTrigger
    {
        private Animator m_animtor;
        private string m_name;

        public override void OnTriggerEnterCallBack(GameObject target)
        {
            if (ScenePrefab == null)
                return;

            m_animtor = ScenePrefab.GetComponentInChildren<Animator>(true);
            if(m_animtor != null)
            {
                if(!m_animtor.enabled)
                {
                    m_animtor.enabled = true;
                    
                }

                m_animtor.SetTrigger(m_name);
            }
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

            GameObject sceneObj  = GameObject.Find(strParam[0]);
            ScenePrefab = sceneObj.transform;
            m_name = strParam[1];
        }

        public override void OnTriggerLeaveCallBack(GameObject target)
        {

        }

        public override void OnBeforeDestroy()
        {
            base.OnBeforeDestroy();
            m_animtor = null;
        }
    }
}
