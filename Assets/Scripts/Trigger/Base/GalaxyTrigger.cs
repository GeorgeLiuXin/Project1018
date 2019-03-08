using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public interface IGalaxyTrigger
    {
        GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get;
        }
        
        int TriggerCounter
        {
            get;
            set;
        }

        bool InitTrigger(TriggerParam param);

        void OnTriggerEnterCallBack(GameObject target);

        void OnTriggerLeaveCallBack(GameObject target);

        void ActiveTrigger();

        void DisableTrigger();

        void OnBeforeDestroy();
    }

    public class GalaxyTrigger : MonoBehaviour, IGalaxyTrigger
    {
        public virtual GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_TYPE_NULL;
            }
        }

        public virtual int TriggerCounter
        {
            get;
            set;
        }

        protected TriggerParam ConfigData
        {
            get;
            set;
        }

        protected bool InitEnd
        {
            get;
            set;
        }

        public bool IsScenePrefab
        {
            get;
            set;
        }

        public Transform ScenePrefab
        {
            get;
            set;
        }

        public int SpwanerId
        {
            get;
            set;
        }

        public GSpawnerState spwanerType
        {
            get;
            set;
        }

        protected LocalPlayer m_localPlayer;
        protected LocalPlayer localPlayer
        {
            get
            {
                if (m_localPlayer == null)
                {
                    m_localPlayer = GalaxyGameModule.GetGameManager<GalaxyActorManager>().GetLocalPlayer();
                }
                return m_localPlayer;
            }
        }

        protected GalaxySceneManager m_sceneMgr;
        protected GalaxySceneManager sceneMgr
        {
            get
            {
                if(m_sceneMgr == null)
                {
                    m_sceneMgr = GalaxyGameModule.GetGameManager<GalaxySceneManager>();
                }

                return m_sceneMgr;
            }
        }


        protected List<string> m_triggerTag;

        public virtual bool InitTrigger(TriggerParam param)
        {
            if (param == null)
                return false;

            if (param.triggerCount <= 0)
            {
                Destroy(this.gameObject);
                return false;
            }

            TriggerCounter = param.triggerCount;

            this.transform.position = param.vWorldPos;
            this.transform.localEulerAngles = param.vDir;
            this.transform.localScale = Vector3.one;
            ConfigData = param;
            ParseAction();
            InitTag(param);
            InitEffect(param.effectId);
            InitSpwanerData(param.spwanerId, param.spwanerType);

            InitEnd = true;
            return true;
        }

        protected virtual void ParseAction()
        {

        }

        public virtual void EnterTriggerLogic()
        {

        }

        private void InitEffect(int effectId)
        {
            ConfigData comEff = ConfigDataTableManager.Instance.GetData("commoneffects", effectId);
            if (comEff == null)
                return;

            string strPath = comEff.GetString("particle");
            ResourcesProxy.LoadAsset(strPath, OnEffectLoadEnd);
        }

        private void InitSpwanerData(int id,int type)
        {
            this.SpwanerId = id;
            this.spwanerType = (GSpawnerState)type;
        }

        private void OnEffectLoadEnd(LoadResult result)
        {
            if (!result.isSuccess)
                return;

            GameObject effObj = RoutineRunner.Instantiate<GameObject>(result.assets[0] as GameObject);
            if (effObj == null)
                return;

            effObj.transform.SetParent(this.gameObject.transform);
            effObj.transform.localPosition = Vector3.zero;
            effObj.transform.localRotation = Quaternion.identity;
            effObj.transform.localScale = Vector3.one;

            if(TriggerCounter > 0)
            {
                ActiveTrigger();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!InitEnd || TriggerCounter == 0)
                return;

            if (!CompareTagType(other))
                return;

            if (!CheckSpwanerState())
                return;

            OnTriggerEnterCallBack(other.gameObject);
            if(TriggerCounter > 0)
            {
                TriggerCounter--;
            }
            
            if (TriggerCounter == 0)
                DisableTrigger();
        }

        private void InitTriggerEffect(int effectId)
        {

        }

        private void OnTriggerExit(Collider other)
        {
            if (!InitEnd)
                return;

            OnTriggerLeaveCallBack(other.gameObject);
        }

        private void OnDestroy()
        {
            OnBeforeDestroy();
        }

        protected virtual void LosePlayerContorl(GameObject actorObject,int animID )
        {
            ActorObj actorObj = GalaxyGameModule.GetGameManager<GalaxyActorManager>().GetByClientID(actorObject.GetInstanceID()) as ActorObj;

            if (actorObj != null)
            {
                //LoseControl
                //localPlayer.m_bIsControllByLocal
                FSMParam_Idle idleParam = new FSMParam_Idle();
                idleParam.vCurPos = actorObj.GetPos();
                idleParam.vCurDir = actorObject.transform.forward;
                idleParam.nAnimID = animID;
                actorObj.SetTargetState(idleParam);
            }
        }

        protected void ResetPlayerControl(GameObject actorObject)
        {

        }

        public virtual void OnTriggerEnterCallBack(GameObject target)
        {

        }

        public virtual void OnTriggerLeaveCallBack(GameObject target)
        {

        }

        public virtual void ActiveTrigger()
        {
            if(this.gameObject.GetComponent<BoxCollider>().enabled)
                this.gameObject.GetComponent<BoxCollider>().enabled = true;

            ParticleSystem[] psList = this.gameObject.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem item in psList)
            {
                item.Play();
            }
        }

        public virtual void DisableTrigger()
        {
            ParticleSystem[] psList = this.gameObject.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem item in psList)
            {
                item.Stop();
            }

            this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        public virtual void OnBeforeDestroy()
        {
            ConfigData = null;

            if(m_triggerTag != null)
            {
                m_triggerTag.Clear();
            }
            
            ScenePrefab = null;
        }
        private void InitTag(TriggerParam param)
        {
            if(param.triggerTagType == (int)GalaxyTriggerDefine.TRIGGER_TAG_TYPE.TRIGGER_ALL_ACTOR)
            {
                m_triggerTag = new List<string>(GalaxyTriggerDefine.TagList.Length);
                m_triggerTag.AddRange(GalaxyTriggerDefine.TagList);
            }
            else
            {
                if(param.triggerTagType == (int)GalaxyTriggerDefine.TRIGGER_TAG_TYPE.TRIGGER_ALL_PLAYER)
                {
                    m_triggerTag = new List<string>(2);
                    m_triggerTag.Add(GalaxyTriggerDefine.TagList[0]);
                    m_triggerTag.Add(GalaxyTriggerDefine.TagList[1]);
                }
                else if(param.triggerTagType == (int)GalaxyTriggerDefine.TRIGGER_TAG_TYPE.TRIGGER_NPC)
                {
                    m_triggerTag = new List<string>(1);
                    m_triggerTag.Add(GalaxyTriggerDefine.TagList[2]);
                }
                else
                {
                    m_triggerTag = new List<string>();
                    if (param.triggerTagType == (int)GalaxyTriggerDefine.TRIGGER_TAG_TYPE.TRIGGER_LOCAL_PLAYER)
                    {
                        m_triggerTag.Add(GalaxyTriggerDefine.TagList[0]);
                    }
                    if(param.triggerTagType == (int)GalaxyTriggerDefine.TRIGGER_TAG_TYPE.TRIGGER_OTHER_PLAYER)
                    {
                        m_triggerTag.Add(GalaxyTriggerDefine.TagList[1]);
                    }
                }
            }
        }

        protected bool CheckSpwanerState()
        {
            if (SpwanerId == -1)
                return true;

            if (spwanerType == GSpawnerState.SpawnerInfo_Deactive)
                return true;

            if (sceneMgr.IsSpawnerFinish(SpwanerId))
            {
                return true;
            }

            return (spwanerType == sceneMgr.GetSpawnerState(SpwanerId));
        }

        protected bool CompareTagType(Collider other)
        {
            if (other == null || other.transform.parent == null || m_triggerTag.Count < 0)
                return false;

            if(ConfigData.triggerTagType > (int)GalaxyTriggerDefine.TRIGGER_TAG_TYPE.TRIGGER_ALL_PLAYER)
            {
                if (other.transform.parent.tag == StaticParam.TAG_NPC)
                {
                    ActorObj actorObj = GalaxyGameModule.GetGameManager<GalaxyActorManager>().GetByClientID(other.gameObject.GetInstanceID()) as ActorObj;
                    if (actorObj == null)
                        return false;

                    return actorObj.IsCanActiveClientTrigger();
                }
            }
            else
            {
                for (int i = 0; i < m_triggerTag.Count; ++i)
                {
                    if (other.transform.parent.tag.CompareTo(m_triggerTag[i]) == 0)
                        return true;
                }
            }

            return false;
        }
    }
}