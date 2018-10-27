namespace XWorld.AssetPipeline
{
    using System;
    using System.IO;

    public class PersistentAssetsLoader
    {
        public byte[] Load(string name)
        {
            string path = ResourcesMgr.PersistentAssetsPath + name;
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            return null;
        }
    }
}

