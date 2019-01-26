//using UnityEngine;
//using System.Collections;
//using System;
//using System.Collections.Generic;

//namespace XWorld
//{
//    /// <summary>
//    /// SkillArea_Singleton = 1, //单体
//    /// SkillArea_Sphere = 2, //球形范围
//    /// SkillArea_Sector = 3, //扇形范围
//    /// 参数1：最近边距自己的距离，参数2：半径，参数3：弧度
//    /// SkillArea_Ring = 4, //环形范围
//    /// SkillArea_Rect = 5, //矩形范围
//    /// 参数1：最近边距自己的距离，参数2：最远边距自己的距离，参数3：宽度
//    /// </summary>
//    public class GColliderManager : XWorldGameManagerBase
//    {

//        private Dictionary<int, SkillAreaLogic> m_vAreaLogicDict;
        
//        public override void InitManager()
//        {
//            m_vAreaLogicDict = new Dictionary<int, SkillAreaLogic>();
//            m_vAreaLogicDict.Add((int)eSkillAreaLogic.SkillArea_Singleton, new SkillAreaSingelton());
//            m_vAreaLogicDict.Add((int)eSkillAreaLogic.SkillArea_Sphere, new SkillAreaSphere());
//            m_vAreaLogicDict.Add((int)eSkillAreaLogic.SkillArea_Sector, new GSkillAreaSector());
//            m_vAreaLogicDict.Add((int)eSkillAreaLogic.SkillArea_Ring, new GSkillAreaRing());
//            m_vAreaLogicDict.Add((int)eSkillAreaLogic.SkillArea_Rect, new GSkillAreaRect());
//        }

//        public override void ShutDown()
//        {
//            m_vAreaLogicDict.Clear();
//            m_vAreaLogicDict = null;
//        }

//        public override void Update(float fElapseTimes)
//        {

//        }

//        public List<GalaxyActor> CalculationHit(GSkillData pSkillData, GTargetInfo targetInfo)
//        {
//            var areaLogicID = pSkillData.MSV_AreaLogic;
//            if ((int) eSkillAreaLogic.SkillArea_Min < areaLogicID && areaLogicID < (int) eSkillAreaLogic.SkillArea_Max)
//            {
//                return m_vAreaLogicDict[pSkillData.MSV_AreaLogic].CalculationHit(pSkillData, targetInfo);
//            }

//            return null;
//        }

//    }
//}
