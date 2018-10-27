using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public class AssetLoadStartEventArgs : BaseEventArgs
    {
        public AssetLoadStartEventArgs(string assetPath) : base()
        {
            this.AssetPath = assetPath;
        }

        public string AssetPath
        {
            get;
            private set;
        }
    }
}
