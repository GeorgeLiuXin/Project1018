//using UnityEngine;
//using System.Collections;
//using System;
//using CurveSystem;

namespace XWorld
{

    public class ProjectileClient
    {

        //        protected GalaxyActorManager m_actorManager;
        //        protected ProjectileManager m_projectMgr;
        //        public ProjectileManager projectMgr
        //        {
        //            get
        //            {
        //                if (m_projectMgr == null)
        //                {
        //                    m_projectMgr = GalaxyGameModule.GetGameManager<ProjectileManager>();
        //                }
        //                return m_projectMgr;
        //            }
        //        }

        //        private EffectManager m_effMgr;
        //        public EffectManager effeMgr
        //        {
        //            get
        //            {
        //                if (m_effMgr == null)
        //                {
        //                    m_effMgr = Galaxy.GalaxyGameModule.GetGameManager<Galaxy.EffectManager>();
        //                }
        //                return m_effMgr;
        //            }
        //        }

        //        public ProjectileClient()
        //		{
        //			m_bDestroy = false;
        //			m_bHitNoRemove = false;
        //			m_nLifeTime = 0;
        //			m_pSkillData = null;
        //			m_nDieEffectID = -1;
        //			m_fSpeed = 0.0f;
        //			m_nCurTime = 0;
        //			m_nServerID = 0;
        //			m_fRadian = 0;
        //			m_nOwnerID = -1;
        //            m_actorManager = GalaxyGameModule.GetGameManager<GalaxyActorManager>();
        //            m_hitType = (int)eProjectileHitType.ProjectileHitType_None;
        //        }

        //		public virtual bool Init(GSkillData pSkillData, ref GTargetInfo sTargetInfo, GPacketBase pkt)
        //		{
        //			if(pSkillData == null)
        //				return false;

        //			m_pSkillData = pSkillData;
        //			m_TargetInfo = sTargetInfo;
        //            m_vLastPos = m_vCurPos = m_TargetInfo.m_vSrcPos;
        //            //sTargetInfo.m_vAimDir = sTargetInfo.m_vTarPos - sTargetInfo.m_vSrcPos;
        //           // sTargetInfo.m_vAimDir = Vector3.Normalize(sTargetInfo.m_vAimDir);
        //            m_vLastPos -= sTargetInfo.m_vAimDir;
        //            m_bHitNoRemove = pSkillData.IsBulletHitNoRemove();
        //			m_fSpeed = pSkillData.MSV_ProjectileSpeed;
        //			m_nLifeTime = pSkillData.MSV_ProjectileTime;
        //			m_nDieEffectID = pSkillData.MSV_ProjectileDieEffectID;
        //            m_hitType = (int)eProjectileHitType.ProjectileHitType_None;

        //            m_FlyEffect = effeMgr.GetCommonEffectByID(pSkillData.MSV_ProjectileEffectID);
        //            if (m_FlyEffect != null)
        //            {
        //                m_FlyEffect.transform.position = sTargetInfo.m_vSrcPos;
        //                m_FlyEffect.transform.forward = sTargetInfo.m_vAimDir;
        //                m_FlyEffect.transform.localScale = Vector3.one;
        //            }
        //            GameObject createEff = effeMgr.GetCommonEffectByID(pSkillData.MSV_ProjectileFly1EffectID);
        //            if (createEff != null)
        //            {
        //                createEff.transform.position = sTargetInfo.m_vSrcPos;
        //                createEff.transform.forward = sTargetInfo.m_vAimDir;
        //                createEff.transform.localScale = Vector3.one;
        //                effeMgr.ReturnEffect(createEff, 3);
        //            }

        //            return true;
        //		}

        //		public void Tick()
        //		{
        //			if(IsDestroy())
        //			{
        //				return;
        //			}

        //            m_nCurTime += (int)(Time.deltaTime * 1000);

        //            if (m_FlyEffect)
        //            {
        //                m_FlyEffect.transform.forward = Vector3.Normalize(m_vCurPos - m_vLastPos);
        //                m_FlyEffect.transform.position = m_vCurPos;
        //            }

