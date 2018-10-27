
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Galaxy.AssetPipeline
{
    internal class BundleConfig
    {
        public static readonly string[] EmptyStringArray = new string[] { };
        public static readonly string[] PathSplit = new string[] { "/", "\\" };
        public static readonly string[] VersionSplit = new string[] { "^", "/", "\\" };

        public static readonly string VERSION_FILE_NAME = "version.txt";
        public static readonly string BUNDLE_MAP_FILE_NAME_BIN = "asset_map.bin";
        public static readonly string BUNDLE_MAP_FILE_NAME_JSON = "asset_map.json";
        public static readonly string BUNDLE_MAP_FILE_NAME_XML = "asset_map.yaml";

        public static readonly string DEFAULT_SPLIT = "/";

        public static readonly string COMMON_CLASSIFY = "Common";

        /// <summary>
        /// AssetDatas最大层数,目前设定为3,也就是Prefabs/UI/Common三层
        /// </summary>
        public static readonly int ASSET_MAX_LAYER = 3;

        /// <summary>
        /// 限制单文件夹下的文件数量
        /// </summary>
        public const int SYSTEM_FILE_MAX_COUNT = 1000;

        /// <summary>
        /// AssetBundle大小限制  4 * 1024 *1024b 4M   * 2 因为LZ4的压缩大小趋近于50%
        /// </summary>
        public const int SINGLE_BUNDLE_MAX_LENGTH = 8388608;

        public static string BundleEditorPath
        {
            get
            {
                return TemporaryEditorPath + DEFAULT_SPLIT + "Bundle";
            }
        }

        public static string BundleLogPath
        {
            get
            {
                return TemporaryEditorPath + DEFAULT_SPLIT + "Log";
            }
        }

        public static string TemporaryEditorPath
        {
            get
            {
                return (Application.dataPath + "/../AssetPipeline/Editor");  
            }
        }
    }
}
