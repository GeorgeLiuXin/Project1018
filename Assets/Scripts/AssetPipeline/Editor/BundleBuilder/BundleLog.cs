using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace Galaxy.AssetPipeline
{
    public class BundleLog
    {
        public static List<LogInfo> Infos = new List<LogInfo>();

        public static string PersistentDataPath;
        public static string FullPath;
        public static string FileName;

        public static void OnEnable()
        {
            PersistentDataPath = BundleConfig.BundleLogPath;
        }

        public static void StartNewLog(RuntimePlatform platform, EnumChannelType channel, VersionInfo previousVersion, VersionInfo newVersion, bool isTest, bool ignoreLastOne, string context = "")
        {
            if (string.IsNullOrEmpty(PersistentDataPath))
            {
                throw new Exception("PersistentDataPath is null");
            }

            string contextStr = string.IsNullOrEmpty(context) ? "" : "[" + context + "]";
            string previousStr = previousVersion != null ? previousVersion.ToString() : "NONE";
            string newStr = newVersion != null ? newVersion.ToString() : "NONE";
            string testStr = isTest ? "[TEST]" : "";
            testStr += (ignoreLastOne ? "[NEW]" : "");
            string timeStr = DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();

            FileName = string.Format("[{4}][{5}][{0}-{1}]{2}{3}({6})", previousStr, newStr, testStr, contextStr, channel.ToString(), BuildHelper.CustomTargetToName(platform), timeStr);
            string recordPath = string.Format("{0}/{1}", PersistentDataPath, "BundleLog");

            BuildHelper.CheckCustomPath(recordPath);
            FullPath = string.Format("{0}/{1}.txt", recordPath, FileName);
            BundleLog.Log(string.Format("建立新的纪录文档:{0}", FullPath));
            Save();
        }

        public static void Log(string content)
        {
            GameLogger.DebugLog(LOG_CHANNEL.ASSET, content);
            LogInfo log = new LogInfo();
            log.Content = content;
            log.Type = LogType.Log;
            log.Time = DateTime.Now;
            Infos.Add(log);
            Save();
        }

        public static void Error(string content)
        {
            GameLogger.Error(LOG_CHANNEL.ASSET, content);
            LogInfo log = new LogInfo();
            log.Content = content;
            log.Type = LogType.Error;
            log.Time = DateTime.Now;
            Infos.Add(log);
            Save();
        }

        public static void Warning(string content)
        {
            GameLogger.Warning(LOG_CHANNEL.ASSET, content);
            LogInfo log = new LogInfo();
            log.Content = content;
            log.Type = LogType.Warning;
            log.Time = DateTime.Now;
            Infos.Add(log);
            Save();
        }
        
        public static void Save()
        {
            if (string.IsNullOrEmpty(FullPath) && !File.Exists(FullPath))
            {
                return;
            }
            using (FileStream fs = new FileStream(FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.ASCII))
                {
                    int length = Infos.Count;
                    for (int i = 0; i < length; i++)
                    {
                        LogInfo log = Infos[i];
                        sw.WriteLine(log.ToString());
                    }
                    Infos.Clear();
                }
            }
        }

        internal static void EndLog()
        {
            Save();
            BundleLog.Log(string.Format("保存纪录文档:{0}", FullPath));
        }
        public class LogInfo
        {
            public DateTime Time
            {
                get; set;
            }

            public string Content
            {
                get; set;
            }

            public LogType Type
            {
                get; set;
            }

            public override string ToString()
            {
                return string.Format("[Time:{0}][Type:{1}][Content:{2}]", Time.ToString(), Type.ToString(), Content.ToString());
            }
        }

    }
}
