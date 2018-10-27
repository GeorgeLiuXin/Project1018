using Galaxy.DataNode;

namespace Galaxy.AssetPipeline
{
    internal abstract class BaseBuilderController : IBundleBuilderController
    {
        protected EAssetTag m_AssetTag;
        protected CrudeAssetNode[] m_Builds;
        protected BuilderTempData m_BuildData;
        protected DataNodeManager m_LastRecorder;
        protected DataNodeManager m_NewRecorder;
        protected bool m_IgnoreLastVersion;
        protected VersionInfo m_VersionInfo;
        protected DataNodeManager m_AssetNodeManager;
        public bool IgnoreLastVersion
        {
            get
            {
                return m_IgnoreLastVersion;
            }

            set
            {
                m_IgnoreLastVersion = value;
            }
        }

        public EAssetTag AssetTag
        {
            get
            {
                return m_AssetTag;
            }

            set
            {
                m_AssetTag = value;
            }
        }

        public BaseBuilderController(EAssetTag assetTag, CrudeAssetNode[] builds, DataNodeManager assetNodeManager, bool ignoreLastVersion,
            DataNodeManager newRecorder, DataNodeManager lastRecorder, VersionInfo versionInfo)
        {
            this.m_AssetNodeManager = assetNodeManager;
            this.m_AssetTag = assetTag;
            this.m_Builds = builds;
            this.m_IgnoreLastVersion = ignoreLastVersion;
            this.m_LastRecorder = lastRecorder;
            this.m_NewRecorder = newRecorder;
            this.m_VersionInfo = versionInfo;
        }
        
        public abstract void Prepare();

        public abstract IOutputData GetBuildData();

        public abstract void Handle();
    }
}