        //            if (m_fSpeed > 0)
        //			{
        //                Vector3 oldPos = m_vCurPos;
        //                TickMove(Time.deltaTime);
        //                m_vLastPos = oldPos;
        //            }

        //            if (Raycast())
        //            {
        //                if (m_FlyEffect)
        //                {
        //                    effeMgr.ReturnEffect(m_FlyEffect, 0.7f);
        //                    m_FlyEffect = null;
        //                }
        //            }

        //            if (m_nCurTime > (m_nLifeTime+100))
        //            {
        //                KillProjectile(true);
        //            }

        //            return;
        //		}

        //		public virtual void TickMove(float fFrameTime)
        //        {
        //            float fLength = fFrameTime * m_fSpeed;
        //            Vector3 dir = m_TargetInfo.m_vAimDir * fLength;
        //            m_vCurPos += dir;
        //        }

        //        public virtual bool Raycast()
        //        {
        //            GalaxySceneManager SceneMgr = GalaxyGameModule.GetGameManager<GalaxySceneManager>();
        //            return SceneMgr.NavGrid.RayCast(m_vLastPos, ref m_vCurPos);
        //        }

        //		public void SetOwnerID(int nOwnerID) { m_nOwnerID = nOwnerID; }
        //		public int GetOwnerID() { return m_nOwnerID; }

        //		public bool IsDestroy() { return m_bDestroy; }

        //        public void KillProjectile(bool bTimeOut = false)
        //        {
        //            m_bDestroy = true;
        //            GameObject eff = GetDieProjectileEffect(bTimeOut);

        //            if (m_FlyEffect)
        //            { 
        //                //m_FlyEffect.SetActive(false);
        //                effeMgr.ReturnEffect(m_FlyEffect,0.7f);
        //                m_FlyEffect = null;
        //            }
        //            if (projectMgr != null || effeMgr != null)
        //            {
        //                if (eff)
        //                {              
        //                    effeMgr.ReturnEffect(eff, 3);
        //                }
        //            }
        //        }

        //        void ShowEffectByTag(GameObject eff, string stag)
        //        {
        //            eff.SetActive(true);
        //            foreach (var item in eff.GetComponentsInChildren<Transform>(true))
        //            {
        //                if (item.tag == stag || item.tag == "Untagged")
        //                {
        //                    item.gameObject.SetActive(true);
        //                }
        //                else
        //                {
        //                    item.gameObject.SetActive(false);
        //                }
        //            }
        //        }

        //        private GameObject GetDieProjectileEffect(bool bTimeOut)
        //        {
        //            int effectID = m_pSkillData.MSV_ProjectileDieEffectID;
        //            if (bTimeOut)
        //            {
        //                effectID = m_pSkillData.MSV_ProjectileTimeOutEffectID;
        //            }
        //            GameObject eff = effeMgr.GetCommonEffectByID(effectID);
        //            if (eff == null)
        //            {
        //                return null;
        //            }
        //            Vector3 pos = GetPos() - GetDir();
        //            Ray ray = new Ray(pos, GetDir());
        //            RaycastHit hitInfo;
        //            Vector3 dir = GetDir() * -1;
        //            bool result = Physics.Raycast(ray, out hitInfo, 4.0f);
        //            string tag = "dirt";
        //            if (result)
        //            {
        //                pos = hitInfo.point;
        //                dir = hitInfo.normal;
        //                //之后根据tag标签设置对应的材质
        //                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Water"))
        //                {
        //                    tag = "water";
        //                }
        //                else if(hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Terrain") || hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Building"))
        //                {
        //                    if (hitInfo.collider.tag == "matel")
        //                    {
        //                        tag = "spark";
        //                    }
        //                }
        //                else
        //                {
        //                    tag = "blood";
        //                    //tag = "dirt";                    
        //                }
        //            }
        //            else
        //            {
        //                pos = GetPos();
        //                dir = m_TargetInfo.m_vAimDir;
        //            }
        //            ShowEffectByTag(eff, tag);
        //            eff.transform.position = pos;
        //            eff.transform.up = dir;
        //            return eff;
        //        }

