using System;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public class PreInitCFG: Singleton<PreInitCFG>
    {
        private INIParse iniParse = new INIParse();
        public void Parse(String content, bool ignoreCase = false)
        {
            iniParse.Parse(content, ignoreCase);
        }
        public bool KeyExists(string section, string key)
        {
            return iniParse.KeyExists(section, key);
        }
        public bool GetInt(String section, String key, out Int32 value)
        {
            return iniParse.GetInt(section, key, out value);
        }

        public Int32 GetInt(String section, String key)
        {
            return iniParse.GetInt(section, key);
        }

        public bool GetSingle(String section, String key, out float value)
        {
            return iniParse.GetSingle(section, key, out value);
        }

        public float GetSingle(String section, String key)
        {
            return iniParse.GetSingle(section, key);
        }

        public bool GetString(String section, String key, out String value)
        {
            return iniParse.GetString(section, key, out value);
        }

        public String GetString(String section, String key)
        {
            return iniParse.GetString(section, key);
        }
		
    }
}
