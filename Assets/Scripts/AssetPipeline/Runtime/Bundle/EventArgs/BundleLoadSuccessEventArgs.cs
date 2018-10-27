using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XWorld.AssetPipeline
{
    public class BundleLoadSuccessEventArgs : BaseEventArgs
    {
        public BundleLoadSuccessEventArgs(string bundlePath, AssetBundle bundle) : base()
        {
            this.Bundle = bundle;
            this.BundlePath = bundlePath;
        }

        public AssetBundle Bundle
        {
            get;
            private set;
        }

        public string BundlePath
        {
            get;
            private set;
        }
    }
}
