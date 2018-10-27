
using System;
using UnityEditor;

namespace Galaxy.AssetPipeline
{
    internal class CustomBuildInfo
    {
        private VersionInfo m_VersionInfo;
        private bool m_IgnoreLastOne;
        public string outputDirectory;
        public BuildAssetBundleOptions options;
        public BuildTarget buildTarget;
        public Action<string> onBuild;
    }
}
