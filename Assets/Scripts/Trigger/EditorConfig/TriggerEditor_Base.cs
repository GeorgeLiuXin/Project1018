using UnityEngine;

namespace Galaxy
{
    public class TriggerEditor_Base : MonoBehaviour
    {
        public virtual GalaxyTriggerDefine.TRIGGER_TYPE TriggerType
        {
            get
            {
                return GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_TYPE_NULL;
            }
        }

        [Tooltip("服務器Spwaner Id，-1代表纯客户端，大于零会优先完成spwaner")]
        public int spawnerId = -1;

        [Tooltip("服务器spawner类型")]
        public GSpawnerState spwanerType = GSpawnerState.SpawnerInfo_Deactive;

        protected string action = "-1";

        private void Start()
        {

        }

        public virtual string DoTriggerParam()
        {
            return "-1";
        }

        public virtual void ParseTriggerParam(string action)
        {
            this.action = action;
        }

        public virtual void ParseSpwanerData(int spawnerId, int spwanerType)
        {
            this.spawnerId = spawnerId;
            this.spwanerType = (GSpawnerState)spwanerType;
        }

        public virtual void NeedUpdate()
        {

        }
    }
}