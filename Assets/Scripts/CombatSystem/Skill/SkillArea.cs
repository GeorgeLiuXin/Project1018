﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    /// <summary>
    /// 技能范围
    /// </summary>
    public class SkillArea : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
//#include "PreComp.h"
//#include "GNodeSkillArea.h"
//#include "NodeAvatar/NodeAvatar.h"
//#include "NodeManager/NodeConditionCheckManager.h"
//#include "GSkillData.h"
//#include "GalaxyMath.h"
//#include "GServerCollider.h"
//#include "NodeAvatar/NodeAvatar.h"
//#include "NodeSkill/GCollisionComponent.h"
//#include "NodeLogic/NodeCondition.h"

//namespace Galaxy
//{
//    bool GSkillAreaLogic::TryAddTarget(GSkillData* pSkillData, GNodeAvatar* pCaster, GNodeAvatar* pTarget, GSkillTargetList& vTargetList, GSkillExcludeList& vExcludeList, Vector3 vSrcPos)
//    {
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        if (vExcludeList.find(pTarget->GetAvatarID()) != vExcludeList.end())
//        {
//            return false;
//        }

//        int32 nTarCheck = pSkillData->GetValue(MSV_AreaTarCheck);
//        if (nTarCheck > 0)
//        {
//            SDConditionParamAvatar sParam;
//            sParam.ParamAvatar = pCaster;
//            if (!GSKillConditionCheckManager::Instance().Check(nTarCheck, pTarget, &sParam))
//                return false;
//        }

//        bool bVisible = true;
//        if (m_bRayCheck && pCaster != pTarget)
//        {
//            Vector3 sPos = vSrcPos + Vector3(0, 0.5, 0);
//            Vector3 ePos = pTarget->GetPos() + Vector3(0, 0.5, 0);
//            bVisible = !pCaster->RayCast(sPos, ePos);
//        }

//        vTargetList[pTarget] = bVisible;

//        if (pSkillData->IsAreaAddExclude())
//        {
//            vExcludeList.insert(pTarget->GetAvatarID());
//        }
//        return true;
//    }

//    Galaxy::int32 GSkillAreaLogic::GetTargetCount(GSkillData* pSkillData, GSkillExcludeList& vExcludeList)
//    {
//        if (!pSkillData)
//            return 0;

//        int32 nCount = pSkillData->GetValue(MSV_AreaTarCnt);
//        if (vExcludeList.m_bCount)
//            nCount = MIN(vExcludeList.m_nCount, nCount);
//        return nCount;
//    }

//    void GSkillAreaLogic::UpdateAreaFilter(int32 nFilter, int32 nCount, GSkillTargetList& vTargetList)
//    {
//        if (vTargetList.Count() <= nCount)
//            return;

//        switch (nFilter)
//        {
//            case AreaFilter_MinHp: UpdateAreaFilterMinHp(vTargetList); break;
//        }
//    }

//    void GSkillAreaLogic::UpdateAreaFilterMinHp(GSkillTargetList& vTargetList)
//    {
//        GNodeAvatar* pAvatar = NULL;
//        vTargetList.Begin();
//        while (!vTargetList.IsEnd())
//        {
//            GNodeAvatar* pTarget = vTargetList.GetKey();
//            if (pAvatar == NULL || pTarget->GetHpPercent() > pAvatar->GetHpPercent())
//            {
//                pAvatar = pTarget;
//            }
//            vTargetList.Next();
//        }

//        if (pAvatar)
//        {
//            vTargetList.Remove(pAvatar);
//        }
//    }

//    GSkillTargetList GSkillAreaSingelton::GetTargetList(GSkillData* pSkillData, GNodeAvatar* pCaster, const GSkillTargetInfo& sTarInfo, GSkillExcludeList& vExcludeList)
//    {
//        GSkillTargetList vTargetList;
//        if (!pSkillData || !pCaster)
//            return vTargetList;

//        GNodeAvatar* pTarget = pCaster->GetSceneAvatar(sTarInfo.m_nTargetID);
//        if (!pTarget)
//            return vTargetList;

//        //单体技能需要判断当前目标与我的朝向夹角与range判断

//        Vector3 pos = pTarget->GetPos();
//        Vector3 dir = pTarget->GetDir();
//        if (pCaster->GetPos().GetDistance(pos) > pSkillData->GetFloatValue(MSV_Range))
//            return vTargetList;
//        Vector3 offestPos = pos - pCaster->GetPos();
//        if (pCaster->GetDir().Dot(offestPos) < 0)
//            return vTargetList;

