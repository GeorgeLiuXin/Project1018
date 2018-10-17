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
    
    
    public class ClientCheckCondition : System.ICloneable
    {
        
        public int GroupID;
        
        public int LogicType;
        
        public string ConditionName1;
        
        public string ParamString1;
        
        public int CmpType1;
        
        public int ErrorCode1;
        
        public int FailTips1;
        
        public string ConditionName2;
        
        public string ParamString2;
        
        public int CmpType2;
        
        public int ErrorCode2;
        
        public int FailTips2;
        
        public string ConditionName3;
        
        public string ParamString3;
        
        public int CmpType3;
        
        public int ErrorCode3;
        
        public int FailTips3;
        
        public string ConditionName4;
        
        public string ParamString4;
        
        public int CmpType4;
        
        public int ErrorCode4;
        
        public int FailTips4;
        
        public string ConditionName5;
        
        public string ParamString5;
        
        public int CmpType5;
        
        public int ErrorCode5;
        
        public int FailTips5;
        
        public string ConditionName6;
        
        public string ParamString6;
        
        public int CmpType6;
        
        public int ErrorCode6;
        
        public int FailTips6;
        
        public string ConditionName7;
        
        public string ParamString7;
        
        public int CmpType7;
        
        public int ErrorCode7;
        
        public int FailTips7;
        
        public string ConditionName8;
        
        public string ParamString8;
        
        public int CmpType8;
        
        public int ErrorCode8;
        
        public int FailTips8;
        
        public string ConditionName9;
        
        public string ParamString9;
        
        public int CmpType9;
        
        public int ErrorCode9;
        
        public int FailTips9;
        
        public string ConditionName10;
        
        public string ParamString10;
        
        public int CmpType10;
        
        public int ErrorCode10;
        
        public int FailTips10;
        
        public object Clone()
        {
            return this.MemberwiseClone();;
        }
    }
    
    public class TableClientCheckCondition : XWorld.DataConfig.TableBase
    {
        
        public List<ClientCheckCondition> m_configList = new List<ClientCheckCondition>();
        
        public void LoadData(XWorld.DataConfig.ClientCheckCondition codeValue)
        {
            m_configList.Add(codeValue);
        }
        
        public int GetRowCount()
        {
            return 52;
        }
        
        public int GetDataCount()
        {
            if (m_configList == null)
            {
                return 0;
            }
            return m_configList.Count;
        }
        
        public ClientCheckCondition GetData(int rowIdx)
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
                ClientCheckCondition data = new ClientCheckCondition();
                int j = 0;
                string[] subValues = values[i].TrimStart('\n').Split(ClientConfigManager.CMD_STRING, StringSplitOptions.None);
                if (subValues != null && subValues.Length == GetRowCount())
                {
                    data.GroupID = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.LogicType = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ConditionName1 = (subValues[j]);
                    j = j + 1;
                    data.ParamString1 = (subValues[j]);
                    j = j + 1;
                    data.CmpType1 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ErrorCode1 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.FailTips1 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ConditionName2 = (subValues[j]);
                    j = j + 1;
                    data.ParamString2 = (subValues[j]);
                    j = j + 1;
                    data.CmpType2 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ErrorCode2 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.FailTips2 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ConditionName3 = (subValues[j]);
                    j = j + 1;
                    data.ParamString3 = (subValues[j]);
                    j = j + 1;
                    data.CmpType3 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ErrorCode3 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.FailTips3 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ConditionName4 = (subValues[j]);
                    j = j + 1;
                    data.ParamString4 = (subValues[j]);
                    j = j + 1;
                    data.CmpType4 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ErrorCode4 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.FailTips4 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ConditionName5 = (subValues[j]);
                    j = j + 1;
                    data.ParamString5 = (subValues[j]);
                    j = j + 1;
                    data.CmpType5 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ErrorCode5 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.FailTips5 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ConditionName6 = (subValues[j]);
                    j = j + 1;
                    data.ParamString6 = (subValues[j]);
                    j = j + 1;
                    data.CmpType6 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ErrorCode6 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.FailTips6 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ConditionName7 = (subValues[j]);
                    j = j + 1;
                    data.ParamString7 = (subValues[j]);
                    j = j + 1;
                    data.CmpType7 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ErrorCode7 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.FailTips7 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ConditionName8 = (subValues[j]);
                    j = j + 1;
                    data.ParamString8 = (subValues[j]);
                    j = j + 1;
                    data.CmpType8 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ErrorCode8 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.FailTips8 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ConditionName9 = (subValues[j]);
                    j = j + 1;
                    data.ParamString9 = (subValues[j]);
                    j = j + 1;
                    data.CmpType9 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ErrorCode9 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.FailTips9 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ConditionName10 = (subValues[j]);
                    j = j + 1;
                    data.ParamString10 = (subValues[j]);
                    j = j + 1;
                    data.CmpType10 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.ErrorCode10 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    data.FailTips10 = ClientConfigManager.ToInt32(subValues[j]);
                    j = j + 1;
                    m_configList.Add(data);
                }
            }
        }
    }
}
