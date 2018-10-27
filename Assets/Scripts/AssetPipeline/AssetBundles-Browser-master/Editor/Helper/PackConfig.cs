using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using System.IO;

namespace Galaxy.AssetBundleBrowser
{
    // TODO 配表
    public class PackConfig
    {
        public class PackData
        {
            public string m_partName;
            public string m_relativePath;
            public string m_assetbundleName;
            public string m_assetbundleVarient;
        }



        private static PackConfig m_config;

        public static PackConfig Config
        {
            get
            {
                if (m_config == null)
                {
                    m_config = new PackConfig();

                    //string filePath = BuildHelper.GetExportBasePath;

                    //string[] resAssets = AssetDatabase.FindAssets("PackConfig t:csv");
                    //if (resAssets != null && resAssets.Length > 0)
                    //{
                    //    string resPath = AssetDatabase.GUIDToAssetPath(resAssets[0]);
                    //    string path = Path.GetFullPath
                    //    TextAsset asset = AssetDatabase.LoadAssetAtPath(resPath, typeof(TextAsset)) as TextAsset;
                    //    if (asset != null)
                    //    {
                    //        string content = asset.text;
                    //        CSVReader.GetGlobalFileData();

                    //    }
                    //}
                    //else {

                    //}
                }

                return m_config;
            }
        }
    }
}
