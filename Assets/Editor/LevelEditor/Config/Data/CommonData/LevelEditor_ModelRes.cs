
namespace Galaxy
{
    public class ModelResData : ILevelDataRow
    {
        public int Id
        {
            get
            {
                return 0;
            }
        }

        public int TemplateID
        {
            get;
            set;
        }

        public string ModelName
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

            for(int i = 0; i < arrTitle.Length;++i)
            {
                if (arrTitle[i].Equals("ModelID"))
                {
                    TemplateID = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("ModelName"))
                {
                    ModelName = arrContent[i];
                }
            }
        }
    }

    public  class LevelEditor_ModelRes : LevelTableBase
    {
        public override string TableName
        {
            get
            {
                return "modelresdefine.txt";
            }
        }

        public override void LoadData(string strTextContent)
        {
            string[] arrContent = MonsterEditor.Utility.Text.SplitToLines(strTextContent);
            string title = arrContent[0];

            int index = 0;
            foreach (string item in arrContent)
            {
                if (index < 2)
                {
                    index++;
                    continue;
                }
                    
                ModelResData modelData = new ModelResData();
                modelData.ParseDataRow(item, title);

                m_dataList.Add(modelData);
            }
        }

        public ModelResData GetDataByID(int modelResID)
        {
            for(int i = 0; i < m_dataList.Count; ++i)
            {
                ModelResData modelData = m_dataList[i] as ModelResData;
                if (modelData == null)
                    continue;

                if (modelData.TemplateID == modelResID)
                    return modelData;
            }

            return null;
        }

        public override void SaveData(int sceneID) { }
    }
}
