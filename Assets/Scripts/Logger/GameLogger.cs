using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public class GameLogger
    {

        public delegate void LogDelegate(LOG_CHANNEL channel, LogType logType, string content);
        public static event LogDelegate OnLogReceived;
        

        static GameLogger()
        {
            //CoreLogger.LogStart();
        }

        public static void InitLog()
        {
            //CoreLogger.LogStart();
        }

        #region multi thread
        private static object s_Lock = new object();
        private static List<DebugData> m_WaitingList = new List<DebugData>();

        struct DebugData
        {
            public LOG_CHANNEL channel;
            public string message;
            public LogType type;

            public DebugData(LOG_CHANNEL channel, string message, LogType type)
            {
                this.channel = channel;
                this.message = message;
                this.type = type;
            }
        }

        public static void Tick()
        {
            if (m_WaitingList.Count > 0)
            {
                lock (s_Lock)
                {
                    for (int i = 0; i < m_WaitingList.Count; i++)
                    {
                        DebugData data = m_WaitingList[i];
                        switch (data.type)
                        {
                            case LogType.Log:
                                DebugLog(data.channel, data.message); break;
                            case LogType.Warning:
                                Warning(data.channel, data.message); break;
                            case LogType.Error:
                                Error(data.channel, data.message); break;
                        }
                    }
                    m_WaitingList.Clear();
                }
            }
        }


        public static void SafeDebugLog(LOG_CHANNEL channel, string message) {
            InternalSafeLog(LogType.Log, channel, message);
        }

        public static void SafeWarning(LOG_CHANNEL channel, string message)
        {
            InternalSafeLog(LogType.Warning, channel, message);
        }

        public static void SafeError(LOG_CHANNEL channel, string message) {
            InternalSafeLog(LogType.Error, channel, message);
        }

        private static void InternalSafeLog(LogType type, LOG_CHANNEL channel, string message)
        {
            lock (s_Lock)
            {
                DebugData data = new DebugData(channel, message, type);
                m_WaitingList.Add(data);
            }
        }

        #endregion multi thread
        
        public static void DebugLog(LOG_CHANNEL channel, string message)
        {
			string content = CoreLogger.Debug(channel, message);
            if (OnLogReceived != null) { OnLogReceived(channel, LogType.Log, content); }
        }

        public static void Warning(LOG_CHANNEL channel, string message)
        {
            string content = CoreLogger.Warning(channel, message);
            if (OnLogReceived != null) { OnLogReceived(channel, LogType.Warning, content); }
        }
        public static void Error(LOG_CHANNEL channel, string message, UnityEngine.Object context = null)
        {
            string content = CoreLogger.Error(channel, message);
            if (OnLogReceived != null) { OnLogReceived(channel, LogType.Error, content); }
        }

        public static void LogFormat(LOG_CHANNEL channel, string message, params object[] paramList)
        {
            string message2 = string.Format(message, paramList);
            DebugLog(channel, message2);
        }
    }
}


