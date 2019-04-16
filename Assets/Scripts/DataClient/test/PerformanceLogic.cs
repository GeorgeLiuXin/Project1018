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
        PlayAnimLogic,
        CameraConfigLogic,
        WeaponFollowLogic,
        ProjectileCreateAtMuzzleLogic,
    }

    public class PerformanceLogicFactory
    {
        #region Logic

        public Dictionary<string, ePerformanceLogic> m_dict;

        public PerformanceLogicFactory()
        {
            //此处添加的logic\enum\字符串 需要与原本C#代码中逻辑名称一致
            m_dict = new Dictionary<string, ePerformanceLogic>();
            m_dict.Add("ChainLineLogic", ePerformanceLogic.ChainLineLogic);
            m_dict.Add("ChainLightningLogic", ePerformanceLogic.ChainLightningLogic);
            m_dict.Add("test", ePerformanceLogic.test);
            m_dict.Add("BuffEffectLogic", ePerformanceLogic.BuffEffectLogic);
            m_dict.Add("PlayAnimLogic", ePerformanceLogic.PlayAnimLogic);
            m_dict.Add("CameraConfigLogic", ePerformanceLogic.CameraConfigLogic);
            m_dict.Add("WeaponFollowLogic", ePerformanceLogic.WeaponFollowLogic);
            m_dict.Add("ProjectileCreateAtMuzzleLogic", ePerformanceLogic.ProjectileCreateAtMuzzleLogic);
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
                case ePerformanceLogic.PlayAnimLogic:
                    return new PlayAnimLogic();
                case ePerformanceLogic.CameraConfigLogic:
                    return new CameraConfigLogic();
                case ePerformanceLogic.WeaponFollowLogic:
                    return new WeaponFollowLogic();
                case ePerformanceLogic.ProjectileCreateAtMuzzleLogic:
                    return new ProjectileCreateAtMuzzleLogic();
                default:
                    return null;
            }
        }
        #endregion
        
        #region TemplateLogic
        private static Dictionary<string, PerformanceLogic> m_TemplateLogicDict;
        public Dictionary<string, PerformanceLogic> TemplateLogicDict
        {
            get
            {
                if (m_TemplateLogicDict == null
                    || m_TemplateLogicDict.Count == 0
                    || m_TemplateLogicDict.Count != m_dict.Count)
                {
                    InitLogicDict();
                }
                return m_TemplateLogicDict;
            }
        }
        private void InitLogicDict()
        {
            if (m_TemplateLogicDict == null)
            {
                m_TemplateLogicDict = new Dictionary<string, PerformanceLogic>();
            }
            if (m_TemplateLogicDict.Count != m_dict.Count)
            {
                foreach (var pair in m_dict)
                {
                    if (m_TemplateLogicDict.ContainsKey(pair.Key))
                        continue;
                    m_TemplateLogicDict.Add(pair.Key, GetPerformanceLogic(pair.Key));
                }
            }
        }


        public PerformanceLogic GetTemplatePerformanceLogic(string logicName)
        {
            if (!TemplateLogicDict.ContainsKey(logicName))
                return null;
            return TemplateLogicDict[logicName];
        }
        #endregion
    }

    /// <summary>
    /// 表现效果模板数据及描述
    /// </summary>
    public abstract class TemplatePerformanceLogic
    {
        //暂留 以用于之后改成正常赋值使用
    }

    /// <summary>
    /// 表现效果整体控制
    /// </summary>
    public abstract class PerformanceLogic : TemplatePerformanceLogic
    {
        protected int m_OwenrID;
        protected bool m_bDestroy;
        protected float m_CurTime;
        protected float m_TotalTime;

        public PerformanceLogic()
        {
            m_OwenrID = 0;
            m_bDestroy = false;
            m_CurTime = 0;
            m_TotalTime = -1;
        }

        public void SetOwner(int nAvatarID)
        {
            m_OwenrID = nAvatarID;
        }

        public abstract void Init(params object[] values);
        public abstract void Reset();

        public virtual bool Tick(float fTime)
        {
            if (IsDestroy())
                return false;

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

        public void SetTotalTime(float fTotalTime)
        {
            m_CurTime = 0;
            m_TotalTime = fTotalTime;
        }

        public void Destroy()
        {
            m_bDestroy = true;
            Reset();
        }
        public bool IsDestroy()
        {
            return m_bDestroy;
        }

    }

    //测试
    [PerformanceLogicDes("测试逻辑")]
    public class test : PerformanceLogic
    {
        [PerformanceLogicItemDes("测试int参数1")]
        public int test1 = 0;
        [PerformanceLogicItemDes("测试string参数2")]
        public string test2 = "";
        [PerformanceLogicItemDes("测试bool参数3")]
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