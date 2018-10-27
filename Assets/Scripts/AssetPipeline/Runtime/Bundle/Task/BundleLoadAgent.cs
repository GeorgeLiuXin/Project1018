using System;
using UnityEngine;

namespace XWorld.AssetPipeline
{
    public class BundleLoadAgent : ITaskAgent<BundleLoadTask>
    {
        public AssetPipelineAction<BundleLoadAgent> BundleLoadAgentStart;
        public AssetPipelineAction<BundleLoadAgent, float> BundleLoadAgentUpdate;
        public AssetPipelineAction<BundleLoadAgent, AssetBundle> BundleLoadAgentSuccess;
        public AssetPipelineAction<BundleLoadAgent, string> BundleLoadAgentFailure;

        private IBundleLoadHelper m_Helper;
        private BundleLoadTask m_Task;

        private float m_Progress;
        public BundleLoadTask Task
        {
            get
            {
                return m_Task;
            }
        }

        public BundleLoadAgent(IBundleLoadHelper bundleLoadHelper)
        {
            if (bundleLoadHelper == null)
            {
                throw new Exception("bundleLoadHelper is NULL");
            }
            m_Helper = bundleLoadHelper;

            BundleLoadAgentStart = null;
            BundleLoadAgentUpdate = null;
        }

        public void Initialize()
        {
            m_Helper.BundleLoadAgentHelperSuccess += BundleLoadAgentHelperSuccess;
            m_Helper.BundleLoadAgentHelperFail += BundleLoadAgentHelperFailure;
            m_Helper.BundleLoadAgentHelperUpdate += BundleLoadAgentHelperUpdate;
            m_Helper.IsShutdown = false;
        }

        private void BundleLoadAgentHelperUpdate(object sender, BundleLoadHelperUpdateEventArgs e)
        {
            if (BundleLoadAgentUpdate != null)
            {
                BundleLoadAgentUpdate(this, e.Progress);
            }
        }

        private void BundleLoadAgentHelperFailure(object sender, BundleLoadHelperFailureEventArgs e)
        {
            m_Task.Status = EBundleLoadTaskStatus.Error;

            if (BundleLoadAgentFailure != null)
            {
                BundleLoadAgentFailure(this, e.ErrorContent);
            }

            m_Task.Done = true;
        }

        private void BundleLoadAgentHelperSuccess(object sender, BundleLoadHelperSuccessEventArgs e)
        {
            m_Task.Status = EBundleLoadTaskStatus.Done;

            if (BundleLoadAgentSuccess != null)
            {
                BundleLoadAgentSuccess(this, e.Bundle);
            }

            m_Task.Done = true;
        }

        public void Shutdown()
        {
            Dispose();
            m_Helper.BundleLoadAgentHelperSuccess -= BundleLoadAgentHelperSuccess;
            m_Helper.BundleLoadAgentHelperFail -= BundleLoadAgentHelperFailure;
            m_Helper.BundleLoadAgentHelperUpdate -= BundleLoadAgentHelperUpdate;

            m_Helper.IsShutdown = true;
        }

        public void Start(BundleLoadTask task)
        {
            m_Task = task;
            m_Helper.Load(task.BundlePath, task.FilePath, task.CRC);
        }

        public void Update(float deltaTime)
        {

        }

        public void Reset()
        {

        }

        private void Dispose()
        {

        }
    }
}
