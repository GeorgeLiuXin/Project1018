
using System;
using System.Collections.Generic;

namespace Galaxy.AssetPipeline
{
    internal class AtlasOutputData : BuilderTempData, IOutputData
    {
        private Dictionary<string, LinkedList<CrudeAssetNode>> m_AtlasCrudeAssetNodeMap = new Dictionary<string, LinkedList<CrudeAssetNode>>();

        internal Dictionary<string, LinkedList<CrudeAssetNode>> AtlasCrudeAssetNodeMap
        {
            get
            {
                return m_AtlasCrudeAssetNodeMap;
            }

            set
            {
                m_AtlasCrudeAssetNodeMap = value;
            }
        }

        public RefinedAssetNode[] GetAddedAsset()
        {
            throw new NotImplementedException();
        }

        public long GetAddedLength()
        {
            throw new NotImplementedException();
        }

        public RefinedAssetNode[] GetChangedAsset()
        {
            throw new NotImplementedException();
        }

        public long GetChangedLength()
        {
            throw new NotImplementedException();
        }

        public RefinedAssetNode[] GetDeletedAsset()
        {
            throw new NotImplementedException();
        }

        public long GetDeletedLength()
        {
            throw new NotImplementedException();
        }

        public long GetFinalChangedLength()
        {
            throw new NotImplementedException();
        }

        public long GetMergedLength()
        {
            throw new NotImplementedException();
        }

        void IOutputData.ToString()
        {
            throw new NotImplementedException();
        }
    }
}
