using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace XWorld
{
    public class BuildHelper
    {
        public static string STREAMING_ASSETS_PATH
        {
            get { return Application.streamingAssetsPath + "/"; }
        }

        public static readonly string PLATFORM_ANDROID = "android";
        public static readonly string PLATFORM_IOS = "ios";
        public static readonly string PLATFORM_PC = "pc";
        public static readonly string PLATFORM_EDITOR = "editor";
        public static readonly string PLATFORM_WINPHONE = "winphone";

        public static readonly string CHANNEL_COMMON = "common";
        public static readonly string CHANNEL_APPSTORE = "appstore";
        public static readonly string CHANNEL_WINDOWS = "windows";
        public static readonly string CHANNEL_GOOGLE = "google";

        public static readonly string ROOT = "AssetPipeline";
        public static readonly string EXPORT_ROOT = "ExportRes";
        public static readonly string CONFIG_ROOT = "Config";
        public static readonly string UPDATE_ROOT = "UpdateRes";

        public static readonly string EXPORT_SCENE_ROOT = "ExportScenes";
        public static readonly string EXPORT_SCENE_CONFIG_ROOT = "Project/Scene";

        public static readonly string CONFIGURE_NAME = "asset_configure.json";
        public static readonly string BUILDER_NAME = "asset_build.json";
        public static readonly string INSPECT_NAME = "asset_inspect.json";

        public static readonly string ASSETBUNDLE_MAP_FILENAME = "assetbundlemap.json";

        public static readonly string GAME_VERSION_FILE_NAME = "GameVersion.txt";
        public static readonly string VERSION_LIST_FILE_NAME = "VersionList.cfg";
        public static readonly string PREVIOUS_VERSION_LIST_FILE_NAME = "PreviousVersionList.cfg";
        public static readonly string SVN_FILE_NAME = "SVN.txt";
        public static readonly string EXPORT_VERSION_FILE = "Version.ver";

        public static readonly string DEST_BUNDLES = "Bundles";
        public static readonly string DEST_TABLE = "Tables";
        public static readonly string DEST_PROJECT = "Project";

        public static readonly string LOGIN_SCENE_NAME = "Login.Scene";
        public static readonly string LOGIN_SCENE_BUNDLE_PATH = "Assets/Res/Login/Login.unity";
        //public static readonly string LOGIN_SCENE_BUNDLE_PATH = "Assets/Resources/Pack/Scene/DL_new/DL_new.unity";

#if UNITY_EDITOR
        public static readonly UnityEditor.BuildAssetBundleOptions BUILD_OPTION =
            UnityEditor.BuildAssetBundleOptions.DeterministicAssetBundle
            | UnityEditor.BuildAssetBundleOptions.UncompressedAssetBundle;
#endif

        public static string GetExportBasePath()
        {
            var dataPath = System.IO.Path.GetFullPath(".");
            dataPath = Path.Combine(dataPath, ROOT);
            CheckCustomPath(dataPath);
            dataPath = dataPath.Replace("\\", "/");
            return dataPath;
        }

        public static string CheckConfigPath(string path = "")
        {
            var dataPath = System.IO.Path.GetFullPath(".");
            dataPath = Path.Combine(dataPath, ROOT);
            dataPath = Path.Combine(dataPath, CONFIG_ROOT);
            dataPath = Path.Combine(dataPath, path);
            CheckCustomPath(dataPath);
            dataPath = dataPath.Replace("\\", "/");
            return dataPath;
        }

        public static string CheckDestPath(string path = "")
        {
            var dataPath = Application.streamingAssetsPath;
            dataPath = Path.Combine(dataPath, path);
            CheckCustomPath(dataPath);
            dataPath = dataPath.Replace("\\", "/");
            return dataPath;
        }

        public static string CheckExportPath(string platform, string channel, string version, string path = "")
        {
            var dataPath = System.IO.Path.GetFullPath(".");
            dataPath = Path.Combine(dataPath, ROOT);
            dataPath = Path.Combine(dataPath, EXPORT_ROOT);
            dataPath = Path.Combine(dataPath, platform);
            dataPath = Path.Combine(dataPath, channel);
            dataPath = Path.Combine(dataPath, version);
            dataPath = Path.Combine(dataPath, path);
            CheckCustomPath(dataPath);
            dataPath = dataPath.Replace("\\", "/");
            return dataPath;
        }

        public static string CheckBundlePath(string platform, string version, string path = "")
        {
            var dataPath = System.IO.Path.GetFullPath(".");
            dataPath = Path.Combine(dataPath, ROOT);
            dataPath = Path.Combine(dataPath, EXPORT_ROOT);
            dataPath = Path.Combine(dataPath, platform);
            dataPath = Path.Combine(dataPath, version);
            dataPath = Path.Combine(dataPath, path);
            CheckCustomPath(dataPath);
            dataPath = dataPath.Replace("\\", "/");
            return dataPath;
        }

        public static void DeleteFolder(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            else
            {
                UnityEngine.Debug.LogWarning("DeleteFolder Warning: The folder is not exist: " + path);
            }
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                UnityEngine.Debug.LogWarning("DeleteFile Warning: The File is not exist, " + path);
            }
        }

        public static void CopyFilesToFolder(string srcFolder, string destFolder, string extension, bool samePath = false, System.Func<string, string, bool> func = null)
        {
            if (!Directory.Exists(srcFolder))
            {
                Debug.LogError(srcFolder + " is Not Exited ,Passed");
                return;
            }

            // create folder if not exist.
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }

            // copy files
            string[] files = null;
            if (extension.Equals("*"))
            {
                files = Directory.GetFiles(srcFolder);
            }
            else
            {
                files = Directory.GetFiles(srcFolder, "*." + extension);
            }

            foreach (string file in files)
            {
                if (string.IsNullOrEmpty(Path.GetExtension(file)))
                {
                    string destFile = destFolder + "/" + Path.GetFileName(file);
                    if (func != null)
                    {
                        func(file, destFile);
                    }
                    else
                    {
                        File.Copy(file, destFile, true);
                    }
                }
                else
                {
                    string destFile = destFolder + "/" + Path.GetFileNameWithoutExtension(file) + Path.GetExtension(file);
                    if (func != null)
                    {
                        func(file, destFile);
                    }
                    else
                    {
                        File.Copy(file, destFile, true);
                    }
                }
            }

            // travel the sub-folder
            string[] folders = Directory.GetDirectories(srcFolder);
            foreach (string folder in folders)
            {
                if (samePath)
                {
                    CopyFilesToFolder(folder, destFolder + "/" + Path.GetFileName(folder), extension, samePath, func);
                }
                else
                {
                    CopyFilesToFolder(folder, destFolder, extension, samePath, func);
                }
            }
        }

        public static void CopyFileToFolder(string fileName, string destFolder, System.Func<string, string, bool> func = null)
        {
            if (!File.Exists(fileName))
            {
                return;
            }
            // create folder if not exist.
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            string destFile = destFolder + "/" + Path.GetFileName(fileName);
            if (func != null)
            {
                func.Invoke(fileName, destFile);
            }
            else
            {
                File.Copy(fileName, destFile, true);
            }
        }

        public static string GameVersionStr(int GameVersion, int ResFirstVersion, int ResSecondVersion)
        {
            return GameVersion.ToString() + "^"
                         + ResFirstVersion.ToString() + "^"
                          + ResSecondVersion.ToString();
        }

        public static string GameVersionStr(string versionStrWithDot)
        {
            string[] dtrs = versionStrWithDot.Split('.');
            if (dtrs.Length == 2)
            {
                string versionStr = string.Join("^", dtrs);
                return versionStr;
            }
            return versionStrWithDot;
        }

