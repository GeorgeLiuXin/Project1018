using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using UnityEngine;

namespace XWorld
{
	namespace DataConfig
	{
		public abstract class DataBase 
		{
			private readonly string[] COMIT_STRING = { "\t", "\n" };
			private bool m_bDataInit = false;

			public DataBase(string strFileName)
			{
				m_bDataInit = LoadTemplate (strFileName);
			}

			protected abstract bool LoadTemplate (string strFileName);

			protected string[] LoadConfigLine(string strFileName)
			{
				string strFileContent = GetTableFileContent (strFileName);
				if (strFileContent == null)
					return null;

				string[] arrLine = strFileContent.Split ("\r"[0]);
				return arrLine;
			}

			protected string[] SplitCsvLine(string line)
			{
				return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
					@"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
					System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
					select m.Groups[1].Value).ToArray();
			}

			protected string[] SplitTableLine(string line)
			{
				string[] strArr = line.Split (COMIT_STRING,StringSplitOptions.RemoveEmptyEntries);
				if (strArr == null)
					return null;

				return strArr;
			}

			private string GetTableFileContent(string relativePath)
			{
				string content = "";

				string strPath = Application.dataPath + "/GameAsset/" + relativePath + ".txt";
				if (!File.Exists (strPath))
					return string.Empty;

				FileStream fs = new FileStream (strPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				if (fs != null) 
				{
					StreamReader sr = new StreamReader (fs);
					if (sr != null) 
					{
						content = sr.ReadToEnd();
					}
				}

				return content;
			}
		}
	}
}


