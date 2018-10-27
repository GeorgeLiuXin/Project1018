
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public class BundleLoadUpdateEventArgs : BaseEventArgs
    {
        public BundleLoadUpdateEventArgs(string bundlePath, float progress) : base()
        {
            this.BundlePath = bundlePath;
            this.Progress = progress;
        }

        public string BundlePath
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
