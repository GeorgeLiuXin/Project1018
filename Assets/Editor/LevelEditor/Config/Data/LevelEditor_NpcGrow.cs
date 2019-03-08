using System.Collections.Generic;

namespace Galaxy
{
    public class NpcGrowDataBase : ILevelDataRow
    {
        public int Id
        {
            get
            {
                return 0;
            }
        }

        public int NpcGrowID
        {
            get;
            set;
        }

        public int NpcNameID
        {
            get;
            set;
        }

        public int NpcGrowModelID
        {
            get;
            set;
        }

        public int NpcGrowEffectID
        {
            get;
            set;
        }

        public string NpcGrowName
        {
            get;
            set;
        }

        public string ModelPath
        {
            get;
            set;
        }

        public string EffectPath
        {
            get;
            set;
        }

        public void ParseDataRow(string dataRowText, string strTitle)
        {
            string[] arrTitle = MonsterEditor.Utility.Text.SplitDataRow(strTitle);
            string[] arrContent = MonsterEditor.Utility.Text.SplitDataRow(dataRowText);
            if (arrContent == null)
                return;

            for(int i = 0;i < arrTitle.Length; ++i)
            {
                if (arrTitle[i].Equals("GrowID"))
                {
                    NpcGrowID = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("NameID"))
                {
                    NpcNameID = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("ModelResID"))
                {
                    NpcGrowModelID = int.Parse(arrContent[i]);
                }

                if(arrTitle[i].Equals("EffectID"))
                {
                    NpcGrowEffectID = int.Parse(arrContent[i]);
                }
            }
        }
    }

    public class NpcGrowData : NpcGrowDataBase
    {
        public void LoadMonsterTxt(ref Dictionary<int, string> dictTemplate, LevelEditor_ModelRes modelConfig,string strTitle)
        {
            string strDataRow = null;
            if (!dictTemplate.TryGetValue(NpcGrowID, out strDataRow) || modelConfig == null)
                return;
            
            string[] arrTitle = MonsterEditor.Utility.Text.SplitDataRow(strTitle);
            string[] dataRow = MonsterEditor.Utility.Text.SplitDataRow(strDataRow);
            for(int i = 0; i < arrTitle.Length; ++i)
            {
                if(arrTitle[i].Equals("modid"))
                {
                    int modelId =  int.Parse(dataRow[i]);
                    ModelResData data = modelConfig.GetDataByID(modelId);
                    if(data != null)
                    {
                        ModelPath = data.ModelName;
                    }
                    break;
                }
            }
        }
    }


    public class LevelEditor_NpcGrow : LevelTableBase
    {
        public override string TableName
        {
            get
            {
                return "grow_template_define.txt";
            }
        }

        public override void LoadData(string strTextContent)
        {
            string[] arrContent = MonsterEditor.Utility.Text.SplitToLines(strTextContent);
            string strTitle = arrContent[0];
            for (int i = 2; i < arrContent.Length; ++i)
            {
                NpcGrowData npcGrow = new NpcGrowData();
                npcGrow.ParseDataRow(arrContent[i], strTitle);
                if(npcGrow.NpcGrowEffectID > 0)
                {
                    CommonEffect_Editor effectData = m_commEffect.GetDataByID(npcGrow.NpcGrowEffectID);
                    if(effectData != null)
                    {
                        npcGrow.EffectPath = effectData.EffectPath;
                    }
                }

                if(npcGrow.NpcGrowModelID > 0)
                {
                    ModelResData modelData = m_modelRes.GetDataByID(npcGrow.NpcGrowModelID);
                    if(modelData != null)
                    {
                        npcGrow.ModelPath = modelData.ModelName;
                    }
                }

                m_dataList.Add(npcGrow);
            }
        }

        public NpcGrowData GetDataByID(int id)
        {
            foreach(ILevelDataRow item in m_dataList)
            {
                NpcGrowData data = item as NpcGrowData;
                if (data.NpcGrowID == id)
                    return data;
            }

            return null;
        }

        public override void SaveData(int sceneID)
        {

        }
    }
}