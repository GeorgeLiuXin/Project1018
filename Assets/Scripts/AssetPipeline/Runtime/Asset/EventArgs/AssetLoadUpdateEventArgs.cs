using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public class AssetLoadUpdateEventArgs : BaseEventArgs
    {
        public AssetLoadUpdateEventArgs(string assetPath, float progress) : base()
        {
            this.AssetPath = assetPath;
            this.Progress = progress;
        }

        public string AssetPath
        {
            get;
            private set;
        }

        public float Progress
        {
            get;
            private set;
        }
    }
}
