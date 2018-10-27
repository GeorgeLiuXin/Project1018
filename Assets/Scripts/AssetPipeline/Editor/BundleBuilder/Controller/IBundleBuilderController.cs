using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galaxy.AssetPipeline
{
    internal interface IBundleBuilderController
    {
        EAssetTag AssetTag
        {
            get; set;
        }

        bool IgnoreLastVersion
        {
            get;
            set;
        }

        void Prepare();

        void Handle();

        IOutputData GetBuildData();
    }
}
