
namespace Galaxy
{
    public class SceneDfData : ILevelDataRow
    {
        public int Id
        {
            get
            {
                return 0;
            }
        }

        public int SceneID
        {
            get;
            set;
        }

        public string SceneName
        {
            get;
            set;
        }

        public int MapType
        {
            get;
            set;
        }

        public void ParseDataRow(string dataRowText,string strTitle)
        {
            string[] text = MonsterEditor.Utility.Text.SplitDataRow(dataRowText);
            if (text == null)
                return;

            SceneID = int.Parse(text[0]);
            SceneName = text[2];
            MapType = int.Parse(text[5]);
        }
    }

    public class LevelEditor_SceneDefine : LevelTableBase
    {
        public override string TableName
        {
            get
            {
                return "scenedefine.txt";
            }
        }

        public override void LoadData(string strTextContent)
        {
            string[] arrDataRow = MonsterEditor.Utility.Text.SplitToLines(strTextContent);
            for(int i = 2; i < arrDataRow.Length;++ i)
            {
                SceneDfData sceneDefineData = new SceneDfData();
                sceneDefineData.ParseDataRow(arrDataRow[i],arrDataRow[0]);

                m_dataList.Add(sceneDefineData);
            }
        }

        public string GetSceneName(int sceneID)
        {
            foreach(ILevelDataRow item in m_dataList)
            {
                SceneDfData data = item as SceneDfData;
                if (data.SceneID == sceneID)
                    return data.SceneName;
            }

            return null;
        }

        public override void SaveData(int sceneID) { }
    }
}