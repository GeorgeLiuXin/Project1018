using System;
using UnityEngine;

namespace XWorld.AssetPipeline
{
    public class BundleAssetLoadHelper : IAssetLoadHelper
    {
        const string SCENES_EXTENSION = ".unity";
        private bool m_IsShutdown = false;
        private bool m_IsBundleLoad = false;
        private bool m_BindBundleEvent = false;
        public event EventHandler<AssetLoadHelperFailureEventArgs> AssetLoadAgentHelperFailure;
        public event EventHandler<AssetLoadHelperSuccessEventArgs> AssetLoadAgentHelperSuccess;
        public event EventHandler<AssetLoadHelperUpdateEventArgs> AssetLoadAgentHelperUpdate;
        public event EventHandler<AssetLoadHelperStartParseEventArgs> AssetLoadAgentHelperStartParse;
        
        public bool IsShutdown
        {
            get
            {
                return m_IsShutdown;
            }

            set
            {
                m_IsShutdown = value;
            }
        }


        public void Load(string assetPath, int priotity)
        {
            m_Done = false;
            m_AssetPath = assetPath;
            m_IsScene = assetPath.EndsWith(SCENES_EXTENSION);
            m_BundlePath = XWorldGameModule.GetGameManager<VersionManager>().GetAssetBundlePath(m_AssetPath);
            m_IsShutdown = false;
            m_IsBundleLoad = false;

            BundleManager bundleManager = XWorldGameModule.GetGameManager<BundleManager>();
            if (!m_BindBundleEvent)
            {
                bundleManager.BundleLoadFail += BundleLoadFail;
                bundleManager.BundleLoadStart += BundleLoadStart;
                bundleManager.BundleLoadSuccess += BundleLoadSuccess;
                bundleManager.BundleLoadUpdate += BundleLoadUpdate;
            }
            bundleManager.StartLoad(m_BundlePath, this, priotity);
        }

        private void BundleLoadUpdate(object sender, BundleLoadUpdateEventArgs e)
        {
            if (!m_Done && e.BundlePath == this.m_BundlePath)
            {
                m_Progress = e.Progress * 0.5f;
                if (AssetLoadAgentHelperUpdate != null)
                {
                    AssetLoadAgentHelperUpdate.Invoke(this, new AssetLoadHelperUpdateEventArgs(m_AssetPath, m_Progress));
                }
            }
        }

        private void BundleLoadFail(object sender, BundleLoadFailEventArgs e)
        {
            if (!m_Done && e.BundlePath == this.m_BundlePath)
            {
                if (AssetLoadAgentHelperFailure != null)
                {
                    AssetLoadAgentHelperFailure.Invoke(this, new AssetLoadHelperFailureEventArgs(m_AssetPath, e.ErrorContent));
                }
            }
        }

        private void BundleLoadSuccess(object sender, BundleLoadSuccessEventArgs e)
        {
            if (!m_Done && !m_IsBundleLoad && e.BundlePath == this.m_BundlePath)
            {
                if (AssetLoadAgentHelperStartParse != null)
                {
                    AssetLoadAgentHelperStartParse.Invoke(this, new AssetLoadHelperStartParseEventArgs(m_AssetPath));
                }

                m_IsBundleLoad = true;

                if (!m_IsScene)
                {
                    m_AssetBundleRequest = e.Bundle.LoadAssetAsync(m_AssetPath);
                }
                else {
                    
                    if (AssetLoadAgentHelperSuccess != null)
                    {
                        AssetLoadAgentHelperSuccess.Invoke(this, new AssetLoadHelperSuccessEventArgs(m_AssetPath, m_Asset));
                    }
                    
                    m_Done = true;
                    m_AssetBundleRequest = null;
                    m_Asset = null;
                }
            }
        }

        private void BundleLoadStart(object sender, BundleLoadStartEventArgs e)
        {
            
        }

        public void Update(float deltaTime)
        {
            if (!m_Done && m_IsBundleLoad)
            {
                if (m_AssetBundleRequest != null)
                {
                    float progress = m_AssetBundleRequest.progress;
                    m_Progress = 0.5f + progress * 0.5f;
                    if (AssetLoadAgentHelperUpdate != null)
                    {
                        AssetLoadAgentHelperUpdate.Invoke(this, new AssetLoadHelperUpdateEventArgs(m_AssetPath, m_Progress));
                    }

                    if (m_AssetBundleRequest.isDone)
                    {
                        m_Done = true;
                        m_Asset = m_AssetBundleRequest.asset;

                        if (m_Asset != null)
                        {
                            if (AssetLoadAgentHelperSuccess != null)
                            {
                                AssetLoadAgentHelperSuccess.Invoke(this, new AssetLoadHelperSuccessEventArgs(m_AssetPath, m_Asset));
                            }
                        }
                        else
                        {
                            if (AssetLoadAgentHelperFailure != null)
                            {
                                AssetLoadAgentHelperFailure.Invoke(this, new AssetPipeline.AssetLoadHelperFailureEventArgs(m_AssetPath,
                                    string.Format("Bundle[{0}] has no asset[{1}].", m_BundlePath, m_AssetPath)));
                            }
                        }

                        m_AssetBundleRequest = null;
                        m_Asset = null;
                    }
                }
            }
        }

        ~BundleAssetLoadHelper()
        {
            BundleManager bundleManager = XWorldGameModule.GetGameManager<BundleManager>();
            bundleManager.BundleLoadFail -= BundleLoadFail;
            bundleManager.BundleLoadStart -= BundleLoadStart;
            bundleManager.BundleLoadSuccess -= BundleLoadSuccess;
            bundleManager.BundleLoadUpdate -= BundleLoadUpdate;
        }

        private bool m_Done;
        private UnityEngine.Object m_Asset;
        private bool m_IsScene;
        private string m_AssetPath;
        private string m_BundlePath;
        private AssetBundleRequest m_AssetBundleRequest;
        private float m_Progress;

    }
}
