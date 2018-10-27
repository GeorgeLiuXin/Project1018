using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public class AssetLoadAgent : ITaskAgent<AssetLoadTask>
    {
        public AssetPipelineAction<AssetLoadAgent> AssetLoadAgentStart;
        public AssetPipelineAction<AssetLoadAgent, float> AssetLoadAgentUpdate;
        public AssetPipelineAction<AssetLoadAgent, UnityEngine.Object> AssetLoadAgentSuccess;
        public AssetPipelineAction<AssetLoadAgent, string> AssetLoadAgentFailure;
        public AssetPipelineAction<AssetLoadAgent> AssetLoadAgentStartParse;

        private IAssetLoadHelper m_Helper;
        private AssetLoadTask m_Task;

        private float m_Progress;
        public AssetLoadTask Task
        {
            get
            {
                return m_Task;
            }
        }

        public AssetLoadAgent(IAssetLoadHelper assetLoadHelper)
        {
            if (assetLoadHelper == null)
            {
                throw new Exception("assetLoadHelper is NULL");
            }
            m_Helper = assetLoadHelper;

            AssetLoadAgentStart = null;
            AssetLoadAgentUpdate = null;
        }

        public void Initialize()
        {
            m_Helper.AssetLoadAgentHelperSuccess += AssetLoadAgentHelperSuccess;
            m_Helper.AssetLoadAgentHelperFailure += AssetLoadAgentHelperFailure;
            m_Helper.AssetLoadAgentHelperUpdate += AssetLoadAgentHelperUpdate;
            m_Helper.AssetLoadAgentHelperStartParse += AssetLoadAgentHelperStartParse;
            m_Helper.IsShutdown = false;
        }

        private void AssetLoadAgentHelperStartParse(object sender, AssetLoadHelperStartParseEventArgs e)
        {
            m_Task.Status = EAssetLoadTaskStatus.ParseBundle;

            if (AssetLoadAgentStartParse != null)
            {
                AssetLoadAgentStartParse(this);
            }
        }

        private void AssetLoadAgentHelperUpdate(object sender, AssetLoadHelperUpdateEventArgs e)
        {
            if (AssetLoadAgentUpdate != null)
            {
                AssetLoadAgentUpdate(this, e.Progress);
            }
        }

        private void AssetLoadAgentHelperFailure(object sender, AssetLoadHelperFailureEventArgs e)
        {
            m_Task.Status = EAssetLoadTaskStatus.Error;

            if (AssetLoadAgentFailure != null)
            {
                AssetLoadAgentFailure(this, e.ErrorContent);
            }

            m_Task.Done = true;
        }

        private void AssetLoadAgentHelperSuccess(object sender, AssetLoadHelperSuccessEventArgs e)
        {
            m_Task.Status = EAssetLoadTaskStatus.Done;

            if (AssetLoadAgentSuccess != null)
            {
                AssetLoadAgentSuccess(this, e.Asset);
            }

            m_Task.Done = true;
        }

        public void Shutdown()
        {
            Dispose();
            m_Helper.AssetLoadAgentHelperSuccess -= AssetLoadAgentHelperSuccess;
            m_Helper.AssetLoadAgentHelperFailure -= AssetLoadAgentHelperFailure;
            m_Helper.AssetLoadAgentHelperUpdate -= AssetLoadAgentHelperUpdate;
            m_Helper.AssetLoadAgentHelperStartParse -= AssetLoadAgentHelperStartParse;
            m_Helper.IsShutdown = true;
        }

        public void Start(AssetLoadTask task)
        {
            m_Task = task;
            m_Helper.Load(task.AssetPath, task.Priority);
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
