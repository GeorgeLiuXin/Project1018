using UnityEngine;
using System.Collections;
using System.Linq;
using System.IO;

namespace Galaxy.AssetBundleBrowser
{
    public class CSVReader
    {
        public TextAsset csvFile;

        // splits a CSV row 
        static public string[] SplitCsvLine(string line)
        {
            return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
            @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
            System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                    select m.Groups[1].Value).ToArray();
        }
        
        public static string[] GetGlobalFileData(string strFile)
        {
            string content = GetTableFileContent(strFile);
            if (content == string.Empty)
                return null;

            string[] lineArray = content.Split("\r"[0]);
            return lineArray;
        }

        public static string GetTableFileContent(string relativePath)
        {
            string content = "";

            string path = Application.dataPath + "/" + relativePath + ".csv";
            if (!File.Exists(path))
                return string.Empty;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    content = sr.ReadToEnd();
                }
            }

            return content;
        }
    }
}

