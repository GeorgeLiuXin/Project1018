using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public class AssetLoadHelperSuccessEventArgs : BaseEventArgs
    {
        public AssetLoadHelperSuccessEventArgs(string assetPath, UnityEngine.Object asset) : base()
        {
            this.Asset = asset;
            this.AssetPath = assetPath;
        }

        public UnityEngine.Object Asset
        {
            get;
            private set;
        }

        public string AssetPath
        {
            get;
            private set;
        }
    }
}
