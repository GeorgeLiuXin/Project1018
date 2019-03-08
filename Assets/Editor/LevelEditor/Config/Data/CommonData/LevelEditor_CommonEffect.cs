
namespace Galaxy
{
    public class CommonEffect_Editor : ILevelDataRow
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

        public string EffectPath
        {
            get;
            set;
        }

        public float EffectScale
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

            for (int i = 0; i < arrTitle.Length; ++i)
            {
                if (arrTitle[i].Equals("effectid"))
                {
                    TemplateID = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("particle"))
                {
                    EffectPath = arrContent[i];
                }

                if (arrTitle[i].Equals("scale"))
                {
                    EffectScale = float.Parse(arrContent[i]);
                }
            }
        }
    }

    public class LevelEditor_CommonEffect : LevelTableBase
    {
        public override string TableName
        {
            get
            {
                return "commoneffects.txt";
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

                CommonEffect_Editor modelData = new CommonEffect_Editor();
                modelData.ParseDataRow(item, title);

                m_dataList.Add(modelData);
            }
        }

        public CommonEffect_Editor GetDataByID(int effectID)
        {
            for (int i = 0; i < m_dataList.Count; ++i)
            {
                CommonEffect_Editor effectData = m_dataList[i] as CommonEffect_Editor;
                if (effectData == null)
                    continue;

                if (effectData.TemplateID == effectID)
                    return effectData;
            }

            return null;
        }

        public override void SaveData(int sceneID) { }
    }
}
