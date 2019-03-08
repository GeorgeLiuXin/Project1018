using System;
using UnityEngine;
using LuaInterface;
using System.Collections.Generic;

namespace Galaxy
{
    public class Trigger_LuaActionEx : Trigger_LuaAction
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_LUA_ACTION_EX;
            }
        }

        protected override void ParseAction()
        {
            base.ParseAction();

        }
    }

    
}
