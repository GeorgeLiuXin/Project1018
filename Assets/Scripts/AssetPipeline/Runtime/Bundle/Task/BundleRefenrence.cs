using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace XWorld.AssetPipeline
{
    public class BundleReference
    {
        const string SCENES_EXTENSION = ".unity";
        private string m_BundlePath;
        private string m_FilePath;
        private bool m_ContainsScene;
        private long m_Size;

        // 为了引用正确,只保存string路径
        private string[] m_SubDependencies;
        private List<string> m_ParentDependencies;

        private BundleCollection m_ABCollection;
        private BundleRefCache m_ABCache;

        public void AddParent(BundleReference parent)
        {
            if (m_ParentDependencies == null)
            {
                m_ParentDependencies = new List<string>();
            }

            if (!m_ParentDependencies.Contains(parent.BundlePath) && parent.BundlePath != BundlePath)
            {
                m_ParentDependencies.Add(BundlePath);
            }
        }
        
        private int m_RefCount;
        private uint m_CRC;
        private AssetBundle m_OriginBundle;
        private float m_Progress;

        private List<string> m_ErrorNodes;

        private ABMonitor m_Monitor;
        
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
                if (m_SubDependencies != null && m_SubDependencies.Length > 0)
                    foreach (string s in m_SubDependencies)
                    {
                        if (m_ABCache.ContainsKey(s))
                        {
                            if (!m_ABCache[s].Done)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                return m_OriginBundle != null;
            }
        }

        public bool Invalid
        {
            get
            {
                if (m_SubDependencies != null && m_SubDependencies.Length > 0)
                    foreach (string s in m_SubDependencies)
                    {
                        if (!m_ABCache.ContainsKey(s))
                        {
                            return true;
                        }
                    }

                return false;
            }
        }

        public BundleReference(BundleCollection collection, BundleRefCache bundleCache)
        {
            m_ABCache = bundleCache;
            m_Monitor = new ABMonitor();
            m_ErrorNodes = new List<string>();
            m_SubDependencies = new string[0];
            m_ParentDependencies = new List<string>();
            m_ABCollection = collection;
        }

        public void Init(string bundlePath, string[] m_SubDependencies, string filePath, uint crc, object receiver)
        {
            this.m_BundlePath = bundlePath;
            this.m_SubDependencies = m_SubDependencies;
            this.m_FilePath = filePath;

            this.m_CRC = crc;
          //  this.m_Monitor.AddCaller(receiver);

            if (m_SubDependencies != null && m_SubDependencies.Length > 0)
                foreach (string s in m_SubDependencies)
                {
                    if (!m_ABCache.ContainsKey(s))
                        m_ABCache[s].AddParent(this);
                }

            // TODO 判断比较粗劣,目前只给编辑器用
            this.m_ContainsScene = bundlePath.EndsWith(SCENES_EXTENSION);
            if (System.IO.File.Exists(m_FilePath))
            {
                System.IO.FileInfo fileinfo = new System.IO.FileInfo(m_FilePath);
                m_Size = fileinfo.Length;
            }
        }

        public void AddError(string errorContent)
        {
            m_ErrorNodes.Add(errorContent);
        }

        public string BundlePath
        {
            get
            {
                return m_BundlePath;
            }

            set
            {
                m_BundlePath = value;
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

        public string[] SubDependencies
        {
            get
            {
                return m_SubDependencies;
            }

            set
            {
                m_SubDependencies = value;
            }
        }

        public bool ContainDependence(string dependence)
        {
            if (m_SubDependencies != null && m_SubDependencies.Length > 0)
            {
                return m_SubDependencies.Contains(dependence);
            }
            return false;
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

        public uint CRC
        {
            get
            {
                return m_CRC;
            }

            set
            {
                m_CRC = value;
            }
        }

        public void Retain(object receiver)
        {
            m_RefCount++;
            m_Monitor.Retain(receiver);

            if (m_SubDependencies != null && m_SubDependencies.Length > 0)
                foreach (string s in m_SubDependencies)
                {
                    if (m_ABCache.ContainsKey(s))
                        m_ABCache[s].Retain(receiver);
                    else
                        GameLogger.Error(LOG_CHANNEL.LOGIC, "FATAL ERROR " + s + "  " + BundlePath);
                }
        }

        public void Release(object receiver, bool destroyImmediately)
        {
            if (!Done)
            {
                GameLogger.Warning(LOG_CHANNEL.LOGIC, "未加载完就产生了卸载或重复卸载,此次卸载将会被拒绝" + BundlePath);
            }
            else
            {
                if (m_RefCount > 0)
                {
                    m_RefCount--;
                    m_Monitor.Release(receiver);
                    if (m_SubDependencies != null && m_SubDependencies.Length > 0)
                        foreach (string s in m_SubDependencies)
                        {
                            if (m_ABCache.ContainsKey(s))
                                m_ABCache[s].Release(receiver, destroyImmediately);
                        }

                    if (m_RefCount == 0)
                    {
                        m_ABCollection.UnloadBundle(m_BundlePath, m_OriginBundle, destroyImmediately);

                        m_Monitor.Clear();
                        m_OriginBundle = null;
                    }
                }
                else
                {
                    GameLogger.Warning(LOG_CHANNEL.LOGIC,
                        string.Format("BundlePath[{0}] 's refCount is low than 0. Must unload too many times", m_BundlePath));
                }
            }
        }

        public AssetBundle OriginBundle
        {
            get
            {
                return m_OriginBundle;
            }

            set
            {
                if (value != null)
                {
                    m_Progress = 1;
                }
                m_OriginBundle = value;
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

        public List<string> ParentDependencies
        {
            get
            {
                return m_ParentDependencies;
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

        public bool ContainsScene
        {
            get
            {
                return m_ContainsScene;
            }
        }

        public long Size
        {
            get
            {
                return m_Size;
            }
        }
    }

    public class ErrorNode
    {
        public string ErrorMessage
        {
            get;
            set;
        }
    }
}


