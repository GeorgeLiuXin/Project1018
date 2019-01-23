using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld
{
    public class CreditType
    {
        private Dictionary<int, CreditLevelId> CreditRelationMap = new Dictionary<int, CreditLevelId>();
        public CreditLevelId GetCredit(int creditType)
        {
            if (!CreditRelationMap.ContainsKey(creditType))
            {
                return m_Default;
            }
            CreditLevelId res;
            if (!CreditRelationMap.TryGetValue(creditType, out res))
            {
                return m_Default;
            }
            return res;
        }
        public void Add(int creditType, CreditLevelId lvID)
        {
            CreditRelationMap.Add(creditType, lvID);
        }

        public CreditLevelId m_Default;
    }

    public class CreditManager //: GameDataManager
    {
        public readonly static CreditManager Instance = new CreditManager();

        public CreditManager()
        {
            Init();
        }
        public bool CheckRelation(ActorObj objA, ActorObj objB, TargetType type)
        {
            if (objA == null || objB == null)
                return false;
            int cmpA = objA.GetCamp();
            int cmpB = objB.GetCamp();
            CreditLevelId cl = CreditLevelId.Credit_Friendly;
            if (cmpB <= 0)
            {
                cl = CreditLevelId.Credit_Friendly;
            }
            else
            {
                CreditType creditA = GetCreditType(cmpA);
                if (creditA != null)
                {
                    cl = creditA.GetCredit(cmpB);
                }
            }
            if ((type & TargetType.ToEnemy) != 0 && IsEnemy(cl))
                return true;

            if ((type & TargetType.ToFriend) != 0 && IsFriend(cl))
                return true;

            if ((type & TargetType.ToNeutral) != 0 && IsNeutral(cl))
                return true;
            return false;
        }
        public CreditType GetCreditType(int typeID)
        {
            CreditType res;
            if (!CreditMap.TryGetValue(typeID, out res))
            {
                res = new CreditType();
                CreditMap.Add(typeID, res);
            }
            return res;
        }

        public bool IsEnemy(CreditLevelId nLevelId)
        {
            return (bool)(nLevelId == CreditLevelId.Credit_Hostile);
        }

        public bool IsFriend(CreditLevelId nLevelId)
        {
            return nLevelId >= CreditLevelId.Credit_Friendly;
        }

        public bool IsNeutral(CreditLevelId nLevelId)
        {
            return (bool)(nLevelId == CreditLevelId.Credit_Neutrality);
        }

        private void Init()
        {

            ConfigData configData = ClientConfigManager.GetTableByName("CreditRelation") as TableCreditRelation;
            if (t_ConfigList == null)
                return;
            foreach (DataConfig.CreditRelation item in t_ConfigList.m_configList)
            {
                if (item == null)
                    continue;

                CreditType cr = GetCreditType(item.typeid);
                cr.Add(item.relationid, (CreditLevelId)item.relationlevel);
            }

            TableCreditType t_List = ClientConfigManager.GetTableByName("TableCreditType") as TableCreditType;
            if (t_List == null)
                return;
            foreach (DataConfig.CreditType item in t_List.m_configList)
            {
                if (item == null)
                    continue;

                CreditType cr = GetCreditType(item.typeid);
                cr.m_Default = (CreditLevelId)item.defaultlevel;
            }

        }

        private Dictionary<int, CreditType> CreditMap = new Dictionary<int, CreditType>();
    }
}
