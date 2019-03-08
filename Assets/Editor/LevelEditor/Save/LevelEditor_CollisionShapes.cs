using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

namespace Galaxy
{
    public class LevelEditor_CollisionShapes : LevelEditor_FileCreator
    {
        public override string FileName
        {
            get
            {
                return "collision_shapes";
            }
        }

        public string SrvFullPath
        {
            get
            {
                DirectoryInfo t_tempInfo = new DirectoryInfo(Application.dataPath);
                t_tempInfo = t_tempInfo.Parent;
                t_tempInfo = t_tempInfo.Parent;
                string strPath = t_tempInfo.FullName + "/Server/bin32/Data/DataTable/collision_shapes.txt";
                strPath = strPath.Replace('/', '\\');

                return strPath;
            }
        }

        public string CltFullPath
        {
            get
            {
                return Application.dataPath + "/AssetDatas/Config/Dynamic/ClientData/collision_shapes.txt";
            }
        }

        public override void SaveFile(int sceneID, string strPath, GameObject groupObj)
        {

        }

        public void CreateServerFile(string strPath,ref Dictionary<int, List<CollisionData>> dictCollisionData,GameObject root)
        {
            for(int i = 0; i < root.transform.childCount; ++i)
            {
                Transform group = root.transform.GetChild(i);
                int groupIdx = int.Parse(group.name);

                int index = 0;
                for(int j = 0; j < group.transform.childCount; ++j)
                {
                    Transform child = group.GetChild(j);
                    SceneCollision_shapes com =  child.gameObject.GetComponent<SceneCollision_shapes>();
                    if(com != null)
                    {
                        if(dictCollisionData.ContainsKey(groupIdx))
                        {
                            dictCollisionData[groupIdx][index].GroupID = com.GroupID;
                            dictCollisionData[groupIdx][index].SphereID = com.ShapeID;
                            dictCollisionData[groupIdx][index].Pos = com.vPos;
                            dictCollisionData[groupIdx][index].Radius = com.Radius;
                            dictCollisionData[groupIdx][index].damageReduction = com.damageReduction;
                            index++;
                        }
                    }
                }
            }

            using (StreamWriter sw = File.CreateText(strPath))
            {
                sw.WriteLine("GroupID" + "\t" +
                            "ShapeID" + "\t" +
                            "SrvRadius" + "\t" +
                            "SrvX" + "\t" +
                            "SrvY" + "\t" +
                            "SrvZ" + "\t" +
                            "DamageReduction");

                sw.WriteLine("int32" + "\t" +
                             "int32" + "\t" +
                             "f32" + "\t" +
                             "f32" + "\t" +
                             "f32" + "\t" +
                             "f32" + "\t" +
                             "f32");

                foreach (var item in dictCollisionData)
                {
                    foreach (CollisionData data in item.Value)
                    {
                        float r = data.Radius / 2.0f;
                        sw.WriteLine(item.Key.ToString() + "\t" +
                                 data.SphereID.ToString() + "\t" +
                                 r.ToString() + "\t" +
                                data.Pos.x.ToString() + "\t" +
                                data.Pos.z.ToString() + "\t" +
                                data.Pos.y.ToString() + "\t" +
                                data.damageReduction.ToString());
                    }

                }

            }
        }
    }
}

