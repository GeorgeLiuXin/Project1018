using UnityEngine;


namespace Galaxy
{
    public class Trigger_PlayEffect : GalaxyTrigger
    {

        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_PLAYEFFECT;
            }
        }

        private bool m_bLoop;

        public override void OnTriggerEnterCallBack(GameObject target)
        {
            if (ScenePrefab == null)
                return;

            ScenePrefab.gameObject.SetActive(true);
            ParticleSystem[] psList = ScenePrefab.GetComponentsInChildren<ParticleSystem>();
            foreach(ParticleSystem item in psList)
            {
                item.Play();
            }
        }

        public override void OnTriggerLeaveCallBack(GameObject target)
        {
            /*if (ScenePrefab == null)
                return;

            ParticleSystem[] psList = ScenePrefab.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem item in psList)
            {
                item.Stop();
            }
            ScenePrefab.gameObject.SetActive(false);*/
        }

        protected override void ParseAction()
        {
            if (ConfigData.triggerAction == "-1")
                return;

            string[] strParam = ConfigData.triggerAction.Split(';');
            if (strParam == null || strParam.Length != 2)
            {
                GameLogger.Error(LOG_CHANNEL.TRIGGER, "Play effect parse action failed!");
                return;
            }

            GameObject sceneObj = GameObject.Find(strParam[0]);
            ScenePrefab = sceneObj.transform;
        }

        public override void OnBeforeDestroy()
        {
            base.OnBeforeDestroy();
            if (ScenePrefab != null)
            {
                ScenePrefab.gameObject.SetActive(false);
                ScenePrefab = null;
            }
            
        }
    }
}
