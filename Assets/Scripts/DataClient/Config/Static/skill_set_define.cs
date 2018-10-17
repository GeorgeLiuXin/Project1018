// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace XWorld.DataConfig
{
    using UnityEngine;
    using System.Collections.Generic;
    using System;
    
    
    public class skill_set_define : System.ICloneable
    {
        
        public int setID;
        
        public string skillGroup;
        
        public int groupID;
        
        public object Clone()
        {
            return this.MemberwiseClone();;
        }
    }
    
    public class Tableskill_set_define : XWorld.DataConfig.TableBase
    {
        
        public List<skill_set_define> m_configList = new List<skill_set_define>();
        
        public void LoadData(XWorld.DataConfig.skill_set_define codeValue)
        {
            m_configList.Add(codeValue);
        }
        
        public int GetRowCount()
        {
            return 3;
        }
        
        public int GetDataCount()
        {
            if (m_configList == null)
            {
                return 0;
            }
            return m_configList.Count;
        }
        
        public skill_set_define GetData(int rowIdx)
        {
            if (m_configList != null && rowIdx >= 0 && rowIdx < m_configList.Count)
            {
                return m_configList[rowIdx];
            }
            return null;
        }
        
        public override void LoadData(string content)
        {
            string[] values = content.Split("\r"[0]);
            for (int i = 2; (i < values.Length); i = (i + 1))
            {
                skill_set_define data = new skill_set_define();
                int j = 0;
                string[] subValues = values[i].TrimStart('\n').Split(ClientConfigManager.CMD_STRING, StringSplitOptions.None);
                if (subValues != null && subValues.Length == GetRowCount())
                {
                    data.setID = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.skillGroup = (subValues[j]);
                    j = j + 1;
                    data.groupID = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    m_configList.Add(data);
                }
            }
        }
    }
}
