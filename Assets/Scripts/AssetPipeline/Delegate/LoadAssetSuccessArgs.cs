using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld
{
    public class LoadAssetSuccessArgs : BaseEventArgs
    {
        public string assetNames;
        public UnityEngine.Object[] assets;
        public object userData;
    }
}
