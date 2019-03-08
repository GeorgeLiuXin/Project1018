using UnityEngine;
using LuaInterface;
using System;
using System.Collections.Generic;

namespace Galaxy
{
    public class Trigger_Hit_Local : MonoBehaviour
    {
        private void Start()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == null || other.tag != StaticParam.TAG_LOCAL_PLAYER)
                return;

            try
            {
                string filePath = "Trigger.Common.Trigger_NativeMoveHit";
                GLuaManager.GetLuaState().Require(filePath);
                string enterFuncName = "LuaTrigger.Trigger_NativeMoveHit.EnterTrigger";
                LuaFunction enterFunc = GLuaManager.GetLuaState().GetFunction(enterFuncName);
                if (enterFunc != null)
                {
                    enterFunc.BeginPCall();
                    enterFunc.PushArgs(null);
                    enterFunc.PCall();
                    enterFunc.EndPCall();
                }
            }
            catch (Exception e)
            {
                string errorMsg = string.Format("Lua LuaAction {0} OnTriggerEnterCallBack error! reason : {1}", "Trigger_NativeMoveHit", e.ToString());
                GameLogger.Error(LOG_CHANNEL.LUA, errorMsg);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            
        }
    }
}