        //        /// <summary>
        //        /// 根据材质返回对应的材质效果
        //        /// </summary>
        //        protected void SetEffectIDByHitMaterialType(object[] values = null)
        //        {

        //        }

        //        public virtual void SynTime(int flightTime)
        //        {

        //        }

        //		public Vector3 GetPos() { return m_vCurPos; }
        //        public void SetPos(Vector3 pos) { m_vCurPos = pos; }
        //        public Vector3 GetDir() { return m_TargetInfo.m_vAimDir; }
        //		public Vector3 GetLastPos() { return m_vLastPos; }
        //		public int GetTargetID() { return m_TargetInfo.m_nTargetID; }

        //		protected bool m_bDestroy;
        //		protected bool m_bHitNoRemove;

        //        protected GameObject m_FlyEffect = null;
        //        public int m_nDieEffectID { get; set; }

        //		public float m_fSpeed { get; set; }
        //		public int m_nLifeTime { get; set; }
        //		protected float m_nCurTime;

        //		public int m_nServerID { get; set; }
        //		protected int m_nOwnerID;

        //		//protected tSoundID m_soundID;
        //		public float m_fRadian { get; set; }

        //		protected Vector3 m_vLastPos;
        //        protected Vector3 m_vCurPos;
        //        protected GTargetInfo m_TargetInfo;
        //		public GSkillData m_pSkillData;

        //        protected int m_hitType;
        //	}

        //	#region 具体子弹逻辑

        //	//////////////////////////////////////////////////////////////////////////
        //	//追踪子弹
        //	public class ProjectileTrack : ProjectileClient
        //	{
        //		public ProjectileTrack() : base() { }

        //		~ProjectileTrack() { }

        //		public override void TickMove(float fFrameTime)
        //		{

        //			Vector3 vTarPos = m_TargetInfo.m_vTarPos;
        //			if(m_pSkillData.IsTargetAvatar())
        //			{
        //				ActorObj pTarget = m_actorManager.GetByServerID(GetTargetID()) as ActorObj;
        //				if(pTarget == null)
        //				{
        //					KillProjectile();
        //					return;
        //				}

        //				vTarPos = pTarget.GetPos();
        //				if(pTarget.GetEngineObj())
        //				{
        //					vTarPos = pTarget.GetEngineObj().transform.position;
        //				}
        //				vTarPos.y += 1.0f; //增加高度偏移
        //			}

        //			if(m_pSkillData.IsTargetDir())
        //			{
        //				float fLength = fFrameTime * m_fSpeed;
        //				Vector3 vDir = m_TargetInfo.m_vAimDir;
        //				vDir.y = 0.0f; //方向类子弹锁定Z轴
        //				vDir.Normalize();
        //				vDir *= fLength;
        //                m_vCurPos = m_vCurPos + vDir;
        //				m_TargetInfo.m_vTarPos = vTarPos;

        //                if (m_FlyEffect != null)
        //                    m_FlyEffect.transform.position = m_vCurPos;
        //            }
        //			else if(m_pSkillData.IsTargetAvatar() || m_pSkillData.IsTargetPos())
        //			{
        //				float fLength = fFrameTime * m_fSpeed;
        //				float fDistance = Vector3.Distance(vTarPos, m_vCurPos);
        //				if(fLength >= fDistance)
        //				{
        //                    m_vCurPos = vTarPos;
        //					m_TargetInfo.m_vTarPos = vTarPos;
        //					if(!m_bHitNoRemove)
        //					{
        //						KillProjectile();
        //					}
        //					return;
        //				}
        //				else
        //				{
        //					Vector3 vDir = vTarPos - m_vCurPos;
        //					vDir.Normalize();
        //					vDir *= fLength;
        //                    m_vCurPos = m_vCurPos + vDir;
        //					m_TargetInfo.m_vTarPos = vTarPos;

