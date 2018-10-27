using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public class BundleRefCache
    {
        public AssetPipelineAction RefChangedEvent;
        public AssetPipelineAction RefShutdownEvent;

        private Dictionary<string, BundleReference> m_BundleRefDict;
        private KeyedPriorityQueue<string, BundleReference, int> m_WaitingBundleRefQueue;

        public Dictionary<string, BundleReference> BundleRefDict
        {
            get
            {
                return m_BundleRefDict;
            }
        }

        public KeyedPriorityQueue<string, BundleReference, int> WaitingBundleRefQueue
        {
            get
            {
                return m_WaitingBundleRefQueue;
            }
        }

        public BundleRefCache()
        {
            m_BundleRefDict = new Dictionary<string, BundleReference>();
            m_WaitingBundleRefQueue = new KeyedPriorityQueue<string, BundleReference, int>();
        }

        public void Add(string bundlePath, BundleReference reference)
        {
            if (!m_BundleRefDict.ContainsKey(bundlePath))
            {
                m_BundleRefDict.Add(bundlePath, reference);
                FireChangedEvent();
            }
        }

        public void AddWaiting(string bundlePath, int priority)
        {
            if (m_BundleRefDict.ContainsKey(bundlePath))
            {
                BundleReference reference = m_BundleRefDict[bundlePath];
                if (!m_WaitingBundleRefQueue.ContainsKey(bundlePath))
                {
                    m_WaitingBundleRefQueue.Enqueue(bundlePath, reference, priority);
                }
            }
        }

        public void Remove(string bundlePath)
        {
            if (m_BundleRefDict.ContainsKey(bundlePath))
            {
                BundleReference reference = m_BundleRefDict[bundlePath];

                if (m_WaitingBundleRefQueue.ContainsKey(bundlePath))
                    m_WaitingBundleRefQueue.Remove(bundlePath);

                m_BundleRefDict.Remove(bundlePath);
                FireChangedEvent();
            }
        }

        public bool ContainsKey(string bundlePath)
        {
            if (m_BundleRefDict.ContainsKey(bundlePath))
            {
                return true;
            }
            return false;
        }

        public BundleReference[] FireWaitingList(out BundleReference[] errorRefs)
        {
            List<BundleReference> downOuputs = new List<BundleReference>();
            List<BundleReference> errorOuputs = new List<BundleReference>();

            if (m_WaitingBundleRefQueue.Count > 0)
            {
                for (int i = m_WaitingBundleRefQueue.Count; i > 0; i--) // 非 m_WaitingBundleRefQueue.Count-1 >=0 
                {
                    BundleReference reference = m_WaitingBundleRefQueue.GetValue(i);
                    if (reference != null)
                    {
                        if (reference.Done)
                        {
                            downOuputs.Add(reference);
                            m_WaitingBundleRefQueue.RemoveValue(i);
                        }
                        else if (reference.Error)
                        {
                            foreach (string s in reference.ParentDependencies)
                            {
                                if (ContainsKey(s))
                                    foreach (string node in reference.ErrorNodes)
                                    {
                                        this[s].AddError("child : " + reference.BundlePath + "  " + node);
                                    }
                            }
                            m_WaitingBundleRefQueue.RemoveValue(i);
                        }
                    }
                }
            }

            errorRefs = errorOuputs.ToArray();
            return downOuputs.ToArray();
        }

        public void DepTryRemove(string bundlePath)
        {
            if (ContainsKey(bundlePath))
            {
                BundleReference bundleReference = this[bundlePath];
                if (bundleReference.RefCount == 0)
                {
                    Remove(bundlePath);
                }

                if (bundleReference.SubDependencies != null && bundleReference.SubDependencies.Length > 0)
                {
                    foreach (string s in bundleReference.SubDependencies)
                        DepTryRemove(s);
                }
            }
        }

        public BundleReference this[string key]
        {
            get
            {
                if (ContainsKey(key))
                {
                    return m_BundleRefDict[key];
                }
                return null;
            }
        }

        public void Shutdown()
        {
            m_BundleRefDict.Clear();
            m_WaitingBundleRefQueue.Clear();
           // FireChangedEvent();
            if (RefShutdownEvent != null)
            {
                RefShutdownEvent();
            }
        }

        private void FireChangedEvent()
        {
            if (RefChangedEvent != null)
            {
                UnityEngine.Debug.LogWarning("Bundle " + RefChangedEvent);
                RefChangedEvent();
            }
        }
    }
}
