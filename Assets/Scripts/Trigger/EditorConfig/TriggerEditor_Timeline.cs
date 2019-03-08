using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Galaxy
{
    public class TriggerEditor_Timeline : TriggerEditor_Base
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_TIMELINE;
            }
        }

        [Tooltip("TimelineId")]
        public int timelineId;

        [HideInInspector]
        public string strPath;
        private GameObject lastPrefab;

        private void Start()
        {
            
        }

        public override void NeedUpdate()
        {
            /*           if(action != "-1")
                       {
                           string[] strContent = action.Split(';');
                           string[] arrParam1 = strContent[0].Split(':');
                           if (arrParam1[1] == "False")
                           {
                               sceneTimeline = false;
                           }
                           else
                           {
                               sceneTimeline = true;
                           }

                           if (sceneTimeline)
                           {
                               prefab = GameObject.Find(strContent[1]);
                           }
                           else
                           {
           #if UNITY_EDITOR
                               strContent[1] = string.Format("Assets/AssetDatas/{0}", strContent[1]);
                               prefab = UnityEditor.AssetDatabase.LoadMainAssetAtPath(strContent[1]) as GameObject;
           #endif
                           }

                           action = "-1";
                       }

                       if (prefab != null)
                       {
                           if (!sceneTimeline && prefab != null)
                           {
                               if (lastPrefab != prefab)
                               {
           #if UNITY_EDITOR
                                   if (PrefabUtility.GetPrefabObject(prefab))
                                   {
                                       strPath = AssetDatabase.GetAssetPath(prefab);

                                       string[] arrPath = strPath.Split('/');
                                       strPath = arrPath[2];
                                       for (int i = 3; i < arrPath.Length; ++i)
                                       {
                                           strPath += "/";
                                           strPath += arrPath[i];
                                       }
                                   }
           #endif
                               }
                           }

                           if (lastPrefab == null)
                           {
                               lastPrefab = prefab;
                           }
                       }
                       */

            if (action != "-1")
            {
                timelineId = int.Parse(action);
            }
        }

        public override string DoTriggerParam()
        {
            NeedUpdate();

            /*string strFormat = "IsScenePrefab:" + sceneTimeline.ToString() + ";";
            if(sceneTimeline)
            {
                List<string> t_list = new List<string>();

                Transform parent = prefab.transform;
                while (parent != null)
                {
                    t_list.Add(parent.name);
                    parent = parent.transform.parent;
                }

                t_list.Reverse();
                strFormat += t_list[0];

                for (int i = 1; i < t_list.Count; ++i)
                {
                    strFormat += "/";
                    strFormat += t_list[i];
                }
            }
            else
            {
                strFormat += strPath;
            }

            return strFormat;*/

            return timelineId.ToString();
        }

        public override void ParseTriggerParam(string action)
        {
            this.action = action;
        }

        
    }
}