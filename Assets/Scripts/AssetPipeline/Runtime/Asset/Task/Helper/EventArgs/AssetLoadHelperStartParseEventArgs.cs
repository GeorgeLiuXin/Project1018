using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public class AssetLoadHelperStartParseEventArgs : BaseEventArgs
    {
        public AssetLoadHelperStartParseEventArgs(string assetPath) : base()
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
