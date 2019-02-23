using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    public enum ePerformanceLogic
    {
        ChainLineLogic = 1,
        ChainLightningLogic,
    }

    public class PerformanceLogicFactory
    {
        public Dictionary<string, ePerformanceLogic> m_dict;

        public PerformanceLogicFactory()
        {
            m_dict = new Dictionary<string, ePerformanceLogic>();
            m_dict.Add("ChainLineLogic", ePerformanceLogic.ChainLineLogic);
            m_dict.Add("ChainLightningLogic", ePerformanceLogic.ChainLightningLogic);
        }

        public PerformanceLogic GetPerformanceLogic(int index)
        {
            switch ((ePerformanceLogic)index)
            {
                case ePerformanceLogic.ChainLineLogic:
                    return new ChainLineLogic();
                case ePerformanceLogic.ChainLightningLogic:
                    return new ChainLightningLogic();
                default:
                    return null;
            }
        }
    }

    /// <summary>
    /// 表现效果整体控制
    /// </summary>
    public class PerformanceLogic
    {
        public PerformanceLogic()
        {

        }

        public virtual void Init(params object[] values)
        {

        }

        public virtual void Tick(float fTime)
        {

        }

        public virtual void Reset()
        {

        }

        //分发给对应的具体表现效果脚本，在脚本中分别控制各自的表现效果推进
        public delegate void PerformanceLogicCallBack(params object[] obj);
        public PerformanceLogicCallBack OnCreate { get; set; }
        public PerformanceLogicCallBack OnUpdate { get; set; }
        public PerformanceLogicCallBack OnDelete { get; set; }

        //所有具体表现效果需要注册对应事件
        //protected void OnPerformanceCreate(params object[] obj)
        //{
        //}
        //protected void OnPerformanceUpdate(params object[] obj)
        //{
        //}
        //protected void OnPerformanceDelete(params object[] obj)
        //{
        //}
    }

}