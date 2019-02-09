using System;

namespace XWorld
{
    public partial class CltEvent
    {
        public class Log
        {
            public static string ON_REIV_MSG = "ON_REIV_MSG";
        }

        public class Skill
        {
            public static readonly string Continuously_Skill_Start = "Continuously_Skill_Start";
            public static readonly string Continuously_Skill_Over = "Continuously_Skill_Over";
        }
		
        public class UI
        {
            public static readonly string UI_HIT_EFFECT = "UI_HIT_EFFECT";
            public static readonly string UI_BLOOD = "UI_BLOOD";

        }

		public class Actor
		{
			public static readonly string INIT_AVATAR_RES = "INIT_AVATAR_RES";
			public static readonly string UPDATE_AVATAR_RES = "UPDATE_AVATAR_RES";
			public static readonly string REMOVE_AVATAR_RES = "REMOVE_AVATAR_RES";
		}

	}
}
