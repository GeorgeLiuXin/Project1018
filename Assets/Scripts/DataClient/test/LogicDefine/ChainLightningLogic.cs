using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{

    /// <summary>
    /// 闪电链逻辑，暂留
    /// </summary>
    [PerformanceLogicDes("闪电链逻辑")]
    public class ChainLightningLogic : PerformanceLogic
    {
        [PerformanceLogicItemDes("闪电链特效对应的特效id")]
        public int m_nEffectID;

        private List<Transform> m_lineTransformList;
        private int m_CurTransform;

        public ChainLightningLogic()
        {
        }
        
        public override void Init(params object[] values)
        {

        }

        public override bool Tick(float fTime)
        {
            return base.Tick(fTime);
        }

        public override void Reset()
        {

        }
    }

}