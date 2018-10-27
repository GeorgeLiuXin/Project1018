using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public enum EAssetLoadTaskStatus
    {
        Todo,
        WaitBundle,
        ParseBundle,
        Done,
        Error
    }
}
