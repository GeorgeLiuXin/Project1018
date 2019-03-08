using System.IO;
using UnityEngine;

namespace Galaxy
{
    public class LevelEditorSaveManager
    {
        protected readonly string SERVER_PATH = "/Server/bin32/Data/DataTable/LevelFile/";
        public string FilePath
        {
            get;
            set;
        }

        public string CltFilePath
        {
            get
            {
                return Application.dataPath + "/AssetDatas/Config/Static/";
            }
        }
        
        public LevelEditorSaveManager()
        {
           
        }

        public void CreatePath()
        {
            DirectoryInfo t_tempInfo = new DirectoryInfo(Application.dataPath);
            t_tempInfo = t_tempInfo.Parent;
            t_tempInfo = t_tempInfo.Parent;
            FilePath = t_tempInfo.FullName + SERVER_PATH;
            FilePath = FilePath.Replace('/', '\\');

            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            else
            {
                DirectoryInfo info = new DirectoryInfo(FilePath);
                FileInfo[] infos = info.GetFiles(FilePath);
                if (infos != null)
                {
                    foreach (var item in infos)
                    {
                        if (item == null)
                            continue;

                        item.Delete();
                    }
                }
            }

            UnityEditor.AssetDatabase.Refresh();
        }
    }
}