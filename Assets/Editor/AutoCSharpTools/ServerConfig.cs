using System;
using System.IO;
using UnityEngine;

public class ServerConfig
{
    private static readonly string strServerTablePath = "/Server/bin32/Data/DataTable";
    private static readonly string strNewTablePath = "/Server/bin32/Data/NewDataTable";
    public static void AutoNew()
    {
        Debug.Log("Server txt change start!");

        DirectoryInfo t_tempInfo = new DirectoryInfo(Application.dataPath);
        t_tempInfo = t_tempInfo.Parent;
        t_tempInfo = t_tempInfo.Parent;
        string strOldPath = t_tempInfo.FullName + strServerTablePath;
        string strNewPath = t_tempInfo.FullName + strNewTablePath;

        if (!Directory.Exists(strNewPath))
        {
            Directory.CreateDirectory(strNewPath);
        }

        string[] oldFiles = Directory.GetFiles(strNewPath);
        foreach (string item in oldFiles)
        {
            File.Delete(item);
        }

        string[] configFile = Directory.GetFiles(strOldPath);
        if (configFile != null)
        {
            foreach(string item in configFile)
            {
                FileStream fs = File.OpenRead(item);
                if(fs != null)
                {
                    StreamReader changeSr = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312"));
                    string strContent = changeSr.ReadToEnd();

                    string newName = item.Replace('\\','/');
                    string[] strAll = newName.Split('/');

                    StreamWriter sw = new StreamWriter(strNewPath + "/" + strAll[strAll.Length - 1], false, System.Text.Encoding.GetEncoding("gb2312"));
                    sw.Write(strContent);

                    sw.Close();
                    sw.Dispose();

                    changeSr.Close();
                    changeSr.Dispose();
                }

                fs.Close();
                fs.Dispose();
            }
        }

        Debug.Log("Server txt change end!");
    }
}
