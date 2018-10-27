using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public interface IAssetLoadHelper
    {
        bool IsShutdown { get; set; }

        event EventHandler<AssetLoadHelperUpdateEventArgs> AssetLoadAgentHelperUpdate;
        event EventHandler<AssetLoadHelperSuccessEventArgs> AssetLoadAgentHelperSuccess;
        event EventHandler<AssetLoadHelperFailureEventArgs> AssetLoadAgentHelperFailure;
        event EventHandler<AssetLoadHelperStartParseEventArgs> AssetLoadAgentHelperStartParse;

        void Load(string assetPath, int priotity);

        void Update(float deltaTime);
    }
}
