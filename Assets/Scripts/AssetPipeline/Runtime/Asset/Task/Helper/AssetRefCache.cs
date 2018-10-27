using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public class AssetRefCache
    {
        public AssetPipelineAction RefChangedEvent;
        public AssetPipelineAction RefShutdownEvent;
        
        private Dictionary<string, AssetReference> m_AssetRefDict;
        private KeyedPriorityQueue<string, AssetReference, int> m_WaitingAssetRefQueue;

        public Dictionary<string, AssetReference> AssetRefDict
        {
            get
            {
                return m_AssetRefDict;
            }
        }
        
        public KeyedPriorityQueue<string, AssetReference, int> WaitingAssetRefQueue
        {
            get
            {
                return m_WaitingAssetRefQueue;
            }
        }

        public AssetRefCache()
        {
            m_AssetRefDict = new Dictionary<string, AssetReference>();
            m_WaitingAssetRefQueue = new KeyedPriorityQueue<string, AssetReference, int>();
        }

        public void Add(string bundlePath, AssetReference reference)
        {
            if (!m_AssetRefDict.ContainsKey(bundlePath))
            {
                m_AssetRefDict.Add(bundlePath, reference);
                FireChangedEvent();
            }
        }
        

        public void AddWaiting(string bundlePath, int priority)
        {
            if (m_AssetRefDict.ContainsKey(bundlePath))
            {
                AssetReference reference = m_AssetRefDict[bundlePath];
                if (!m_WaitingAssetRefQueue.ContainsKey(bundlePath))
                {
                    m_WaitingAssetRefQueue.Enqueue(bundlePath, reference, priority);
                }
            }
        }

        public void Remove(string bundlePath)
        {
            if (m_AssetRefDict.ContainsKey(bundlePath))
            {
                AssetReference reference = m_AssetRefDict[bundlePath];
                
                if (m_WaitingAssetRefQueue.ContainsKey(bundlePath))
                    m_WaitingAssetRefQueue.Remove(bundlePath);

                m_AssetRefDict.Remove(bundlePath);

                FireChangedEvent();
            }
        }

        public bool ContainsKey(string bundlePath)
        {
            if (m_AssetRefDict.ContainsKey(bundlePath))
            {
                return true;
            }
            return false;
        }

        public AssetReference[] FireWaitingList(out AssetReference[] errorRefs)
        {
            List<AssetReference> doneOuputs = new List<AssetReference>();
            List<AssetReference> errorOuputs = new List<AssetReference>();
            
            if (m_WaitingAssetRefQueue.Count > 0)
            {
                for (int i = m_WaitingAssetRefQueue.Count; i > 0; i--) // 非 m_WaitingBundleRefQueue.Count-1 >=0  
                {
                    AssetReference reference = m_WaitingAssetRefQueue.GetValue(i);
                    if (reference != null)
                    {
                        if (reference.Done)
                        {
                            doneOuputs.Add(reference);
                            m_WaitingAssetRefQueue.RemoveValue(i);
                        }
                        else if (reference.Error)
                        {
                            errorOuputs.Add(reference);
                            m_WaitingAssetRefQueue.RemoveValue(i);
                        }
                    }
                }
            }
            
            errorRefs = errorOuputs.ToArray();
            return doneOuputs.ToArray();
        }

        public void DepTryRemove(string bundlePath)
        {
            if (ContainsKey(bundlePath))
            {
                AssetReference bundleReference = this[bundlePath];
                if (bundleReference.RefCount == 0)
                {
                    Remove(bundlePath);
                }
            }
        }

        public AssetReference this[string key]
        {
            get
            {
                if (ContainsKey(key))
                {
                    return m_AssetRefDict[key];
                }
                return null;
            }
        }

        public void Shutdown()
        {
            m_AssetRefDict.Clear();
            m_WaitingAssetRefQueue.Clear();
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
                UnityEngine.Debug.LogWarning("Asset " + RefChangedEvent);
                RefChangedEvent();
            }
        }
    }
}
