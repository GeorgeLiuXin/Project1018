using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public interface IBundleLoadHelper
    {
        bool IsShutdown { get; set; }

        event EventHandler<BundleLoadHelperUpdateEventArgs> BundleLoadAgentHelperUpdate;
        event EventHandler<BundleLoadHelperSuccessEventArgs> BundleLoadAgentHelperSuccess;
        event EventHandler<BundleLoadHelperFailureEventArgs> BundleLoadAgentHelperFail;

        void Load(string bundlePath,string filePath, uint crc);

        void Update(float deltaTime);
    }
}
