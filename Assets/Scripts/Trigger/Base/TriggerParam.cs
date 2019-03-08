using UnityEngine;
using System;

namespace Galaxy
{
    public class TriggerParam
    {
        public int eventId;
        public int groupId;
        public int dataId;
        public int effectId;
        public int triggerCount;
        public int triggerTagType;
        public int triggerType;
        public int spwanerId;
        public int spwanerType;
        public bool bAutoActive;
        public bool bLoseContorl;
        public Vector3 vWorldPos;
        public Vector3 vDir;
        public Vector3 vScale;
        public string triggerAction;

        public TriggerParam(ConfigData data)
        {
            this.eventId = data.GetInt("EventID");
            this.groupId = data.GetInt("GroupID");
            this.dataId = data.GetInt("DataID");
            this.effectId = data.GetInt("EffectID");
            this.triggerCount = data.GetInt("TriggerCount");
            this.triggerType = data.GetInt("TriggerType");
            this.triggerTagType = data.GetInt("TriggerTag");
            this.spwanerId = data.GetInt("SpwanerId");
            this.spwanerType = data.GetInt("SpwanerType");
            this.triggerAction = data.GetString("TriggerAction");

            Vector3 t_vPos = Vector3.zero;
            Vector3 t_vDir = Vector3.zero;
            Vector3 t_vScale = Vector3.one;
            t_vPos.x = data.GetFloat("PosX");
            t_vPos.y = data.GetFloat("PosY");
            t_vPos.z = data.GetFloat("PosZ");
            t_vDir.x = data.GetFloat("DirX");
            t_vDir.y = data.GetFloat("DirY");
            t_vDir.z = data.GetFloat("DirZ");
            t_vScale.x = data.GetFloat("ScaleX");
            t_vScale.y = data.GetFloat("ScaleY");
            t_vScale.z = data.GetFloat("ScaleZ");
            vWorldPos = t_vPos;
            vDir = t_vDir;
            vScale = t_vScale;

            //预留参数
            bAutoActive = true;
            bLoseContorl = true;
        }
    }
}