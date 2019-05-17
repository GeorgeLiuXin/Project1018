using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Galaxy
{
    /// <summary>
    /// SkillArea_Singleton = 1, //单体
    /// SkillArea_Sphere = 2, //球形范围
    /// SkillArea_Sector = 3, //扇形范围
    /// 参数1：最近边距自己的距离，参数2：半径，参数3：弧度
    /// SkillArea_Ring = 4, //环形范围
    /// SkillArea_Rect = 5, //矩形范围
    /// 参数1：最近边距自己的距离，参数2：最远边距自己的距离，参数3：宽度
    /// </summary>
    public class GColliderManager : GalaxyGameManagerBase
    {
        protected GalaxyActorManager m_actorMgr;
        public GalaxyActorManager ActorMgr
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

        public List<GalaxyActor> CollideCheck(SSphere sSphere, Vector3 vDir)
        {
            List<GalaxyActor> pActorList = ActorMgr.GetAllActor();
            if (pActorList == null || pActorList.Count <= 0)
                return null;
            List<GalaxyActor> list = new List<GalaxyActor>();
            foreach (GalaxyActor actor in pActorList)
            {
                if (ActorMgr.IsLocalPlayer(actor.ServerID))
                    continue;
                ActorObj obj = (ActorObj)actor;
                if (obj.CollisionCom == null)
                {
                    continue;
                }
                for (int i = 0; i < obj.CollisionCom.GetShapeCount(); i++)
                {
                    SShapeData shp = obj.CollisionCom.GetShape(i);
                    if (shp != null)
                    {
                        SSphere sTarSphere = new SSphere(shp.Pos, shp.r);
                        if (GCollider.SphereCollideCheck(sSphere, sTarSphere, vDir))
                        {
                            list.Add(actor);
                            break;
                        }
                    }
                }
            }
            return list;
        }
        public List<GalaxyActor> CollideCheck(SRect sRect, Vector3 vDir)
        {
            List<GalaxyActor> pActorList = ActorMgr.GetAllActor();
            if (pActorList == null || pActorList.Count <= 0)
                return null;
            List<GalaxyActor> list = new List<GalaxyActor>();
            foreach (GalaxyActor actor in pActorList)
            {
                if (ActorMgr.IsLocalPlayer(actor.ServerID))
                    continue;
                ActorObj obj = (ActorObj)actor;
                if (obj.CollisionCom == null)
                {
                    continue;
                }
                for (int i = 0; i < obj.CollisionCom.GetShapeCount(); i++)
                {
                    SShapeData shp = obj.CollisionCom.GetShape(i);
                    if (shp != null)
                    {
                        SSphere sTarSphere = new SSphere(shp.Pos, shp.r);
                        if (GCollider.RectCollideCheck(sRect, sTarSphere, vDir))
                        {
                            list.Add(actor);
                            break;
                        }
                    }
                }
            }
            return list;
        }
        public List<GalaxyActor> CollideCheck(SSector sSector, Vector3 vDir)
        {
            List<GalaxyActor> pActorList = ActorMgr.GetAllActor();
            if (pActorList == null || pActorList.Count <= 0)
                return null;
            List<GalaxyActor> list = new List<GalaxyActor>();
            foreach (GalaxyActor actor in pActorList)
            {
                if (ActorMgr.IsLocalPlayer(actor.ServerID))
                    continue;
                ActorObj obj = (ActorObj)actor;
                if (obj.CollisionCom == null)
                {
                    continue;
                }
                for (int i = 0; i < obj.CollisionCom.GetShapeCount(); i++)
                {
                    SShapeData shp = obj.CollisionCom.GetShape(i);
                    if (shp != null)
                    {
                        SSphere sTarSphere = new SSphere(shp.Pos, shp.r);
                        if (GCollider.SectorCollideCheck(sSector, sTarSphere, vDir))
                        {
                            list.Add(actor);
                            break;
                        }
                    }
                }

            }
            return list;
        }
        public List<GalaxyActor> CollideCheck(SRing sRing, Vector3 vDir)
        {
            List<GalaxyActor> pActorList = ActorMgr.GetAllActor();
            if (pActorList == null || pActorList.Count <= 0)
                return null;
            List<GalaxyActor> list = new List<GalaxyActor>();
            foreach (GalaxyActor actor in pActorList)
            {
                if (ActorMgr.IsLocalPlayer(actor.ServerID))
                    continue;
                ActorObj obj = (ActorObj)actor;
                if (obj.CollisionCom == null)
                {
                    continue;
                }
                for (int i = 0; i < obj.CollisionCom.GetShapeCount(); i++)
                {
                    SShapeData shp = obj.CollisionCom.GetShape(i);
                    if (shp != null)
                    {
                        SSphere sTarSphere = new SSphere(shp.Pos, shp.r);
                        if (GCollider.RingCollideCheck(sRing, sTarSphere, vDir))
                        {
                            list.Add(actor);
                            break;
                        }
                    }
                }
            }
            return list;
        }


        private Dictionary<int, GSkillAreaLogic> m_vAreaLogicDict;
        
        public override void InitManager()
        {
            m_vAreaLogicDict = new Dictionary<int, GSkillAreaLogic>();
            m_vAreaLogicDict.Add((int)eSkillAreaLogic.SkillArea_Singleton, new GSkillAreaSingelton());
            m_vAreaLogicDict.Add((int)eSkillAreaLogic.SkillArea_Sphere, new GSkillAreaSphere());
            m_vAreaLogicDict.Add((int)eSkillAreaLogic.SkillArea_Sector, new GSkillAreaSector());
            m_vAreaLogicDict.Add((int)eSkillAreaLogic.SkillArea_Ring, new GSkillAreaRing());
            m_vAreaLogicDict.Add((int)eSkillAreaLogic.SkillArea_Rect, new GSkillAreaRect());
        }

        public override void ShutDown()
        {
            m_vAreaLogicDict.Clear();
            m_vAreaLogicDict = null;
        }

        public override void Update(float fElapseTimes)
        {

        }

        public List<GalaxyActor> CalculationHit(GSkillData pSkillData, GTargetInfo targetInfo)
        {
            var areaLogicID = pSkillData.MSV_AreaLogic;
            if ((int) eSkillAreaLogic.SkillArea_Min < areaLogicID && areaLogicID < (int) eSkillAreaLogic.SkillArea_Max)
            {
                return m_vAreaLogicDict[pSkillData.MSV_AreaLogic].CalculationHit(pSkillData, targetInfo);
            }

            return null;
        }

    }

    public class GSkillAreaLogic
    {
        #region 相关Manager

        protected GalaxyActorManager m_actorMgr;
        public GalaxyActorManager ActorMgr
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

        #endregion

        protected ActorObj m_pLocalPlayer;

        public GSkillAreaLogic()
        {
            m_pLocalPlayer = GalaxyGameModule.GetGameManager<GalaxyActorManager>().GetLocalPlayer();
        }

        public virtual List<GalaxyActor> CalculationHit(GSkillData skilldata, GTargetInfo targetInfo)
        {
            return null;
        }
    }


    public class GSkillAreaSingelton : GSkillAreaLogic
    {
        public override List<GalaxyActor> CalculationHit(GSkillData skilldata, GTargetInfo targetInfo)
        {
           
            ActorObj act = ActorMgr.GetByServerID(targetInfo.m_nTargetID) as ActorObj;
            if (act == null)
            {
                return null;
            }
            float dis = Vector3.Distance(targetInfo.m_vSrcPos, act.GetPos());
            if (dis > skilldata.MSV_Range)
            {
                return null;
            }
            if (!GCollider.SingletonCollideCheck())
            {
                return null;
            }
            List<GalaxyActor> list = new List<GalaxyActor>();
            list.Add(act);
            return list;
        }
    }

    public class GSkillAreaSphere : GSkillAreaLogic
    {
        public override List<GalaxyActor> CalculationHit(GSkillData skilldata, GTargetInfo targetInfo)
        {
            float r = skilldata.MSV_AreaParam1 / 10.0f;
            float dis = skilldata.MSV_AreaParam2 / 10.0f;

            Vector3 pos = skilldata.IsAreaUseTarPos() ? targetInfo.m_vTarPos : targetInfo.m_vSrcPos;
            Vector3 dir = targetInfo.m_vAimDir;

            Vector3 spPos = pos + dir * dis;
            SSphere sSphere = new SSphere(spPos, r);
            
            List<GalaxyActor> pActorList = ActorMgr.GetAllActor();
            if (pActorList.Count<=0)
            {
                return null;
            }
            List<GalaxyActor> list = new List<GalaxyActor>();
            foreach (GalaxyActor actor in pActorList)
            {
                if (actor == m_pLocalPlayer)
                    continue;
                ActorObj obj = (ActorObj)actor;
                if (obj.CollisionCom == null)
                {
                    continue;
                }
                for (int i = 0; i < obj.CollisionCom.GetShapeCount(); i++)
                {
                    SShapeData shp = obj.CollisionCom.GetShape(i);
                    if (shp != null)
                    {
                        SSphere sTarSphere = new SSphere(shp.Pos, shp.r);
                        if (GCollider.SphereCollideCheck(sSphere, sTarSphere, dir))
                        {
                            list.Add(actor);
                            break;
                        }
                    }
                }
                
            }
            return list;
        }
    }

    public class GSkillAreaSector : GSkillAreaLogic
    {
        public override List<GalaxyActor> CalculationHit(GSkillData skilldata, GTargetInfo targetInfo)
        {
            float minDis = skilldata.MSV_AreaParam1 / 10.0f;
            float maxDis = skilldata.MSV_AreaParam2 / 10.0f;
            int angle = skilldata.MSV_AreaParam3;

            Vector3 pos = skilldata.IsAreaUseTarPos() ? targetInfo.m_vTarPos : targetInfo.m_vSrcPos;
            Vector3 dir = targetInfo.m_vAimDir;

            SSector sSector = new SSector(pos, dir, maxDis, minDis, angle);

            List<GalaxyActor> pActorList = ActorMgr.GetAllActor();
            if (pActorList.Count <= 0)
            {
                return null;
            }
            List<GalaxyActor> list = new List<GalaxyActor>();
            foreach (GalaxyActor actor in pActorList)
            {
                if (actor == m_pLocalPlayer)
                    continue;
                ActorObj obj = (ActorObj)actor;
                if (obj.CollisionCom == null)
                {
                    continue;
                }
                for (int i = 0; i < obj.CollisionCom.GetShapeCount(); i++)
                {
                    SShapeData shp = obj.CollisionCom.GetShape(i);
                    if (shp != null)
                    {
                        SSphere sTarSphere = new SSphere(shp.Pos, shp.r);
                        if (GCollider.SectorCollideCheck(sSector, sTarSphere, dir))
                        {
                            list.Add(actor);
                            break;
                        }
                    }
                }
                
            }
            return list;
        }
    }

    public class GSkillAreaRing : GSkillAreaLogic
    {
        public override List<GalaxyActor> CalculationHit(GSkillData skilldata, GTargetInfo targetInfo)
        {
            float rMin = skilldata.MSV_AreaParam1 / 10.0f;
            float rMax = skilldata.MSV_AreaParam2 / 10.0f;
            float dis = skilldata.MSV_AreaParam3 / 10.0f;

            Vector3 pos = skilldata.IsAreaUseTarPos() ? targetInfo.m_vTarPos : targetInfo.m_vSrcPos;
            Vector3 dir = targetInfo.m_vAimDir;

            SRing sRing = new SRing(pos, rMax, rMin);

            List<GalaxyActor> pActorList = ActorMgr.GetAllActor();
            if (pActorList.Count <= 0)
            {
                return null;
            }
            List<GalaxyActor> list = new List<GalaxyActor>();
            foreach (GalaxyActor actor in pActorList)
            {
                if (actor == m_pLocalPlayer)
                    continue;
                ActorObj obj = (ActorObj)actor;
                if (obj.CollisionCom == null)
                {
                    continue;
                }
                for (int i = 0; i < obj.CollisionCom.GetShapeCount(); i++)
                {
                    SShapeData shp = obj.CollisionCom.GetShape(i);
                    if (shp != null)
                    {
                        SSphere sTarSphere = new SSphere(shp.Pos, shp.r);
                        if (GCollider.RingCollideCheck(sRing, sTarSphere, dir))
                        {
                            list.Add(actor);
                            break;
                        }
                    }
                }
            }
            return list;
        }
    }

    public class GSkillAreaRect : GSkillAreaLogic
    {
        public override List<GalaxyActor> CalculationHit(GSkillData skilldata, GTargetInfo targetInfo)
        {
            float minDis = skilldata.MSV_AreaParam1 / 10.0f;
            float maxDis = skilldata.MSV_AreaParam2 / 10.0f;
            float w = skilldata.MSV_AreaParam3 / 10.0f;

            Vector3 pos = skilldata.IsAreaUseTarPos() ? targetInfo.m_vTarPos : targetInfo.m_vSrcPos;
            Vector3 dir = targetInfo.m_vAimDir;
            Vector3 center = pos + dir * ((minDis + maxDis) / 2);

            SRect sRect = new SRect(center, maxDis - minDis, w);
            
            List<GalaxyActor> pActorList = ActorMgr.GetAllActor();
            if (pActorList.Count <= 0)
            {
                return null;
            }
            List<GalaxyActor> list = new List<GalaxyActor>();
            foreach (GalaxyActor actor in pActorList)
            {
                if (actor == m_pLocalPlayer)
                    continue;
                ActorObj obj = (ActorObj)actor;
                if (obj.CollisionCom == null)
                {
                    continue;
                }
                for (int i = 0; i < obj.CollisionCom.GetShapeCount(); i++)
                {
                    SShapeData shp = obj.CollisionCom.GetShape(i);
                    if (shp != null)
                    {
                        SSphere sTarSphere = new SSphere(shp.Pos, shp.r);
                        if (GCollider.RectCollideCheck(sRect, sTarSphere, dir))
                        {
                            list.Add(actor);
                            break;
                        }
                    }
                }
            }
            return list;
        }
    }

}
