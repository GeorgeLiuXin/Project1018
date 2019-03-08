using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    public class TriggerData_Editor : ILevelDataRow
    {
        public int Id
        {
            get
            {
                return 0;
            }
        }

        public int EventID
        {
            get;
            set;
        }

        public int DataID
        {
            get;
            set;
        }

        public int GroupID
        {
            get;
            set;
        }

        public int ActionId
        {
            get;
            set;
        }

        public int TriggerType
        {
            get;
            set;
        }

        public int TriggerTag
        {
            get;
            set;
        }

        public int TriggerCount
        {
            get;
            set;
        }

        public string TriggerAction
        {
            get;
            set;
        }

        public string NextActionList
        {
            get;
            set;
        }

        public int EffectID
        {
            get;
            set;
        }

        public int SpwanerId
        {
            get;
            set;
        }

        public int SpwanerType
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

        public Vector3 Scale
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
            Vector3 vScale = Vector3.one;
            for (int i = 0; i < arrTitle.Length; ++i)
            {
                if (arrTitle[i].Equals("EventID"))
                {
                    this.EventID = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("DataID"))
                {
                    this.DataID = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("GroupID"))
                {
                    this.GroupID = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("ActionID"))
                {
                    this.ActionId = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("TriggerCount"))
                {
                    this.TriggerCount = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("EffectID"))
                {
                    this.EffectID = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("TriggerType"))
                {
                    this.TriggerType = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("TriggerTag"))
                {
                    this.TriggerTag = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("TriggerAction"))
                {
                    this.TriggerAction = arrContent[i];
                }

                if (arrTitle[i].Equals("NextActionList"))
                {
                    this.NextActionList = arrContent[i];
                }

                if (arrTitle[i].Equals("SpwanerId"))
                {
                    this.SpwanerId = int.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("SpwanerType"))
                {
                    this.SpwanerType = int.Parse(arrContent[i]);
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

                if (arrTitle[i].Equals("ScaleX"))
                {
                    vScale.x = float.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("ScaleY"))
                {
                    vScale.y = float.Parse(arrContent[i]);
                }

                if (arrTitle[i].Equals("ScaleZ"))
                {
                    vScale.z = float.Parse(arrContent[i]);
                }
            }

            this.Pos = vPos;
            this.Dir = vDir;
            this.Scale = vScale;
        }
    }

    public class LevelEditor_TriggerDefine : LevelTableBase
    {
        public override string TableName
        {
            get
            {
                return "scene_trigger_content.txt";
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

                TriggerData_Editor triggerData = new TriggerData_Editor();
                triggerData.ParseDataRow(item, title);

                m_dataList.Add(triggerData);
            }
        }

        public override void SaveData(int sceneID) { }
    }
}
