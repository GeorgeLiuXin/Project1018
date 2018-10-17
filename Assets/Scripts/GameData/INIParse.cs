namespace XWorld
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class INIParse
    {
        private Dictionary<string, Dictionary<string, string>> m_SectionDic;
        private const string regexKeyValue = ".*=.*";
        private const string regexSection = @"\[[\w\s\.]*\][^\[]*";
        private const string regexSectionTitle = @"\[(.*)\]";

        public INIParse()
        {
            this.m_SectionDic = new Dictionary<string, Dictionary<string, string>>();
        }

        public INIParse(string content, bool ignoreCase = false)
        {
            this.m_SectionDic = new Dictionary<string, Dictionary<string, string>>();
            this.Parse(content, ignoreCase);
        }

        protected void AddSection(string section, string key, string value)
        {
            if (this.m_SectionDic.ContainsKey(section))
            {
                if (this.m_SectionDic[section].ContainsKey(key))
                {
                    GameLogger.Warning(LOG_CHANNEL.LOGIC, string.Format("INI配置文件{0}中key重复", new object[] { section }));
                }
                else
                {
                    this.m_SectionDic[section][key] = value;
                }
            }
            else
            {
                this.m_SectionDic.Add(section, new Dictionary<string, string>());
                this.m_SectionDic[section][key] = value;
            }
        }

        protected void Clear()
        {
            this.m_SectionDic.Clear();
        }

        public string[] GetAllSections()
        {
            if (m_SectionDic != null)
            {
                return new List<string>(m_SectionDic.Keys).ToArray();
            }
            else
            {
                return null;
            }
        }

        public string[] GetAllKeys(string section)
        {
            if (m_SectionDic != null)
            {
                if (m_SectionDic.ContainsKey(section)) {
                    Dictionary<string, string> keyDict = m_SectionDic[section];
                    return new List<string>(keyDict.Keys).ToArray();
                }
            }
            return null;
        }

        public Dictionary<string, string> GetKeyDict(string section)
        {
            if (m_SectionDic != null)
            {
                if (m_SectionDic.ContainsKey(section))
                {
                    return m_SectionDic[section];
                }
            }
            return null;
        }

        public int GetInt(string section, string key)
        {
            int num;
            string sectionValue = this.GetSectionValue(section, key);
            if ((sectionValue != null) && int.TryParse(sectionValue, out num))
            {
                return num;
            }
            return -2147483648;
        }

        public bool GetInt(string section, string key, out int value)
        {
            string sectionValue = this.GetSectionValue(section, key);
            if (sectionValue == null)
            {
                value = -2147483648;
                return false;
            }
            return int.TryParse(sectionValue, out value);
        }

        protected string GetSectionValue(string section, string key)
        {
            if (this.m_SectionDic.ContainsKey(section) && this.m_SectionDic[section].ContainsKey(key))
            {
                return this.m_SectionDic[section][key];
            }
            return null;
        }

        public float GetSingle(string section, string key)
        {
            float num;
            string sectionValue = this.GetSectionValue(section, key);
            if ((sectionValue != null) && float.TryParse(sectionValue, out num))
            {
                return num;
            }
            return float.MinValue;
        }

        public bool GetSingle(string section, string key, out float value)
        {
            string sectionValue = this.GetSectionValue(section, key);
            if (sectionValue == null)
            {
                value = float.MinValue;
                return false;
            }
            return float.TryParse(sectionValue, out value);
        }

        public string GetString(string section, string key)
        {
            return this.GetSectionValue(section, key);
        }

        public bool GetString(string section, string key, out string value)
        {
            string sectionValue = this.GetSectionValue(section, key);
            if (sectionValue == null)
            {
                value = null;
                return false;
            }
            value = sectionValue;
            return true;
        }

        public bool KeyExists(string section, string key)
        {
            return
            (this.m_SectionDic.ContainsKey(section) && this.m_SectionDic[section].ContainsKey(key));
        }

        public void Parse(string content, bool ignoreCase = false)
        {
            if (ignoreCase)
            {
                content = content.ToLower();
            }
            this.Clear();
            foreach (Match match in Regex.Matches(content, @"\[[\w\s\.]*\][^\[]*"))
            {
                string section = Regex.Replace(Regex.Match(match.ToString(), @"\[(.*)\]").ToString(), @"\[(.*)\]", "$1").ToString().Trim();
                foreach (Match match2 in Regex.Matches(match.ToString(), ".*=.*"))
                {
                    string str2 = match2.ToString().Trim();
                    if (str2.ToCharArray()[0] != ';')
                    {
                        int index = str2.IndexOf('=');
                        int num2 = str2.IndexOf(';');
                        if (num2 >= 0)
                        {
                            if (num2 >= index)
                            {
                                this.AddSection(section, str2.Substring(0, index).Trim(), str2.Substring(index + 1, (num2 - index) - 1).Trim());
                            }
                        }
                        else
                        {
                            this.AddSection(section, str2.Substring(0, index).Trim(), str2.Substring(index + 1).Trim());
                        }
                    }
                }
            }
        }

        public bool SectionExists(string section)
        {
            return this.m_SectionDic.ContainsKey(section);
        }
    }
}

