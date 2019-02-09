using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace XWorld
{
    public class ModifyDataManager : XWorldGameManagerBase
    {
        private SkillDataManager m_SkillDataManager = new SkillDataManager();
        private BuffDataManager m_BuffDataManager = new BuffDataManager();

        public override void InitManager()
        {          
            SkillData.InitSkillDefine();
            BuffData.InitBuffDefine();
        }
        public override void Update(float fElapseTimes)
        {

        }
        public override void ShutDown()
        {

        }
        public void OnLoadSkillData(ConfigData data)
        {
            m_SkillDataManager.OnLoadSkillData(data);
        }
        public void OnLoadSkillSlotsData(ConfigData data)
        {
            m_SkillDataManager.OnLoadSkillSlotsData(data);
        }
        public SkillData GetSkillData(int nDataID, int nLevel = 1)
        {
            return m_SkillDataManager.GetData(nDataID, nLevel);
        }
        public ModifyData GetSkillSlotsData(int nDataID)
        {
            return m_SkillDataManager.GetSkillSlotsData(nDataID);
        }
        public List<int> GetSubSkillList(int nSkillID)
        {
            return m_SkillDataManager.GetSubSkillList(nSkillID);
        }
        public void OnLoadBuffData(ConfigData data)
        {
            m_BuffDataManager.OnLoadBuffData(data);
        }       
        public void OnLoadBuffLevelData(ConfigData data)
        {
            m_BuffDataManager.OnLoadBuffLevelData(data);
        }
        public BuffData GetBuffData(int nDataID, int nLevel = 1)
        {
            return m_BuffDataManager.GetData(nDataID, nLevel);
        }
        public void ClearSkillData()
        {
            m_SkillDataManager.ClearData();
        }
        public void ClearBuffData()
        {
            m_BuffDataManager.ClearData();
        }
    };

    public class ModifyDataDict_T<T> : Dictionary<int, Dictionary<int, T>> where T: ModifyObject<T>
    {
        public void AddData(int nDataID, int nLevel, T data)
        {
            Dictionary<int, T> dict;
            if (!TryGetValue(nDataID, out dict))
            {
                dict = new Dictionary<int, T>();
                Add(nDataID, dict);
            }
            dict.Add(nLevel, data);
        }
        public T GetData(int nDataID, int nLevel = 1)
        {
            Dictionary<int, T> dict;
            if (!TryGetValue(nDataID, out dict))
                return null;

            T data;
            if (!dict.TryGetValue(nLevel, out data))
                return null;

            return data;
        }

        public virtual void ClearData()
        {
            Clear();
        }
    }
}
