
namespace Galaxy.AssetPipeline
{
    internal interface IOutputData
    {
        void ToString();

        RefinedAssetNode[] GetDeletedAsset();
        RefinedAssetNode[] GetChangedAsset();
        RefinedAssetNode[] GetAddedAsset();

        long GetDeletedLength();
        long GetChangedLength();
        long GetAddedLength();
        long GetMergedLength();

        long GetFinalChangedLength();
    }
}
