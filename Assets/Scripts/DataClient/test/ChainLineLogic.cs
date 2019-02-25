using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    /// <summary>
    /// buff连线效果逻辑
    /// eg:莫甘娜大招，小精灵连线
    /// </summary>
    public class ChainLineLogic : PerformanceLogic
    {
        private Transform m_pCaster;
        private Transform m_pTarget;

        public ChainLineLogic()
        {
        }

        /// <summary>
        /// 1、自己的Transform
        /// 2、目标的Transform
        /// </summary>
        public override void Init(params object[] values)
        {
            
        }

        public override void Tick(float fTime)
        {
            base.Tick(fTime);
        }

        public override void Reset()
        {
            base.Reset();
        }
    }

}