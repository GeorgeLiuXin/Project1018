using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XWorld.AssetPipeline
{
    public class ABMonitorNode
    {
        private string m_Path;
        private string m_Type;
        private WeakReference m_Refenrence;
        private DateTime m_Time;
        private int m_RefCount;
        private bool m_IsAwakeCaller;

        public bool IsAlive
        {
            get
            {
                return (m_Refenrence != null && m_Refenrence.IsAlive);
            }
        }

        public object Target
        {
            get
            {
                if (IsAlive)
                {
                    return this.m_Refenrence.Target;
                }
                return null;
            }
        }

        public string Path
        {
            get
            {
                return m_Path;
            }
        }

        public string Type
        {
            get
            {
                return m_Type;
            }
        }

        public int RefCount
        {
            get
            {
                return m_RefCount;
            }
        }

        public bool IsAwakeCaller
        {
            get
            {
                return m_IsAwakeCaller;
            }

            set
            {
                m_IsAwakeCaller = value;
            }
        }

        public DateTime Time
        {
            get
            {
                return m_Time;
            }
        }

        public ABMonitorNode(object obj)
        {
            if (obj != null)
            {
                if (obj is GameObject)
                {
                    var go = obj as GameObject;
                    this.m_Path = Utility.GetHierarchyPath(go);
                }
                else if (obj is Component)
                {
                    var com = obj as Component;
                    this.m_Path = Utility.GetHierarchyPath(com.transform);
                }
                else
                {
                    this.m_Path = obj.ToString();
                }

                this.m_Type = obj.GetType().ToString();
                this.m_Refenrence = new WeakReference(obj);
            }

            Retain();
        }

        public void Retain()
        {
            this.m_RefCount++;
            UpdateTime();
        }

        public void Release()
        {
            this.m_RefCount--;
            UpdateTime();
        }

        private void UpdateTime()
        {
            this.m_Time = DateTime.Now;
        }
    }
}
