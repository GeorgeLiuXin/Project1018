using XWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public class ClientConfig
    {
        #region readonly member
        public static readonly string CLIENT_CONFIG = "PreInit.cfg";

        public static readonly string SACTION_GLOBAL = "Global";
        public static readonly string SACTION_TABLE = "Table";
        public static readonly string SACTION_EDITOR = "Editor";
        public static readonly string SACTION_COMBAT = "Combat";

        public static readonly string GAME_VERSION = "GameVersion";
        public static readonly string SERVER_IP = "ServerAddress";
        public static readonly string SERVER_PORT = "ServerPort";
        public static readonly string CDN_URL = "CdnAddress";

        public static readonly string MAX_WEB_STREAM_READ_SIZE = "MaxWebStreamReadSize";
        public static readonly string RES_VERSION_MD5 = "ResVersionMd5";
        public static readonly string DEBUG = "Debug ";

        public static readonly string RES_FOLDER = "ResFolder";

        public static readonly string LOG = "LogChannel";

        public static readonly string USE_CUSTOM_IP = "UseCustomIp";
        public static readonly string CUSTOM_IP = "CustomIp";

        public static readonly string COMBAT_BULLET_OFFEST_RANGE = "CombatBulletOffestRange";
        public static readonly string COMBAT_BULLET_FLIGHT_SPEED = "CombatBulletFlightSpeed";
        public static readonly string COMBAT_BULLET_FLIGHT_RANGE = "CombatBulletFlightRange";
        public static readonly string COMBAT_ENVEFFECT_RANGE     = "CombatEnvEffectRange";
        public static readonly string COMBAT_DEFAULT_RANGE       = "CombatDefaultRange";
        public static readonly string ANIM_PAUSE_TIME            = "AnimPauseTime";
        public static readonly string COMBAT_SLIP_DIS            = "CombatSlipDis";
        public static readonly string AIMASSIST_RESET_TIME       = "AimAssistResetTime";
        public static readonly string CANCEL_CHOOSE_MODE_TIME    = "CancelChooseModeTime";
        public static readonly string TAB_TARGET_DISTANCE        = "TabTargetDistance";

        public static readonly string MAX_HERO_EFFECT_COUNT = "MaxHeroEffectCount";
        #endregion readonly member
		
        public static void Init()
        {
            InitConfigFiles();
        }

        private static void InitConfigFiles()
        {
            string data = ResourcesProxy.LoadTextString(CLIENT_CONFIG);
            if (data != null)
            {
                GameLogger.DebugLog(LOG_CHANNEL.LOGIC, CLIENT_CONFIG + "加载成功！");
                PreInitCFG.Instance.Parse(data);
            }
            else
            {
                GameLogger.Error(LOG_CHANNEL.LOGIC, CLIENT_CONFIG + "加载失败！");
            }
			
            InitDebugConfigs();
        }

        private static void InitDebugConfigs()
        {
            Dictionary<LOG_CHANNEL, bool> dictLogFlag = new Dictionary<LOG_CHANNEL, bool>();

            string LogChannelStr = string.Empty;
            PreInitCFG.Instance.GetString(SACTION_EDITOR, LOG, out LogChannelStr);
            if (!string.IsNullOrEmpty(LogChannelStr))
            {
                string[] enumContents = LogChannelStr.Split(',');
                foreach (string content in enumContents)
                {
                    try
                    {
                        LOG_CHANNEL channel = (LOG_CHANNEL)Enum.Parse(typeof(LOG_CHANNEL), content);
                        dictLogFlag.Add(channel, true);
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError("LOG解析错误" + e.ToString());
                    }
                }
            }
            CoreLogger.SetDictLogFlag(dictLogFlag);
            GameLogger.DebugLog(LOG_CHANNEL.LOGIC, string.Format("当前LOG输出：{0}", LogChannelStr));
        }
    }
}
