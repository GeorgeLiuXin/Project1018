using UnityEngine;
using XWorld;

public class ResourceManagerTest
{
    public static string path = "Prefabs/Cube.prefab";
    public static void LoadTest()
    {
      //   ResourcesMgr.Instance.LoadAsset(path, Result);
    }

    private static void Result(LoadResult result)
    {
        if (result.isSuccess)
        {
            GameObject.Instantiate(result.assets[0]);
        }
    }
}
