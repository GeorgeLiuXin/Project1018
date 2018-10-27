/**************************************************
 *  创建人   : 夏佳文
 *  创建时间 : 2018.6.21
 *  说明     : build过程中的临时数据，也可用作事后分析
 * ************************************************/
using System.Collections.Generic;

namespace Galaxy.AssetPipeline
{
    internal class BuilderTempData
    {
        #region origin
        // Fullpath - Asset
        protected Dictionary<string, CrudeAssetNode> m_ChangedAssetMap = new Dictionary<string, CrudeAssetNode>();
        // Fullpath - Asset
        protected Dictionary<string, CrudeAssetNode> m_AddAssetMap = new Dictionary<string, CrudeAssetNode>();
        // Fullpath - Asset
        protected Dictionary<string, CrudeAssetNode> m_DeleteAssetMap = new Dictionary<string, CrudeAssetNode>();
        // RefinedRelativePath - Assets
        protected Dictionary<string, List<CrudeAssetNode>> m_addRefinedAssetListMap = new Dictionary<string, List<CrudeAssetNode>>();

        internal Dictionary<string, CrudeAssetNode> ChangedAssetMap
        {
            get
            {
                return m_ChangedAssetMap;
            }

            set
            {
                m_ChangedAssetMap = value;
            }
        }

        internal Dictionary<string, CrudeAssetNode> AddAssetMap
        {
            get
            {
                return m_AddAssetMap;
            }

            set
            {
                m_AddAssetMap = value;
            }
        }

        internal Dictionary<string, CrudeAssetNode> DeleteAssetMap
        {
            get
            {
                return m_DeleteAssetMap;
            }

            set
            {
                m_DeleteAssetMap = value;
            }
        }
        #endregion origin

        #region afterHandle
        private List<string> m_AddedAssetPathsAfterHandle;
        public List<string> AddedAssetPathsAfterHandle
        {
            get
            {
                return m_AddedAssetPathsAfterHandle;
            }

            set
            {
                m_AddedAssetPathsAfterHandle = value;
            }
        }

        public Dictionary<string, List<CrudeAssetNode>> AddRefinedAssetListMap
        {
            get
            {
                return m_addRefinedAssetListMap;
            }

            set
            {
                m_addRefinedAssetListMap = value;
            }
        }
        #endregion
    }
}
