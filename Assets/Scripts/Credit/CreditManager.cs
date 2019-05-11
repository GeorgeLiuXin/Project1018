using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld
{
    public class CreditType
    {
        private Dictionary<int, CreditLevelId> CreditRelationMap = new Dictionary<int, CreditLevelId>();
        public CreditLevelId GetCredit(int campType)
        {
            if (!CreditRelationMap.ContainsKey(campType))
            {
                return m_Default;
            }
            CreditLevelId res;
            if (!CreditRelationMap.TryGetValue(campType, out res))
            {
                return m_Default;
            }
            return res;
        }
        public void Add(int campType, CreditLevelId lvID)
        {
            CreditRelationMap.Add(campType, lvID);
        }

        public CreditLevelId m_Default;
    }

	public class CreditManager : Singleton<CreditManager>
	{
		private Dictionary<int, CreditType> CreditMap = new Dictionary<int, CreditType>();

		public CreditManager()
        {
            Init();
        }
        public bool CheckRelation(ActorEntity objA, ActorEntity objB, TargetType type)
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

        public bool IsEnemy(CreditLevelId nLevelId)
        {
            return nLevelId == CreditLevelId.Credit_Hostile;
        }

        public bool IsFriend(CreditLevelId nLevelId)
        {
            return nLevelId >= CreditLevelId.Credit_Friendly;
        }

        public bool IsNeutral(CreditLevelId nLevelId)
        {
            return nLevelId == CreditLevelId.Credit_Neutrality;
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

		private void Init()
		{
			ConfigData[] configDataList1 = GameDataProxy.GetAllData("CreditRelation");
			if (configDataList1 == null)
				return;
			foreach (ConfigData item in configDataList1)
			{
				if (item == null)
					continue;

				CreditType cr = GetCreditType(item.GetInt("typeid"));
				cr.Add(item.GetInt("relationid"), (CreditLevelId) item.GetInt("relationlevel"));
			}

			ConfigData[] configDataList2 = GameDataProxy.GetAllData("CreditType");
			if (configDataList2 == null)
				return;
			foreach (ConfigData item in configDataList2)
			{
				if (item == null)
					continue;

				CreditType cr = GetCreditType(item.GetInt("typeid"));
				cr.m_Default = (CreditLevelId) item.GetInt("defaultlevel");
			}
		}
	}
}
