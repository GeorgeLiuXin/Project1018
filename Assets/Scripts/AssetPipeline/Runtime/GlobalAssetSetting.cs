using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace XWorld
{
    public class GlobalAssetSetting
    {
        public const string ASSETBUNDLE_STREAMING_ASSETS_DIR = "Bundles";
        public static bool EnableEditorSimulation = true;

        public static string RemoteUrl = "http://127.0.0.1/Bundle";

        public static string StreamingDataPath
        {
            get
            {
                return Path.Combine(Application.streamingAssetsPath,
                       ASSETBUNDLE_STREAMING_ASSETS_DIR);
            }
        }

        public static string PersistentDataPath
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.OSXEditor:
                        return (Application.dataPath + "/../Sandbox/Document");
                    case RuntimePlatform.WindowsEditor:
                        return (Application.dataPath + "/../Sandbox/Document");
                    case RuntimePlatform.Android:
                        return Application.persistentDataPath;
                    case RuntimePlatform.IPhonePlayer:
                        return Application.persistentDataPath;
                    default:
                        return Application.persistentDataPath;
                }
            }
        }

        public static string TemporaryDataPath
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.OSXEditor:
                        return (Application.dataPath + "/../Sandbox/Cache");
                    case RuntimePlatform.WindowsEditor:
                        return (Application.dataPath + "/../Sandbox/Cache");
                    case RuntimePlatform.Android:
                        return Application.temporaryCachePath;
                    case RuntimePlatform.IPhonePlayer:
                        return Application.temporaryCachePath;
                    default:
                        return Application.temporaryCachePath;
                }
            }
        }


        public static string PersistentBundlePath
        {
            get
            {
                return Path.Combine(PersistentDataPath, "Bundles");
            }
        }

        public static string TemporaryBundlePath
        {
            get
            {
                return Path.Combine(TemporaryDataPath, "Bundles");
            }
        }
    }
}