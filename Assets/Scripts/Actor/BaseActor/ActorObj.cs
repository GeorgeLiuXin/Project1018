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
        public bool IsDead()
        {
            //return CheckState((int)eState.State_Dead);
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
            int nModelID = GetCurModelID();
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

        protected void OnEngineObjectLoadEnd(LoadResult result)
        {
            if (!result.isSuccess)
            {
                GameLogger.Error(LOG_CHANNEL.ERROR, "Load Engine Object Failed!  modelName : " + m_strName);
                return;
            }

            m_OriginEngineObj = result.assets[0] as GameObject;
            m_EngineObj = GameObject.Instantiate(result.assets[0] as GameObject);

            //GameLogger.LogFormat(Galaxy.LOG_CHANNEL.LOGIC, "actor name = " + result.assetNames[0]);

            if (m_EngineObj == null)
            {
                GameLogger.Error(LOG_CHANNEL.ERROR, "Create Engine Object Failed!  modelName : " + m_strName);
                return;
            }

            //GameLogger.Error(LOG_CHANNEL.ERROR, "cccwwwlll---OnEngineObjectLoadEnd!  modelName : " + m_strName);

            MakeParentObject();


            ClientID = m_EngineObj.gameObject.GetInstanceID();
            AfterEngineObjCreate();
            SetOwner(this);
            RequestCommonData();

            // TODO 暂时去掉血条,Flag
            //UIManager.Instance.Form3dManager.CreatePlayerBlood(m_EngineObj, modelData);
            GalaxyGameModule.GetGameManager<UIManager>().Form3dManager.AddView(EFormGroupType.TEAMMATE_FLAG, ClientID);
        }

        public override void Tick(float fFrameTime)
        {
            CheckDeadAndAliveState();
        }

        private void CheckDeadAndAliveState()
        {
            if (LiveState)
            {
                if (CheckState((int)eState.State_Dead))
                {
                    OnDead();
                }
            }
            else
            {
                if (CheckState((int)eState.State_Born))
                {
                    OnRelive();
                }
            }
        }

        public override void BeforeDestroy()
        {
            base.BeforeDestroy();

            GameLogicObjectPoolManager.Instance.ReturnObjectToPool(m_EngineObj, actorType);
            GalaxyGameModule.GetGameManager<UIManager>().Form3dManager.OnClientEngineDestroy(ClientID);
        }

        public void DeleteEngineObj()
        {
            if (m_EngineObj == null)
                return;

            BeforeDestroy();
            GameObject.Destroy(m_EngineObj);
            m_EngineObj = null;
        }



        public GameObject GetOriginEngineObj()
        {
            return m_OriginEngineObj;
        }

        /// <summary>
        /// 此接口下创建游戏物体的脚本
        /// </summary>
        /// 
        public override void AfterEngineObjCreate()
        {
            base.AfterEngineObjCreate();
            CollisionCom.mOnwer = this;
            GalaxyGameModule.GetGameManager<GalaxyActorManager>().UpdateClientID(ServerID, ClientID);
        }

        public void RequestCommonData()
        {
            GPacketBase pktCom = PacketDefine.CreatePacket("GPacketClientCommonDataRequest");
            if (pktCom == null)
                return;

            pktCom.SetInt32("TarAvatarID", ServerID);
            GalaxyNetManager.Instance.SendPacket(pktCom);
        }
        /// <summary>
        /// 获取当前控制卡牌
        /// </summary>
        /// <param name="battlecardidset">参数集字段，默认(=0)从自身参数池中取</param>
        /// <returns></returns>
        public int GetCurCardID(UInt64 battlecardidset = 0)   //应该是int16，为了现有代码先转成int
        {
            UInt64 val = battlecardidset;
            if (0 == val)
                val = GetBattleCardIDSet();
            UInt64 mask = (UInt64)eAvatarBattleCardIDParamDef.eBattleCardID_MASK <<
                          (int)eAvatarBattleCardIDParamDef.eBattleCardID_CURCARDID_OFFSET;
            val = (val & mask) >> (int)eAvatarBattleCardIDParamDef.eBattleCardID_CURCARDID_OFFSET;

            // TODO 填坑
            if (val <= 0)
            {
                val = 1;
            }
            return (int)(val);
        }

        /// <summary>
        /// 获取当前出战三张卡牌
        /// </summary>
        /// <param name="battlecardidset">参数集字段，默认(=0)从自身参数池中取</param>
        /// <returns>Vector的x,y,z对应当前出战卡牌1,2,3;0表示未设置</returns>
        public List<int> GetBattleCard(UInt64 battlecardidset)
        {
            UInt64 val = battlecardidset;
            List<int> ret = new List<int>();
            #region 应该封一个bitmap相关操作的接口
            UInt64 mask = (UInt64)eAvatarBattleCardIDParamDef.eBattleCardID_MASK <<
                          (int)eAvatarBattleCardIDParamDef.eBattleCardID_ACTIVE_1_OFFSET;
            var cardid = (val & mask) >> (int)eAvatarBattleCardIDParamDef.eBattleCardID_ACTIVE_1_OFFSET;
            if (0 != cardid)
                ret.Add(Convert.ToUInt16(cardid));

            mask = (UInt64)eAvatarBattleCardIDParamDef.eBattleCardID_MASK <<
                   (int)eAvatarBattleCardIDParamDef.eBattleCardID_ACTIVE_2_OFFSET;
            cardid = (val & mask) >> (int)eAvatarBattleCardIDParamDef.eBattleCardID_ACTIVE_2_OFFSET;
            if (0 != cardid)
                ret.Add(Convert.ToUInt16(cardid));

            mask = (UInt64)eAvatarBattleCardIDParamDef.eBattleCardID_MASK <<
                   (int)eAvatarBattleCardIDParamDef.eBattleCardID_ACTIVE_3_OFFSET;
            cardid = (val & mask) >> (int)eAvatarBattleCardIDParamDef.eBattleCardID_ACTIVE_3_OFFSET;
            if (0 != cardid)
                ret.Add(Convert.ToUInt16(cardid));
            #endregion
            return ret;
        }
        public List<int> GetBattleCard()
        {
            return GetBattleCard(GetBattleCardIDSet());

        }
        public UInt64 GetBattleCardIDSet()
        {
            return Pool.GetUInt64("battlecardidset");
        }
        public float GetSpeedFactor()
        {
            if (Pool == null)
                return 1.0f;

            return 1 + Pool.GetFloat("spr", 1.0f);
        }
        public virtual float GetSkillWalkSpeed()
        {
            return 0.0f;
        }
        public virtual float GetMoveSpeed()
        {
            return 0.0f;
        }
        public virtual float GetCircuitySpeed()
        {
            return 0.0f;
        }
        public int GetTargetID()
        {
            if (Pool == null)
                return -1;

            return Pool.GetInt32("target_id", 0);
        }

        public virtual float GetDodgeDis()
        {
            return 0.0f;
        }
        public virtual float GetDodgeTime()
        {
            return 0.0f;
        }
        public int GetCurModelID()
        {
            if (Pool == null)
                return -1;

            int nTmpModelID = GetTmpModelID();
            if (nTmpModelID > 0)
            {
                return nTmpModelID;
            }

            return GetModelID();
        }
        public virtual int GetModelID()
        {
            if (Pool == null)
                return -1;

            return Pool.GetInt32("modid", 1);
        }
        public int GetHairID()
        {
            if (Pool == null)
                return -1;

            return Pool.GetInt32("hairid", 0);
        }
        public int GetHeadID()
        {
            if (Pool == null)
                return -1;

            return Pool.GetInt32("faceid", 0);
        }
        public int GetBodyID()
        {
            if (Pool == null)
                return -1;

            return Pool.GetInt32("clothes", 0);
        }
        public int GetTmpModelID()
        {
            if (Pool == null)
                return -1;

            return Pool.GetInt32("tmpmodid", 0);
        }
        public void SetBlendTree_SkillMove(float fX, float fY)
        {
            if (aniCom == null)
                return;

            aniCom.SetBlendTree_SkillMove(fX, fY);
        }
        public void PlayAnimation(int animID, Vector3 vGoalPos = default(Vector3), float fMotionTime = 0, bool fPhy = false)
        {
            if (aniCom == null)
                return;

            aniCom.PlayAnimation(animID, vGoalPos, Vector3.zero, fMotionTime, fPhy);
        }
        public void ChangeAnimSpeed(float fSpeed)
        {
            if (aniCom == null)
                return;

            aniCom.ChangeAnimSpeed(fSpeed);
        }
        public bool GetAnimPos(int nAnimID, float fMotionTime, ref Vector3 vAnimDeltaPos)
        {
            if (aniCom == null)
                return false;

            return aniCom.GetAnimPos(nAnimID, fMotionTime, ref vAnimDeltaPos);
        }
        public float GetAnimTime(int animID)
        {
            if (aniCom == null)
                return 0.0f;

            return aniCom.GetAnimTime(animID);
        }
        public bool IsAnimPlaying(int animID)
        {
            if (aniCom == null)
                return false;

            return aniCom.IsAnimPlaying(animID);
        }
        public void StartShootIK(int nTargetID, int nShapeID)
        {
            if (aniCom == null)
                return;

            aniCom.StartShootIK(nTargetID, nShapeID);
        }
        public void StopShootIK()
        {
            if (aniCom == null)
                return;

            aniCom.StopShootIK();
        }
        public virtual void StartMovement(Vector3 vMoveSpeed, bool b2D = true, bool bPhy = true)
        {
            if (moveCom != null)
            {
                //if (fsmCom != null && fsmCom.m_bApproachValid)
                //{
                //    vMoveSpeed += fsmCom.m_vApproachSpeed;
                //}

                moveCom.StartMove(vMoveSpeed, b2D, bPhy);
            }
        }
        public virtual void MoveToPosition(Vector3 vTarPos)
        {
            FSMParam_Move moveParam = new FSMParam_Move();
            moveParam.nMoveMode = (int)eStateMoveMode.MoveMode_Normal;
            moveParam.vCurPos = GetPos();
            moveParam.vTarPos = vTarPos;
            SetTargetState(moveParam);
        }
        public virtual void MoveDistance(Vector3 vMotion, bool bPhy = true)
        {
            if (moveCom != null)
            {
                moveCom.MoveMotion(vMotion, bPhy);
            }
        }
        public virtual void StopMovement()
        {
            StartMovement(Vector3.zero, false, false);
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

        public void StateEvent(eStateEventType nStateEvent)
        {
            if (fsmCom == null)
                return;

            fsmCom.StateEvent(nStateEvent);
        }

        public void SkillEvent(Int32 nSkillID)
        {

        }


        public bool SetTargetState(StateParamBase StateParam, bool bForce)
        {
            return SetTargetState(StateParam, eStateMach.BaseStateMach, bForce);
        }

        public bool SetTargetState(StateParamBase StateParam, eStateMach eMach = eStateMach.BaseStateMach, bool bForce = false)
        {
            if (fsmCom == null)
                return false;

            return fsmCom.SetTargetState(StateParam, eMach, bForce);
        }

        public void ReEnterState(StateParamBase StateParam)
        {
            if (null == fsmCom)
                return;
            fsmCom.ReEnterState(StateParam);
        }

        public void SetSkillMove(bool bSkillMove)
        {
            if (fsmCom == null)
                return;

            fsmCom.m_bSkillMove = bSkillMove;
        }

        public void SyncFSMState(StateBase curState, StateParamBase curParam, eStateMach eMach)
        {
            if (!m_bIsControllByLocal)
                return;

            if (fsmCom == null)
                return;

            if (fsmCom.IsActiveState((eState)curState.m_nStateID) == false)
                return;

            GPacketBase pkt = PacketDefine.CreatePacket("GPacketFSMState");
            if (pkt == null)
                return;

            pkt.SetInt32("StateAvatarID", ServerID);
            //pkt.SetByte("CSMode", (byte)eCSMode.CSMode_Active);
            pkt.SetInt32("StateMach", (int)eMach);
            pkt.SetByte("StateID", (byte)curState.m_nStateID);
            //pkt.SetByte("StateKernelID", (byte)curState.m_nStateID);
            //pkt.SetByte("StateCommonData", (byte)0);
            //pkt.SetInt16("x", (short)(GetPos().x * 10));
            //pkt.SetInt16("y", (short)(GetPos().z * 10));
            //pkt.SetInt16("z", (short)(GetPos().y * 10));
            //pkt.SetInt16("dx", (short)(GetDir().x * 10000));
            //pkt.SetInt16("dy", (short)(GetDir().z * 10000));
            //pkt.SetInt16("dz", (short)(GetDir().y * 10000));

            byte[] buffer = new byte[512];
            int offset = 0;
            curParam.SerialParam(buffer, ref offset, true);

            pkt.WriteBuffer(buffer, (ushort)offset);

            GalaxyNetManager.Instance.SendPacket(pkt);
        }
        protected override void ParamDataChangeHandle(string name, ParamPool pool, object oldVal, object newVal)
        {
            base.ParamDataChangeHandle(name, pool, oldVal, newVal);
        }
        public void SyncFSMValue(StateBase curState)
        {
            if (!m_bIsControllByLocal)
                return;

            if (fsmCom == null)
                return;

            if (curState.m_nStateID != (int)eState.State_Skill && fsmCom.IsActiveState((eState)curState.m_nStateID) == false)
                return;

            if (curState.UpdateSendMoveSkillPacket() == true)
            {
                return;
            }

            GPacketBase pkt = PacketDefine.CreatePacket("GPacketFSMValue");
            if (pkt == null)
                return;

            pkt.SetInt32("StateAvatarID", ServerID);
            pkt.SetByte("StateID", (byte)curState.m_nStateID);
            //pkt.SetByte("StateKernelID", (byte)curState.m_nStateID);
            //pkt.SetByte("StateCommonData", (byte)0);
            pkt.SetInt16("x", (short)(GetPos().x * 10));
            pkt.SetInt16("y", (short)(GetPos().z * 10));
            pkt.SetInt16("z", (short)(GetPos().y * 10));
            pkt.SetInt16("dx", (short)(curState.GetDir().x * 10000));
            pkt.SetInt16("dy", (short)(curState.GetDir().z * 10000));
            pkt.SetInt16("dz", (short)(curState.GetDir().y * 10000));

            GalaxyNetManager.Instance.SendPacket(pkt);
        }

        //public void TrySyncFSMValue()
        //{
        //    if (fsmCom == null)
        //        return;

        //    fsmCom.TrySyncFSMValue();
        //}

        public Vector3 GetSyncSpeed()
        {
            if (fsmCom == null)
                return Vector3.zero;

            return fsmCom.m_vApproachSpeed;
        }

        public int GetCurStateID()
        {
            if (fsmCom == null)
                return (int)eState.State_Idle;

            return fsmCom.GetCurStateID();
        }

        public virtual void LockSkill(bool bLock) { }


        public AvatarStateData m_StateData = new AvatarStateData();
        public void UpdateAvatarStateData(GPacketBase pkt)
        {
            byte[] buff = new byte[pkt.mBuffSize];
            pkt.ReadBuffer(buff, pkt.mBuffSize);
            m_StateData.UpdateFromBuffer(buff);
        }
        public void SetState(int nState)
        {
            m_StateData.SetBit(nState);
        }
        public void ClearState(int nState)
        {
            m_StateData.ClearBit(nState);
        }
        public bool CheckState(int nState)
        {
            return m_StateData.CheckBit(nState);
        }

        public GameObject GetRootBone()
        {
            if (m_EngineObj == null)
                return null;

            if (m_ModelEngineObj == null)
            {
                Transform transEngineObj = m_EngineObj.transform.Find("Bip001");
                if (transEngineObj != null)
                {
                    return transEngineObj.gameObject;
                }
            }

            Transform transModelEngineObj = m_ModelEngineObj.transform.Find("Bip001");
            if (transModelEngineObj != null)
            {
                return transModelEngineObj.gameObject;
            }

            return null;
        }

        public Animator GetAnimator()
        {
            if (m_EngineObj == null)
                return null;

            if (m_ModelEngineObj == null)
                return m_EngineObj.GetComponent<Animator>();

            return m_ModelEngineObj.GetComponent<Animator>();
        }

        public virtual bool IsCanActiveClientTrigger()
        {
            return false;
        }

        public virtual float GetRemoveDelayTime()
        {
            return 0.5f;
        }

    }
}
