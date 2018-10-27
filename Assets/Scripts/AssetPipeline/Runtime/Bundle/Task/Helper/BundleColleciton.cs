using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XWorld.AssetPipeline
{
    public class BundleCollection
    {
        public const int GENERATION_MAX = 9;
        public const float GC_TIME = 20f;

        public AssetPipelineAction CollectionChangedEvent;
        public AssetPipelineAction CollectionShutdownEvent;

        private Dictionary<string, GarbageData> m_GarbageAssetBundleMap;
        private Dictionary<string, int> m_GarbageGenerationMap;
        private List<GarbageData> m_GarbageAssetList;

        #region UnloadQueue

        public const int MAX_UNLOAD_COUNT_PER_FRAME = 2;   //每帧卸载的最大数量;
        public const float MAX_UNLOAD_INTERVAL = 30f;   //除非开发者要求强制卸载, 每30秒触发一次卸载;
        private LinkedList<GarbageData> m_UnloadWaitingList;
        private float m_UnloadInterval;

        public float UnloadInterval
        {
            get
            {
                return m_UnloadInterval;
            }
        }

        #endregion

        public List<GarbageData> GarbageAssetList
        {
            get
            {
                return m_GarbageAssetList;
            }
        }

        public BundleCollection()
        {
            m_GarbageAssetBundleMap = new Dictionary<string, GarbageData>();
            m_GarbageAssetList = new List<GarbageData>();
            m_GarbageGenerationMap = new Dictionary<string, int>();
            m_UnloadWaitingList = new LinkedList<GarbageData>();
        }

        private void AddBundle(string bundlePath, AssetBundle bundle)
        {
            GarbageData gd = null;
            if (m_GarbageAssetBundleMap.ContainsKey(bundlePath))
            {
                gd = m_GarbageAssetBundleMap[bundlePath];
            }
            else
            {
                gd = new GarbageData();
                gd.BundleName = bundlePath;
                gd.Bundle = bundle;
                m_GarbageAssetBundleMap.Add(bundlePath, gd);
                m_GarbageAssetList.Add(gd);
            }

            gd.Generation = GetGeneration(bundlePath);
            gd.ResetInterval();
            FireChangedEvent();
        }

        public AssetBundle GetBundle(string bundlePath)
        {
            if (m_GarbageAssetBundleMap.ContainsKey(bundlePath))
            {
                GarbageData gd = m_GarbageAssetBundleMap[bundlePath];
                m_GarbageAssetBundleMap.Remove(bundlePath);
                m_GarbageAssetList.Remove(gd);
                if (m_UnloadWaitingList.Contains(gd))
                {
                    m_UnloadWaitingList.Remove(gd);
                }
                return gd.Bundle;
            }
            return null;
        }

        public void UnloadBundle(string bundlePath, AssetBundle bundle, bool destroyImmediately)
        {
            if (bundle != null)
            {
                if (destroyImmediately)
                {
                    if (m_GarbageAssetBundleMap.ContainsKey(bundlePath))
                    {
                        GarbageData gd = m_GarbageAssetBundleMap[bundlePath];
                        gd.Bundle = null;
                        m_GarbageAssetList.Remove(gd);
                        m_GarbageAssetBundleMap.Remove(gd.BundleName);
                        if (m_UnloadWaitingList.Contains(gd))
                        {
                            m_UnloadWaitingList.Remove(gd);
                        }

                        bundle.Unload(true);
                        TryRemoveGeneration(bundlePath);
                    }
                    else
                    {
                        bundle.Unload(true);
                    }
                }
                else
                {
                    AddBundle(bundlePath, bundle);
                }
            }
        }

        public void Update(float deltaTime)
        {
            int count = m_GarbageAssetList.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                GarbageData gd = m_GarbageAssetList[i];
                gd.Interval -= deltaTime;
                if (gd.Interval <= 0)
                {
                    if (!gd.ShouldUnload)
                    {
                        gd.Unload(false);
                        m_UnloadWaitingList.AddLast(gd);
                        FireChangedEvent();
                    }
                }
            }


            if (m_UnloadInterval >= MAX_UNLOAD_INTERVAL)
            {
                m_UnloadInterval = 0;
                FrameUnload();
            }
            else
            {
                m_UnloadInterval += deltaTime;
            }
        }

        private void FrameUnload()
        {
            if (m_UnloadWaitingList.Count > 0)
            {
                for (int i = 0; i < MAX_UNLOAD_COUNT_PER_FRAME; i++)
                {
                    if (m_UnloadWaitingList.Count > 0)
                    {
                        GarbageData gd = m_UnloadWaitingList.First.Value;
                        gd.Unload(true);
                        m_GarbageAssetList.Remove(gd);
                        m_GarbageAssetBundleMap.Remove(gd.BundleName);
                        m_UnloadWaitingList.RemoveFirst();
                        TryRemoveGeneration(gd.BundleName);

                        if (m_UnloadWaitingList.Count == 0)
                        {
                            break;
                        }
                    }
                }

                FireChangedEvent();
            }
        }

        public void Collect()
        {
            int count = m_GarbageAssetList.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                GarbageData gd = m_GarbageAssetList[i];
                gd.Unload(true);
            }

            m_GarbageAssetList.Clear();
            m_GarbageAssetBundleMap.Clear();
            m_GarbageGenerationMap.Clear();
            m_UnloadWaitingList.Clear();
            FireChangedEvent();
        }

        private int GetGeneration(string bundlePath)
        {
            if (m_GarbageGenerationMap.ContainsKey(bundlePath))
            {
                int value = m_GarbageGenerationMap[bundlePath];
                if (value < GENERATION_MAX)
                {
                    value++;
                }
                m_GarbageGenerationMap[bundlePath] = value;
                FireChangedEvent();
                return value;
            }
            else
            {
                m_GarbageGenerationMap.Add(bundlePath, 1);
                FireChangedEvent();
                return 1;
            }
        }

        private void TryRemoveGeneration(string bundlePath)
        {
            if (m_GarbageGenerationMap.ContainsKey(bundlePath))
            {
                m_GarbageGenerationMap.Remove(bundlePath);
            }
            FireChangedEvent();
        }
        private void FireChangedEvent()
        {
            if (CollectionChangedEvent != null)
            {
                CollectionChangedEvent();
            }
        }

        public void Shutdown()
        {
            Collect();
        }
    }

    public class GarbageData
    {
        private string m_BundleName;
        private AssetBundle m_Bundle;
        private float m_Interval;
        private int m_Generation;
        private bool m_ShouldUnload = false;


        public AssetBundle Bundle
        {
            get
            {
                return m_Bundle;
            }

            set
            {
                m_Bundle = value;
            }
        }

        public float Interval
        {
            get
            {
                return m_Interval;
            }

            set
            {
                m_Interval = value;
            }
        }

        public int Generation
        {
            get
            {
                return m_Generation;
            }

            set
            {
                m_Generation = value;
            }
        }

        public string BundleName
        {
            get
            {
                return m_BundleName;
            }

            set
            {
                m_BundleName = value;
            }
        }

        public bool ShouldUnload
        {
            get
            {
                return m_ShouldUnload;
            }
        }

        public void Unload(bool immediately)
        {
            if (immediately)
            {
                Bundle.Unload(true);
            }
            else
            {
                m_ShouldUnload = true;
            }
        }
        
        public void ResetInterval()
        {
            m_Interval = m_Generation * m_Generation * BundleCollection.GC_TIME;
        }
    }
}
