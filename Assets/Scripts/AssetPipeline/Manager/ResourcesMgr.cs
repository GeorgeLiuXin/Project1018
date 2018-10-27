using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld.AssetPipeline
{
    public class ResourcesMgr : Singleton<ResourcesMgr>
    {
        public static string PersistentAssetsPath { get; set; }

        private static bool m_useBundle;

        public ResourcesMgr()
        {
            
        }
    }

}
