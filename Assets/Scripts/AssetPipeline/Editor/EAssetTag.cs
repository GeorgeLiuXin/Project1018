/**************************************************
 *  创建人   : 夏佳文
 *  创建时间 : 2018.6.19
 *  说明     : Asset的特殊标识，默认是正常逻辑
 *             统计时优先处理AssetTag逻辑
 *             不同Tag标识的统计算法不同,数值高的Tag优先级高
 * ************************************************/

namespace Galaxy.AssetPipeline
{

    /// <summary>
    /// Asset的特殊标识，默认是正常逻辑
    /// 统计时优先处理AssetTag逻辑
    /// 不同Tag标识的统计算法不同,数值高的Tag优先级高
    /// </summary>
    internal enum EAssetTag
    {
        Unkown,

        [AssetBuild("Atlas")]
        Atlas,
        Normal,
    }
}
