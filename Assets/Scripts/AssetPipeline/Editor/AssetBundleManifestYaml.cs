using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public class AssetBundleManifestYaml
    {
        public class SingleHash
        {
            public int serializedVersion { get; set; }
            public string Hash { get; set; }
        }

        public class MultipleHash
        {
            public SingleHash AssetFileHash { get; set; }
            public SingleHash TypeTreeHash { get; set; }
        }

        public class ClassType
        {
            public int Class { get; set; }
            public Dictionary<string, string> Script { get; set; }
        }

        public int ManifestFileVersion { get; set; }
        public uint CRC { get; set; }
        public MultipleHash Hashes { get; set; }
        public int HashAppended { get; set; }
        public List<ClassType> ClassTypes { get; set; }
        public List<string> Assets { get; set; }
        public List<string> Dependencies { get; set; }
    }
}
