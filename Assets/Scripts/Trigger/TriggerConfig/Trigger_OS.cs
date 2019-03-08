using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public class Trigger_OS : GalaxyTrigger
    {
        private int m_Id;

        public override void OnTriggerEnterCallBack(GameObject target)
        {
            if (m_Id <= 0)
                return;
            GalaxyGameModule.GetGameManager<GOSManager>().BeginOS(m_Id);
        }

        public override void OnTriggerLeaveCallBack(GameObject target)
        {

        }

        protected override void ParseAction()
        {
            if (ConfigData.triggerAction == "-1")
                return;

            int.TryParse(ConfigData.triggerAction, out m_Id);
        }

        public override void OnBeforeDestroy()
        {
            base.OnBeforeDestroy();

        }
    }
}
