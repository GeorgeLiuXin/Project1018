using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{

    /// <summary>
    /// 闪电链逻辑，暂留
    /// </summary>
    public class ChainLightningLogic : PerformanceLogic
    {
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