using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace XWorld
{

    public class ProjectileManager : XWorldGameManagerBase
    {
        public override void InitManager()
        {
            throw new NotImplementedException();
        }

        public override void ShutDown()
        {
            throw new NotImplementedException();
        }

        public override void Update(float fElapseTimes)
        {
            throw new NotImplementedException();
        }
        //        private List<int> m_RemoveList;
        //        private GalaxyActorManager m_actorMgr;
        //        public override void InitManager()
        //        {
        //            m_bHide = false;
        //            m_vProjectileMap = new Dictionary<int, List<ProjectileClient>>();
        //            m_RemoveList = new List<int>();

        //            m_actorMgr = GalaxyGameModule.GetGameManager<GalaxyActorManager>();
        //            RegisterEvents();
        //        }

        //        private void RegisterEvents()
        //        {
        //            EventListener.Instance.AddListener(CltEvent.Projectile.CREATE_PROJECTILE, OnPacketCreateProjectile);
        //            EventListener.Instance.AddListener(CltEvent.Projectile.SYNC_PROJECTILE, OnPacketSyncProjectile);
        //            EventListener.Instance.AddListener(CltEvent.Projectile.DESTROY_PROJECTILE, OnPacketDestroyProjectile);
        //            EventListener.Instance.AddListener(CltEvent.Projectile.CLEAR_PROJECTILE, OnPacketClearProjectile);
        //        }

        //        private void UnRegisterEvents()
        //        {
        //            EventListener.Instance.RemoveListener(CltEvent.Projectile.CREATE_PROJECTILE, OnPacketCreateProjectile);
        //            EventListener.Instance.RemoveListener(CltEvent.Projectile.SYNC_PROJECTILE, OnPacketSyncProjectile);
        //            EventListener.Instance.RemoveListener(CltEvent.Projectile.DESTROY_PROJECTILE, OnPacketDestroyProjectile);
        //            EventListener.Instance.RemoveListener(CltEvent.Projectile.CLEAR_PROJECTILE, OnPacketClearProjectile);
        //        }

        //        private void OnPacketCreateProjectile(object[] values)
        //        {
        //            GPacketBase pkt = values[0] as GPacketBase;
        //            if (pkt == null)
        //                return;

        //            ActorObj pAvatar = m_actorMgr.GetByServerID(pkt.mAvatarID) as ActorObj;
        //            if (pAvatar == null || pAvatar.skillCom == null)
        //                return;

        //            int nSkillID = pkt.GetInt32("SkillID");
        //            int nSlots = pkt.GetInt32("SkillSlots");
        //            GSkillData skillData = pAvatar.skillCom.CloneSkillData(nSkillID, nSlots);
        //            if (skillData == null)
        //                return;

        //            GTargetInfo sTarInfo = new GTargetInfo();
        //            sTarInfo.m_nTargetID = pkt.GetInt32("TargetID");
        //            sTarInfo.m_vSrcPos = new Vector3(pkt.GetFloat("x"), pkt.GetFloat("z"), pkt.GetFloat("y"));
        //            sTarInfo.m_vAimDir = new Vector3(pkt.GetFloat("dx"), pkt.GetFloat("dz"), pkt.GetFloat("dy"));
        //            sTarInfo.m_vTarPos = new Vector3(pkt.GetFloat("x2"), pkt.GetFloat("z2"), pkt.GetFloat("y2"));
        //            sTarInfo.m_nFightMilliTime = pkt.GetInt32("TotalTime");
        //            CreateProjectile(pkt.mAvatarID, pkt.GetInt32("ProjectileID"), skillData, ref sTarInfo, pkt);
        //        }

        //        private void OnPacketSyncProjectile(object[] values)
        //        {
        //            GPacketBase pkt = values[0] as GPacketBase;
        //            if (pkt == null)
        //                return;

        //            ActorObj pAvatar = m_actorMgr.GetByServerID(pkt.mAvatarID) as ActorObj;
        //            if (pAvatar == null)
        //                return;

        //            int ProjectileID = pkt.GetInt32("ProjectileID");
        //            int FlightTime = pkt.GetInt32("FlyTime");
        //            SynProjectile(pkt.mAvatarID, ProjectileID, FlightTime);
        //        }

        //        private void OnPacketDestroyProjectile(object[] values)
        //        {
        //            GPacketBase pkt = values[0] as GPacketBase;
        //            if (pkt == null)
        //                return;
        //            Vector3 pos = new Vector3(pkt.GetFloat("x"), pkt.GetFloat("z"), pkt.GetFloat("y"));
        //            DestroyProjectile(pkt.mAvatarID, pkt.GetInt32("ProjectileID"), pkt.GetBool("bTimeOut"), pos);
        //        }

        //        private void OnPacketClearProjectile(object[] values)
        //        {
        //            GPacketBase pkt = values[0] as GPacketBase;
        //            if (pkt == null)
        //                return;

        //            ClearProjectile(pkt.mAvatarID);
        //        }

        //        public override void Update(float fElapseTimes)
        //        {
        //            if (m_vProjectileMap == null)
        //                return;

        //            foreach (KeyValuePair<int, List<ProjectileClient>> vList in m_vProjectileMap)
        //            {
        //                for (int i = vList.Value.Count - 1; i >= 0; i--)
        //                {
        //                    if (vList.Value[i] == null)
        //                    {
        //                        vList.Value.RemoveAt(i);
        //                        continue;
        //                    }

        //                    vList.Value[i].Tick();
        //                    if (vList.Value[i].IsDestroy())
        //                    {
        //                        vList.Value.RemoveAt(i);
        //                        continue;
        //                    }
        //                }
        //                if (vList.Value.Count == 0)
        //                {
        //                    m_RemoveList.Add(vList.Key);
        //                }
        //            }

        //            if (m_RemoveList.Count != 0)
        //            {
        //                foreach (int key in m_RemoveList)
        //                {
        //                    m_vProjectileMap.Remove(key);
        //                }
        //                m_RemoveList.Clear();
        //            }
        //        }

        //        public override void ShutDown()
        //        {
        //            UnRegisterEvents();
        //            m_vProjectileMap.Clear();
        //            m_RemoveList.Clear();
        //        }

        //        public bool CreateProjectile(int nAvatarID, int nProjectileID, GSkillData pSkillData, ref GTargetInfo sTargetInfo, GPacketBase pkt)
        //        {
        //            if (pSkillData == null)
        //                return false;
        //            GalaxyActorManager actorMgr = GalaxyGameModule.GetGameManager<GalaxyActorManager>();
        //            if (!pSkillData.IsPassiveSkill())
        //            {
        //                ActorObj act = (ActorObj)actorMgr.GetByServerID(nAvatarID);
        //                if (act != null)
        //                {
        //                    List<Transform> list = act.WeaponCom.GetWeaponAttach((int)eCombatPerformanceFireMode.FireMode_MainHand);
        //                    if (list != null && list.Count > 0)
        //                    {
        //                        sTargetInfo.m_vSrcPos = list[0].position;
        //                        //rand
        //                        sTargetInfo.m_vSrcPos.x += UnityEngine.Random.Range(0.1f, -0.1f);
        //                        sTargetInfo.m_vSrcPos.y += UnityEngine.Random.Range(0.1f, -0.1f);
        //                        sTargetInfo.m_vAimDir = list[0].up * -1;
        //                    }
        //                }
        //            }
        //            //tempcode
        //            sTargetInfo.m_vAimDir = sTargetInfo.m_vTarPos - sTargetInfo.m_vSrcPos;
        //            sTargetInfo.m_vAimDir = Vector3.Normalize(sTargetInfo.m_vAimDir);

        //            int nType = pSkillData.MSV_ProjectileLogic;

        //            ProjectileClient pProj = null;
        //            switch (nType)
        //            {
        //                case (int)eProjectileType.Projectile_Track:
        //                    pProj = new ProjectileTrack();
        //                    break;
        //                case (int)eProjectileType.Projectile_Parabola:
        //                    pProj = new ProjectileParabola();
        //                    break;
        //                case (int)eProjectileType.Projectile_Trap:
        //                    pProj = new ProjectileTrap();
        //                    break;
        //                case (int)eProjectileType.Projectile_Missile:
        //                    pProj = new ProjectileMissile();
        //                    break;
        //                case (int)eProjectileType.Projectile_Base:
        //                    pProj = new ProjectileClient();
        //                    break;
        //            }

        //            if (pProj == null || !pProj.Init(pSkillData, ref sTargetInfo, pkt))
        //            {
        //                if (pProj != null)
        //                {
        //                    pProj.KillProjectile();
        //                }
        //                return false;
        //            }

        //            pProj.m_nServerID = nProjectileID;
        //            pProj.SetOwnerID(nAvatarID);
        //            //pProj.Hide(m_bHide);

        //            if (m_vProjectileMap.ContainsKey(nAvatarID))
        //            {
        //                m_vProjectileMap[nAvatarID].Add(pProj);
        //            }
        //            else
        //            {
        //                List<ProjectileClient> projectileList = new List<ProjectileClient>();
        //                projectileList.Add(pProj);
        //                m_vProjectileMap.Add(nAvatarID, projectileList);
        //            }
        //            ActorObj actor = (ActorObj)actorMgr.GetByServerID(nAvatarID);
        //            if (null != actor && actor.m_bIsControllByLocal && null != pProj.m_pSkillData)
        //            {
        //                EventListener.Instance.Dispatch(CltEvent.Projectile.CREATE_SKILL_PROJECTILE, pProj.m_pSkillData.SkillID);
        //            }
        //            if (nType == (int)eProjectileType.Projectile_Missile)
        //            {
        //                //shake
        //                if (m_actorMgr.IsLocalPlayer(nAvatarID))
        //                {
        //                    new Galaxy.CameraShakeEffect(new Vector3(0.01f, 0.01f, 0.01f), Vector3.zero, 20, 0.15f);
        //                }
        //            }
        //            return true;
        //        }

        //        public void SynProjectile(int mAvatarID, int projectileID, int flightTime)
        //        {
        //            List<ProjectileClient> projectileList = null;
        //            if (m_vProjectileMap.TryGetValue(mAvatarID, out projectileList))
        //            {
        //                foreach (ProjectileClient item in projectileList)
        //                {
        //                    if (item.m_nServerID == projectileID)
        //                    {
        //                        item.SynTime(flightTime);
        //                    }
        //                }
        //            }

        //        }

        //        public void DestroyProjectile(int nAvatarID, int nProjectileID, bool bTimeOut, Vector3 pos)
        //        {
        //            if (m_vProjectileMap.ContainsKey(nAvatarID))
        //            {
        //                List<ProjectileClient> vList = m_vProjectileMap[nAvatarID];
        //                foreach (ProjectileClient pProj in vList)
        //                {
        //                    if (pProj != null)
        //                    {
        //                        if (pProj.m_nServerID == nProjectileID)
        //                        {
        //                            // GameLogger.Warning(LOG_CHANNEL.NETWORK, "Server KillProjectile : " + nProjectileID);
        //                            pProj.SetPos(pos);
        //                            pProj.KillProjectile(bTimeOut);
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        public void ClearProjectile(int nAvatarID)
        //        {
        //            if (m_vProjectileMap.ContainsKey(nAvatarID))
        //            {
        //                List<ProjectileClient> vList = m_vProjectileMap[nAvatarID];
        //                foreach (ProjectileClient pProj in vList)
        //                {
        //                    if (pProj != null)
        //                    {
        //                        pProj.KillProjectile();
        //                    }
        //                }
        //            }
        //        }

        //        public void Hide(bool bHide)
        //        {
        //            m_bHide = bHide;

        //            foreach (KeyValuePair<int, List<ProjectileClient>> vList in m_vProjectileMap)
        //            {
        //                foreach (ProjectileClient pProj in vList.Value)
        //                {
        //                    if (pProj != null)
        //                    {
        //                        //pProj.Hide(m_bHide);
        //                    }
        //                }
        //            }
        //        }

        //        protected Dictionary<int, List<ProjectileClient>> m_vProjectileMap;

        //        private bool m_bHide;

        //        public enum CurveType
        //        {
        //            CurveLaunch,
        //            CurveFlight,
        //            CurveAttack,
        //        }

        //        Dictionary<string, KeyFrameBasedCurve> m_vKeyCurve = new Dictionary<string, KeyFrameBasedCurve>();
        //        KeyFrameBasedCurve GetKeyFrameCurve(string pathData)
        //        {
        //            KeyFrameBasedCurve curve = null;
        //            if (m_vKeyCurve.TryGetValue(pathData, out curve))
        //            {
        //                return curve;
        //            }
        //            else
        //            {
        //#if UNITY_EDITOR
        //                string path = Application.dataPath + "/" + pathData;
        //#else
        //                string path = pathData.Substring(18);
        //#endif
        //                curve = new KeyFrameBasedCurve(path);
        //                if (curve != null)
        //                {
        //                    m_vKeyCurve.Add(pathData, curve);
        //                }
        //                return curve;
        //            }
        //        }

        //        public KeyFrameBasedCurve GetKeyFrameCurve(int curveID, CurveType type)
        //        {
        //            KeyFrameBasedCurve curve = null;
        //            ConfigData data = GetMissileCurveData(curveID);
        //            if (data == null)
        //            {
        //                return curve;
        //            }
        //            switch (type)
        //            {
        //                case CurveType.CurveLaunch:
        //                    {
        //                        curve = GetKeyFrameCurve(data.GetString("CltPath") + data.GetString("Up"));
        //                    }
        //                    break;
        //                case CurveType.CurveFlight:
        //                    {

        //                        curve = GetKeyFrameCurve(data.GetString("CltPath") + data.GetString("Fly"));
        //                    }
        //                    break;
        //                case CurveType.CurveAttack:
        //                    {
        //                        curve = GetKeyFrameCurve(data.GetString("CltPath") + data.GetString("Down"));
        //                    }
        //                    break;
        //                default:
        //                    break;
        //            }
        //            return curve;
        //        }

        //        private ConfigData GetMissileCurveData(int curveId)
        //        {
        //            return ConfigDataTableManager.Instance.GetData("missilecurve", curveId);
        //        }

        //        public List<ProjectileClient> GetProjectileListByAvatarID(int nAvatarID)
        //        {
        //            List<ProjectileClient> projectileList = null;
        //            if (m_vProjectileMap.TryGetValue(nAvatarID, out projectileList))
        //            {
        //                return projectileList;
        //            }
        //            return null;
        //        }

        //        public List<ProjectileClient> GetProjectileByAvatarSkill(int nAvatarID, int nSkillID, int sortRule = 0)
        //        {
        //            List<ProjectileClient> projectileList = GetProjectileListByAvatarID(nAvatarID);
        //            if (null == projectileList)
        //                return null;
        //            List<ProjectileClient> result = new List<ProjectileClient>();
        //            for (int i = 0; i < projectileList.Count; ++i)
        //            {
        //                if (null != projectileList[i].m_pSkillData && projectileList[i].m_pSkillData.SkillID == nSkillID)
        //                {
        //                    result.Add(projectileList[i]);
        //                }
        //            }
        //            return result;
        //            //foreach (ProjectileClient projectile in projectileList)
        //            //{
        //            //    if (null != projectile.m_pSkillData && projectile.m_pSkillData.SkillID == nSkillID)
        //            //    {
        //            //           //TODO 返回排序结果
        //            //        return projectile;
        //            //    }
        //            //}
        //            //return null;
        //        }
    }

}