using System;
using System.Collections.Generic;
using System.Reflection;
using XWorld.DataConfig;

namespace XWorld
{
    public class GCommonConditionParam
    {

    }

    public enum GCommonConditionType
	{
		ConditionCheck_AND = 0,
		ConditionCheck_OR,
		ConditionCheck_Equal,
	};

    public enum GCommonConditionCmp
    {
        ConditionCmp_Greater = 0,       // >
        ConditionCmp_GreaterAndEqual,   // >=
        ConditionCmp_Less,              // <
        ConditionCmp_LessAndEqual,      // <=
        ConditionCmp_Equal,             // ==
        ConditionCmp_NotEqual,          // !=
        ConditionCmp_And,               // &
        ConditionCmp_Or,                // |
        ConditionCmp_Not,
    };

    public class GCommonCondition
    {
        public bool bNot = false;
        public GCommonConditionType m_nLogicType = GCommonConditionType.ConditionCheck_AND;
        public GCommonConditionCmp m_nCmpType = GCommonConditionCmp.ConditionCmp_Greater;
        public int m_nErrorCode = 0;
        public int m_nFailTips = -1;
        public int m_nCheckGroup = -1;
        public GCommonConditionManager manager { get; set; }

        public virtual void Init(string initStr)
        {
            m_nCheckGroup = Convert.ToInt32(initStr);
        }

        public virtual bool Check(ActorObj obj, GCommonConditionParam param)
        {
            if (m_nCheckGroup != -1 && manager != null)
            {
                return manager.Check(m_nCheckGroup, obj, param);
            }
            return false;
        }
   
        public virtual void OnCheckFail(ActorObj obj, GCommonConditionParam param)
        {
            if (manager == null)
                return;

            manager.ErrorCode = m_nErrorCode;
        }
    }

    public class GCommonConditionGroup : List<GCommonCondition>
    {
        public bool Check(ActorObj obj, GCommonConditionParam param)
        {
            bool bSuccess = false;
            for (int i = 0; i < Count; ++i)
            {
                GCommonCondition condition = this[i];
                if (condition == null)
                    continue;

                bSuccess = condition.Check(obj, param);
                if (condition.m_nCmpType == GCommonConditionCmp.ConditionCmp_Not)
                    bSuccess = !bSuccess;

                if (bSuccess)
                {
                    if (condition.m_nLogicType == GCommonConditionType.ConditionCheck_OR)
                        break;
                }
                else
                {
                    if (condition.m_nLogicType == GCommonConditionType.ConditionCheck_AND)
                    {
                        condition.OnCheckFail(obj, param);
                        break;
                    }
                }
            }
            return bSuccess;
        }
    }

    public class GCommonConditionManager //: GameDataManager
    {
        public static readonly GCommonConditionManager Instance = new GCommonConditionManager();
        public static readonly int CommonConditionSize = 10;
        Dictionary<int, GCommonConditionGroup> m_vConditionGroup;
        Dictionary<int, GCommonCondition> ConditionList;
        public int ErrorCode { get; set; }

//         public string TableName{get;set;}
//         public string ConTableName{get;set;}

        private bool bInit = false;

        public bool Check(int nGroupID, ActorObj obj, GCommonConditionParam param)
        {
            if (!bInit)
            {
                Load();
                bInit = true;
            }

            ErrorCode = 0;
            if (m_vConditionGroup == null || m_vConditionGroup[nGroupID] == null)
                return false;

            return m_vConditionGroup[nGroupID].Check(obj, param);
        }
        public void Load()
        {
            if (bInit)
                return;

            LoadConditionFile();
        }

        void LoadConditionFile()
        {
            TableClientCheckCondition t_tableConfig = ClientConfigManager.GetTableByName("TableClientCheckCondition") as TableClientCheckCondition;
            if (t_tableConfig == null)
                return;

            if (m_vConditionGroup == null)
                m_vConditionGroup = new Dictionary<int, GCommonConditionGroup>();

            string[] vConditionName = FillConditionVarName("ConditionName");
            string[] vParamString = FillConditionVarName("ParamString");
            string[] vCmpType = FillConditionVarName("CmpType");
            string[] vErrorCode = FillConditionVarName("ErrorCode");
            string[] vFailTips = FillConditionVarName("FailTips");

            foreach (ClientCheckCondition def in t_tableConfig.m_configList)
            {
                int nGroupID = def.GroupID;
                int nLogicType = 0;// def.LogicType; 

                for(int i = 0; i < CommonConditionSize; ++i)
                {
                    string sConditionName = def.GetType().GetField(vConditionName[i]).GetValue(def).ToString();
                    if (string.IsNullOrEmpty(sConditionName))
                        break;

                    Assembly ass = Assembly.GetExecutingAssembly();
                    GCommonCondition condition = (GCommonCondition)ass.CreateInstance("XWorld." + sConditionName);
                    if (condition == null)
                        continue;

                    condition.manager = this;
                    condition.m_nLogicType = (GCommonConditionType)nLogicType; 
                    condition.m_nCmpType = (GCommonConditionCmp)Convert.ToInt32(def.GetType().GetField(vCmpType[i]).GetValue(def).ToString());
                    condition.m_nErrorCode = Convert.ToInt32(def.GetType().GetField(vErrorCode[i]).GetValue(def).ToString());
                    condition.m_nFailTips = Convert.ToInt32(def.GetType().GetField(vFailTips[i]).GetValue(def).ToString());
                    condition.Init(def.GetType().GetField(vParamString[i]).GetValue(def).ToString());
           
                    if (m_vConditionGroup[nGroupID] == null)
                        m_vConditionGroup[nGroupID] = new GCommonConditionGroup();

                    m_vConditionGroup[nGroupID].Add(condition);
                }
            }
            t_tableConfig.m_configList.Clear();
        }

        string[] FillConditionVarName(string strName)
        {
            string[] strList = new string[CommonConditionSize];
            for (int i = 0; i < CommonConditionSize; ++i)
            {
                strList[i] = strName + i.ToString();
            }
            return strList;
        }
    }
}
