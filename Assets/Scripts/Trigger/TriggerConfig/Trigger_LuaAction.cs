using UnityEngine;
using LuaInterface;
using System;
using System.Collections.Generic;

namespace Galaxy
{
    //模板LuaTrigger 调用
    public class Trigger_LuaAction : GalaxyTrigger
    {
        public string m_LuaFileName;
        public List<object> m_Params = new List<object>();

        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_LUA_ACTIONS;
            }
        }

        public override bool InitTrigger(TriggerParam param)
        {
            if (!base.InitTrigger(param))
                return false;

            return true;
        }

        protected override void ParseAction()
        {
            //解析参数，把参数按照顺序传入Lua，注意：都是字符串，这里没有类型
            if (string.IsNullOrEmpty(ConfigData.triggerAction))
                return;

            string[] strParams = ConfigData.triggerAction.Trim().Split(';');
            if (strParams.Length < 1)
                return;

            m_LuaFileName = strParams[0];
            if (string.IsNullOrEmpty(m_LuaFileName))
                return;

            //编辑器保证，第一个参数一定是lua文件的名字
            for (int i = 1; i < strParams.Length; ++i)
            {
                m_Params.Add(strParams[i]);
            }
        }

        public override void OnTriggerEnterCallBack(GameObject target)
        {
            try
            {
                string filePath = "Trigger.Common." + m_LuaFileName;
                GLuaManager.GetLuaState().Require(filePath);
                string enterFuncName = "LuaTrigger." + m_LuaFileName + ".EnterTrigger";
                LuaFunction enterFunc = GLuaManager.GetLuaState().GetFunction(enterFuncName);
                if (enterFunc != null)
                {
                    enterFunc.BeginPCall();
                    enterFunc.PushArgs(m_Params.ToArray());
                    enterFunc.PCall();
                    enterFunc.EndPCall();
                }
            }
            catch (Exception e)
            {
                string errorMsg = string.Format("Lua LuaAction {0} OnTriggerEnterCallBack error! reason : {1}", m_LuaFileName,e.ToString());
                GameLogger.Error(LOG_CHANNEL.LUA, errorMsg);
            }
        }

        public override void OnTriggerLeaveCallBack(GameObject target)
        {
            try
            {
                string leaveFuncName = "LuaTrigger." + m_LuaFileName + ".LeaveTrigger";
                LuaFunction leaveFunc = GLuaManager.GetLuaState().GetFunction(leaveFuncName);
                if (leaveFunc != null)
                {
                    leaveFunc.BeginPCall();
                    leaveFunc.PushArgs(m_Params.ToArray());
                    leaveFunc.PCall();
                    leaveFunc.EndPCall();
                }
            }
            catch (Exception e)
            {
                string errorMsg = string.Format("Lua LuaAction {0} OnTriggerLeaveCallBack error! reason : {1}", m_LuaFileName, e.ToString());
                GameLogger.Error(LOG_CHANNEL.LUA, errorMsg);
            }
        }

        public override void OnBeforeDestroy()
        {
            base.OnBeforeDestroy();
        }
    }
}
