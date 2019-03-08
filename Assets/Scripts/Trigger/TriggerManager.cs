using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace Galaxy
{
    public class TriggerManager : GalaxyGameManagerBase
    {
        private enum Trigger_Content_State
        {
            Trigger_Content_Waiting = 0,
            Trigger_Content_Creating,
            Trigger_Content_Inited,
        }
        
        
        private Queue<ConfigData> m_queueCreator;
        private Trigger_Content_State m_state;
        private TriggerCreator m_creator;


        private float m_fCurTimes = 0f;
        private float m_fDelayTimes = 0.3f;

        public void OnAfterEnterScene()
        {
            m_state = Trigger_Content_State.Trigger_Content_Waiting;
            RoutineRunner.WaitForSeconds(2f, LoadSceneContent);
        }

        public void OnBeforeLeaveScene()
        {
            m_creator.ClearAll();
        }

        private void LoadSceneContent()
        {
            string strFileName = string.Format(StaticParam.LEVEL_FILE_NAME_BASE+"{0}",GalaxyGameModule.GetGameManager<GalaxySceneManager>().CurSceneID); 
            ConfigData[] configData = ConfigDataTableManager.Instance.GetAllData(strFileName);
            if (configData == null || configData.Length <= 0)
                return;

            m_creator.PreTriggerCreate(ref configData);
            m_state = Trigger_Content_State.Trigger_Content_Creating;
        }

        public override void InitManager()
        {
            m_creator = new TriggerCreator();
        }

        public void EnterLogicTrigger(int eventID)
        {
            if (m_creator == null)
                return;

            GalaxyTrigger trigger =  m_creator.GetTriggerComByEventID(eventID);
            if(trigger == null)
            {
                GameLogger.Error(LOG_CHANNEL.LOGIC,"EnterLogicTrigger failed! eventId : " + eventID);
                return;
            }


        }

        public override void Update(float fElapseTimes)
        {
            if (m_state == Trigger_Content_State.Trigger_Content_Waiting)
                return;

            if(m_state == Trigger_Content_State.Trigger_Content_Creating)
            {
                m_fCurTimes += fElapseTimes;
                if(m_fCurTimes > m_fDelayTimes)
                {
                    if (m_creator != null)
                    {
                        if (m_creator.CreateEnd())
                        {
                            m_state = Trigger_Content_State.Trigger_Content_Inited;
                        }
                        else
                        {
                            m_creator.TestCreateTrigger();
                        }
                    }

                    m_fCurTimes = 0f;
                }

                return;
            }

            //真正创建好以后的Trigger 更新 
            //..
        }

        public override void ShutDown()
        {
            
        }
    }
}
