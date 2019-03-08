using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public class Trigger_FindPath : GalaxyTrigger
    {
        protected Vector3 m_vDestPos = Vector3.zero;
        protected LinkedList<Navigation.Grid.Position> m_lWayPoint = new LinkedList<Navigation.Grid.Position>();

        public override void OnTriggerEnterCallBack(GameObject target)
        {
            if (target == null)
                return;

            if (m_vDestPos == Vector3.zero)
                return;
            
            Vector3 vSrcPos = target.transform.position;
            bool bStart = GalaxyGameModule.GetGameManager<GalaxySceneManager>().NavGrid.FindPath(vSrcPos, m_vDestPos, ref m_lWayPoint);
            if(!bStart)
            {
                GameLogger.Error(LOG_CHANNEL.LOGIC,"Trigger_FindPath error! ");
            }

        }

        public override void OnTriggerLeaveCallBack(GameObject target)
        {

        }

        protected override void ParseAction()
        {
            if (ConfigData.triggerAction == "-1")
                return;

            string[] arrPos = ConfigData.triggerAction.Split(',');
            if (arrPos != null || arrPos.Length != 3)
                return;

            m_vDestPos.x = float.Parse(arrPos[0]);
            m_vDestPos.y = float.Parse(arrPos[1]);
            m_vDestPos.z = float.Parse(arrPos[2]);
        }

        public override void OnBeforeDestroy()
        {
            base.OnBeforeDestroy();

            if (m_lWayPoint != null)
                m_lWayPoint.Clear();
        }
    }
}
