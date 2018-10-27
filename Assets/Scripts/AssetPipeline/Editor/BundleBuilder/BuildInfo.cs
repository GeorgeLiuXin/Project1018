using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Galaxy.AssetPipeline
{
    public class BuildInfo2
    {
        public IABDataSource DataSource;
        public VersionInfo VersionInfo;
        public RuntimePlatform Platform;
        public EnumChannelType Channel;
        public BuildTarget BuildTarget;

    }
}
