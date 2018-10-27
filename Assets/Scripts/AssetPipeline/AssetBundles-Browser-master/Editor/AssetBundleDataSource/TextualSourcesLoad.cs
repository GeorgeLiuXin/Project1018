using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Galaxy;

namespace Galaxy.AssetBundleBrowser.AssetBundleDataSource
{
    [SerializeField]
    public class TextualSourcesLoad
    {
        [SerializeField]
        internal AssetBundleBuildMap m_buildAssetBundleMap;
        internal void OnEnable()
        {
            m_buildAssetBundleMap = new AssetBundleBuildMap();
            //LoadData...
            string dataPath = BuildHelper.CheckConfigPath(BuildHelper.CONFIGURE_NAME);

            if (File.Exists(dataPath))
            {
                using (FileStream file = File.Open(dataPath, FileMode.Open))
                {
                    using (StreamReader tr = new StreamReader(file))
                    {
                        this.m_buildAssetBundleMap = LitJson.JsonMapper.ToObject<AssetBundleBuildMap>(tr);
                        m_buildAssetBundleMap.Init();
                    }
                }
            }
        }

        internal void OnDisable()
        {
            string dataPath = BuildHelper.CheckConfigPath(BuildHelper.CONFIGURE_NAME);
            
            using (FileStream file = File.Create(dataPath))
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    LitJson.JsonWriter jw = new LitJson.JsonWriter(sw);
                    jw.PrettyPrint = true;
                    LitJson.JsonMapper.ToJson(this.m_buildAssetBundleMap, jw);
                    this.m_buildAssetBundleMap.Over();
                }
            }
        }
    }
}

