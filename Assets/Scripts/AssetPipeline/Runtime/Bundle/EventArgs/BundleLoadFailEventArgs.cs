﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public class BundleLoadFailEventArgs : BaseEventArgs
    {
        public BundleLoadFailEventArgs(string bundlePath, string errorContent) : base()
        {
            this.BundlePath = bundlePath;
            this.ErrorContent = errorContent;
        }

        public string BundlePath
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
