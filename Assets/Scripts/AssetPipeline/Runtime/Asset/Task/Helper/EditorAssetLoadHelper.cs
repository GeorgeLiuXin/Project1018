using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public class EditorAssetLoadHelper : IAssetLoadHelper
    {
        private bool m_IsShutdown;

        public bool IsShutdown
        {
            get
            {
                return m_IsShutdown;
            }

            set
            {
                m_IsShutdown = value;
            }
        }
        
        public event EventHandler<AssetLoadHelperUpdateEventArgs> AssetLoadAgentHelperUpdate;
        public event EventHandler<AssetLoadHelperSuccessEventArgs> AssetLoadAgentHelperSuccess;
        public event EventHandler<AssetLoadHelperFailureEventArgs> AssetLoadAgentHelperFailure;
        public event EventHandler<AssetLoadHelperStartParseEventArgs> AssetLoadAgentHelperStartParse;

        public void Load(string assetPath, int priotity)
        {
#if UNITY_EDITOR
            UnityEngine.Object asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetPath);
            if (asset)
            {
                if (AssetLoadAgentHelperSuccess != null)
                {
                    AssetLoadAgentHelperSuccess.Invoke(this, new AssetLoadHelperSuccessEventArgs(assetPath, asset));
                }
            }

            else
            {
                if (AssetLoadAgentHelperFailure != null)
                {
                    AssetLoadAgentHelperFailure.Invoke(this, new AssetLoadHelperFailureEventArgs(assetPath, "Local file is not existed"));
                }
            }
#endif
        }
        
        public void Update(float deltaTime)
        {
            
        }
    }
}
