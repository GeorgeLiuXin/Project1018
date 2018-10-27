using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld.AssetPipeline
{
    public class BundleLoadTask : ITask
    {
        private bool m_Done;
        private EBundleLoadTaskStatus m_Status;
        private string m_BundlePath;
        private string m_FilePath;
        private uint m_CRC;
        //private BundleLoadTask[] m_SubTasks;

        //public bool IsSubDone
        //{
        //    get
        //    {
        //        if (m_SubTasks != null)
        //        {
        //            foreach (BundleLoadTask t in m_SubTasks)
        //            {
        //                if (t != null && !t.Done)
        //                {
        //                    return false;
        //                }
        //            }
        //        }
        //        return true;
        //    }
        //}

        public bool Done
        {
            get
            {
                return m_Done;
            }

            set
            {
                m_Done = value;
            }
        }

        public int SerialId
        {
            get
            {
                return GetHashCode();
            }
        }

        public EBundleLoadTaskStatus Status
        {
            get
            {
                return m_Status;
            }

            set
            {
                m_Status = value;
            }
        }

        public string BundlePath
        {
            get
            {
                return m_BundlePath;
            }
        }

        public uint CRC
        {
            get
            {
                return m_CRC;
            }
        }

        public string FilePath
        {
            get
            {
                return m_FilePath;
            }
        }

        public void Init(BundleReference bundleRef)
        {
            if (bundleRef != null)
            {
                m_BundlePath = bundleRef.BundlePath;
                m_FilePath = bundleRef.FilePath;
                m_CRC = bundleRef.CRC;
                m_Done = false;
                m_Status = EBundleLoadTaskStatus.Todo;

                //BundleLoadTask[] subTasks = new BundleLoadTask[0];
                //if (bundleRef.SubDependencies != null && bundleRef.SubDependencies.Length > 0)
                //{
                //    int length = bundleRef.SubDependencies.Length;

                //    for (int i = 0; i < length; i++)
                //    {
                //        BundleLoadTask task = new BundleLoadTask();
                //        task.Init(bundleRef.SubDependencies[i]);
                //        subTasks[i] = task;
                //    }
                //}

               // m_SubTasks = subTasks;
            }
            else
            {
                GameLogger.Error(LOG_CHANNEL.LOGIC, "bundleRef can't be null");
            }
        }

        public void Dispose()
        {
            m_Done = false;
            m_Status = EBundleLoadTaskStatus.Todo;
            m_BundlePath = string.Empty;
            m_CRC = 0;
         //   m_SubTasks = null;
        }
    }
}