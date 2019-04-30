using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

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


        public bool Check(int nGroupID, ActorObj obj, GCommonConditionParam param)
        {
            ErrorCode = 0;
            if (m_vConditionGroup == null || !m_vConditionGroup.ContainsKey(nGroupID))
                return false;

            return m_vConditionGroup[nGroupID].Check(obj, param);
        }

        public void OnLoadALLConfig(ref ConfigData[] list)
        {

            if (m_vConditionGroup == null)
                m_vConditionGroup = new Dictionary<int, GCommonConditionGroup>();

            string[] vConditionName = FillConditionVarName("ConditionName");
            string[] vParamString = FillConditionVarName("ParamString");
            string[] vCmpType = FillConditionVarName("CmpType");
            string[] vErrorCode = FillConditionVarName("ErrorCode");
            string[] vFailTips = FillConditionVarName("FailTips");

            for (int x = 0; x < list.Length; ++x)
            {
                int nGroupID = list[x].GetInt("GroupID");
                int nLogicType = 0;// def.LogicType; 

                for (int i = 0; i < CommonConditionSize; ++i)
                {
                    string sConditionName = list[x].GetString(vConditionName[i]);
                    if (string.IsNullOrEmpty(sConditionName))
                        break;

                    Assembly ass = Assembly.GetExecutingAssembly();
                    GCommonCondition condition = (GCommonCondition)ass.CreateInstance("XWorld." + sConditionName);
                    if (condition == null)
                        continue;

                    condition.manager = this;
                    condition.m_nLogicType = (GCommonConditionType)nLogicType;
                    condition.m_nCmpType = (GCommonConditionCmp)list[x].GetInt(vCmpType[i]);// Convert.ToInt32(def.GetType().GetField(vCmpType[i]).GetValue(def).ToString());
                    condition.m_nErrorCode = list[x].GetInt(vErrorCode[i]);//Convert.ToInt32(def.GetType().GetField(vErrorCode[i]).GetValue(def).ToString());
                    condition.m_nFailTips = list[x].GetInt(vFailTips[i]);//Convert.ToInt32(def.GetType().GetField(vFailTips[i]).GetValue(def).ToString());
                    condition.Init(list[x].GetString(vParamString[i]));

                    if (!m_vConditionGroup.ContainsKey(nGroupID))
                        m_vConditionGroup.Add(nGroupID, new GCommonConditionGroup());

                    m_vConditionGroup[nGroupID].Add(condition);
                }
            }
        }

        string[] FillConditionVarName(string strName)
        {
            string[] strList = new string[CommonConditionSize];
            for (int i = 0; i < CommonConditionSize; ++i)
            {
                strList[i] = strName + (i + 1).ToString();
            }
            return strList;
        }
    }

    enum eWeekDay
    {
        Sunday = 0,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
    }
    
    public class SDCheckProjectile : GCommonCondition
    {
        public override void Init(string initStr)
        {
            nSkillID = Convert.ToInt32(initStr);
        }

        public override bool Check(ActorObj obj, GCommonConditionParam param)
        {
            //List<ProjectileClient> projectileList = XWorldGameModule.GetGameManager<ProjectileManager>().GetProjectileByAvatarSkill(obj.ServerID, nSkillID);
            //if (null == projectileList || 0 == projectileList.Count)
            //{
            //    return false;
            //}
            return true;
        }

        private int nSkillID;
    }

}
