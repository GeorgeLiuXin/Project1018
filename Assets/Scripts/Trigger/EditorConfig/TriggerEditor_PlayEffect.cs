using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public class TriggerEditor_PlayEffect : TriggerEditor_Base
    {
        public override GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_PLAYEFFECT;
            }
        }

        [Tooltip("场景GameObject")]
        public GameObject prefab;

        [Tooltip("是否循环")]
        public bool bLoop;

        [HideInInspector]
        public string strPath;

        private void Start()
        {

        }

        public override string DoTriggerParam()
        {
            List<string> t_list = new List<string>();

            Transform parent = prefab.transform;
            while (parent != null)
            {
                t_list.Add(parent.name);
                parent = parent.transform.parent;
            }

            t_list.Reverse();

            string strObjectPath = t_list[0];
            for (int i = 1; i < t_list.Count; ++i)
            {
                strObjectPath += "/";
                strObjectPath += t_list[i];
            }

            strObjectPath += ";";
            strObjectPath += bLoop.ToString();

            return strObjectPath;
        }

        public override void NeedUpdate()
        {
            if (action != "-1")
            {
                string[] arrParam = action.Split(';');
                if (arrParam != null && arrParam.Length == 2)
                {
                    this.prefab = GameObject.Find(arrParam[0]);
                    strPath = arrParam[0];
                    bLoop  = arrParam[1].Equals("true") ? true : false;
                }

                action = "-1";
            }
        }
    }
}
