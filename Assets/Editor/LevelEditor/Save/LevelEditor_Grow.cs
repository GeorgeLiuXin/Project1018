using UnityEngine;
using System.IO;

namespace Galaxy
{
    public class LevelEditor_Grow : LevelEditor_FileCreator
    {
        public override string FileName
        {
            get
            {
                return "scene_grow_client";
            }
        }

        public override void SaveFile(int sceneID, string strPath, GameObject groupObj)
        {
            string strFilePath = strPath + FileName + ".txt";

            if (groupObj == null)
                return;

            using (StreamWriter sw = File.CreateText(strFilePath))
            {
                sw.WriteLine("SceneID" + "\t" +
                            "DataIndex" + "\t" +
                            "GrowID" + "\t" +
                            "Radius" + "\t" +
                            "ClientBorn" + "\t" +
                            "PosX" + "\t" +
                            "PosY" + "\t" +
                            "PosZ" + "\t" +
                            "DirX" + "\t" +
                            "DirY" + "\t" +
                            "DirZ");

                sw.WriteLine("int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "f32" + "\t" +
                             "int32" + "\t" +
                             "f32" + "\t" +
                             "f32" + "\t" +
                             "f32" + "\t" +
                             "f32" + "\t" +
                             "f32" + "\t" +
                             "f32");

                for (int i = 0; i < groupObj.transform.childCount; ++i)
                {
                    Transform runTimeObj = groupObj.transform.GetChild(i);
                    SceneContentNpcGrow_RunTime runTimeCom = runTimeObj.GetComponent<SceneContentNpcGrow_RunTime>();

                    int state = (runTimeCom.bClient == true ? 1 : 0);

                    sw.WriteLine(sceneID.ToString() + "\t" +
                           i.ToString() + "\t" +
                           runTimeCom.ID.ToString() + "\t" +
                           runTimeCom.Radius.ToString() + "\t" +
                           state.ToString() + "\t" +
                           runTimeCom.Pos.x.ToString() + "\t" +
                           runTimeCom.Pos.y.ToString() + "\t" +
                           runTimeCom.Pos.z.ToString() + "\t" +
                           runTimeCom.Dir.x.ToString() + "\t" +
                           runTimeCom.Dir.y.ToString() + "\t" +
                           runTimeCom.Dir.z.ToString());
                }
            }

        }
    }

}