using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace XWorld.AssetPipeline
{
    public class AssetReference
    {
        const string SCENES_EXTENSION = ".unity";
        private string m_AssetPath;
        private string m_FilePath;
        private bool m_IsScene;
        private string m_BundlePath;
        private AssetRefCache m_ABCache;
        private int m_RefCount;
        private UnityEngine.Object m_OriginAsset;
        private float m_Progress;
        private List<string> m_ErrorNodes;
        private bool m_BundlePrepared;
        private ABMonitor m_Monitor;
        private int m_Priority;

        public bool Error
        {
            get
            {
                return m_ErrorNodes.Count > 0;
            }
        }

        public bool Done
        {
            get
            {
                if (!m_IsScene)
                {
                    return m_OriginAsset != null;
                }
                else
                {
                    return m_Progress == 1;
                }
            }
        }
        
        public AssetReference(AssetRefCache assetCache)
        {
            m_ABCache = assetCache;
            m_Monitor = new ABMonitor();
            m_ErrorNodes = new List<string>();
        }

        public void Init(string assetPath, string bundlePath, object receiver, int priority)
        {
            this.m_Priority = priority;
            this.m_AssetPath = assetPath;
            this.m_BundlePath = bundlePath;
            this.m_IsScene = assetPath.EndsWith(SCENES_EXTENSION);
        }

        public void AddError(string errorContent)
        {
            m_ErrorNodes.Add(errorContent);
        }

        public string AssetPath
        {
            get
            {
                return m_AssetPath;
            }

            set
            {
                m_AssetPath = value;
            }
        }

        public string FilePath
        {
            get
            {
                return m_FilePath;
            }

            set
            {
                m_FilePath = value;
            }
        }

        public int RefCount
        {
            get
            {
                return m_RefCount;
            }

            set
            {
                m_RefCount = value;
            }
        }

        public void ForceRelease(object reciever, bool destroyImmediately)
        {
            int tempCount = m_RefCount;
            for (int i = 0; i < tempCount; i++)
            {
                Debug.Log(i + "  " + m_RefCount);
                Release(reciever, destroyImmediately);
            }
        }

        public void Retain(object receiver)
        {
            m_RefCount++;
            m_Monitor.Retain(receiver);
        }

        public void Release(object receiver, bool destroyImmediately)
        {
            if (!Done)
            {
                GameLogger.Warning(LOG_CHANNEL.LOGIC, "未加载完就产生了卸载或重复卸载,此次卸载将会被拒绝" + AssetPath);
            }
            else
            {
                if (m_RefCount > 0)
                {
                    m_RefCount--;
                    m_Monitor.Release(receiver);

                    if (m_RefCount == 0)
                    {
                        XWorldGameModule.GetGameManager<BundleManager>().Unload(m_BundlePath, receiver, destroyImmediately);
                        m_OriginAsset = null;
                    }
                }
                else
                {
                    GameLogger.Warning(LOG_CHANNEL.LOGIC,
                        string.Format("AssetPath[{0}] 's refCount is low than 0. Must unload too many times", m_AssetPath));
                }
            }
        }

        public UnityEngine.Object OriginAsset
        {
            get
            {
                return m_OriginAsset;
            }

            set
            {
                m_OriginAsset = value;
            }
        }

        public ABMonitor Monitor
        {
            get
            {
                return m_Monitor;
            }

            set
            {
                m_Monitor = value;
            }
        }

        public float Progress
        {
            get
            {
                return m_Progress;
            }

            set
            {
                m_Progress = value;
            }
        }

        public List<string> ErrorNodes
        {
            get
            {
                return m_ErrorNodes;
            }

            set
            {
                m_ErrorNodes = value;
            }
        }

        public bool IsScene
        {
            get
            {
                return m_IsScene;
            }
        }

        public bool BundlePrepared
        {
            get
            {
                return m_BundlePrepared;
            }

            set
            {
                m_BundlePrepared = value;
            }
        }

        public string BundlePath
        {
            get
            {
                return m_BundlePath;
            }
        }

        public bool Invalid
        {
            get
            {
                return !BundlePrepared;
            }
        }
    }
}


