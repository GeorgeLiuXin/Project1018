using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    public class TriggerCreator
    {
        private GameObject m_triggerRoot;
        private Dictionary<int, GalaxyTrigger> m_dictTrigger = new Dictionary<int, GalaxyTrigger>();
        private Dictionary<int,GameObject> m_dictTriggerGroup = new Dictionary<int, GameObject>();
        private Queue<TriggerParam> m_lSceneContent = new Queue<TriggerParam>();

        private Dictionary<GalaxyTriggerDefine.TRIGGER_TYPE, string> m_registerMapping = new Dictionary<GalaxyTriggerDefine.TRIGGER_TYPE, string>();

        private void InitTriggerRoot()
        {
            if (m_triggerRoot != null)
                return;

            m_triggerRoot = new GameObject();
            m_triggerRoot.name = "Scene_Trigger_Root";
            m_triggerRoot.transform.position = Vector3.zero;
            m_triggerRoot.transform.localScale = Vector3.one;

            InitRegisterMapping();
        }

        #region  InitRegisterMapping
        private void InitRegisterMapping()
        {
            if (m_registerMapping.Count > 0)
                return;

            m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_NPC_TALK, "Galaxy.Trigger_NpcTalk");
            m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_TIMELINE, "Galaxy.Trigger_Timeline_Common");
            m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_ANIMATOR, "Galaxy.Trigger_Animator");
            m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_CINE_MAHCHINE, "Galaxy.Trigger_CineMachine");
            m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_LUA_ACTIONS, "Galaxy.Trigger_LuaAction");
            //m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_VISABLE_GROUP, "Galaxy.Trigger_NpcTalk");
            m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_GUIDE, "Galaxy.Trigger_Guide");
            m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_OS, "Galaxy.Trigger_OS");
            m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_NPC_ANIMATION, "Galaxy.Trigger_NpcAnimation");
            m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_NPC_FINDPATH, "Galaxy.Trigger_FindPath");
            m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_LOOKATSCENE, "Galaxy.Trigger_LookAtScene");
            m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_PLAYEFFECT, "Galaxy.Trigger_PlayEffect");
            m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_SEND_PACKET, "Galaxy.Trigger_SendPacket");
            m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_STOP_CAR, "Galaxy.Trigger_StopCar");
            m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_CAR_SPEED_CHANGE, "Galaxy.Trigger_CarChangeSpeed");
            m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_LUA_ACTION_EX, "Galaxy.Trigger_LuaActionEx");
        }
        #endregion


        public void PreTriggerCreate(ref ConfigData[] preConfig)
        {
            if (preConfig == null || preConfig.Length == 0)
                return;

            InitTriggerRoot();

            for (int i = 0; i < preConfig.Length; ++i)
            {
                TriggerParam param = new TriggerParam(preConfig[i]);
                CreateTriggerGroup(param.groupId);
                m_lSceneContent.Enqueue(param); 
            }
        }

        public void ClearAll()
        {
            //m_triggerRoot = null;
            if (m_dictTrigger != null)
                m_dictTrigger.Clear();

            if(m_lSceneContent != null)
                m_lSceneContent.Clear();

            if(m_dictTriggerGroup != null)
                m_dictTriggerGroup.Clear();
        }

        public bool CreateEnd()
        {
            return m_lSceneContent.Count == 0 ? true : false;
        }

        private void CreateTriggerGroup(int groupID)
        {
            if (m_dictTriggerGroup.ContainsKey(groupID))
                return;

            GameObject newGroup = new GameObject();
            newGroup.name = "TriggerGroup_" + groupID.ToString();
            newGroup.transform.SetParent(m_triggerRoot.transform);
            newGroup.transform.localPosition = Vector3.zero;
            newGroup.transform.localRotation = Quaternion.identity;
            newGroup.transform.localScale = Vector3.one;

            m_dictTriggerGroup.Add(groupID,newGroup);
        }

        private GameObject FindTriggerGroupObject(int groupId)
        {
            if (m_dictTriggerGroup.ContainsKey(groupId))
                return m_dictTriggerGroup[groupId];

            return null;
        }

        public GalaxyTrigger GetTriggerComByEventID(int eventId)
        {
            if (m_dictTrigger.ContainsKey(eventId))
                return m_dictTrigger[eventId];

            return null;
        }

        public GalaxyTrigger TestCreateTrigger()
        {
            if (m_lSceneContent.Count == 0)
                return null;

            int createNumPerFrame = m_lSceneContent.Count > 5 ? 5 : m_lSceneContent.Count;

            for(int i = 0; i < createNumPerFrame; ++i)
            {
                TriggerParam param = m_lSceneContent.Dequeue();

                GalaxyTriggerDefine.TRIGGER_TYPE type = (GalaxyTriggerDefine.TRIGGER_TYPE)param.triggerType;
                if (type == GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_TYPE_NULL
                    || type == GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_TYPE_COUNT)
                    continue;

                GameObject group = FindTriggerGroupObject(param.groupId);
                if (group == null)
                    continue;

                Transform trigger = null;
                if (group.transform.childCount <= param.dataId)
                {
                    GameObject newTrigger = new GameObject();
                    newTrigger.name = "Trigger_" + param.dataId.ToString();
                    newTrigger.transform.SetParent(group.transform);
                    newTrigger.transform.position = param.vWorldPos;
                    newTrigger.transform.localEulerAngles = param.vDir;
                    newTrigger.transform.localScale = Vector3.one;
                    trigger = newTrigger.transform;

                    BoxCollider collider = trigger.gameObject.AddComponent<BoxCollider>();
                    collider.isTrigger = true;
                    collider.size = param.vScale;
                }
                else
                {
                    GameObject oldTrigger = group.GetChild("Trigger_" + param.dataId.ToString());
                    if(oldTrigger != null)
                    {
                        trigger = oldTrigger.transform;
                    }
                }

                GalaxyTrigger newTriggerComponent = NewTriggerComponent(trigger,(GalaxyTriggerDefine.TRIGGER_TYPE)param.triggerType);
                if (newTriggerComponent == null)
                {
                    string strMsg = string.Format("Add Trigger Component error! trigger type : {0}", ((GalaxyTriggerDefine.TRIGGER_TYPE)param.triggerType).ToString());
                    GameLogger.Error(LOG_CHANNEL.TRIGGER, strMsg);

                    BoxCollider com = trigger.GetComponent<BoxCollider>();
                    if(com != null)
                    {
                        GameObject.Destroy(com);
                    }
                    return null;
                }

                newTriggerComponent.InitTrigger(param);

                //GameLogger.DebugLog(LOG_CHANNEL.ASSET," Trigger Id : " + param.eventId.ToString());

                if(!m_dictTrigger.ContainsKey(param.eventId))
                    m_dictTrigger.Add(param.eventId, newTriggerComponent);

                return newTriggerComponent;
            }

            return null;
        }

        #region attach trigger component by trigger type
        private GalaxyTrigger NewTriggerComponent(Transform triggerTrans, GalaxyTriggerDefine.TRIGGER_TYPE type)
        {
            if (triggerTrans == null)
                return null;

            GalaxyTrigger newTriggerComponent = null;
            if(!m_registerMapping.ContainsKey(type))
            {
                GameLogger.Error(LOG_CHANNEL.TRIGGER,"Create Trigger Component failed! The unkonwn trigger type!  " + type.ToString());
                return null;
            }

            System.Type componentType = System.Type.GetType(m_registerMapping[type]);
            if (componentType == null)
                return null;

            newTriggerComponent = triggerTrans.gameObject.AddComponent(componentType) as GalaxyTrigger;
            if (newTriggerComponent == null)
                return null;

            return newTriggerComponent;
        }
        #endregion
    }
}
