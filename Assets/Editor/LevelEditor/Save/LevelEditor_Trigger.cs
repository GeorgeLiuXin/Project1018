using UnityEngine;
using System.IO;
using System;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Galaxy
{
    public class LevelEditor_Trigger : LevelEditor_FileCreator
    {
        public override string FileName
        {
            get
            {
                return "scene_trigger_content";
            }
        }

        public string CltFullPath
        {
            get
            {
                return Application.dataPath + "/AssetDatas/Config/LevelTrigger/";
            }
        }

        public override void SaveFile(int sceneID, string strPath, GameObject groupObj)
        {
            string strFilePath = strPath + StaticParam.LEVEL_FILE_NAME_BASE + sceneID.ToString() + ".txt";

            if (groupObj == null)
                return;

            if(File.Exists(strFilePath))
            {
                File.Delete(strFilePath);
            }

            using (StreamWriter sw = File.CreateText(strFilePath))
            {
                sw.WriteLine("EventID" + "\t" +
                            "GroupID" + "\t" +
                            "DataID" + "\t" +
                            "ActionID" + "\t" +
                            "TriggerCount" + "\t" +
                            "EffectID" + "\t" +
                            "TriggerType" + "\t" +
                            "TriggerTag" + "\t" +
                            "TriggerAction" + "\t" +
                            "NextActionList" + "\t" +
                            "PosX" + "\t" +
                            "PosY" + "\t" +
                            "PosZ" + "\t" +
                            "DirX" + "\t" +
                            "DirY" + "\t" +
                            "DirZ" + "\t" +
                            "ScaleX" + "\t" +
                            "ScaleY" + "\t" +
                            "ScaleZ" + "\t" +
                            "SpwanerId" + "\t" +
                            "SpwanerType");

                sw.WriteLine("int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "int32" + "\t" +
                             "char" + "\t" +
                             "char" + "\t" +
                             "f32" + "\t" +
                             "f32" + "\t" +
                             "f32" + "\t" +
                             "f32" + "\t" +
                             "f32" + "\t" +
                             "f32" + "\t" +
                             "f32" + "\t" +
                             "f32" + "\t" +
                             "f32" + "\t" +
                             "int32" + "\t" +
                             "int32");

                int globalIndex = 0;
                for (int i = 0; i < groupObj.transform.childCount; ++i)
                {
                    Transform groupChild = groupObj.transform.GetChild(i);
                    if (groupChild == null)
                        continue;

                    for(int j = 0; j < groupChild.childCount; ++j)
                    {
                        Transform runTime = groupChild.GetChild(j);

                        Vector3 t_vPos = runTime.transform.position;
                        Vector3 t_Dir = runTime.transform.localEulerAngles;
                        Vector3 t_scale = runTime.GetComponent<BoxCollider>().size;

                        int childIndex = 0;
                        SceneTrigger_RunTime[] arrRunTimeData = runTime.GetComponentsInChildren<SceneTrigger_RunTime>();
                        foreach(SceneTrigger_RunTime item in arrRunTimeData)
                        {
                            if (item == null)
                            {
                                Debug.LogError("LevelEditor_Trigger create failed! index : " + i.ToString());

                                File.Delete(strFilePath);
                                sw.Close();
                                sw.Dispose();
                                return;
                            }

                            sw.WriteLine(globalIndex.ToString() + "\t" +
                                        item.TriggerGroup.ToString() + "\t" +
                                         item.ID + "\t" +
                                         item.actionId + "\t" + 
                                        item.TriggerCount.ToString() + "\t" +
                                        item.EffectID.ToString() + "\t" +
                                        GetTriggerType(item) + "\t" +
                                        GetTriggerTagType(item).ToString() + "\t" +
                                        GetTriggerAction(item.transform) + "\t" +
                                        item.ActiveList + "\t" +
                                        t_vPos.x.ToString() + "\t" +
                                        t_vPos.y.ToString() + "\t" +
                                        t_vPos.z.ToString() + "\t" +
                                        t_Dir.x.ToString() + "\t" +
                                        t_Dir.y.ToString() + "\t" +
                                        t_Dir.z.ToString() + "\t" +
                                        t_scale.x.ToString() + "\t" +
                                        t_scale.y.ToString() + "\t" +
                                        t_scale.z.ToString() + "\t" +
                                        item.SpwanerId.ToString() + "\t" +
                                        (int)item.SpwanerType);

                            childIndex++;
                            globalIndex++;
                        }
                    }
                        
                } 
            }

            AssetDatabase.Refresh();
        }


        private int GetTriggerType(SceneTrigger_RunTime runTime)
        {
            return (int)runTime.triggerType;
        }

        private int GetTriggerTagType(SceneTrigger_RunTime runTime)
        {
            return (int)runTime.tagType;
        }

        private string GetTriggerAction(Transform triggerTrans)
        {
            TriggerEditor_Base triggerAction = triggerTrans.GetComponent<TriggerEditor_Base>();
            if (triggerAction != null)
                return triggerAction.DoTriggerParam();

            return "";
        }
    }
       

}