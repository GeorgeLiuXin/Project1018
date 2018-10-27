
using System;

namespace Galaxy.AssetPipeline
{
    internal class DefaultOutputData : BuilderTempData, IOutputData
    {
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
