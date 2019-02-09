using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace XWorld
{
    public class BuffData : ModifyObject<BuffData>
    {
        public static void InitBuffDefine()
        {
            if (m_ModifyDefine == null)
                m_ModifyDefine = new Dictionary<string, int>();

            if (m_ModifyValue == null)
                m_ModifyValue = new Dictionary<int, int>();

            if (m_ModifyLogic == null)
                m_ModifyLogic = new Dictionary<int, eModifyLogic>();

            foreach (eModifyBuff _enum in Enum.GetValues(typeof(eModifyBuff)))
            {
                m_ModifyDefine.Add(_enum.ToString(), (int)_enum);
            }

            BuffData data = new BuffData();
            m_DataFields = data.GetType().GetFields();
            for (int i = 0; i < m_DataFields.Length; ++i)
            {
                int _enum;
                if (m_ModifyDefine.TryGetValue(m_DataFields[i].Name, out _enum))
                {
                    m_ModifyValue[_enum] = i;
                }
            }

            m_ModifyLogic.Add((int)eModifyBuff.MBV_BuffType, eModifyLogic.NOR); //Buff类型
            m_ModifyLogic.Add((int)eModifyBuff.MBV_BuffAttr, eModifyLogic.NOR); //Buff属性
            m_ModifyLogic.Add((int)eModifyBuff.MBV_BuffRemove, eModifyLogic.REP); //Buff移除
            m_ModifyLogic.Add((int)eModifyBuff.MBV_BuffInummue, eModifyLogic.NOR); //Buff免疫
            m_ModifyLogic.Add((int)eModifyBuff.MBV_BuffCleanUp, eModifyLogic.NOR); //Buff清除
            m_ModifyLogic.Add((int)eModifyBuff.MBV_BuffCleanUpGroup, eModifyLogic.NOR); //Buff清除组	
            m_ModifyLogic.Add((int)eModifyBuff.MBV_BuffState, eModifyLogic.NOR); //Buff状态
            m_ModifyLogic.Add((int)eModifyBuff.MBV_AttrValue, eModifyLogic.REP); //角色属性集
            m_ModifyLogic.Add((int)eModifyBuff.MBV_LayerCnt, eModifyLogic.Sum); //叠加层数
            m_ModifyLogic.Add((int)eModifyBuff.MBV_DurationTime, eModifyLogic.Sum); //持续时间
            m_ModifyLogic.Add((int)eModifyBuff.MBV_BuffSkill, eModifyLogic.REP); //Buff技能
            m_ModifyLogic.Add((int)eModifyBuff.MBV_BuffSkillLv, eModifyLogic.Sum); //Buff技能等级
            m_ModifyLogic.Add((int)eModifyBuff.MBV_BuffSkillUserData, eModifyLogic.Sum);    //Buff技能参数
            m_ModifyLogic.Add((int)eModifyBuff.MBV_BuffLogic, eModifyLogic.REP);            //Buff逻辑
            m_ModifyLogic.Add((int)eModifyBuff.MBV_BuffLogicParam1, eModifyLogic.Sum);  //Buff逻辑参数1
            m_ModifyLogic.Add((int)eModifyBuff.MBV_BuffLogicParam2, eModifyLogic.Sum);  //Buff逻辑参数2
            m_ModifyLogic.Add((int)eModifyBuff.MBV_BuffLogicParam3, eModifyLogic.Sum);  //Buff逻辑参数3
            m_ModifyLogic.Add((int)eModifyBuff.MBV_EffectID, eModifyLogic.REP); //特效ID
            m_ModifyLogic.Add((int)eModifyBuff.MBV_EffectSurface, eModifyLogic.REP); //特效材质
            m_ModifyLogic.Add((int)eModifyBuff.MBV_EffectAddID, eModifyLogic.REP); //生成特效ID
            m_ModifyLogic.Add((int)eModifyBuff.MBV_EffectAddTime, eModifyLogic.Sum); //生成特效持续时间
            m_ModifyLogic.Add((int)eModifyBuff.MBV_EffectRemoveID, eModifyLogic.REP); //消失特效ID
            m_ModifyLogic.Add((int)eModifyBuff.MBV_EffectRemoveTime, eModifyLogic.Sum); //消失特效持续时间	
            m_ModifyLogic.Add((int)eModifyBuff.MBV_NameID, eModifyLogic.REP); //Name字典
            m_ModifyLogic.Add((int)eModifyBuff.MBV_TipsID, eModifyLogic.REP); //Tips字典
            m_ModifyLogic.Add((int)eModifyBuff.MBV_IconID, eModifyLogic.REP); //图标
        }

        public override void OnLoadData(ConfigData data)
        {
            base.OnLoadData(data);
            BuffID = data.GetInt("BuffID");
            MBV_IconID = data.GetString("MBV_IconID");
            MBV_HeadIconID = data.GetString("MBV_HeadIconID");
            PriorityLevel = data.GetInt("PriorityLevel");
        }

        public override int DataID
        {
            get { return BuffID; }
        }

        #region property
        public int BuffID = 0;

        public int MBV_NameID = 0;

        public int MBV_TipsID = 0;

        public int MBV_BuffType = 0;

        public int MBV_BuffAttr = 0;

        public int MBV_BuffRemove = 0;

        public int MBV_BuffInummue = 0;

        public int MBV_BuffCleanUp = 0;

        public int MBV_BuffCleanUpGroup = 0;

        public int MBV_BuffState = 0;

        public int MBV_AttrValue = 0;

        public int MBV_LayerCnt = 0;

        public int MBV_DurationTime = 0;

        public int MBV_BuffSkill = 0;

        public int MBV_BuffSkillLv = 0;

        public int MBV_BuffLogic = 0;

        public int MBV_BuffSkillUserData = 0;

        public int MBV_BuffLogicParam1 = 0;

        public int MBV_BuffLogicParam2 = 0;

        public int MBV_BuffLogicParam3 = 0;

        public int MBV_EffectID = 0;

        public int MBV_EffectSurface = 0;

        public int MBV_EffectAddID = 0;

        public int MBV_EffectAddTime = 0;

        public int MBV_EffectRemoveID = 0;

        public int MBV_EffectRemoveTime = 0;

        public string MBV_IconID = "";

        public string MBV_HeadIconID = "";

        public int PriorityLevel = 0;
        #endregion
    }
}
