using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public class TriggerEditor_LookAtScene : TriggerEditor_Base
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_LOOKATSCENE;
            }
        }
         
        [Tooltip("需要看的位置")]
        public Vector3 vPos;

        [Tooltip("摄像机过去的时间总长度")]
        public float fLookTimes;

        [HideInInspector]
        public Vector3 vSavePos;

        private void Start()
        {
            vSavePos = Vector3.zero;
        }

        public override string DoTriggerParam()
        {
            string strParam = string.Format("{0},{1},{2};{3}", vSavePos.x, vSavePos.y, vSavePos.z,fLookTimes);
            return strParam;
        }

        public override void NeedUpdate()
        {
            if( vPos != vSavePos)
            {
                vSavePos = vPos;
            }

            if (action != "-1")
            {
                string[] arrParam = action.Split(';');
                if (arrParam != null && arrParam.Length == 2)
                {
                    string[] arrPos = arrParam[0].Split(',');
                    if(arrPos.Length == 3)
                    {
                        vPos.x = float.Parse(arrPos[0]);
                        vPos.y = float.Parse(arrPos[1]);
                        vPos.z = float.Parse(arrPos[2]);
                    }

                    fLookTimes = float.Parse(arrParam[1]);
                }

                action = "-1";
            }
        }
    }
}
