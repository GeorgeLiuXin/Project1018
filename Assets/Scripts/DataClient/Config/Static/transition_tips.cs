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
    
    
    public class transition_tips : System.ICloneable
    {
        
        public int id;
        
        public string content;
        
        public object Clone()
        {
            return this.MemberwiseClone();;
        }
    }
    
    public class Tabletransition_tips : XWorld.DataConfig.TableBase
    {
        
        public List<transition_tips> m_configList = new List<transition_tips>();
        
        public void LoadData(XWorld.DataConfig.transition_tips codeValue)
        {
            m_configList.Add(codeValue);
        }
        
        public int GetRowCount()
        {
            return 2;
        }
        
        public int GetDataCount()
        {
            if (m_configList == null)
            {
                return 0;
            }
            return m_configList.Count;
        }
        
        public transition_tips GetData(int rowIdx)
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
                transition_tips data = new transition_tips();
                int j = 0;
                string[] subValues = values[i].TrimStart('\n').Split(ClientConfigManager.CMD_STRING, StringSplitOptions.None);
                if (subValues != null && subValues.Length == GetRowCount())
                {
                    data.id = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.content = (subValues[j]);
                    j = j + 1;
                    m_configList.Add(data);
                }
            }
        }
    }
}
