
namespace Galaxy
{
    public static class GalaxyTriggerDefine
    {
        /// <summary>
        /// 配置类定义  不准插入 末尾添加
        /// </summary>
        public enum TRIGGER_TYPE
        {
            TRIGGER_TYPE_NULL = -1,

            TRIGGER_NPC_TALK,               //Npc对话
            TRIGGER_TIMELINE,               //剧情动画
            TRIGGER_ANIMATOR,               //触发动画
            TRIGGER_CINE_MAHCHINE,          //虚拟摄像机
            TRIGGER_LUA_ACTIONS,            //客户端lua自定义
            TRIGGER_VISABLE_GROUP,          //显示隐藏Trigger组
            TRIGGER_GUIDE,                  //新手指引
            TRIGGER_OS,                     //OS系统
            TRIGGER_NPC_ANIMATION,          //NPC动画
            TRIGGER_NPC_FINDPATH,           //NPC寻路
            TRIGGER_LOOKATSCENE,            //玩家摄像机看场景的动画
            TRIGGER_PLAYEFFECT,             //播放特效
            TRIGGER_SEND_PACKET,            //发送通用消息给服务器
            TRIGGER_STOP_CAR,               //停车逻辑
            TRIGGER_CAR_SPEED_CHANGE,       //修改车速
            TRIGGER_LUA_ACTION_EX,          //客户端lua自定义

            TRIGGER_TYPE_COUNT,
        }

        public enum TRIGGER_TAG_TYPE
        {
            TRIGGER_LOCAL_PLAYER = 1,
            TRIGGER_OTHER_PLAYER,
            TRIGGER_ALL_PLAYER,
            TRIGGER_NPC,
            TRIGGER_ALL_ACTOR,
        }

        public enum TRIGGER_GUIDE
        {
            TRIGGER_GUIDE_BEGIN = 0,
            TRIGGER_GUIDE_END,
        }

        public static string[] TagList = new string[] { "LocalPlayer", "Player", "Npc"};

        public static int MakeTriggerDataID(int groupId, int dataId)
        {
            int id = (groupId << 16) | (dataId & 8);
            return id;
        }

        public static void GetTriggerID(int dataId,out int triggerId, out int selfId)
        {
            triggerId = (dataId >> 16);
            selfId = (dataId | 4);
        }
    }

}