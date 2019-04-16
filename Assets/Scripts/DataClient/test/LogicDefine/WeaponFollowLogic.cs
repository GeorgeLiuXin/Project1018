using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    [PerformanceLogicDes("武器(法宝)跟随逻辑")]
    public class WeaponFollowLogic : PerformanceLogic
    {
        [PerformanceLogicItemDes("武器跟随距离")]
        public float m_fFollowDis = 0;
        [PerformanceLogicItemDes("武器自主起伏效果高度")]
        public float m_fHeightOffset = 0;
        [PerformanceLogicItemDes("对应武器id")]
        public int m_nWeaponID = 0;

        private Transform m_playerTF;
        private Transform m_weaponTF;
        private Vector3 m_PrePos;
        
        private float m_fKeepDis;       //追到后保持的距离

        private float m_fOffsetY;
        private float m_fCurTime;
        private float m_fDefaultHeight;
        private /*const*/ float fHeightPeriod = 1;

        //自定义曲线
        //private AnimationCurve curve;
        //public void InitCurve()
        //{
        //    Keyframe[] kfs = new Keyframe[3];
        //    kfs[0] = new Keyframe(0.0f, 0.3f);
        //    kfs[0].inTangent = 1.4f;
        //    kfs[0].outTangent = 1.4f;
        //    kfs[1] = new Keyframe(0.5f, 1.0f);
        //    kfs[1].inTangent = 1.4f;
        //    kfs[1].outTangent = -1.4f;
        //    kfs[2] = new Keyframe(1.0f, 0.3f);
        //    kfs[2].inTangent = -1.4f;
        //    kfs[2].outTangent = 1.4f;
        //    curve = new AnimationCurve(kfs);
        //    curve.preWrapMode = WrapMode.Loop;
        //    curve.postWrapMode = WrapMode.Loop;
        //}

        public WeaponFollowLogic()
        {

        }

        public override void Init(params object[] values)
        {
            ActorObj actor = GalaxyGameModule.GetGameManager<GalaxyActorManager>().GetByServerID(m_OwenrID) as ActorObj;
            if (actor == null || actor.WeaponCom == null)
            {
                Destroy();
                return;
            }

            m_fOffsetY = 0;
            m_fCurTime = 0;
            m_playerTF = actor.GetEngineObj().transform;
            m_weaponTF = actor.WeaponCom.GetWeaponTF(m_nWeaponID);
            m_fDefaultHeight = m_weaponTF.position.y;
            m_PrePos = new Vector3(m_weaponTF.position.x, m_weaponTF.position.y, m_weaponTF.position.z);
            if (m_weaponTF == null)
            {
                Destroy();
                return;
            }
        }

        public override bool Tick(float fTime)
        {
            if (m_playerTF == null || m_weaponTF == null)
                return false;

            m_fCurTime += fTime;
            
            UpdateWeaponPosition();
            UpdateWeaponHeight();
            return base.Tick(fTime);
        }

        private void UpdateWeaponPosition()
        {
            Vector3 vWeaponLocalPos = m_weaponTF.localPosition;
            vWeaponLocalPos.y = 0;

            if (vWeaponLocalPos.magnitude < m_fFollowDis)
            {
                m_PrePos.y = m_fOffsetY + m_fDefaultHeight;
                m_weaponTF.position = m_PrePos;
            }
            else
            {
                vWeaponLocalPos.y = m_fOffsetY;
                m_weaponTF.localPosition = vWeaponLocalPos;
            }
            m_PrePos = m_weaponTF.position;
        }
        private void UpdateWeaponHeight()
        {
            float fProcess = (m_fCurTime / fHeightPeriod) % (2 * Mathf.PI);
            m_fOffsetY = m_fHeightOffset * Mathf.Sin(fProcess);
        }

        public override void Reset()
        {

        }

        class WeaponState
        {

        }
    }

    [PerformanceLogicDes("将任何子弹的特效产生在武器位置(子弹本身特效不配置，仅通过xml播放特效，目前仅用于无人机表现效果的特殊需求)")]
    public class ProjectileCreateAtMuzzleLogic : PerformanceLogic
    {
        [PerformanceLogicItemDes("对应子弹技能id")]
        public int m_nSkillID = 0;
        [PerformanceLogicItemDes("对应武器id")]
        public int m_nWeaponID = 0;
        [PerformanceLogicItemDes("子弹对应特效id")]
        public int m_nEffectID = 0;
        [PerformanceLogicItemDes("该表现逻辑总时长")]
        public float m_fLifeTime = 0;

        private List<ProjectileEffect> m_projectileList;

        public ProjectileCreateAtMuzzleLogic()
        {

        }

        public override void Init(params object[] values)
        {
            m_projectileList = new List<ProjectileEffect>();
            RegisterEvents();
            m_TotalTime = m_fLifeTime;
        }

        public override bool Tick(float fTime)
        {
            for (int i = m_projectileList.Count - 1; i >= 0; i--)
            {
                if (m_projectileList[i] == null)
                {
                    m_projectileList.RemoveAt(i);
                    continue;
                }

                m_projectileList[i].Tick(fTime);
                if (m_projectileList[i].m_bDestroy)
                {
                    m_projectileList.RemoveAt(i);
                    continue;
                }
            }

            return base.Tick(fTime);
        }

        public override void Reset()
        {
            UnRegisterEvents();
        }



        private GalaxyActorManager m_actorMgr;
        protected GalaxyActorManager ActorMgr
        {
            get
            {
                if (m_actorMgr == null)
                {
                    m_actorMgr = GalaxyGameModule.GetGameManager<GalaxyActorManager>();
                }
                return m_actorMgr;
            }
        }

        private void RegisterEvents()
        {
            EventListener.Instance.AddListener(CltEvent.Projectile.CREATE_PROJECTILE, OnPacketCreateProjectile);
            EventListener.Instance.AddListener(CltEvent.Projectile.DESTROY_PROJECTILE, OnPacketDestroyProjectile);
            EventListener.Instance.AddListener(CltEvent.Projectile.CLEAR_PROJECTILE, OnPacketClearProjectile);
        }

        private void UnRegisterEvents()
        {
            EventListener.Instance.RemoveListener(CltEvent.Projectile.CREATE_PROJECTILE, OnPacketCreateProjectile);
            EventListener.Instance.RemoveListener(CltEvent.Projectile.DESTROY_PROJECTILE, OnPacketDestroyProjectile);
            EventListener.Instance.RemoveListener(CltEvent.Projectile.CLEAR_PROJECTILE, OnPacketClearProjectile);
        }

        private void OnPacketCreateProjectile(object[] values)
        {
            GPacketBase pkt = values[0] as GPacketBase;
            if (pkt == null)
                return;

            if (m_OwenrID != pkt.mAvatarID || pkt.GetInt32("SkillID") != m_nSkillID)
                return;

            ActorObj pAvatar = ActorMgr.GetByServerID(pkt.mAvatarID) as ActorObj;
            if (pAvatar == null || pAvatar.skillCom == null)
                return;
            
            CreateProjectile(pkt.GetInt32("ProjectileID"));
        }
        
        private void OnPacketDestroyProjectile(object[] values)
        {
            GPacketBase pkt = values[0] as GPacketBase;
            if (pkt == null)
                return;

            if (m_OwenrID != pkt.mAvatarID || pkt.GetInt32("SkillID") != m_nSkillID)
                return;

            ActorObj pAvatar = ActorMgr.GetByServerID(pkt.mAvatarID) as ActorObj;
            if (pAvatar == null || pAvatar.skillCom == null)
                return;

            DestroyProjectile(pkt.GetInt32("ProjectileID"));
        }

        private void OnPacketClearProjectile(object[] values)
        {
            GPacketBase pkt = values[0] as GPacketBase;
            if (pkt == null)
                return;

            if (m_OwenrID != pkt.mAvatarID || pkt.GetInt32("SkillID") != m_nSkillID)
                return;

            ActorObj pAvatar = ActorMgr.GetByServerID(pkt.mAvatarID) as ActorObj;
            if (pAvatar == null || pAvatar.skillCom == null)
                return;

            ClearProjectile();
        }

        public bool CreateProjectile(int nProjectileID)
        {
            ActorObj act = ActorMgr.GetByServerID(m_OwenrID) as ActorObj;
            if (act == null || act.WeaponCom == null)
                return false;
            
            List<Transform> list = act.WeaponCom.GetWeaponAttach((int)eCombatPerformanceFireMode.FireMode_MainHand);
            if (list == null || list.Count <= 0)
                return false;

            Vector3 vPos = list[0].position;
            Vector3 vDir = Vector3.up;
            
            GameObject effectObj = GalaxyGameModule.GetGameManager<EffectManager>().GetCommonEffectByID(m_nEffectID);
            if (effectObj == null)
            {
                GameLogger.DebugLog(LOG_CHANNEL.ASSET, string.Format("ProjectileCreateAtMuzzleLogic effectID: [{0}] ,effect is null! load failed!", m_nEffectID));
                return false;
            }

            ProjectileEffect pProj = new ProjectileEffect(nProjectileID);
            pProj.InitProjectile(vPos, vDir, effectObj);
            
            m_projectileList.Add(pProj);
            return true;
        }
        public void DestroyProjectile(int nProjectileID)
        {
            foreach (ProjectileEffect pProj in m_projectileList)
            {
                if (pProj != null)
                {
                    if (pProj.m_nServerID == nProjectileID)
                    {
                        pProj.KillProjectile();
                        break;
                    }
                }
            }
        }
        public void ClearProjectile()
        {
            foreach (ProjectileEffect pProj in m_projectileList)
            {
                if (pProj != null)
                {
                    pProj.KillProjectile();
                }
            }
        }


        class ProjectileEffect
        {
            internal int m_nServerID;
            internal Vector3 m_vPos;
            internal Vector3 m_vDir;
            
            internal GameObject m_effectObj;

            public bool m_bDestroy;

            internal ProjectileEffect(int serverID)
            {
                m_nServerID = serverID;
            }

            internal void InitProjectile(Vector3 pos, Vector3 dir, GameObject effect)
            {
                m_bDestroy = false;
                m_vPos = pos;
                m_vDir = dir;
                m_effectObj = effect;
            }

            internal void Tick(float fTime)
            {

            }

            internal void KillProjectile()
            {
                GalaxyGameModule.GetGameManager<EffectManager>().ReturnEffect(m_effectObj, 0.5f);
                m_effectObj = null;
                m_bDestroy = true;
            }
        }

    }
}