//        TryAddTarget(pSkillData, pCaster, pTarget, vTargetList, vExcludeList, pCaster->GetPos());
//        return vTargetList;
//    }

//    GSkillTargetList GSkillAreaSector::GetTargetList(GSkillData* pSkillData, GNodeAvatar* pCaster, const GSkillTargetInfo& sTarInfo, GSkillExcludeList& vExcludeList)
//    {
//        GSkillTargetList vTargetList;
//        if (!pSkillData || !pCaster)
//            return vTargetList;

//        GNodeAvatar* pTarget = pCaster->GetSceneAvatar(sTarInfo.m_nTargetID);

//        int32 cnt = GetTargetCount(pSkillData, vExcludeList);
//        int32 filter = pSkillData->GetIntValue(MSV_AreaFilter);
//        f32 minDis = pSkillData->GetValue(MSV_AreaParam1) / 10.0f;
//        f32 maxDis = pSkillData->GetValue(MSV_AreaParam2) / 10.0f;
//        int32 angle = pSkillData->GetValue(MSV_AreaParam3);
//        AOIList* pList = pCaster->GetArroundAvatar();
//        if (!pList)
//        {
//            return vTargetList;
//        }
//        Vector3 pos = pSkillData->IsAreaUseTarPos() ? sTarInfo.m_vTarPos : sTarInfo.m_vSrcPos;
//        Vector3 dir = sTarInfo.m_vAimDir;

//        SSector sector;
//        sector.angle = angle;
//        sector.r_min = minDis;
//        sector.r_max = maxDis;
//        sector.dx = dir.x;
//        sector.dy = dir.y;
//        sector.dz = dir.z;
//        sector.x = pos.x;
//        sector.y = pos.y;
//        sector.z = pos.z - 2.0f;
//        sector.height = 5.0f;

//        for (int32 i = 0; i < pList->GetCount(); ++i)
//        {
//            int32 avatarID = pList->GetIdByIndex(i);
//            GNodeAvatar* pTar = pCaster->GetSceneAvatar(avatarID);
//            if (!pTar || !pTar->GetCollisionComponent())
//                continue;

//            //check shape
//            SSphere sph;
//            //pTar->GetCollisionComponent()->OrderByDistance(pCaster->GetPos());
//            for (int32 a = 0; a < pTar->GetCollisionComponent()->GetShapeCount(); a++)
//            {
//                if (!pTar->GetCollisionComponent()->GetShape(a, sph))
//                    continue;

//                Vector3 sCenter(sph.x, sph.y, sph.z);
//        GServerCollider::Local2World(sCenter, pTar->GetPos(), pTar->GetDir());
//        sph.x = sCenter.x;
//        sph.y = sCenter.y;
//        sph.z = sCenter.z;
//        if (GServerCollider::CollideCheckSS(sph, sector))
//        {
//            TryAddTarget(pSkillData, pCaster, pTar, vTargetList, vExcludeList, pos);

//            if (vTargetList.Count() >= cnt)
//            {
//                if (filter > 0)
//                    UpdateAreaFilter(filter, cnt, vTargetList);
//                else
//                    return vTargetList;
//            }
//            break;
//        }
//    }
//}
//		return vTargetList;
//	}
//    void GSkillAreaSector::Draw(GSkillData* pSkillData, GNodeAvatar* pCaster, GNodeAvatar* pTarget, const GSkillTargetInfo& sTarInfo)
//{
//#if DEBUG_COMBAT
//        if (!pSkillData || !pCaster)
//            return;
//        f32 minDis = pSkillData->GetValue(MSV_AreaParam1) / 10.0f;
//        f32 maxDis = pSkillData->GetValue(MSV_AreaParam2) / 10.0f;
//        int32 angle = pSkillData->GetValue(MSV_AreaParam3);
//        Vector3 pos = pSkillData->IsAreaUseTarPos() ? sTarInfo.m_vTarPos : sTarInfo.m_vSrcPos;
//        Vector3 dir = sTarInfo.m_vAimDir;
//        SSector sector;
//        sector.angle = angle;
//        sector.r_min = minDis;
//        sector.r_max = maxDis;
//        sector.dx = dir.x;
//        sector.dy = dir.y;
//        sector.dz = dir.z;
//        sector.x = pos.x;
//        sector.y = pos.y;
//        sector.z = pos.z - 2.0f;
//        sector.height = 5.0f;

