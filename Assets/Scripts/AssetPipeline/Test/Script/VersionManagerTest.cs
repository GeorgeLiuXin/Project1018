using UnityEngine;
using XWorld;

namespace XWorld.Test
{
    public class VersionManagerTest : MonoBehaviour
    {
        private void Start()
        {
            XWorldGameModule.GetGameManager<VersionManager>().InitAssetBundleMap();
        }
    }
}
