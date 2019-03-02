using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    public enum ePerformanceLogic
    {
        ChainLineLogic = 0,
        ChainLightningLogic,
        test,
        BuffEffectLogic,
    }

    public class PerformanceLogicFactory
    {
        public Dictionary<string, ePerformanceLogic> m_dict;

        public PerformanceLogicFactory()
        {
            m_dict = new Dictionary<string, ePerformanceLogic>();
            m_dict.Add("ChainLineLogic", ePerformanceLogic.ChainLineLogic);
            m_dict.Add("ChainLightningLogic", ePerformanceLogic.ChainLightningLogic);
            m_dict.Add("test", ePerformanceLogic.test);
            m_dict.Add("BuffEffectLogic", ePerformanceLogic.BuffEffectLogic);
        }


        public PerformanceLogic GetPerformanceLogic(string logicName)
        {
            if (m_dict == null || !m_dict.ContainsKey(logicName))
                return null;
            ePerformanceLogic tempEnum = m_dict[logicName];
            return GetPerformanceLogic((int)tempEnum);
        }
        public PerformanceLogic GetPerformanceLogic(int index)
        {
            switch ((ePerformanceLogic)index)
            {
                case ePerformanceLogic.ChainLineLogic:
                    return new ChainLineLogic();
                case ePerformanceLogic.ChainLightningLogic:
                    return new ChainLightningLogic();
                case ePerformanceLogic.test:
                    return new test();
                case ePerformanceLogic.BuffEffectLogic:
                    return new BuffEffectLogic();
                default:
                    return null;
            }
        }
    }

    /// <summary>
    /// 表现效果整体控制
    /// </summary>
    public abstract class PerformanceLogic
    {
        public PerformanceLogic()
        {
            m_bDestroy = false;
            m_CurTime = 0;
            m_TotalTime = -1;
        }

        public bool m_bDestroy;
        public void Destroy()
        {
            m_bDestroy = true;
        }
        public bool IsDestroy()
        {
            return m_bDestroy;
        }

        public float m_CurTime;
        public float m_TotalTime;
        public void SetTotalTime(float fTotalTime)
        {
            m_CurTime = 0;
            m_TotalTime = fTotalTime;
        }

        public virtual void Init(params object[] values)
        {

        }
        public virtual bool Tick(float fTime)
        {
            if (m_TotalTime == -1)
                return true;
            if (m_CurTime < m_TotalTime)
            {
                m_CurTime += fTime;
                return true;
            }
            Destroy();
            return false;
        }
        public virtual void Reset()
        {

        }
    }

    //测试
    public class test : PerformanceLogic
    {
        public int test1 = 0;
        public string test2 = "";
        public bool test3 = true;

        public test()
        {

        }

        public override void Init(params object[] values)
        {
            throw new NotImplementedException();
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }

        public override bool Tick(float fTime)
        {
            return base.Tick(fTime);
        }
    }
}