//        GPacketDrawSector pkt;
//        pkt.height = sector.height;
//        pkt.angle = sector.angle;
//        pkt.x = sector.x;
//        pkt.y = sector.y;
//        pkt.z = sector.z;
//        pkt.dx = sector.dx;
//        pkt.dy = sector.dy;
//        pkt.dz = sector.dz;
//        pkt.minDis = sector.r_min;
//        pkt.maxDis = sector.r_max;
//        pCaster->BroadcastPacket(&pkt);
//#endif
//}
//GSkillTargetList GSkillAreaSphere::GetTargetList(GSkillData* pSkillData, GNodeAvatar* pCaster, const GSkillTargetInfo& sTarInfo, GSkillExcludeList& vExcludeList)
//{
//    GSkillTargetList vTargetList;
//    if (!pSkillData || !pCaster)
//    {
//        return vTargetList;
//    }
//    GNodeAvatar* pTarget = pCaster->GetSceneAvatar(sTarInfo.m_nTargetID);
//    if (!pTarget)
//    {
//        pTarget = pCaster;
//    }
//    int32 cnt = GetTargetCount(pSkillData, vExcludeList);
//    int32 filter = pSkillData->GetIntValue(MSV_AreaFilter);
//    f32 r = pSkillData->GetValue(MSV_AreaParam1) / 10.0f;
//    f32 dis = pSkillData->GetValue(MSV_AreaParam2) / 10.0f;
//    AOIList* pList = pTarget->GetArroundAvatar();
//    if (!pList)
//    {
//        return vTargetList;
//    }
//    Vector3 pos = pSkillData->IsAreaUseTarPos() ? sTarInfo.m_vTarPos : sTarInfo.m_vSrcPos;
//    Vector3 dir = sTarInfo.m_vAimDir;


//    Vector3 spPos = pos + dir * dis;

//    for (int32 i = 0; i < pList->GetCount(); ++i)
//    {
//        int32 avatarID = pList->GetIdByIndex(i);
//        GNodeAvatar* pTar = pCaster->GetSceneAvatar(avatarID);
//        if (!pTar || !pTar->GetCollisionComponent())
//            continue;

//        //check shape
//        SSphere sph;
//        //pTar->GetCollisionComponent()->OrderByDistance(pCaster->GetPos());
//        for (int32 a = 0; a < pTar->GetCollisionComponent()->GetShapeCount(); a++)
//        {
//            if (!pTar->GetCollisionComponent()->GetShape(a, sph))
//                continue;

//            Vector3 sCenter(sph.x, sph.y, sph.z);
//    GServerCollider::Local2World(sCenter, pTar->GetPos(), pTar->GetDir());

//    if (spPos.GetDistance(sCenter) <= (r + sph.r))
//    {
//        TryAddTarget(pSkillData, pCaster, pTar, vTargetList, vExcludeList, spPos);

//        if (vTargetList.Count() >= cnt)
//        {
//            if (filter > 0)
//                UpdateAreaFilter(filter, cnt, vTargetList);
//            else
//                return vTargetList;
//        }
//        break;
//    }
//}
//		}

//		return vTargetList;
//	}
//    void GSkillAreaSphere::Draw(GSkillData* pSkillData, GNodeAvatar* pCaster, GNodeAvatar* pTarget, const GSkillTargetInfo& sTarInfo)
//{
//#if DEBUG_COMBAT
//        if (!pSkillData || !pCaster)
//            return;
//        f32 r = pSkillData->GetValue(MSV_AreaParam1) / 10.0f;
//        f32 dis = pSkillData->GetValue(MSV_AreaParam2) / 10.0f;
//        Vector3 pos = pSkillData->IsAreaUseTarPos() ? sTarInfo.m_vTarPos : sTarInfo.m_vSrcPos;
//        Vector3 dir = sTarInfo.m_vAimDir;
//        Vector3 spPos = pos + dir*dis;

//        GPacketDrawSphere pkt;
//        pkt.radius = r;
//        pkt.x = spPos.x;
//        pkt.y = spPos.y;
//        pkt.z = spPos.z;
//        pkt.r = 0;
//        pkt.g = 100;
//        pkt.b = 0;
//        pCaster->SendPacket(&pkt);
//#endif
//}
//GSkillTargetList GSkillAreaRect::GetTargetList(GSkillData* pSkillData, GNodeAvatar* pCaster, const GSkillTargetInfo& sTarInfo, GSkillExcludeList& vExcludeList)
//{
//    GSkillTargetList vTargetList;
//    if (!pSkillData || !pCaster)
//    {
//        return vTargetList;
//    }

