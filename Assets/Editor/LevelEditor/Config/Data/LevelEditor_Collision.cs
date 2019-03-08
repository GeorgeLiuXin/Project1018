using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    public class CollisionData : ILevelDataRow
    {
        public int Id
        {
            get
            {
                return 1;
            }
        }

        public int GroupID
        {
            get;
            set;
        }

        public int SphereID
        {
            get;
            set;
        }

        public float Radius
        {
            get;
            set;
        }

        public float damageReduction
        {
            get;
            set;
        }

        public Vector3 Pos
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
            damageReduction = 1;
            Vector3 vTempPos = Vector3.zero;
            for (int i = 0; i < arrTitle.Length; ++i)
            {
                if (arrTitle[i].Equals("GroupID"))
                {
                    GroupID = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("ShapeID"))
                {
                    SphereID = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("SrvRadius"))
                {
                    Radius = float.Parse(arrContent[i]) * 2f;
                }

                if (arrTitle[i].Equals("SrvX"))
                {
                    vTempPos.x = float.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("SrvY"))
                {
                    vTempPos.z = float.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("SrvZ"))
                {
                    vTempPos.y = float.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("DamageReduction"))
                {
                    damageReduction = float.Parse(arrContent[i]);
                }
            }

            this.Pos = vTempPos;
        }
        
    }

    public class LevelEditor_Collision : LevelTableBase
    {
        public override string TableName
        {
            get
            {
                return "collision_shapes.txt";
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

                CollisionData collisionData = new CollisionData();
                collisionData.ParseDataRow(item, title);

                m_dataList.Add(collisionData);
            }
        }

        public override void SaveData(int sceneID) { }
    }
}