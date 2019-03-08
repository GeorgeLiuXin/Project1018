using UnityEngine;
using LuaInterface;
using System;

namespace Galaxy
{
    public class Trigger_Guide : GalaxyTrigger
    {

        public int m_GuideID;
        public bool m_bBegin;

        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_GUIDE;
            }
        }

        public override bool InitTrigger(TriggerParam param)
        {
            if (!base.InitTrigger(param))
                return false;

            //解析参数，把参数按照顺序传入Lua，注意：都是字符串，这里没有类型
            if (string.IsNullOrEmpty(param.triggerAction))
                return false;

            string[] strParams = param.triggerAction.Trim().Split(';');

            if (strParams.Length < 2)
                return false;

            m_GuideID = System.Convert.ToInt32(strParams[0]);
            m_bBegin = (GalaxyTriggerDefine.TRIGGER_GUIDE)System.Convert.ToInt32(strParams[1]) == GalaxyTriggerDefine.TRIGGER_GUIDE.TRIGGER_GUIDE_BEGIN;
            return true;
        }
        public override void OnTriggerEnterCallBack(GameObject target)
        {
            try
            {
                string filePath = "Trigger.Trigger_Guide";
                GLuaManager.GetLuaState().Require(filePath);
                string enterFuncName = "LuaTrigger.Trigger_Guide.EnterTrigger";
                LuaFunction enterFunc = GLuaManager.GetLuaState().GetFunction(enterFuncName);
                if (enterFunc != null)
                {
                    enterFunc.BeginPCall();
                    enterFunc.Push(m_GuideID);
                    enterFunc.Push(m_bBegin);
                    enterFunc.PCall();
                    enterFunc.EndPCall();
                }
            }
            catch (Exception e)
            {
                string errorMsg = string.Format("Lua Trigger_Guide OnTriggerEnterCallBack error! reason : {0}",  e.ToString());
                GameLogger.Error(LOG_CHANNEL.LUA, errorMsg);
            }
        }

        public override void OnTriggerLeaveCallBack(GameObject target)
        {
            try
            {
                string leaveFuncName = "LuaTrigger.Trigger_Guide.LeaveTrigger";
                LuaFunction leaveFunc = GLuaManager.GetLuaState().GetFunction(leaveFuncName);
                if (leaveFunc != null)
                {
                    leaveFunc.BeginPCall();
                    leaveFunc.Push(m_GuideID);
                    leaveFunc.Push(m_bBegin);
                    leaveFunc.PCall();
                    leaveFunc.EndPCall();
                }
            }
            catch (Exception e)
            {
                string errorMsg = string.Format("Lua Trigger_Guide OnTriggerLeaveCallBack error! reason : {0}", e.ToString());
                GameLogger.Error(LOG_CHANNEL.LUA, errorMsg);
            }
        }

        public override void OnBeforeDestroy()
        {
            base.OnBeforeDestroy();
        }
    }
}
