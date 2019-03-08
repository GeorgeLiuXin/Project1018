using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public class TriggerEditor_DoLua : TriggerEditor_Base
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_LUA_ACTIONS;
            }
        }

        [Tooltip("lua名称 不用后缀")]
        public string luaFileName;

        [Tooltip("是不是本地操作模式 比如QTE")]
        public int IsClientPlayer = 0;

        private void Start()
        {

        }

        public override void NeedUpdate()
        {
            if (action == "-1")
                return;

            string[] arrParam = action.Split(';');
            if (arrParam == null || arrParam.Length != 2)
                return;

            luaFileName = arrParam[0];
            IsClientPlayer = int.Parse(arrParam[1]);

            action = "-1";
        }


        public override string DoTriggerParam()
        {
            string param = string.Format("{0};{1}",luaFileName, IsClientPlayer);
            return param;
        }

        public override void ParseTriggerParam(string action)
        {
            this.action = action;
        }
    }
}