        //                    if (m_FlyEffect != null)
        //                        m_FlyEffect.transform.position = m_vCurPos;
        //                }
        //			}

        //		}
        //	}
        //	//////////////////////////////////////////////////////////////////////////
        //	//抛物线子弹
        //	public class ProjectileParabola : ProjectileClient
        //	{
        //		public ProjectileParabola() : base()
        //		{
        //			m_fRaHeight = 0f;
        //			m_fUpTime = 0f;
        //			m_fDownTime = 0f;
        //			m_vDir = Vector3.zero;
        //			m_vPos = Vector3.zero;
        //		}

        //		~ProjectileParabola() { }

        //		public override bool Init(GSkillData pSkillData, ref GTargetInfo sTargetInfo, GPacketBase pkt)
        //		{
        //			if(!base.Init(pSkillData, ref sTargetInfo, pkt))
        //				return false;

        //			if(!pSkillData.IsTargetPos())
        //				return false;

        //			if(m_fSpeed <= 0.0001)
        //				return false;

        //			m_fRaHeight = pSkillData.MSV_ProjectileParam1;
        //			m_fRaHeight = Mathf.Max(0, Mathf.Min(m_fRaHeight, 10));

        //			Vector3 vSrcPos = m_vCurPos;
        //			Vector3 vTarPos = m_TargetInfo.m_vTarPos;

        //			float fDistance = Vector3.Distance(vTarPos, vSrcPos);
        //			m_vDir = vTarPos - vSrcPos;
        //			m_vDir.Normalize();

        //			m_vPos = vSrcPos + m_vPos * fDistance * 0.5f;
        //			m_vPos.z += fDistance * m_fRaHeight / 10.0f;

        //			m_fUpTime = Vector3.Distance(m_vPos, vSrcPos) / m_fSpeed;
        //			m_fDownTime = Vector3.Distance(m_vPos, vTarPos) / m_fSpeed;
        //			return true;
        //		}

        //		public override void TickMove(float fFrameTime)
        //		{
        //			if(m_nCurTime >= m_nLifeTime)
        //				return;

        //			Vector3 vSrcPos = m_vCurPos;
        //			Vector3 vTarPos = m_TargetInfo.m_vTarPos;

        //			float fLength = fFrameTime * m_fSpeed;
        //			if(fLength * 0.5f > 0.5f)
        //			{
        //				fLength += 0.5f;
        //			}

        //			float fDistance = Vector3.Distance(vTarPos, vSrcPos);
        //			if(fLength >= fDistance || m_nCurTime / 1000.0f >= m_fUpTime + m_fDownTime)
        //			{
        //                m_vCurPos = vTarPos;
        //				m_TargetInfo.m_vTarPos = vTarPos;
        //				if(!m_bHitNoRemove)
        //				{
        //					KillProjectile();
        //				}
        //				return;
        //			}
        //			else
        //			{
        //                Vector3 oldPos = m_FlyEffect.transform.position;
        //                m_FlyEffect.transform.position = new Vector3(oldPos.x + fLength, oldPos.y + fLength, oldPos.z);
        //                //暂时
        //                //Vector3 vCurPos = Vector3.zero;
        //                //if (CCurveSimulation::Parabola_GetPos(vSrcPos, m_vPos, vTarPos, m_fUpTime, m_fDownTime, m_nCurTime / 1000.0f, vCurPos))
        //                //{
        //                //    m_vCurPos = vCurPos;
        //                //}
        //            }
        //        }

        //		private float m_fRaHeight;
        //		private float m_fUpTime;
        //		private float m_fDownTime;
        //		private Vector3 m_vDir;
        //		private Vector3 m_vPos;
        //	}
        //	//////////////////////////////////////////////////////////////////////////
        //	//陷阱子弹
        //	public class ProjectileTrap : ProjectileClient
        //	{
        //		public ProjectileTrap() : base() { }

