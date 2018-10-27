namespace Galaxy.AssetPipeline
{
    public partial interface IABDataSource
    {
        string[] GetAssetPathsFromAssetBundle(string assetBundleName);

        string GetAssetBundleName(string assetPath);

        string GetImplicitAssetBundleName(string assetPath);

        string[] GetAllAssetBundleNames();

        void SetAssetBundleNameAndVariant(string assetPath, string bundleName, string variantName);

        UnityEditor.AssetBundleBuild[] GetAssetBundleBuilds();
    }
}
