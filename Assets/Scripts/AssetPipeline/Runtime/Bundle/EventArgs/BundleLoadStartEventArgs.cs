using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public class BundleLoadStartEventArgs : BaseEventArgs
    {
        public BundleLoadStartEventArgs(string bundlePath) : base()
        {
            this.BundlePath = bundlePath;
        }

        public string BundlePath
        {
            get;
            private set;
        }
    }
}