//    GNodeAvatar* pTarget = pCaster->GetSceneAvatar(sTarInfo.m_nTargetID);
//    if (!pTarget)
//    {
//        pTarget = pCaster;
//    }
//    int32 cnt = GetTargetCount(pSkillData, vExcludeList);
//    int32 filter = pSkillData->GetIntValue(MSV_AreaFilter);
//    f32 minDis = pSkillData->GetValue(MSV_AreaParam1) / 10.0f;
//    f32 maxDis = pSkillData->GetValue(MSV_AreaParam2) / 10.0f;
//    int32 w = pSkillData->GetValue(MSV_AreaParam3) / 10.0f;
//    AOIList* pList = pTarget->GetArroundAvatar();
//    if (!pList)
//    {
//        return vTargetList;
//    }
//    Vector3 pos = pSkillData->IsAreaUseTarPos() ? sTarInfo.m_vTarPos : sTarInfo.m_vSrcPos;
//    Vector3 dir = sTarInfo.m_vAimDir;
//    if (dir.IsZero())
//    {
//        dir = pCaster->GetDir();
//    }
//    Vector3 minPos = pos + dir * minDis;
//    Vector3 maxPos = pos + dir * maxDis;
//    f32 len = minPos.GetDistance(maxPos);

//    Vec3 offset = Vec3(w * 0.5f, len * 0.5f, 5.0f * 0.5f);

//    Avalon::Matrix34 m34;
//    m34.SetIdentity();
//    m34.AddTranslation((minPos + maxPos) * 0.5f);

//    Avalon::Matrix33 m33 = Avalon::Matrix33::CreateRotationVDir(dir);
//    m34 = m34 * m33;

//    SFrustum frustum;
//    frustum.SetFrustum(m34, offset);
//    Draw(pSkillData, pCaster, NULL, sTarInfo);
//    for (int32 i = 0; i < pList->GetCount(); ++i)
//    {
//        int32 avatarID = pList->GetIdByIndex(i);
//        GNodeAvatar* pTar = pCaster->GetSceneAvatar(avatarID);
//        if (!pTar || !pTar->GetCollisionComponent())
//            continue;

//        //check shape
//        SSphere sph;
//        //pTar->GetCollisionComponent()->OrderByDistance(pCaster->GetPos());
//        for (int32 a = 0; a < pTar->GetCollisionComponent()->GetShapeCount(); a++)
//        {
//            if (!pTar->GetCollisionComponent()->GetShape(a, sph))
//                continue;

//            Vector3 sCenter(sph.x, sph.y, sph.z);
//    GServerCollider::Local2World(sCenter, pTar->GetPos(), pTar->GetDir());
//    sph.x = sCenter.x;
//    sph.y = sCenter.y;
//    sph.z = sCenter.z;
//    if (GServerCollider::CollideCheckPS(frustum, sph))
//    {
//        TryAddTarget(pSkillData, pCaster, pTar, vTargetList, vExcludeList, pos);

//        if (vTargetList.Count() >= cnt)
//        {
//            if (filter > 0)
//                UpdateAreaFilter(filter, cnt, vTargetList);
//            else
//                return vTargetList;
//        }
//        break;
//    }
//}			
//		}
//		return vTargetList;
//	}
//    void GSkillAreaRect::Draw(GSkillData* pSkillData, GNodeAvatar* pCaster, GNodeAvatar* pTarget, const GSkillTargetInfo& sTarInfo)
//{
//#if DEBUG_COMBAT
//        if (!pSkillData || !pCaster)
//            return;
//        f32 minDis = pSkillData->GetValue(MSV_AreaParam1) / 10.0f;
//        f32 maxDis = pSkillData->GetValue(MSV_AreaParam2) / 10.0f;
//        int32 len = pSkillData->GetValue(MSV_AreaParam3) / 10.0f;
//        Vector3 pos = pSkillData->IsAreaUseTarPos() ? sTarInfo.m_vTarPos : sTarInfo.m_vSrcPos;
//        Vector3 dir = sTarInfo.m_vAimDir;
//        Vector3 minPos = pos + dir * minDis;
//        Vector3 maxPos = pos + dir * maxDis;
//        f32 w = minPos.GetDistance(maxPos);

//        Vector3 vCenter = (minPos + maxPos)*0.5f;

//        //Vec3 offset = Vec3(w*0.5f, len*0.5f, 5.0f*0.5f);
//        //Avalon::Matrix34 m34;
//        //m34.SetIdentity();
//        //m34.AddTranslation((minPos + maxPos)*0.5f);

//        //Avalon::Matrix33 m33 = Avalon::Matrix33::CreateRotationVDir(dir);
//        //m34 = m34*m33;

