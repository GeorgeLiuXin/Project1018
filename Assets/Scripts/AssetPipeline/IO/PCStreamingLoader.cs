
namespace XWorld.AssetPipeline
{
    using System;
    using System.IO;
    using UnityEngine;

    internal class PCStreamingAssetsLoader : StreamingAssetsLoader
    {
        private string m_streamingAssetsPath = (Application.streamingAssetsPath + "/");

        public override byte[] Load(string name)
        {
            string path = this.m_streamingAssetsPath + name;
            if (!File.Exists(path))
            {
                return null;
            }
            return File.ReadAllBytes(path);
        }
    }
}

