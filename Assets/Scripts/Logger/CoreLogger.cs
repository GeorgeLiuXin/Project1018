using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public class CoreLogger
    {
        private static bool m_bLogInit = false;
        private static bool m_bLogShow = true;
        private static Dictionary<LOG_CHANNEL, bool> m_dictLogFlag = new Dictionary<LOG_CHANNEL, bool>();
        private static Dictionary<LOG_CHANNEL, List<string>> m_dictLogChannels = new Dictionary<LOG_CHANNEL, List<string>>();

        public static void SetDictLogFlag(Dictionary<LOG_CHANNEL, bool> dictLogFlag)
        {
            m_dictLogFlag = dictLogFlag;
            m_bLogInit = true;
        }

        public static void LogStart()
        {
            //m_dictLogChannels.Clear();
            //m_dictLogFlag.Clear();

            //string data = ResourcesProxy.LoadTextString("PreInit.cfg");
            //if (data != null)
            //{
            //    PreInitCFG.Instance.Parse(data);
            //}
            //string LogChannelStr = string.Empty;
            //PreInitCFG.Instance.GetString("Editor", "LogChannel", out LogChannelStr);
            //if (!string.IsNullOrEmpty(LogChannelStr))
            //{
            //    string[] enumContents = LogChannelStr.Split(',');
            //    foreach (string content in enumContents)
            //    {
            //        try
            //        {
            //            LOG_CHANNEL channel = (LOG_CHANNEL)Enum.Parse(typeof(LOG_CHANNEL), content);
            //            m_dictLogFlag.Add(channel, true);
            //        }
            //        catch (Exception e)
            //        {
            //            UnityEngine.Debug.LogError("LOG_CHANNEL解析错误" + e.ToString());
            //        }
            //    }
            //}
        }

        public static string Debug(LOG_CHANNEL channel, string strLog)
        {
            string strDebugLog = GetLogText(channel, strLog);
            if (strDebugLog == null)
                return strDebugLog;

            UnityEngine.Debug.Log(strDebugLog);
            return strDebugLog;
        }

        public static string Warning(LOG_CHANNEL channel, string strLog)
        {
            string strWarning = GetLogText(channel, strLog);
            if (strWarning == null)
                return strWarning;

            UnityEngine.Debug.LogWarning(strWarning);
            return strWarning;
        }

        public static string Error(LOG_CHANNEL channel, string strLog)
        {
            string strErrorLog = GetLogText(channel, strLog);
            if (strErrorLog == null)
                return strErrorLog;

            UnityEngine.Debug.LogError(strErrorLog);
            return strErrorLog;
        }

        private static string GetLogText(LOG_CHANNEL channel, string strLog)
        {
            if (!IsLogActive(channel))
                return null;

            return string.Format("<color=yellow>{0}{1}</color>: {2}", channel, Time.frameCount, strLog);
        }

        public static void EnableLogChannel(LOG_CHANNEL channel, bool bEnable)
        {
            if (!m_dictLogFlag.ContainsKey(channel))
                return;

            m_dictLogFlag[channel] = bEnable;
        }

        public static bool IsLogActive(LOG_CHANNEL channel)
        {
            if (!m_bLogShow)
                return false;

            if (!m_bLogInit)
                return true;

            if (!m_dictLogFlag.ContainsKey(channel))
                return false;

            return m_dictLogFlag[channel];
        }

    }
}