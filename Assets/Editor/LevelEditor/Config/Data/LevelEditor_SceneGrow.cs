using System;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    public class SceneGrowBase : ILevelDataRow
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

        public int DataIndex
        {
            get;
            set;
        }

        public int GrowID
        {
            get;
            set;
        }

        public float Radius
        {
            get;
            set;
        }

        public Vector3 Pos
        {
            get;
            set;
        }

        public Vector3 Dir
        {
            get;
            set;
        }

        public bool ClientBorn
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

            Vector3 vPos = Vector3.zero;
            Vector3 vDir = Vector3.zero;
            for (int i = 0; i < arrTitle.Length; ++i)
            {
                if (arrTitle[i].Equals("SceneID"))
                {
                    SceneID = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("DataIndex"))
                {
                    DataIndex = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("GrowID"))
                {
                    GrowID = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("Radius"))
                {
                    Radius = float.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("PosX"))
                {
                    vPos.x = float.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("PosY"))
                {
                    vPos.y = float.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("PosZ"))
                {
                    vPos.z = float.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("DirX"))
                {
                    vDir.x = float.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("DirY"))
                {
                    vDir.y = float.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("DirZ"))
                {
                    vDir.z = float.Parse(arrContent[i]);
                }

                if(arrTitle[i].Equals("ClientBorn"))
                {
                    ClientBorn = int.Parse(arrContent[i]) == 1 ? true : false;
                }
            }

            this.Pos = vPos;
            this.Dir = vDir;
        }
    }

    public class SceneGrow : SceneGrowBase
    {
        public NpcGrowData GrowConfig
        {
            get;
            set;
        }

        public void LoadSceneGrow(NpcGrowData data)
        {
            this.GrowConfig = data;
        }
    }

    public class LevelEditor_SceneGrow : LevelTableBase
    {
        public override string TableName
        {
            get
            {
                return "scene_grow_client.txt";
            }
        }

        public override void LoadData(string strTextContent)
        {
            string[] arrContent = MonsterEditor.Utility.Text.SplitToLines(strTextContent);
            string strTitle = arrContent[0];
            for (int i = 2; i < arrContent.Length; ++i)
            {
                SceneGrow sceneGrowData = new SceneGrow();
                sceneGrowData.ParseDataRow(arrContent[i], strTitle);
                m_dataList.Add(sceneGrowData);
            }
        }

        public void LoadGrowConfig(ref LevelEditor_NpcGrow npcGrow)
        {
            foreach(ILevelDataRow item in m_dataList)
            {
                SceneGrow grow = item as SceneGrow;
                NpcGrowData npcGrowData = npcGrow.GetDataByID(grow.GrowID);
                grow.LoadSceneGrow(npcGrowData);
            }
        }

        public override void SaveData(int sceneID)
        {
        }
    }
}
