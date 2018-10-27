using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XWorld.AssetPipeline
{
    public class AssetBundleLoadHelper : IBundleLoadHelper
    {
        public bool IsShutdown
        {
            get
            {
                return m_IsShutDown;
            }

            set
            {
                m_IsShutDown = value;
            }
        }

        public bool Done
        {
            get
            {
                return m_Done;
            }
        }

        public event EventHandler<BundleLoadHelperFailureEventArgs> BundleLoadAgentHelperFail;
        public event EventHandler<BundleLoadHelperSuccessEventArgs> BundleLoadAgentHelperSuccess;
        public event EventHandler<BundleLoadHelperUpdateEventArgs> BundleLoadAgentHelperUpdate;

        public AssetBundleLoadHelper(BundleCollection gc)
        {
            m_ABCollection = gc;
        }

        public void Load(string bundlePath, string filePath, uint crc)
        {
            this.m_IsShutDown = false;
            this.m_Done = false;
            this.m_BundlePath = bundlePath;
            this.m_FilePath = filePath;
            this.m_CRC = crc;

            if (m_ABCollection != null)
            {
                m_Bundle = m_ABCollection.GetBundle(bundlePath);
            }

            if (m_Bundle == null)
            {
                LoadFromFile();
            }
        }

        private void LoadFromFile()
        {
            if (System.IO.File.Exists(m_FilePath))
            {
                m_AssetBundleRequest = AssetBundle.LoadFromFileAsync(m_FilePath, this.m_CRC);
            }
            else
            {
                Debug.LogError("IO Exception : " + m_FilePath + " is not existed");
                if (BundleLoadAgentHelperFail != null)
                {
                    BundleLoadAgentHelperFail.Invoke(this,
                        new BundleLoadHelperFailureEventArgs(m_BundlePath, "IO Exception : " + m_FilePath + " is not existed"));
                }
                m_Bundle = null;
                m_AssetBundleRequest = null;
                m_IsShutDown = false;
                m_Done = true;
            }
        }

        public void Update(float deltaTime)
        {
            if (!m_Done)
            {
                if (m_AssetBundleRequest != null)
                {
                    float progress = m_AssetBundleRequest.progress;
                    if (BundleLoadAgentHelperUpdate != null)
                    {
                        BundleLoadAgentHelperUpdate.Invoke(this, new BundleLoadHelperUpdateEventArgs(m_BundlePath, progress));
                    }

                    if (m_AssetBundleRequest.isDone)
                    {
                        m_Done = true;
                        m_Bundle = m_AssetBundleRequest.assetBundle;

                        if (m_Bundle == null)
                        {
                            if (BundleLoadAgentHelperFail != null)
                            {
                                BundleLoadAgentHelperFail.Invoke(this,
                                    new BundleLoadHelperFailureEventArgs(m_BundlePath, "Bundle Exception : " + m_FilePath + " is Invalid, Please Check CRC,length,path,etc."));
                            }
                            m_AssetBundleRequest = null;
                            m_IsShutDown = false;
                        }
                        else
                        {
                            if (!m_IsShutDown)
                            {
                                if (BundleLoadAgentHelperSuccess != null)
                                {
                                    BundleLoadAgentHelperSuccess.Invoke(this, new BundleLoadHelperSuccessEventArgs(m_BundlePath, m_Bundle));
                                }
                                m_Bundle = null;
                                m_AssetBundleRequest = null;
                            }
                            else
                            {
                                // 如果已经shutdown ,将资源转到垃圾回收
                                m_ABCollection.UnloadBundle(m_BundlePath, m_Bundle, false);
                                m_Bundle = null;
                                m_AssetBundleRequest = null;
                                m_IsShutDown = false;
                                m_Done = true;
                            }
                        }
                    }
                }

                else if (m_Bundle != null)
                {
                    m_Done = true;
                    //为了时序 必须在Update中触发
                    if (BundleLoadAgentHelperSuccess != null)
                    {
                        BundleLoadAgentHelperSuccess.Invoke(this, new BundleLoadHelperSuccessEventArgs(m_BundlePath, m_Bundle));
                    }
                    m_Bundle = null;
                    m_AssetBundleRequest = null;
                }
                else
                {
                    // 可能Bundle在Update之前已经被卸载
                    Debug.Log("LoadFromFile LoadFromFile");
                    LoadFromFile();
                }
            }
        }

        private bool m_Done = true;
        private string m_FilePath;
        private string m_BundlePath;
        private uint m_CRC;
        private AssetBundleCreateRequest m_AssetBundleRequest;
        private AssetBundle m_Bundle;
        private BundleCollection m_ABCollection;
        private bool m_IsShutDown;
    }
}
