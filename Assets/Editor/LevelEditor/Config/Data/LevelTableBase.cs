using System.Collections.Generic;
using System.IO;

namespace Galaxy
{
    public abstract class LevelTableBase
    {
        protected List<ILevelDataRow> m_dataList = new List<ILevelDataRow>();
        protected LevelEditor_ModelRes m_modelRes;
        protected LevelEditor_CommonEffect m_commEffect;

        public abstract string TableName
        {
            get;
        } 

        public void SetModelRes(LevelEditor_ModelRes modelRes)
        {
            m_modelRes = modelRes;
        }

        public void SetCommonEffect(LevelEditor_CommonEffect commonEffect)
        {
            m_commEffect = commonEffect;
        }

        public abstract void LoadData(string strTextContent);

        public abstract void SaveData(int sceneID);
          
        public virtual ILevelDataRow[] GetAllData()
        {
            if (m_dataList == null)
                return null;

            return m_dataList.ToArray();
        }

        protected LevelEditor_ModelRes GetModelRes()
        {
            return m_modelRes;
        }

        protected ModelResData GetModelResByID(int modelID)
        {
            if (m_modelRes == null)
                return null;

            return m_modelRes.GetModelResByID(modelID);
        }

        protected CommonEffect_Editor GetEffectByID(int effID)
        {
            if (m_commEffect == null)
                return null;

            return m_commEffect.GetEffectByID(effID);
        }
    }
}