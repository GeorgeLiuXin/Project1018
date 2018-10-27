using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld.AssetPipeline
{
    public class AssetLoadTask : ITask
    {
        const string SCENE_EXTENSION = ".unity";
        private EventHandler<AssetLoadSuccessEventArgs> m_AssetLoadSuccess;
        private EventHandler<AssetLoadFailureEventArgs> m_AssetLoadFail;
        private EventHandler<AssetLoadUpdateEventArgs> m_AssetLoadUpdate;

        private bool m_Done;

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

        public bool Done
        {
            get
            {
                return m_Done;
            }
            set
            {
                m_Done = value;
            }
        }

        public int SerialId
        {
            get
            {
                return GetHashCode();
            }
        }

        public string AssetPath
        {
            get
            {
                return m_AssetPath;
            }
        }

        public EAssetLoadTaskStatus Status
        {
            get
            {
                return m_LoadStatus;
            }

            set
            {
                m_LoadStatus = value;
            }
        }

        public int Priority
        {
            get
            {
                return m_Priority;
            }

            set
            {
                m_Priority = value;
            }
        }

        public string BundlePath
        {
            get
            {
                return m_BundlePath;
            }
        }

        public void Update(float deltaTime)
        {
            if (!m_Done)
            {
                if (m_AssetBundleRequest != null)
                {
                    float progress = m_AssetBundleRequest.progress;

                    //progress = progress >= 1 ? 0.99f : progress;
                    m_AssetLoadUpdate.Invoke(this, new AssetLoadUpdateEventArgs(m_AssetPath, progress));
                }

                if (m_AssetBundleRequest.isDone)
                {
                    m_Done = true;
                    m_Asset = m_AssetBundleRequest.asset;
                }
            }
        }

        public void Init(string assetPath, string bundlePath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                throw new Exception("assetName is Invalid");
            }

            m_AssetPath = assetPath;
            m_BundlePath = bundlePath;
            m_IsScene = this.m_AssetPath.EndsWith(SCENE_EXTENSION);
        }

        private UnityEngine.Object m_Asset;
        private bool m_IsScene;
        private string m_AssetPath;
        private string m_BundlePath;
        private AssetBundleRequest m_AssetBundleRequest;
        private int m_Priority;
        private EAssetLoadTaskStatus m_LoadStatus;
    }
}