using UnityEngine;
using System.IO;
using System;
namespace Galaxy
{
    public class LevelEditor_Path : LevelEditor_FileCreator
    {
        public override string FileName
        {
            get
            {
                return "scenecontenttagpoint";
            }
        }

        public override void SaveFile(int sceneID, string strPath, GameObject groupObj)
        {
            string strFilePath = strPath + FileName + Convert.ToString(sceneID) + ".txt";

            if (groupObj == null)
                return;

            using (StreamWriter sw = File.CreateText(strFilePath))
            {
                sw.WriteLine("ContentID" + "\t" +
                            "GroupID" + "\t" +
                            "ID" + "\t" +
                            "Des" + "\t" +
                            "PosX" + "\t" +
                            "PosY" + "\t" +
                            "PosZ" + "\t" +
                            "DirX" + "\t" +
                            "DirY" + "\t" +
                            "DirZ");

                sw.WriteLine("int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "char" + "\t" +
                             "f64" + "\t" +
                             "f64" + "\t" +
                             "f64" + "\t" +
                             "f64" + "\t" +
                             "f64" + "\t" +
                             "f64");

                for (int i = 0; i < groupObj.transform.childCount; ++i)
                {
                    Transform subChild = groupObj.transform.GetChild(i);
                    SceneContentPath_RunTime runTimeData = subChild.GetComponent<SceneContentPath_RunTime>();
                    if (runTimeData == null)
                    {
                        Debug.LogError("LevelEditor_Path create failed! index : " + i.ToString());

                        File.Delete(strFilePath);
                        sw.Close();
                        sw.Dispose();
                        return;
                    }

                    for (int j = 0; j < runTimeData.arrPath.Length; ++j)
                    {
                        sw.WriteLine(sceneID.ToString() + "\t" +
                                i.ToString() + "\t" +
                                j.ToString() + "\t" +
                                "" + "\t" +
                                runTimeData.arrPath[j].transform.position.x.ToString() + "\t" +
                                runTimeData.arrPath[j].transform.position.y.ToString() + "\t" +
                                runTimeData.arrPath[j].transform.position.z.ToString() + "\t" +
                                runTimeData.arrPath[j].transform.eulerAngles.x.ToString() + "\t" +
                                runTimeData.arrPath[j].transform.eulerAngles.y.ToString() + "\t" +
                                runTimeData.arrPath[j].transform.eulerAngles.z.ToString());

                    }
                }
            }
               
        }
    }

}