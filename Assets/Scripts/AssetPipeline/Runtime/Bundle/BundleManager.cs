using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XWorld.AssetPipeline
{
    public class BundleManager : XWorldGameManagerBase
    {
#if UNITY_IOS
        public static int s_MaxLoadCount = 4;
#else
        public static int s_MaxLoadCount = 8;
#endif

        private EventHandler<BundleLoadSuccessEventArgs> m_BundleLoadSuccess;
        private EventHandler<BundleLoadFailEventArgs> m_BundleLoadFailure;
        private EventHandler<BundleLoadUpdateEventArgs> m_BundleLoadUpdate;
        private EventHandler<BundleLoadStartEventArgs> m_BundleLoadStart;

        private VersionManager m_VersionManager;
        private BundleRefCache m_ABCache;
        private BundleCollection m_ABCollection;

        private TaskPool<BundleLoadTask> m_TaskPool;
        private List<AssetBundleLoadHelper> m_HelperQueue;
        private bool m_ShouldFireReference = false;
        public EventHandler<BundleLoadSuccessEventArgs> BundleLoadSuccess
        {
            get
            {
                return m_BundleLoadSuccess;
            }

            set
            {
                m_BundleLoadSuccess = value;
            }
        }

        public EventHandler<BundleLoadFailEventArgs> BundleLoadFail
        {
            get
            {
                return m_BundleLoadFailure;
            }

            set
            {
                m_BundleLoadFailure = value;
            }
        }

        public EventHandler<BundleLoadUpdateEventArgs> BundleLoadUpdate
        {
            get
            {
                return m_BundleLoadUpdate;
            }

            set
            {
                m_BundleLoadUpdate = value;
            }
        }

        public EventHandler<BundleLoadStartEventArgs> BundleLoadStart
        {
            get
            {
                return m_BundleLoadStart;
            }

            set
            {
                m_BundleLoadStart = value;
            }
        }

        public BundleRefCache Cache
        {
            get
            {
                return m_ABCache;
            }
        }

        public BundleCollection ABCollection
        {
            get
            {
                return m_ABCollection;
            }
        }

        public void StartLoad(string bundlePath, object receiver, int priority)
        {
            if (string.IsNullOrEmpty(bundlePath))
            {
                string errorContent = "bundlePath is invalid.";
                GameLogger.Error(LOG_CHANNEL.LOGIC, errorContent);
                if (m_BundleLoadFailure != null)
                {
                    m_BundleLoadFailure.Invoke(this, new BundleLoadFailEventArgs(bundlePath, errorContent));
                }
                return;
            }

            BundleReference bundleRef = InitBundleRefence(bundlePath, receiver, priority);
            if (bundleRef != null)
            {
                bundleRef.Retain(receiver);
                m_ABCache.AddWaiting(bundlePath, priority);
            }
            m_ShouldFireReference = true;
        }

        public void Unload(string bundlePath, object reciever, bool destroyImmediately)
        {
            if (m_ABCache.ContainsKey(bundlePath))
            {
                BundleReference reference = m_ABCache[bundlePath];
                reference.Release(reciever, destroyImmediately);

                m_ABCache.DepTryRemove(bundlePath);
            }
        }

        public void ForceUnload(string bundlePath, object reciever, bool destroyImmediately)
        {
            if (m_ABCache.ContainsKey(bundlePath))
            {
                BundleReference reference = m_ABCache[bundlePath];
                reference.ForceRelease(reciever, destroyImmediately);

                m_ABCache.DepTryRemove(bundlePath);
            }
        }

        public void Collect()
        {
            m_ABCollection.Collect();
        }

        private BundleReference InitBundleRefence(string bundlePath, object receiver, int priority)
        {
            BundleReference reference = null;
            if (m_ABCache.ContainsKey(bundlePath))
            {
                reference = m_ABCache[bundlePath];
                string[] dependences = XWorldGameModule.GetGameManager<VersionManager>().GetDirectDependenciesPath(bundlePath);
                if (dependences != null && dependences.Length > 0)
                {
                    int length = dependences.Length;
                    for (int i = 0; i < length; i++)
                    {
                        InitBundleRefence(dependences[i], receiver, priority);
                    }
                }
            }
            else
            {
                uint CRC = XWorldGameModule.GetGameManager<VersionManager>().GetAssetBundleCRC(bundlePath);
                string[] dependences = XWorldGameModule.GetGameManager<VersionManager>().GetDirectDependenciesPath(bundlePath);
                string filePath = XWorldGameModule.GetGameManager<VersionManager>().GetAssetBundleFilePath(bundlePath);
                if (string.IsNullOrEmpty(filePath))
                {
                    GameLogger.Error(LOG_CHANNEL.LOGIC, "bundlePath " + bundlePath + " has no filePath");
                    if (m_BundleLoadFailure != null)
                    {
                        m_BundleLoadFailure.Invoke(this, new BundleLoadFailEventArgs(bundlePath, "bundlePath " + bundlePath + " has no filePath"));
                    }
                    return reference;
                }

                if (dependences != null && dependences.Length > 0)
                {
                    int length = dependences.Length;
                    for (int i = 0; i < length; i++)
                    {
                        InitBundleRefence(dependences[i], receiver, priority);
                    }
                }

                reference = new BundleReference(m_ABCollection, m_ABCache);
                reference.Init(bundlePath, dependences, filePath, CRC, receiver);
                m_ABCache.Add(bundlePath, reference);

                AssetBundle collectedBundle = m_ABCollection.GetBundle(bundlePath);
                if (collectedBundle != null)
                {
                    reference.OriginBundle = collectedBundle;
                }
                else
                {
                    BundleLoadTask task = new BundleLoadTask();
                    task.Init(reference);
                    m_TaskPool.AddTask(task, priority);
                }

                if (m_BundleLoadStart != null)
                {
                    m_BundleLoadStart.Invoke(this, new BundleLoadStartEventArgs(bundlePath));
                }
            }

            return reference;
        }

        public override void InitManager()
        {
            m_ABCollection = new BundleCollection();
            m_ABCache = new BundleRefCache();
            m_VersionManager = XWorldGameModule.GetGameManager<VersionManager>();
            m_TaskPool = new TaskPool<BundleLoadTask>();
            m_HelperQueue = new List<AssetBundleLoadHelper>();

            for (int i = 0; i < s_MaxLoadCount; i++)
            {
                AssetBundleLoadHelper helper = new AssetBundleLoadHelper(m_ABCollection);
                m_HelperQueue.Add(helper);

                BundleLoadAgent agent = new BundleLoadAgent(helper);
                agent.BundleLoadAgentFailure += BundleLoadAgentFailure;
                agent.BundleLoadAgentSuccess += BundleLoadAgentSuccess;
                agent.BundleLoadAgentUpdate += BundleLoadAgentUpdate;
                m_TaskPool.AddAgent(agent);
            }
        }

        private void BundleLoadAgentUpdate(BundleLoadAgent arg0, float arg1)
        {
            BundleLoadTask task = arg0.Task;
            if (task == null)
            {
                return;
            }

            string bundlePath = task.BundlePath;
            if (m_ABCache.ContainsKey(bundlePath))
            {
                m_ABCache[bundlePath].Progress = arg1;
            }
        }

        private void BundleLoadAgentSuccess(BundleLoadAgent arg0, AssetBundle arg1)
        {
            BundleLoadTask task = arg0.Task;
            if (task == null)
            {
                return;
            }

            if (m_ABCache.ContainsKey(task.BundlePath))
            {
                BundleReference reference = m_ABCache[task.BundlePath];

                reference.OriginBundle = arg1;
                reference.Progress = 1;

                if (!reference.Done)
                {
                    return;
                }
                m_ShouldFireReference = true;
            }
        }

        private void BundleLoadAgentFailure(BundleLoadAgent arg0, string arg1)
        {
            BundleLoadTask task = arg0.Task;
            if (task == null)
            {
                return;
            }

            string bundlePath = task.BundlePath;
            if (m_ABCache.ContainsKey(bundlePath))
            {
                m_ABCache[bundlePath].AddError(arg1);
            }
            m_ShouldFireReference = true;
        }

        public override void Update(float fElapseTimes)
        {
            if (m_TaskPool != null)
            {
                m_TaskPool.Update(fElapseTimes);
            }

            if (m_HelperQueue != null)
            {
                for (int i = 0; i < m_HelperQueue.Count; i++)
                {
                    m_HelperQueue[i].Update(fElapseTimes);
                }
            }

            if (m_ABCollection != null)
            {
                m_ABCollection.Update(fElapseTimes);
            }

            if (m_ShouldFireReference)
            {
                FireWaitingReference();
                m_ShouldFireReference = false;
            }
        }

        public override void OnReEnterGame()
        {
            base.OnReEnterGame();
            m_BundleLoadSuccess = null;
            m_ABCache.Shutdown();
            m_TaskPool.Shutdown();

            for (int i = 0; i < s_MaxLoadCount; i++)
            {
                AssetBundleLoadHelper helper = null;
                if (m_HelperQueue.Count > 0)
                {
                    AssetBundleLoadHelper originHelper = m_HelperQueue[0];
                    if (originHelper.IsShutdown && !originHelper.Done)
                    {
                        m_HelperQueue.RemoveAt(0);
                        helper = new AssetBundleLoadHelper(m_ABCollection);
                        m_HelperQueue.Add(helper);
                    }
                    else
                    {
                        helper = originHelper;
                    }
                }

                BundleLoadAgent agent = new BundleLoadAgent(helper);
                agent.BundleLoadAgentFailure += BundleLoadAgentFailure;
                agent.BundleLoadAgentSuccess += BundleLoadAgentSuccess;
                agent.BundleLoadAgentUpdate += BundleLoadAgentUpdate;
                m_TaskPool.AddAgent(agent);
            }
            //gc 不能参与此流程
            //helper 不能被清空
        }

        private void FireWaitingReference()
        {
            BundleReference[] failRefs = null;
            BundleReference[] successRefs = m_ABCache.FireWaitingList(out failRefs);
            if (successRefs.Length != 0)
            {
                int length = successRefs.Length;
                for (int i = 0; i < length; i++)
                {
                    BundleReference r = successRefs[i];
                    if (m_BundleLoadSuccess != null)
                    {
                        m_BundleLoadSuccess.Invoke(this, new BundleLoadSuccessEventArgs(r.BundlePath, r.OriginBundle));
                    }
                }
            }


            if (failRefs.Length != 0)
            {
                int length = failRefs.Length;
                for (int i = 0; i < length; i++)
                {
                    BundleReference r = failRefs[i];
                    if (m_BundleLoadSuccess != null)
                    {
                        m_BundleLoadFailure.Invoke(this, new BundleLoadFailEventArgs(r.BundlePath, string.Join("\n", r.ErrorNodes.ToArray())));
                    }
                }
            }

        }

        public override void ShutDown()
        {
            m_ABCache.Shutdown();
            m_TaskPool.Shutdown();
            m_ABCollection.Shutdown();
        }
    }
}