        //        public override bool Init(GSkillData pSkillData, ref GTargetInfo sTargetInfo, GPacketBase pkt)
        //        {
        //            bool res = base.Init(pSkillData, ref sTargetInfo, pkt);
        //            Galaxy.GalaxySceneManager mgr = Galaxy.GalaxyGameModule.GetGameManager<Galaxy.GalaxySceneManager>();
        //            if (mgr != null && m_FlyEffect != null)
        //            {
        //                float height = mgr.NavGrid.GetHeight(sTargetInfo.m_vSrcPos);
        //                m_FlyEffect.transform.position = new Vector3(sTargetInfo.m_vSrcPos.x, height, sTargetInfo.m_vSrcPos.z); // 使用场景计算的高度
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }

        //        }

        //        ~ProjectileTrap() { }

        //        public override bool Raycast()
        //        {
        //            return false;
        //        }
        //    }

        //    //////////////////////////////////////////////////////////////////////////
        //    //导弹子弹
        //    public class ProjectileMissile : ProjectileClient
        //    {
        //        public ProjectileMissile() : base() { }

        //        private GPacketDefineManager m_packetDefine;
        //        protected GPacketDefineManager PacketCreator
        //        {
        //            get
        //            {
        //                if (m_packetDefine == null)
        //                {
        //                    m_packetDefine = GalaxyGameModule.GetGameManager<GPacketDefineManager>();
        //                }
        //                return m_packetDefine;
        //            }
        //        }

        //        private GalaxyActorManager m_actorMgr;
        //        public GalaxyActorManager ActorMgr
        //        {
        //            get
        //            {
        //                if (m_actorMgr == null)
        //                {
        //                    m_actorMgr = GalaxyGameModule.GetGameManager<GalaxyActorManager>();
        //                }
        //                return m_actorMgr;
        //            }
        //        }

        //        private RetargetableCurve m_curve;
        //        private CurveStage m_CurveStage;
        //        int launchTime;
        //        int attackTime;
        //        int flyTime;
        //        enum CurveStage
        //        {
        //            Launch ,
        //            Flight ,
        //            Attack ,
        //        }

        //        public override bool Init(GSkillData pSkillData, ref GTargetInfo sTargetInfo, GPacketBase pkt)
        //        {
        //            bool res = base.Init(pSkillData, ref sTargetInfo, pkt);
        //            if (res == false)
        //            {
        //                return false;
        //            }
        //            var builder = new RetargetableCurve.PieceWiseCurveBuilder();

        //            int curveId = pkt.GetInt32("curveID");
        //            float lrat = ((float)pSkillData.MSV_LauncherParam2 / 100);
        //            float arat = ((float)pSkillData.MSV_LauncherParam3 / 100);
        //            float frat = 1.0f - lrat - arat;
        //            if (frat < 0.1f)
        //            {
        //                frat = 0.1f;
        //            }
        //            launchTime = (int)((float)m_TargetInfo.m_nFightMilliTime * lrat);
        //            attackTime = (int)((float)m_TargetInfo.m_nFightMilliTime * arat);
        //            flyTime = (int)((float)m_TargetInfo.m_nFightMilliTime * frat);
        //            if (launchTime>0)
        //            {
        //                builder.AddPiece(projectMgr.GetKeyFrameCurve(curveId, ProjectileManager.CurveType.CurveLaunch), launchTime);
        //            }
        //            if (flyTime > 0)
        //            {
        //                builder.AddPiece(projectMgr.GetKeyFrameCurve(curveId, ProjectileManager.CurveType.CurveFlight), flyTime);
        //            }
        //            if (attackTime>0)
        //            {
        //                builder.AddPiece(projectMgr.GetKeyFrameCurve(curveId, ProjectileManager.CurveType.CurveAttack), attackTime);
        //            }
        //            m_nLifeTime = m_TargetInfo.m_nFightMilliTime;
        //            m_curve = builder.Build();
        //            m_CurveStage = CurveStage.Launch;
        //            if (launchTime<=0)
        //            {
        //                m_CurveStage = CurveStage.Flight;
        //            }

