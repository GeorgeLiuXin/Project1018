using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace XWorld
{
	namespace DataConfig
	{
		public partial class ClientConfigManager
		{
			private static readonly string strPath = @"\Config\";
			private static readonly string strPattern = "*.txt";
			public static readonly string[] CMD_STRING = { "\n", "\t" };
            public static readonly char[] CMD_CHAR = { '\n', '\t' };

            /// <summary>
            /// 客户端配置总表格 优先读取的表格
            /// </summary>
            private static readonly string CONFIG_DEFINE_PATH = "Config/Static/";
            private static readonly string ALL_CONFIG_BEGIN = "client_config_define";

            public static string ClientConfigDefinePath
            {
                get
                { return CONFIG_DEFINE_PATH + ALL_CONFIG_BEGIN + ".txt"; }
            }

            /// <summary>
            /// 客户端阶段性表格配置 0 : appStart 1 : Login 2 : RoleCreate 3  Gaming
            /// </summary>
            public enum TABLE_LOAD_STATE
            {
                TABLE_LOAD_APP_START = 0,
                TABLE_LOAD_LOGIN,
                TABLE_LOAD_ROLE_CREATE,
                TABLE_LOAD_GAMING,
            }

            private static List<client_config_define> m_lAppInitLoader = new List<client_config_define>();
            private static List<client_config_define> m_lLoginLoader = new List<client_config_define>();
            private static List<client_config_define> m_lRoleCreateLoader = new List<client_config_define>();
            private static List<client_config_define> m_lGamingLoader = new List<client_config_define>();

            /// <summary>
            /// 客户端当前序列化完毕的表格配置结构
            /// </summary>
            private static Dictionary<string, TableBase> m_dictTableAll = new Dictionary<string, TableBase>();


            private static GameObject m_configObj;

            public static GameObject ConfigObj
            {
                get
                {
                    return m_configObj;
                }
            }


            public static List<client_config_define> LAppInitLoader
            {
                get
                {
                    return m_lAppInitLoader;
                }
            }

            public static List<client_config_define> LLoginLoader
            {
                get
                {
                    return m_lLoginLoader;
                }
            }

            public static List<client_config_define> LRoleCreateLoader
            {
                get
                {
                    return m_lRoleCreateLoader;
                }
            }

            public static List<client_config_define> LGamingLoader
            {
                get
                {
                    return m_lGamingLoader;
                }
            }

            public static void AddToDictTableAll(string className, TableBase table)
            {
                // 暂时
                //m_dictTableAll.ForceAdd(className, table);
            }


            /// <summary>
            /// 通过名字获取表数据
            /// </summary>
            public static TableBase GetTableByName(string strTableName)
            {
                if (!m_dictTableAll.ContainsKey(strTableName))
                    return null;

                return m_dictTableAll[strTableName];
            }

            public static void InitConfigByJob(string content)
            {
            }

            internal static void InitInitConfigByJob(string strConfigData)
            {
                m_lAppInitLoader.Clear();
                m_lLoginLoader.Clear();
                m_lRoleCreateLoader.Clear();
                m_lGamingLoader.Clear();
                m_dictTableAll.Clear();

                if (m_configObj != null)
                {
                    GameObject.Destroy(m_configObj);
                    m_configObj = null;
                }

                m_configObj = new GameObject();
                m_configObj.name = "allm_configObj";
                GameObject.DontDestroyOnLoad(m_configObj);
                
                if (strConfigData == null)
                {
                    GameLogger.Error(LOG_CHANNEL.ASSET, ALL_CONFIG_BEGIN + " init failed!");
                    return;
                }

                string[] values = strConfigData.Split("\r"[0]);
                if (values == null || values.Length <= 2)
                {
                    GameLogger.Error(LOG_CHANNEL.ASSET, ALL_CONFIG_BEGIN + " parse failed!");
                    return;
                }

                int j = 0;
                for (int i = 2; i < values.Length; ++i)
                {
                    client_config_define data = new client_config_define();
                    j = 0;
                    string[] subValues = values[i].TrimStart('\n').Split(CMD_STRING, StringSplitOptions.None);
                    if (subValues != null && subValues.Length > 1)
                    {
                        data.tableKey = subValues[j];
                        j++;
                        data.tableName = subValues[j];
                        j++;
                        data.tableClassName = subValues[j];
                        j++;
                        data.tablePath = subValues[j];
                        j++;
                        data.tableInitOrder = ClientConfigManager.ToInt32(subValues[j]);
                        j++;

                        if (data.tableInitOrder == (int)TABLE_LOAD_STATE.TABLE_LOAD_APP_START)
                        {
                            m_lAppInitLoader.Add(data);
                        }
                        else if (data.tableInitOrder == (int)TABLE_LOAD_STATE.TABLE_LOAD_LOGIN)
                        {
                            m_lLoginLoader.Add(data);
                        }
                        else if (data.tableInitOrder == (int)TABLE_LOAD_STATE.TABLE_LOAD_ROLE_CREATE)
                        {
                            m_lRoleCreateLoader.Add(data);
                        }
                        else if (data.tableInitOrder == (int)TABLE_LOAD_STATE.TABLE_LOAD_GAMING)
                        {
                            m_lGamingLoader.Add(data);
                        }
                    }
                }
            }

            public static void InitConfigByLogin()
            {
                return;
                if (m_lLoginLoader == null || m_lLoginLoader.Count == 0)
                    return;

                foreach (client_config_define item in m_lLoginLoader)
                {
                    LoadTable(item);
                }

                m_lLoginLoader.Clear();
            }

            public static void InitConfigByRoleCreate()
            {
                return;
                if (m_lRoleCreateLoader == null || m_lRoleCreateLoader.Count == 0)
                    return;

                foreach (client_config_define item in m_lRoleCreateLoader)
                {
                    LoadTable(item);
                }

                m_lRoleCreateLoader.Clear();
            }

            public static void InitConfigByEnterGame()
            {
                return;
                if (m_lGamingLoader == null || m_lGamingLoader.Count == 0)
                    return;

                foreach (client_config_define item in m_lGamingLoader)
                {
                    LoadTable(item);
                }

                m_lGamingLoader.Clear();
            }

            private static void LoadTable(client_config_define defConfig)
            {
                if (defConfig == null)
                    return;

                System.Type classType = System.Type.GetType("XWorld.DataConfig." + defConfig.tableClassName);
                if (classType != null)
                {
                    TableBase table = m_configObj.AddComponent(classType) as TableBase;
                    if (table != null)
                    {
                        string strParse = ParseTxt(defConfig.tablePath, defConfig.tableName);
                        if (strParse != null)
                        {
                            table.LoadData(strParse);
                            m_dictTableAll.Add(defConfig.tableClassName, table);
                        }
                        else
                        {
                            GameLogger.Error(LOG_CHANNEL.ASSET, "The config txt named " + defConfig.tableName + " parse failed! ");
                        }
                    }
                    else
                    {
                        GameLogger.Error(LOG_CHANNEL.ASSET,"The config txt named " + defConfig.tableName + " parse failed! ");
                    }
                }
            }

            private static string ParseTxt(string strPath,string strTableName)
            {
                TextAsset asset =  Resources.Load<TextAsset>(strPath + strTableName);
                if (asset == null || asset.text == "")
                    return null;

                return asset.text;
            }

			private string ParseFiles(string strPath)
			{
				string strContent = "";
				FileStream fs = new FileStream(strPath, FileMode.Open, FileAccess.Read, FileShare.None);
				if (fs != null)
				{
					StreamReader sr = new StreamReader(fs);
					if(sr != null)
					{
						strContent = sr.ReadToEnd();
					}
				}

				return strContent;
			}

            #region convert helper

            public static byte ToByte(string content)
            {
                if (string.IsNullOrEmpty(content))
                {
                    return default(int);
                }
                else
                {
                    return Convert.ToByte(content);
                }
            }

            public static sbyte ToSByte(string content)
            {
                if (string.IsNullOrEmpty(content))
                {
                    return default(int);
                }
                else
                {
                    return Convert.ToSByte(content);
                }
            }

            public static int ToInt32(string content)
            {
                if (string.IsNullOrEmpty(content))
                {
                    return default(int);
                }
                else
                {
                    return Convert.ToInt32(content);
                }
            }

            public static short ToInt16(string content)
            {
                if (string.IsNullOrEmpty(content))
                {
                    return default(short);
                }
                else
                {
                    return Convert.ToInt16(content);
                }
            }

            public static long ToInt64(string content)
            {
                if (string.IsNullOrEmpty(content))
                {
                    return default(long);
                }
                else
                {
                    return Convert.ToInt64(content);
                }
            }

            public static bool ToBoolean(string content)
            {
                if (string.IsNullOrEmpty(content))
                {
                    return default(bool);
                }
                else
                {
                    return Convert.ToBoolean(content);
                }
            }

            public static ushort ToUInt16(string content)
            {
                if (string.IsNullOrEmpty(content))
                {
                    return default(ushort);
                }
                else
                {
                    return Convert.ToUInt16(content);
                }
            }

            public static uint ToUInt32(string content)
            {
                if (string.IsNullOrEmpty(content))
                {
                    return default(uint);
                }
                else
                {
                    return Convert.ToUInt32(content);
                }
            }

            public static ulong ToUInt64(string content)
            {
                if (string.IsNullOrEmpty(content))
                {
                    return default(ulong);
                }
                else
                {
                    return Convert.ToUInt64(content);
                }
            }

            public static float ToSingle(string content)
            {
                if (string.IsNullOrEmpty(content))
                {
                    return default(float);
                }
                else
                {
                    return Convert.ToSingle(content);
                }
            }

            public static double ToDouble(string content)
            {
                if (string.IsNullOrEmpty(content))
                {
                    return default(double);
                }
                else
                {
                    return Convert.ToDouble(content);
                }
            }
            #endregion
        }
    }
}