# if UNITY_EDITOR
        public static UnityEditor.BuildTarget CustomTargetToBuildTarget(RuntimePlatform target)
        {
            switch (target)
            {
                case RuntimePlatform.OSXEditor:
                    return UnityEditor.BuildTarget.StandaloneWindows64;
                case RuntimePlatform.WindowsEditor:
                    return UnityEditor.BuildTarget.StandaloneWindows64;
                case RuntimePlatform.Android:
                    return UnityEditor.BuildTarget.Android;
                case RuntimePlatform.IPhonePlayer:
                    return UnityEditor.BuildTarget.iOS;
            }
            return UnityEditor.BuildTarget.StandaloneWindows64;
        }
#endif

        public static string CustomTargetToName(RuntimePlatform target)
        {
            switch (target)
            {
                case RuntimePlatform.OSXEditor:
                    return BuildHelper.PLATFORM_PC;
                case RuntimePlatform.WindowsEditor:
                    return BuildHelper.PLATFORM_PC;
                case RuntimePlatform.Android:
                    return BuildHelper.PLATFORM_ANDROID;
                case RuntimePlatform.IPhonePlayer:
                    return BuildHelper.PLATFORM_IOS;
            }
            return BuildHelper.PLATFORM_PC; 
        }

        public static string CustomTargetToName(CustomTarget target)
        {
            switch (target)
            {
                case CustomTarget.Android:
                    return BuildHelper.PLATFORM_ANDROID;
                case CustomTarget.IOS:
                    return BuildHelper.PLATFORM_IOS;
                case CustomTarget.WindowsPC:
                    return BuildHelper.PLATFORM_PC;
                case CustomTarget.WinPhone:
                    return BuildHelper.PLATFORM_WINPHONE;
                default:
                    return BuildHelper.PLATFORM_PC;
            }
        }

        public static string ChannelToName(EnumChannelType target)
        {
            switch (target)
            {
                case EnumChannelType.AppStore:
                    return BuildHelper.CHANNEL_APPSTORE;
                case EnumChannelType.Google:
                    return BuildHelper.CHANNEL_GOOGLE;
                case EnumChannelType.WindowsPC:
                    return BuildHelper.CHANNEL_WINDOWS;
                case EnumChannelType.Common:
                    return BuildHelper.CHANNEL_COMMON;
                default:
                    return BuildHelper.PLATFORM_PC;
            }
        }

        public static string CheckExportPath(string path = "")
        {
            var dataPath = System.IO.Path.GetFullPath(".");
            dataPath = Path.Combine(dataPath, ROOT);
            dataPath = Path.Combine(dataPath, path);
            CheckCustomPath(dataPath);
            dataPath = dataPath.Replace("\\", "/");
            return dataPath;
        }

        public static string CheckUpdatePath(string path = "")
        {
            var dataPath = System.IO.Path.GetFullPath(".");

            dataPath = Path.Combine(dataPath, ROOT);
            dataPath = Path.Combine(dataPath, UPDATE_ROOT);
            dataPath = Path.Combine(dataPath, path);
            CheckCustomPath(dataPath);
            dataPath = dataPath.Replace("\\", "/");
            return dataPath;
        }

        //判断比较粗暴,最后一位是数字表示是版本号,不是拓展符
        private static bool CheckIsNumberExtension(string targetPath)
        {
            if (Regex.IsMatch(targetPath, "[0-9]+$"))
            {
                return true;
            }
            return false;
        }

        public static string CheckCustomPathIgnoreExtension(params string[] paths)
        {
            string path = string.Join("/", paths);

            string outPath = path.Replace('\\', '/');
            string targetPath = outPath;
            if (Directory.Exists(targetPath))
            {
                return outPath;
            }


            int lastPathPos = targetPath.LastIndexOf('/');

            if (lastPathPos > 0)
            {
                targetPath = targetPath.Substring(0, lastPathPos);
            }

            string[] subPath = targetPath.Split('/');
            string curCheckPath = "";
            int subContentSize = subPath.Length;
            for (int i = 0; i < subContentSize; i++)
            {
                curCheckPath += subPath[i] + '/';
                if (!Directory.Exists(curCheckPath))
                {
                    Directory.CreateDirectory(curCheckPath);
                }
            }
            return outPath;
        }

        public static string CheckCustomPath(params string[] paths)
        {
            string path = string.Join("/", paths);

            string outPath = path.Replace('\\', '/');
            string targetPath = outPath;
            if (Directory.Exists(targetPath))
            {
                return outPath;
            }

            if(targetPath.Contains("Editor/Bundle/Common"))
                Debug.LogError(targetPath);

            int dotPos = targetPath.LastIndexOf('.');
            //暂时改变版本号的书写方式,这样不容易产生问题
            //if (CheckIsNumberExtension(targetPath)) {
            //    dotPos = 0;
            //}
            int lastPathPos = targetPath.LastIndexOf('/');

            if (dotPos > 0 && lastPathPos < dotPos)
            {
                targetPath = targetPath.Substring(0, lastPathPos);
            }

            string[] subPath = targetPath.Split('/');
            string curCheckPath = "";
            int subContentSize = subPath.Length;
            for (int i = 0; i < subContentSize; i++)
            {
                curCheckPath += subPath[i] + '/';
                if (!Directory.Exists(curCheckPath))
                {
                    Directory.CreateDirectory(curCheckPath);
                }
            }
            return outPath;
        }


        public static long GetFileSize(string path)
        {
            if (!File.Exists(path))
            {
                return 0;
            }
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                return fi.Length;
            }
            return 0;
        }

        // 获取MD5
        public static string GetFileMD5(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            try
            {
                FileStream get_file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
#if UNITY_WP8
				byte[] buf = new byte[64*1024]; // 64k buffer;
				int siz = 1;
				CryptoNet.Md5 md5 = CryptoNet.Md5.Create();
				get_file.Position = 0;
				while(siz>0)
				{
					siz = get_file.Read(buf, 0, 64*1024); 
					md5.Update(buf, 0, siz);
				}
				string result = md5.Result().ToLower();
#else
                System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash_byte = get_md5.ComputeHash(get_file);
                string result = System.BitConverter.ToString(hash_byte).Replace("-", "");
#endif
                get_file.Dispose();
                return result;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
                return null;
            }
        }

        public static string ColorStr(string color, string content)
        {
            return "<color=#" + color + ">" + content + "</color>";
        }

        /// <summary>
        /// 域名转Ip地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DomainConvertToAddress(string str)
        {
            if (str == null) return null;

            //域名转IP
            int num;
            string[] parts = str.Split('.');
            if (parts.Length == 0)
            {
                Debug.LogError("NetWorkSystem.cs DomainConvertToAddress parts.Length == 0");
                return str;
            }
            int.TryParse(parts[0], out num);
            if (num == 0)
            {
                string[] IpAndPort = str.Split(':');
                if (IpAndPort.Length >= 2)
                {
                    IPAddress[] ip = Dns.GetHostAddresses(IpAndPort[0]);
                    str = ip[0] + ":" + IpAndPort[1];
                }
                else
                {
                    IPAddress[] ip = Dns.GetHostAddresses(str);
                    str = ip[0].ToString();
                }
            }
            return str;
        }

        public static string PathNormalized(string path)
        {
            return path.Replace('\\', '/'); ;
        }

        public static string GetFileName(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(path);
                return name;
            }
            return "";
        }

        // 封装是为可修改为测试项
        public static NetworkReachability GNetworkReachability
        {
            get
            {
                return Application.internetReachability;
            }
        }
    }

    //类型数值跟Unity自带数值对应,千万别更改
    public enum CustomTarget
    {
        [Tooltip("Unknown")]
        None,
        [Tooltip("Android")]
        Android = 13,
        [Tooltip("IOS")]
        IOS = 9,
        [Tooltip("Windows PC")]
        WindowsPC = 5,
        [Tooltip("WindowPhone")]
        WinPhone = 21,
    }

    public enum EnumChannelType
    {
        [Tooltip("编辑器")]
        WindowsPC = 0,  // 编辑器（用于测试）
        [Tooltip("AppStore")]
        AppStore,   // AppStore
        [Tooltip("Google市场")]
        Google,        // 应用宝
        [Tooltip("通用渠道")]
        Common,        // 通用渠道
    }

    public enum ExportType
    {
        [Tooltip("没有选择导出资源类型")]
        EXPORT_None = 0,
        [Tooltip("导出全部资源")]
        EXPORT_ALL,
        [Tooltip("导出初始化资源")]
        EXPORT_PREINIT_RESOURCE,
        [Tooltip("导出Resource资源")]
        EXPORT_RESOURCE,
        [Tooltip("导出配表")]
        EXPORT_DATA_TABLE,
        [Tooltip("导出场景SCEX文件")]
        EXPORT_SCENE_SCEX,
        [Tooltip("导出场景配置文件")]
        EXPORT_SCENE_CONFIG,
        [Tooltip("导出Lua文件")]
        EXPORT_LUA,
        [Tooltip("导出配置文件")]
        EXPORT_CONFIG,
        [Tooltip("导出登录场景")]
        EXPORT_LOGIN_SCENE,
        [Tooltip("导出增量包")]
        EXPORT_INCREATEMENT,
    };

    public enum ExportStep
    {
        [Tooltip("总 - 导出开始")]
        STEP_BEGIN,

        // Login Scene
        [Tooltip("登陆场景 - 导出开始")]
        STEP_EXPORT_LOGIN_SCENE_BEGIN,
        [Tooltip("登陆场景 - 清空导出目录")]
        STEP_DEL_LOGIN_SCENE_EXPORT_PATH,
        [Tooltip("登陆场景 - 导出登录场景")]
        STEP_EXPORT_LOGIN_SCENE,
        [Tooltip("登陆场景 - 导出结束")]
        STEP_EXPORT_LOGIN_SCENE_END,

        // Export PreInit Resource
        [Tooltip("初始化资源 - 导出开始")]
        STEP_EXPORT_PREINIT_RESOURCE_BEGIN,
        [Tooltip("初始化资源 - 清空导出目录")]
        STEP_DEL_PREINIT_RESOURCE_PATH,
        [Tooltip("初始化资源 - 导出初始化资源")]
        STEP_EXPORT_PREINIT_RESOURCE,
        [Tooltip("初始化资源 - 导出结束")]
        STEP_EXPORT_PREINIT_RESOURCE_END,

        // Export Resource
        [Tooltip("导出Resource资源 - 导出开始")]
        STEP_EXPORT_RESOURCE_BEGIN,
        [Tooltip("导出Resource资源 - 清空导出目录")]
        STEP_DEL_RESOURCE_PATH,
        [Tooltip("导出Resource资源 - 导出Resource资源")]
        STEP_EXPORT_RESOURCE,
        [Tooltip("导出Resource资源 - 导出结束")]
        STEP_EXPORT_RESOURCE_END,

        // DataTable
        [Tooltip("导出配表 - 导出开始")]
        STEP_EXPORT_TABLE_BEGIN,
        [Tooltip("导出配表 - 清空导出目录")]
        STEP_DEL_EXPORT_PATH_TABLE,
        [Tooltip("导出配表 - 导出配表")]
        STEP_EXPORT_TABLE,
        [Tooltip("导出配表 - 导出结束")]
        STEP_EXPORT_TABLE_END,

        // Scex
        [Tooltip("导出场景SCEX - 导出开始")]
        STEP_EXPORT_SCEX_BEGIN,
        [Tooltip("导出场景SCEX - 清空导出目录")]
        STEP_DEL_EXPORT_PATH_SCEX,
        [Tooltip("导出场景SCEX - 导出场景SCEX")]
        STEP_EXPORT_SCEX,
        [Tooltip("导出场景SCEX - 导出结束")]
        STEP_EXPORT_SCEX_END,

        // Scene config
        [Tooltip("导出场景Config - 导出开始")]
        STEP_EXPORT_SCENE_CONFIG_BEGIN,
        [Tooltip("导出场景Config - 清空导出目录")]
        STEP_DEL_EXPORT_PATH_SCENE_CONFIG,
        [Tooltip("导出场景Config - 导出场景Config")]
        STEP_EXPORT_SCENE_CONFIG,
        [Tooltip("导出场景Config - 导出结束")]
        STEP_EXPORT_SCENE_CONFIG_END,

        // Lua
        [Tooltip("导出Lua - 导出开始")]
        STEP_EXPORT_LUA_BEGIN,
        [Tooltip("导出Lua - 清空导出目录")]
        STEP_DEL_EXPORT_PATH_LUA,
        [Tooltip("导出Lua - 导出Lua")]
        STEP_EXPORT_LUA,
        [Tooltip("导出Lua - 导出结束")]
        STEP_EXPORT_LUA_END,

        // Config
        [Tooltip("导出配置 - 导出开始")]
        STEP_EXPORT_CONFIG_BEGIN,
        [Tooltip("导出配置 - 清空导出目录")]
        STEP_DEL_EXPORT_PATH_CONFIG,
        [Tooltip("导出配置 - 导出配置")]
        STEP_EXPORT_CONFIG,
        [Tooltip("导出配置 - 导出结束")]
        STEP_EXPORT_CONFIG_END,

        [Tooltip("总 - 导出结束")]
        STEP_END
    };
}

