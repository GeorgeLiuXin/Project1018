namespace XWorld.AssetPipeline
{
    using System;
    using UnityEngine;

    public abstract class StreamingAssetsLoader
    {
        protected static string m_strStreamingAssetsPath;

        static StreamingAssetsLoader()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                m_strStreamingAssetsPath = Application.dataPath + "!assets/";
            }
            else
            {
                m_strStreamingAssetsPath = Application.streamingAssetsPath + "/";
            }
        }

        protected StreamingAssetsLoader()
        {
        }

        public abstract byte[] Load(string name);

        public string StreamingAssetsPath
        {
            get
            {
                return m_strStreamingAssetsPath;
            }
        }
    }
}

