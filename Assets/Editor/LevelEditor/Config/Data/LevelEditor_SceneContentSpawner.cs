using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public interface ILevelDataRow
    {
        void ParseDataRow(string dataRowText,string strTitle);
    }

    //BaseContent
    public class MonsterDataBase : ILevelDataRow
    {
        public int Id
        {
            get
            {
                return 0;
            }
        }

        public int MonsterID
        {
            get;
            set;
        }

        public string MonsterName
        {
            get;
            set;
        }

        public int NpcFlag
        {
            get;
            set;
        }

        #region Npc/Monster
        public int flag
        {
            get;
            set;
        }
        #endregion

        public string ModelPath
        {
            get;
            set;
        }

        public int ModelID
        {
            get;
            set;
        }


        #region Parse
        public void ParseDataRow(string dataRowText, string strTitle)
        {
            string[] arrTitle = MonsterEditor.Utility.Text.SplitDataRow(strTitle);
            string[] arrContent = MonsterEditor.Utility.Text.SplitDataRow(dataRowText);
            if (arrContent == null)
                return;

            for(int i = 0;i < arrTitle.Length;++i)
            {
                if (arrTitle[i].Equals("ID"))
                {
                    MonsterID = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("MNName"))
                {
                    MonsterName = arrContent[i];
                }

                if (arrTitle[i].Equals("npcflag"))
                {
                    NpcFlag = int.Parse(arrContent[i]);
                }

                 if (arrTitle[i].Equals("modid"))
                {
                    ModelID = int.Parse(arrContent[i]);
                    /*ModelResData data = modelConfig.GetDataByID(modelRes);
                    if (data != null)
                    {
                        ModelPath = data.ModelName;
                    }*/
                }
            }
        }
        #endregion
    }

    //
    public class ParamMonsterData : MonsterDataBase
    {
        public void LoadMonsterTxt(LevelEditor_ModelRes modelConfig,string strTitle)
        {
            ModelResData data = modelConfig.GetDataByID(ModelID);
            if (data != null)
            {
                ModelPath = data.ModelName;
            }
        }
    }

    public class LevelEditor_SceneContentSpawner : LevelTableBase
    {
        public override string TableName
        {
            get
            {
                return "param_monster.txt";
            }
        }

        public override void LoadData(string strTextContent)
        {
            string[] arrContent = MonsterEditor.Utility.Text.SplitToLines(strTextContent);

            string strTitle = arrContent[0];

            for (int i = 2; i < arrContent.Length; ++i)
            {
                ParamMonsterData monsterData = new ParamMonsterData();
                monsterData.ParseDataRow(arrContent[i], strTitle);
                m_dataList.Add(monsterData);
            }

            foreach (ILevelDataRow item in m_dataList)
            {
                ParamMonsterData monsterData = item as ParamMonsterData;
                monsterData.LoadMonsterTxt(GetModelRes(), strTitle);
            }
        }

        public override void SaveData(int sceneID) { }
    }
}