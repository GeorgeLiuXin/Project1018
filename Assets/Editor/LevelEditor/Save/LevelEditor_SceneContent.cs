using UnityEngine;
using System.IO;
using System;


namespace Galaxy
{
    //NPC && Monster
    public class  LevelEditor_Spawner : LevelEditor_FileCreator
    {
        public override string FileName
        {
            get
            {
                return "scenecontentspawner";
            }
        }

        public override void SaveFile(int sceneID, string strPath, GameObject groupObj)
        {
            string strFilePath = strPath + "scenecontentspawner" + Convert.ToString(sceneID) + ".txt";

            if (groupObj == null)
                return;

            using (StreamWriter sw = File.CreateText(strFilePath))
            {
                sw.WriteLine("SceneID" + "\t" +
                            "ScenarioID" + "\t" +
                            "ID" + "\t" +
                            "Range" + "\t" +
                            "SpawnerType" + "\t" +
                            "SpawnerTargetData" + "\t" +
                            "Count" + "\t" +
                            "Branch" + "\t" +
                            "CheckPlayerDis" + "\t" +
                            "DelayTime" + "\t" +
                            "ParentSpawner" + "\t" +
                            "GroupID" + "\t" +
                            "FindPath" + "\t" +
                            "Hide" + "\t" +
                            "Pos" + "\t" +
                            "DirPos");

                sw.WriteLine("int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "f32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "f32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "char" + "\t" +
                             "char");

               

                for (int i = 0; i < groupObj.transform.childCount; ++i)
                {
                    Transform subChild = groupObj.transform.GetChild(i);
                    SceneContentSpawner_RunTime runTimeData = subChild.GetComponent<SceneContentSpawner_RunTime>();
                    if (runTimeData == null)
                    {
                        Debug.LogError("LevelEditor_SceneContent create failed! index : " + i.ToString());

                        File.Delete(strFilePath);
                        sw.Close();
                        sw.Dispose();
                        return;
                    }

                    string strPos = runTimeData.Pos.x + ";" + runTimeData.Pos.z + ";" + runTimeData.Pos.y;
                    string strDir = runTimeData.Dir.x + ";" + runTimeData.Dir.z + ";" + runTimeData.Dir.y;

                    sw.WriteLine(runTimeData.SceneID.ToString() + "\t" +
                                runTimeData.ScenarioID.ToString() + "\t" +
                                runTimeData.ID.ToString() + "\t" +
                                runTimeData.Range.ToString() + "\t" +
                                runTimeData.SpawnerType.ToString() + "\t" +
                                runTimeData.SpawnerTargetData.ToString() + "\t" +
                                runTimeData.Count.ToString() + "\t" +
                                runTimeData.Branch.ToString() + "\t" +
                                runTimeData.CheckPlayerDistance.ToString() + "\t" +
                                runTimeData.DelayTime.ToString() + "\t" +
                                runTimeData.ParentSpawnerID.ToString() + "\t" +
                                runTimeData.GroupID.ToString() + "\t" +
                                runTimeData.FindPath.ToString() + "\t" +
                                runTimeData.Hide.ToString() + "\t" +
                                strPos + "\t" +
                                strDir);
                }
                
            }   
        }
    }

    public class LevelEditor_Npc : LevelEditor_FileCreator
    {
        public override string FileName
        {
            get
            {
                return "scenecontentnpc";
            }
        }

        public override void SaveFile(int sceneID, string strPath, GameObject npcObj)
        {

        }

        public void SaveFile(int sceneID, string strPath, GameObject npcObj, GameObject monsterObj)
        {
            string strFilePath = strPath + "scenecontentnpc" + Convert.ToString(sceneID) + ".txt";

            if (npcObj == null)
                return;

            using (StreamWriter sw = File.CreateText(strFilePath))
            {
                sw.WriteLine("SceneID" + "\t" +
                            "ScenarioID" + "\t" +
                            "ID" + "\t" +
                            "DataID" + "\t" +
                            "NpcType" + "\t" +
                            "Pos" + "\t" +
                            "DirPos");

                sw.WriteLine("int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "char" + "\t" +
                             "char");

               

                for (int i = 0; i < npcObj.transform.childCount; ++i)
                {
                    Transform subChild = npcObj.transform.GetChild(i);
                    SceneContentNpc_RunTime runTimeData = subChild.GetComponent<SceneContentNpc_RunTime>();
                    if (runTimeData == null)
                    {
                        Debug.LogError("LevelEditor_Npc create failed! index : " + i.ToString());

                        File.Delete(strFilePath);
                        sw.Close();
                        sw.Dispose();
                        return;
                    }

                    string strPos = runTimeData.Pos.x + ";" + runTimeData.Pos.z + ";" + runTimeData.Pos.y;
                    string strDir = runTimeData.Dir.x + ";" + runTimeData.Dir.z + ";" + runTimeData.Dir.y;

                    sw.WriteLine(sceneID.ToString() + "\t" +
                                runTimeData.ScenarioID.ToString() + "\t" +
                                runTimeData.ID.ToString() + "\t" +
                                runTimeData.DataID.ToString() + "\t" +
                                runTimeData.NpcType.ToString() + "\t" +
                                strPos + "\t" +
                                strDir);
                }

            }
        }
    }

}