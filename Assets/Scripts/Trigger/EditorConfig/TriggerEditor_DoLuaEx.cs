using UnityEngine;

namespace Galaxy
{
    public class TriggerEditor_DoLuaEx : TriggerEditor_Base
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_LUA_ACTION_EX;
            }
        }

        [Tooltip("lua名称 不用后缀")]
        public string luaFileName;

        [Tooltip("参数列表 半角分号分割")]
        public string luaParamList;

        public override void NeedUpdate()
        {
            if (action == "-1")
                return;

            string[] arrParam = action.Split(';');
            if (arrParam == null || arrParam.Length < 2)
                return;

            luaFileName = arrParam[0];
            luaParamList = arrParam[1];

            for (int i = 2; i < arrParam.Length; ++i)
            {
                luaParamList += arrParam[i];
            }

            action = "-1";
        }

        public override string DoTriggerParam()
        {
            string param = string.Format("{0};{1}", luaFileName, luaParamList);
            return param;
        }
    }
}
