using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public static class StaticParam
    {
        //数据读取
        public static readonly string CONFIG_DEFINE_NAME = "client_config_define";
        public static readonly string CONFIG_DEFINE_PATH = "Config/Static/client_config_define";

        //tag
        public static readonly string MAIN_CAMERA_TAG = "MainCamera";
        public static readonly string NPC_TAG = "NPC";

        //layer
        public static readonly string LAYER_TERRAIN = "Terrain";
        public static readonly string LAYER_PLAYER = "PLAYER";
        public static readonly string LAYER_NET_NPC = "NPC";
        public static readonly string LAYER_NET_BOSS = "BOSS";
    }
}
