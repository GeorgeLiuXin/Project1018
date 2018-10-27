using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public class AssetLoadFailureEventArgs : BaseEventArgs
    {
        public AssetLoadFailureEventArgs(string assetPath, string errorContent) : base()
        {
            this.AssetPath = assetPath;
            this.ErrorContent = errorContent;
        }

        public string AssetPath
        {
            get;
            private set;
        }

        public string ErrorContent
        {
            get;
            private set;
        }
    }
}
