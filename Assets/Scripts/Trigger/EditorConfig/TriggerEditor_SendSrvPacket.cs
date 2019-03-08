using UnityEngine;

namespace Galaxy
{
    public class TriggerEditor_SendSrvPacket : TriggerEditor_Base
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_SEND_PACKET;
            }
        }

        public bool bSendMsgWhenLeave;

        [HideInInspector]
        public string strPacketName = "-1";

        private void Awake()
        {
            
        }

        public override void NeedUpdate()
        {
            if(string.IsNullOrEmpty(strPacketName) ||  strPacketName == "-1")
            {
                strPacketName = "GPacketClientTriggerEvent";
            }

            if (action != "-1")
            {
                string[] arrParam = action.Split(';');
                if (arrParam != null && arrParam.Length == 2)
                {
                    strPacketName = arrParam[0];
                    bSendMsgWhenLeave = (arrParam[1].Equals("true") ? true : false);
                }
                
                action = "-1";
            }
        }

        public override string DoTriggerParam()
        {
            string str = string.Format("{0};{1}", strPacketName, bSendMsgWhenLeave);
            return str;
        }

        public override void ParseTriggerParam(string action)
        {
            this.action = action;
        }
    }
}
