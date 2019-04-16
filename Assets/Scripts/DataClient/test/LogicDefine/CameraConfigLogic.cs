using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    [PerformanceLogicDes("相机抬升逻辑")]
    public class CameraConfigLogic : PerformanceLogic
    {
        protected GalaxyCameraManager m_cameraMgr;
        protected GalaxyCameraManager CameraMgr
        {
            get
            {
                if (m_cameraMgr == null)
                {
                    m_cameraMgr = GalaxyGameModule.GetGameManager<GalaxyCameraManager>();
                }
                return m_cameraMgr;
            }
        }

        [PerformanceLogicItemDes("想要修正到的相机配置")]
        public int m_nConfigID;
        [PerformanceLogicItemDes("当前想修正的相机层级0.场景1.卡牌2.trigger3.技能")]
        public int m_eCameraConfigFlag;

        public override void Init(params object[] values)
        {
            if (m_eCameraConfigFlag < 0 || m_eCameraConfigFlag >= (int)eCameraConfigFlag.CameraConfig_Size)
                return;
            CameraMgr.ResetConfig(m_nConfigID, (eCameraConfigFlag)m_eCameraConfigFlag);
        }

        public override bool Tick(float fTime)
        {
            return base.Tick(fTime);
        }

        public override void Reset()
        {
            CameraMgr.CancelConfig((eCameraConfigFlag)m_eCameraConfigFlag);
        }
    }

}