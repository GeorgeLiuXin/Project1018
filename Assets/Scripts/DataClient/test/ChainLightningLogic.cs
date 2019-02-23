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

        private List<Transform> m_lineTransformList;
        private int m_CurTransform;

        public ChainLightningLogic()
        {
        }

        public override void Init(params object[] values)
        {
            base.Init(values);
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