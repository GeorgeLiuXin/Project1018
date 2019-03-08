using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public class TriggerEditor_FindPath : TriggerEditor_Base
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_NPC_FINDPATH;
            }
        }

        [Tooltip("动画Trigger名称")]
        public GameObject targetPosObject;

        [HideInInspector]
        public Vector3 vWorldPos;

        private void Start()
        {

        }

        public override string DoTriggerParam()
        {
#if UNITY_EDITOR
            if (targetPosObject != null)
                GameObject.DestroyImmediate(targetPosObject);
#endif

            string str = string.Format("{0},{1},{2}",vWorldPos.x,vWorldPos.y,vWorldPos.z);
            return str;
        }

        public override void NeedUpdate()
        {
            if (action != "-1")
            {
                string[] arrPos = action.Split(',');
                if(arrPos.Length == 3)
                {
                    vWorldPos.x = float.Parse(arrPos[0]);
                    vWorldPos.y = float.Parse(arrPos[0]);
                    vWorldPos.z = float.Parse(arrPos[0]);

                    GameObject root = GameObject.Find("DestPosObjectList");
                    if(root == null)
                    {
                        root = new GameObject();
                        root.name = "DestPosObjectList";
                        root.transform.position = Vector3.zero;
                    }

                    GameObject newPathObj = new GameObject();
                    newPathObj.transform.SetParent(root.transform);
                    newPathObj.name = "DestPosObjcet";
                    targetPosObject = newPathObj;
                    targetPosObject.tag = "EditorOnly";
                    targetPosObject.transform.position = vWorldPos;
                }
                action = "-1";
            }

            if(targetPosObject != null)
            {
                vWorldPos = targetPosObject.transform.position;
            }
        }
    }
}