//        GPacketDrawBox pktBox;
//        pktBox.x = vCenter.x;
//        pktBox.y = vCenter.y;
//        pktBox.z = vCenter.z;
//        pktBox.dx = dir.x;
//        pktBox.dy = dir.y;
//        pktBox.dz = dir.z;
//        pktBox.l = len;
//        pktBox.w = w;
//        pktBox.h = 2.5f;    //
//        pktBox.r = 0;
//        pktBox.g = 100;
//        pktBox.b = 0;
//        pCaster->SendPacket(&pktBox);
//#endif
//}
//GSkillTargetList GSkillAreaRing::GetTargetList(GSkillData* pSkillData, GNodeAvatar* pCaster, const GSkillTargetInfo& sTarInfo, GSkillExcludeList& vExcludeList)
//{
//    GSkillTargetList vTargetList;
//    if (!pSkillData || !pCaster)
//    {
//        return vTargetList;
//    }
//    GNodeAvatar* pTarget = pCaster->GetSceneAvatar(sTarInfo.m_nTargetID);
//    if (!pTarget)
//    {
//        pTarget = pCaster;
//    }
//    int32 cnt = GetTargetCount(pSkillData, vExcludeList);
//    int32 filter = pSkillData->GetIntValue(MSV_AreaFilter);
//    f32 rMin = pSkillData->GetValue(MSV_AreaParam1) / 10.0f;
//    f32 rMax = pSkillData->GetValue(MSV_AreaParam2) / 10.0f;
//    f32 dis = pSkillData->GetValue(MSV_AreaParam3) / 10.0f;
//    AOIList* pList = pTarget->GetArroundAvatar();
//    if (!pList)
//    {
//        return vTargetList;
//    }
//    Vector3 pos = pSkillData->IsAreaUseTarPos() ? sTarInfo.m_vTarPos : sTarInfo.m_vSrcPos;
//    Vector3 dir = sTarInfo.m_vAimDir;


//    Vector3 spPos = pos + dir * dis;

//    for (int32 i = 0; i < pList->GetCount(); ++i)
//    {
//        int32 avatarID = pList->GetIdByIndex(i);
//        GNodeAvatar* pTar = pCaster->GetSceneAvatar(avatarID);
//        if (!pTar || !pTar->GetCollisionComponent())
//            continue;

//        //check shape
//        SSphere sph;
//        //pTar->GetCollisionComponent()->OrderByDistance(pCaster->GetPos());
//        for (int32 a = 0; a < pTar->GetCollisionComponent()->GetShapeCount(); a++)
//        {
//            if (!pTar->GetCollisionComponent()->GetShape(a, sph))
//                continue;

//            Vector3 sCenter(sph.x, sph.y, sph.z);
//    GServerCollider::Local2World(sCenter, pTar->GetPos(), pTar->GetDir());
//    f32 dis = spPos.GetDistance(sCenter);
//    f32 r1 = rMin + sph.r;
//    f32 r2 = rMax + sph.r;
//    if (dis <= r2 && dis >= r1)
//    {
//        TryAddTarget(pSkillData, pCaster, pTar, vTargetList, vExcludeList, spPos);

//        if (vTargetList.Count() >= cnt)
//        {
//            if (filter > 0)
//                UpdateAreaFilter(filter, cnt, vTargetList);
//            else
//                return vTargetList;
//        }
//        break;
//    }
//}
//		}

//		return vTargetList;
//	}
//    void GSkillAreaRing::Draw(GSkillData* pSkillData, GNodeAvatar* pCaster, GNodeAvatar* pTarget, const GSkillTargetInfo& sTarInfo)
//{
//#if DEBUG_COMBAT
//        if (!pSkillData || !pCaster)
//            return;
//        f32 rMin = pSkillData->GetValue(MSV_AreaParam1) / 10.0f;
//        f32 rMax = pSkillData->GetValue(MSV_AreaParam2) / 10.0f;
//        f32 dis = pSkillData->GetValue(MSV_AreaParam3) / 10.0f;

//        Vector3 pos = pSkillData->IsAreaUseTarPos() ? sTarInfo.m_vTarPos : sTarInfo.m_vSrcPos;
//        Vector3 dir = sTarInfo.m_vAimDir;

//        Vector3 spPos = pos + dir*dis;

//        GPacketDrawCylinder pktCylinder;
//        pktCylinder.x1 = spPos.x;
//        pktCylinder.y1 = spPos.y;
//        pktCylinder.z1 = spPos.z;
//        pktCylinder.x2 = spPos.x;
//        pktCylinder.y2 = spPos.y;
//        pktCylinder.z2 = spPos.z + 2.5;
//        pktCylinder.r_min = rMin;
//        pktCylinder.r_max = rMax;
//        pCaster->SendPacket(&pktCylinder);
//#endif
//}
//}