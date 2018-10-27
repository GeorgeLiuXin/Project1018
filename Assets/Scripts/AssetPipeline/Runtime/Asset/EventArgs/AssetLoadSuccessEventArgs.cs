using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public class AssetLoadSuccessEventArgs : BaseEventArgs
    {
        public AssetLoadSuccessEventArgs(string assetPath, UnityEngine.Object asset, bool isScene) : base()
        {
            this.Asset = asset;
            this.IsScene = isScene;
            this.AssetPath = assetPath;
        }

        public UnityEngine.Object Asset
        {
            get;
            private set;
        }

        public bool IsScene
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
