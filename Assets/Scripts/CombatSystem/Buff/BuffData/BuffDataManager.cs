using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace XWorld
{
    public class BuffDataManager : ModifyDataDict_T<BuffData>
    {
        #region buff icon 
        private string strPatten = ".png";
        #endregion

        public BuffDataManager()
        {

        }
        public void OnLoadBuffData(ConfigData data)
        {
            BuffData buffData = new BuffData();
            if (buffData != null && data != null)
            {
                buffData.OnLoadData(data);
                AddData(buffData.BuffID, 1, buffData);
            }           
        }
        public void OnLoadBuffLevelData(ConfigData data)
        {
            int buffID = data.GetInt("DataID");
            int buffLevel = data.GetInt("ModifyID");
            string valueName = data.GetString("ValueName");
            int valueID = BuffData.GetModifyValueID(valueName);
            int iValue = data.GetInt("IValue");
            float fValue = data.GetFloat("FValue");
 
            BuffData levelData = GetData(buffID, buffLevel);
            if (levelData == null)
            {
                BuffData buffData = GetData(buffID);
                if (buffData == null)
                    return;

                levelData = buffData.Clone() as BuffData;
                if (levelData == null)
                    return;

                levelData.m_nLevel = buffLevel;
                AddData(levelData.DataID, levelData.m_nLevel, levelData);
            }

            levelData.Combine(valueID, iValue, fValue);
        }

        public override void ClearData()
        {
            base.ClearData();
        }
    }
}


