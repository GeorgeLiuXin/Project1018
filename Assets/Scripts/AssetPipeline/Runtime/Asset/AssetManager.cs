using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XWorld.AssetPipeline
{
    public class AssetManager : XWorldGameManagerBase
    {
        public static int s_MaxLoadCount = 8;

        private List<IAssetLoadHelper> m_HelperQueue;
        private bool m_ShouldFireReference = false;

#if UNITY_EDITOR
        private bool m_LastEnableEditor = false;
        public AssetPipelineAction OnReInitManger;
#endif

        private EventHandler<AssetLoadSuccessEventArgs> m_AssetLoadSuccess;
        private EventHandler<AssetLoadFailureEventArgs> m_AssetLoadFail;
        private EventHandler<AssetLoadUpdateEventArgs> m_AssetLoadUpdate;
        private EventHandler<AssetLoadStartEventArgs> m_AssetLoadStart;

        public EventHandler<AssetLoadSuccessEventArgs> AssetLoadSuccess
        {
            get
            {
                return m_AssetLoadSuccess;
            }

            set
            {
                m_AssetLoadSuccess = value;
            }
        }

        public EventHandler<AssetLoadFailureEventArgs> AssetLoadFail
        {
            get
            {
                return m_AssetLoadFail;
            }

            set
            {
                m_AssetLoadFail = value;
            }
        }

        public EventHandler<AssetLoadUpdateEventArgs> AssetLoadUpdate
        {
            get
            {
                return m_AssetLoadUpdate;
            }

            set
            {
                m_AssetLoadUpdate = value;
            }
        }

        public EventHandler<AssetLoadStartEventArgs> AssetLoadStart
        {
            get
            {
                return m_AssetLoadStart;
            }

            set
            {
                m_AssetLoadStart = value;
            }
        }

        public AssetRefCache Cache
        {
            get
            {
                return m_Cache;
            }
        }

        private AssetRefCache m_Cache;
        private TaskPool<AssetLoadTask> m_TaskPool;
        public void StartLoad(string assetPath, object receiver, int priority)
        {
#if UNITY_EDITOR
            // 加此是为了防止编辑器提前创建了Manager
            if (m_LastEnableEditor != GlobalAssetSetting.EnableEditorSimulation)
            {
                InitManager();
                if (OnReInitManger != null)
                {
                    OnReInitManger();
                }
            }
#endif

            if (string.IsNullOrEmpty(assetPath))
            {
                string errorContent = "assetPath is invalid.";
                GameLogger.Error(LOG_CHANNEL.LOGIC, errorContent);
                if (m_AssetLoadFail != null)
                {
                    m_AssetLoadFail.Invoke(this, new AssetLoadFailureEventArgs(assetPath, errorContent));
                }
                return;
            }

            AssetReference assetRef = InitAssetReference(assetPath, receiver, priority);
            if (assetRef != null)
            {
                assetRef.Retain(receiver);
                m_Cache.AddWaiting(assetPath, priority);
            }
            m_ShouldFireReference = true;
        }

        public void Unload(string filePath, object receiver, bool destroyImmediately)
        {
            if (m_Cache.ContainsKey(filePath))
            {
                AssetReference reference = m_Cache[filePath];
                reference.Release(receiver, destroyImmediately);

                m_Cache.DepTryRemove(filePath);
            }
        }

        public int GetRefCount(string filePath)
        {
            if (m_Cache.ContainsKey(filePath))
            {
                AssetReference reference = m_Cache[filePath];
                return reference.RefCount;
            }
            return -1;
        }

        private AssetReference InitAssetReference(string assetPath, object receiver, int priority)
        {
            AssetReference reference = null;
            if (m_Cache.ContainsKey(assetPath))
            {
                reference = m_Cache[assetPath];
            }
            else
            {
                string bundlePath = XWorldGameModule.GetGameManager<VersionManager>().GetAssetBundlePath(assetPath);
                if (string.IsNullOrEmpty(bundlePath) && !GlobalAssetSetting.EnableEditorSimulation)
                {
                    if (m_AssetLoadFail != null)
                    {
                        GameLogger.Warning(LOG_CHANNEL.LOGIC, "Invalid bundlePath by FilePath " + assetPath);
                        m_AssetLoadFail.Invoke(this, new AssetLoadFailureEventArgs(assetPath, "Invalid bundlePath by FilePath " + assetPath));
                    }
                    return reference;
                }

                reference = new AssetReference(m_Cache);
                reference.Init(assetPath, bundlePath, receiver, priority);
                m_Cache.Add(assetPath, reference);

                AssetLoadTask task = new AssetLoadTask();
                task.Init(assetPath, bundlePath);
                m_TaskPool.AddTask(task, priority);

                if (m_AssetLoadStart != null)
                {
                    m_AssetLoadStart.Invoke(this, new AssetLoadStartEventArgs(assetPath));
                }
            }
            return reference;
        }
        
        private void AssetLoadAgentUpdate(AssetLoadAgent arg0, float arg1)
        {
            AssetLoadTask task = arg0.Task;
            if (task == null)
            {
                return;
            }

            string assetPath = task.AssetPath;
            if (m_Cache.ContainsKey(assetPath))
            {
                m_Cache[assetPath].Progress = arg1;
            }
        }

        private void AssetLoadAgentSuccess(AssetLoadAgent arg0, UnityEngine.Object arg1)
        {
            AssetLoadTask task = arg0.Task;
            if (task == null)
            {
                return;
            }

            if (m_Cache.ContainsKey(task.AssetPath))
            {
                AssetReference reference = m_Cache[task.AssetPath];

                reference.OriginAsset = arg1;
                reference.Progress = 1;

                if (!reference.Done)
                {
                    GameLogger.Error(LOG_CHANNEL.LOGIC, task.AssetPath + " is Success but asset is null");
                    return;
                }
                m_ShouldFireReference = true;
            }
        }

        private void AssetLoadAgentFailure(AssetLoadAgent arg0, string arg1)
        {
            AssetLoadTask task = arg0.Task;
            if (task == null)
            {
                return;
            }

            string assetPath = task.AssetPath;
            if (m_Cache.ContainsKey(assetPath))
            {
                m_Cache[assetPath].AddError(arg1);
            }
            m_ShouldFireReference = true;
        }
        
        private void AssetLoadAgentStartParse(AssetLoadAgent obj)
        {
            AssetLoadTask task = obj.Task;
            if (task == null)
            {
                return;
            }
            string assetPath = task.AssetPath;
            if (m_Cache.ContainsKey(assetPath))
            {
                m_Cache[assetPath].BundlePrepared = true;
            }
        }

        private void FireWaitingReference()
        {
            AssetReference[] failRefs = null;
            AssetReference[] successRefs = m_Cache.FireWaitingList(out failRefs);
            if (successRefs.Length != 0)
            {
                int length = successRefs.Length;
                for (int i = 0; i < length; i++)
                {
                    AssetReference r = successRefs[i];
                    if (m_AssetLoadSuccess != null)
                    {
                        m_AssetLoadSuccess.Invoke(this, new AssetLoadSuccessEventArgs(r.AssetPath, r.OriginAsset, r.IsScene));
                    }
                }
            }


            if (failRefs.Length != 0)
            {
                int length = failRefs.Length;
                for (int i = 0; i < length; i++)
                {
                    AssetReference r = failRefs[i];
                    if (m_AssetLoadSuccess != null)
                    {
                        m_AssetLoadFail.Invoke(this, new AssetLoadFailureEventArgs(r.AssetPath, string.Join("\n", r.ErrorNodes.ToArray())));
                    }
                }
            }
        }

        public override void InitManager()
        {
            m_Cache = new AssetRefCache();
            m_TaskPool = new TaskPool<AssetLoadTask>();
            m_HelperQueue = new List<IAssetLoadHelper>();

            for (int i = 0; i < s_MaxLoadCount; i++)
            {
                IAssetLoadHelper helper = null;
#if UNITY_EDITOR
                if (GlobalAssetSetting.EnableEditorSimulation)
                {
                    helper = new EditorAssetLoadHelper();
                    m_HelperQueue.Add(helper);
                }
                else
                {
                    helper = new BundleAssetLoadHelper();
                    m_HelperQueue.Add(helper);
                }
                m_LastEnableEditor = GlobalAssetSetting.EnableEditorSimulation;
#else
                helper = new BundleAssetLoadHelper();
                m_HelperQueue.Add(helper);
#endif

                AssetLoadAgent agent = new AssetLoadAgent(helper);
                agent.AssetLoadAgentFailure += AssetLoadAgentFailure;
                agent.AssetLoadAgentSuccess += AssetLoadAgentSuccess;
                agent.AssetLoadAgentUpdate += AssetLoadAgentUpdate;
                agent.AssetLoadAgentStartParse += AssetLoadAgentStartParse;
                m_TaskPool.AddAgent(agent);
            }
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

            if (m_ShouldFireReference)
            {
                FireWaitingReference();
                m_ShouldFireReference = false;
            }
        }

        public override void ShutDown()
        {
            m_Cache.Shutdown();
            m_TaskPool.Shutdown();
        }
    }
}