        //            //XTODO
        //            //临时代码  需要测试得到用那种方式比较
        //            //1、客户端一旦碰撞立刻销毁
        //            //2、非本地客户端的子弹通过服务器销毁消息销毁
        //            //3、第三种完全根据服务器来设置       (当前模式)
        //            //当前为1
        //            //第一种
        //            //if (m_FlyEffect != null)
        //            //{
        //            //    EffectProjectile script = m_FlyEffect.GetOrAddComponent<EffectProjectile>();
        //            //    script.OnHitSomething = this.HitEffect;
        //            //}
        //            //第二种
        //            //if(ActorMgr.IsLocalPlayer(GetOwnerID()))
        //            //{
        //            //    if (m_FlyEffect != null)
        //            //    {
        //            //        EffectProjectile script = m_FlyEffect.GetOrAddComponent<EffectProjectile>();
        //            //        script.OnHitSomething = this.HitEffect;
        //            //    }
        //            //}
        //            //else
        //            //{
        //            //    if (m_FlyEffect != null)
        //            //    {
        //            //        EffectProjectile script = m_FlyEffect.GetOrAddComponent<EffectProjectile>();
        //            //        script.SetMaterialType = this.SetEffectIDByHitMaterialType;
        //            //    }
        //            //}

        //            return res;
        //        }

        //        public override void TickMove(float fFrameTime)
        //        {
        //            if (m_curve != null)
        //            {
        //                if (m_nCurTime > (launchTime + flyTime + attackTime))
        //                {
        //                    Vector3 dir = Vector3.Normalize(m_vCurPos - m_vLastPos) * (m_fSpeed * fFrameTime);
        //                    m_vCurPos += dir;
        //                }
        //                else
        //                {
        //                    m_vCurPos = m_curve.GetPosition(m_TargetInfo.m_vSrcPos, m_TargetInfo.m_vTarPos, m_nCurTime);
        //                }

        //                switch (m_CurveStage)
        //                {
        //                    case CurveStage.Launch:
        //                        {
        //                            if (m_nCurTime > launchTime)
        //                            {
        //                                m_CurveStage = CurveStage.Flight;
        //                            }
        //                        }
        //                        break;
        //                    case CurveStage.Flight:
        //                        {
        //                            if (m_nCurTime > flyTime + launchTime)
        //                            {
        //                                m_CurveStage = CurveStage.Attack;
        //                            }
        //                        }
        //                        break;
        //                    case CurveStage.Attack:                        
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //        }

        //        public override void SynTime(int flightTime)
        //        {
        //            // 收到包说明应该进入下一阶段，根据时间判断阶段，不依赖于客户端的阶段判断

        //        }

        //        /// <summary>
        //        /// 根据子弹当前碰撞的东西的tag进行分类处理并标识出来
        //        /// </summary>
        //        private void HitEffect(object[] objs)
        //        {
        //            //第一种
        //            SetEffectIDByHitMaterialType(objs);
        //            if (ActorMgr.IsLocalPlayer(GetOwnerID()))
        //            {
        //                SendDestroyProjectile();
        //            }
        //            //GameLogger.Warning(LOG_CHANNEL.NETWORK, "Client Hit KillProjectile : ");
        //            KillProjectile();
        //            //第二种
        //            //SetEffectIDByHitMaterialType(objs);
        //            //SendDestroyProjectile();
        //        }

        //        private void SendDestroyProjectile()
        //        {
        //            if (m_FlyEffect == null)
        //                return;

        //            GPacketBase pkt = PacketCreator.CreatePacket("GPacketDestroyProjectile");
        //            if (pkt == null)
        //                return;

        //            pkt.SetInt32("SkillID", m_pSkillData.SkillID);
        //            pkt.SetInt32("ProjectileID", m_nServerID);
        //            pkt.SetBool("bTimeOut", false);

        //            GalaxyNetManager.Instance.SendPacket(pkt);
        //        }

        //        ~ProjectileMissile() { }
    }
//#endregion

}
