/**************************************************
 *  创建人   : 夏佳文
 *  创建时间 : 2018.6.27
 *  说明     : 自定义AB信息,替代Unity系统中使用AssetBundleName定位的功能
 * ************************************************/

namespace Galaxy.AssetPipeline
{
    internal partial class CustomAssetDataSource : IABDataSource
    {
        private AssetBundleBuildMap m_buildAssetBundleMap = new AssetBundleBuildMap();

        public string GetAssetBundleName(string assetPath)
        {
            return m_buildAssetBundleMap.GetAssetBundleName(assetPath);
        }

        public string GetImplicitAssetBundleName(string assetPath)
        {
            // 暂时不能理解
            return m_buildAssetBundleMap.GetAssetBundleName(assetPath);
        }

        public string[] GetAllAssetBundleNames()
        {
            return m_buildAssetBundleMap.GetAllAssetBundleNames();
        }


        public void SetAssetBundleNameAndVariant(string assetPath, string bundleName, string variantName)
        {
            m_buildAssetBundleMap.SetAssetBundleNameAndVariant(assetPath, bundleName, variantName);
        }

        public string[] GetAssetPathsFromAssetBundle(string assetBundleName)
        {
            return m_buildAssetBundleMap.GetAssetPathsFromAssetBundle(assetBundleName);
        }

        public UnityEditor.AssetBundleBuild[] GetAssetBundleBuilds()
        {
            return m_buildAssetBundleMap.GetBuilders();
        }
    }
}
