using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public class TriggerEditor_AnimatorControl : TriggerEditor_Base
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_ANIMATOR;
            }
        }

        [Tooltip("场景GameObject")]
        public GameObject prefab;

        [Tooltip("animator 的条件名称")]
        public string aniName;

        [HideInInspector]
        public string strPath;

        private void Start()
        {

        }
        
        public override string DoTriggerParam()
        {
            List<string> t_list = new List<string>();

            Transform parent = prefab.transform;
            while(parent != null)
            {
                t_list.Add(parent.name);
                parent = parent.transform.parent;
            }

            t_list.Reverse();

            string strObjectPath = t_list[0];
            for(int i = 1; i < t_list.Count; ++i)
            {
                strObjectPath += "/";
                strObjectPath += t_list[i];
            }

            strObjectPath += ";";
            strObjectPath += aniName;

            return strObjectPath;
        }

        public override void NeedUpdate()
        {
            if(action != "-1")
            {
                string[] arrParam = action.Split(';');
                if(arrParam != null && arrParam.Length == 2)
                {
                    this.prefab = GameObject.Find(arrParam[0]);
                    strPath = arrParam[0];
                    aniName = arrParam[1];
                }

                action = "-1";
            }
        }

    }
}