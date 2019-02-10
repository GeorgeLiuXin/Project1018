using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public static class StaticParam
    {
        public static readonly float Threat_Last_Time = 1f;

		//XML统一路径
		public static readonly string PATH_XML_FILES = "XML";

		//数据读取
		public static readonly string Config_Define_Name = "client_config_define";
        public static readonly string Config_Define_Path = "Config/client_config_define";

        //tag
        public static readonly string Main_Camera_Tag = "MainCamera";
        public static readonly string NPC_Tag = "NPC";

        //layer
        public static readonly string Layer_Terrain = "Terrain";
        public static readonly string Layer_Player = "Player";
        public static readonly string Layer_Enemy = "Enemy";
        public static readonly string Layer_Boss = "Boss";
    }
}
