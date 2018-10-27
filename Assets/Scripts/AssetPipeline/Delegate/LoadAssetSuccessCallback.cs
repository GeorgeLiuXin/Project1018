
namespace XWorld
{
    public delegate void LoadAssetCallback(LoadResult result);
}

public class LoadResult
{
    public bool isSuccess = true;
    public string[] assetNames;
    public UnityEngine.Object[] assets;
    public string[] errorAssetNames;
    public object userData;
}

