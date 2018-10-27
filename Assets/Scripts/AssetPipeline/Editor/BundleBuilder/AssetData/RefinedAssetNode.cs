/**************************************************
 *  创建人   : 夏佳文
 *  创建时间 : 2018.6.19
 *  说明     : 整理后的Asset信息，最后生成打包的依据
 * ************************************************/

using System.Collections.Generic;

namespace Galaxy.AssetPipeline
{
    [System.Serializable]
    internal class RefinedAssetNode
    {
        public RefinedAssetNode() { }

        public RefinedAssetNode(CrudeAssetNode crudeNode)
        {
            this.m_OriginAsset = crudeNode;
        }

        private CrudeAssetNode m_OriginAsset;
        private string m_BundleName;
        private List<VersionInfo> m_ChangedVersion = new List<VersionInfo>();
        private bool m_IsDEMO;

        internal CrudeAssetNode CrudeNode
        {
            get
            {
                return m_OriginAsset;
            }

            set
            {
                m_OriginAsset = value;
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

        internal List<VersionInfo> ChangedVersion
        {
            get
            {
                return m_ChangedVersion;
            }

            set
            {
                m_ChangedVersion = value;
            }
        }

        public bool IsDEMO
        {
            get
            {
                return m_IsDEMO;
            }

            set
            {
                m_IsDEMO = value;
            }
        }
    }

}
