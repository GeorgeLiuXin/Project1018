
using UnityEngine;
namespace Galaxy
{
    public abstract class LevelEditor_FileCreator
    {

        public abstract string FileName
        {
            get;
        }

        public abstract void SaveFile(int sceneID,string strPath, GameObject groupObj);
    }
}
