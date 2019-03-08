using UnityEngine;

namespace Galaxy
{
    public enum Trigger_Srv_Event
    {
        Trigger_Srv_Event_Enter,
        Trigger_Srv_Event_Leave,
    }

    public class Trigger_SendPacket : GalaxyTrigger
    {
        protected string m_strPacket;
        protected bool m_bSendPacketWhenLeave;

        public override void OnTriggerEnterCallBack(GameObject target)
        {
            if (target == null)
                return;

            SendPacketToSrv(Trigger_Srv_Event.Trigger_Srv_Event_Enter);
        }

        public override void OnTriggerLeaveCallBack(GameObject target)
        {
            if (!m_bSendPacketWhenLeave)
                return;

            SendPacketToSrv(Trigger_Srv_Event.Trigger_Srv_Event_Leave);
        }

        protected override void ParseAction()
        {
            if (ConfigData.triggerAction == "-1")
                return;

            string[] strParam = ConfigData.triggerAction.Split(';');
            if (strParam == null || strParam.Length != 2)
            {
                GameLogger.Error(LOG_CHANNEL.TRIGGER, "Trigger_SendPacket parse action failed!");
                return;
            }

            m_strPacket = strParam[0];
            m_bSendPacketWhenLeave = (strParam[1].Equals("true")? true : false);
        }

        private void SendPacketToSrv(Trigger_Srv_Event type)
        {
            GPacketBase pktTrigger = GalaxyGameModule.GetGameManager<GPacketDefineManager>().CreatePacket("GPacketClientTriggerEvent");
            pktTrigger.SetInt32("triggerID", base.ConfigData.eventId);
            pktTrigger.SetInt32("eventType", (int)type);
            GalaxyNetManager.Instance.SendPacket(pktTrigger);
        }
    }
}
