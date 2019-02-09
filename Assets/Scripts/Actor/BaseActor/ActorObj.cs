using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
	//所有游戏物体的基本单位
	public class ActorObj : ActorComponent
    {
        string m_strName;
        public string ModelName
        {
            get
            {
                return m_strName;
            }
            set
            {
                m_strName = value;
            }
        }

        public int ClientID;

        public bool m_bIsControllByLocal = false;
        
        public ActorObj()
        {
            RegEngineObjEvent();
        }
        
        public virtual bool IsPlayer()
        {
            return false;
        }

        public virtual bool IsNPC()
        {
            return false;
        }
        public virtual bool IsMonster()
        {
            return false;
        }
        protected void SetTag(string strTag)
        {
            GameObject engineObject = GetEngineObj();
            if (engineObject == null)
                return;

            engineObject.tag = strTag;
        }

        private void RegEngineObjEvent()
        {
        }
        private void UnRegEngineObjEvent()
        {
        }
        
        public void SetPos(Vector3 vPos, bool bOnLand = true)
        {
            if (m_EngineObj == null)
                return;
            
            m_EngineObj.transform.position = vPos;
            StopMovement();
        }
        public void SetDir(Vector3 vDir)
        {
            if (m_EngineObj == null)
                return;
            if (vDir.magnitude == 0)
                return;

            vDir.Normalize();
            Quaternion TargetRotation = Quaternion.LookRotation(vDir);
            m_EngineObj.transform.rotation = TargetRotation;
        }
        public void SetDir2D(Vector3 vDir)
        {
            if (m_EngineObj == null)
                return;
            if (vDir.magnitude == 0)
                return;

            vDir.y = 0;
            vDir.Normalize();

            Quaternion TargetRotation = Quaternion.LookRotation(vDir);
            m_EngineObj.transform.rotation = TargetRotation;
        }
        public override Vector3 GetPos()
        {
            if (m_EngineObj == null)
                return Vector3.zero;

            return m_EngineObj.transform.position;
        }
        public override Vector3 GetDir()
        {
            if (m_EngineObj == null)
                return Vector3.zero;

            Vector3 vDir = m_EngineObj.transform.TransformDirection(Vector3.forward);
            vDir.Normalize();
            return vDir;
        }

        public bool CreateEngineObj(bool bAsync)
        {
            if (m_EngineObj != null/* || GameLogicObjectPoolManager.Instance == null*/)
            {
                return true;
            }
            int nModelID = GetModelID();
            ConfigData modelData = GameDataProxy.GetData("modelresdefine", nModelID);
            if (modelData == null)
            {
                return false;
            }
            string strName = modelData.GetString("ModelName");
            m_strName = strName;

            ResourcesProxy.LoadAsset(strName, OnEngineObjectLoadEnd, null);
            return true;
        }

        protected virtual int GetModelID()
        {
            return 0;
        }

        protected void OnEngineObjectLoadEnd(LoadResult result)
        {
            if (!result.isSuccess)
            {
                GameLogger.Error(LOG_CHANNEL.ERROR, "Load Engine Object Failed!  modelName : " + m_strName);
                return;
            }

            m_ModelResObj = result.assets[0] as GameObject;
            m_EngineObj = GameObject.Instantiate(result.assets[0] as GameObject);

            //GameLogger.LogFormat(Galaxy.LOG_CHANNEL.LOGIC, "actor name = " + result.assetNames[0]);

            if (m_EngineObj == null)
            {
                GameLogger.Error(LOG_CHANNEL.ERROR, "Create Engine Object Failed!  modelName : " + m_strName);
                return;
            }

            ClientID = m_EngineObj.gameObject.GetInstanceID();
            AfterCreateEngineObj();
            SetOwner(this);
        }

        public override void Tick(float fFrameTime)
        {

        }
        
        public override void BeforeDestroy()
        {
            base.BeforeDestroy();
            //GameLogicObjectPoolManager.Instance.ReturnObjectToPool(m_EngineObj, actorType);
        }

        public void DeleteEngineObj()
        {
            if (m_EngineObj == null)
                return;

            BeforeDestroy();
            GameObject.Destroy(m_EngineObj);
            m_EngineObj = null;
        }

        /// <summary>
        /// 此接口下创建游戏物体的脚本
        /// </summary>
        public override void AfterCreateEngineObj()
        {
            base.AfterCreateEngineObj();
            //GalaxyGameModule.GetGameManager<GalaxyActorManager>().UpdateClientID(ServerID, ClientID);
        }
        
        public void PlayAnimation(int animID, Vector3 vGoalPos = default(Vector3), float fMotionTime = 0, bool fPhy = false)
        {
            //if (aniCom == null)
            //    return;
            //aniCom.PlayAnimation(animID, vGoalPos, Vector3.zero, fMotionTime, fPhy);
        }
        public virtual void StartMovement(Vector3 vMoveSpeed, bool b2D = true, bool bPhy = true)
        {
            //if (moveCom != null)
            //{
            //    //if (fsmCom != null && fsmCom.m_bApproachValid)
            //    //{
            //    //    vMoveSpeed += fsmCom.m_vApproachSpeed;
            //    //}

            //    moveCom.StartMove(vMoveSpeed, b2D, bPhy);
            //}
        }
        public virtual void MoveToPosition(Vector3 vTarPos)
        {
            //FSMParam_Move moveParam = new FSMParam_Move();
            //moveParam.nMoveMode = (int)eStateMoveMode.MoveMode_Normal;
            //moveParam.vCurPos = GetPos();
            //moveParam.vTarPos = vTarPos;
            //SetTargetState(moveParam);
        }
        public virtual void MoveDistance(Vector3 vMotion, bool bPhy = true)
        {
            //if (moveCom != null)
            //{
            //    moveCom.MoveMotion(vMotion, bPhy);
            //}
        }
        public virtual void StopMovement()
        {
            //StartMovement(Vector3.zero, false, false);
        }

        public virtual void SmoothRotated(Vector3 vDestDir)
        {
            if (vDestDir == Vector3.zero)
                return;

            if (m_EngineObj == null)
                return;

            Quaternion TargetRotation = Quaternion.LookRotation(vDestDir);
            m_EngineObj.transform.rotation = Quaternion.Slerp(m_EngineObj.transform.rotation, TargetRotation, Time.deltaTime * 10.0f);
        }

        public int GetCurStateID()
        {
            //if (fsmCom == null)
            //    return (int)eState.State_Idle;

            //return fsmCom.GetCurStateID();
            return 0;
        }
        
        public void SetState(int nState)
        {
            //m_StateData.SetBit(nState);
            return;
        }
        public void ClearState(int nState)
        {
            //m_StateData.ClearBit(nState);
            return;
        }
        public bool CheckState(int nState)
        {
            //return m_StateData.CheckBit(nState);
            return false;
        }
        public bool IsDead()
        {
            //return CheckState((int)eState.State_Dead);
            return false;
        }
        public virtual bool IsCanActiveClientTrigger()
        {
            return false;
        }

        public Animator GetAnimator()
        {
            if (m_EngineObj != null)
                return m_EngineObj.GetComponent<Animator>();

            return null;
        }

    }
}
