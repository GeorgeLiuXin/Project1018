using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld.AssetPipeline
{
    public class ABMonitor
    {
        public AssetPipelineAction OnMonitorUpdate;

        private ABMonitorNode m_WakeCaller;
        private List<ABMonitorNode> m_RefenceNodess;

        private DateTime m_CallerTime;
        private DateTime m_ReferenceTime;

        public ABMonitorNode WakeCaller
        {
            get
            {
                return m_WakeCaller;
            }
        }

        public List<ABMonitorNode> RefenceNodess
        {
            get
            {
                return m_RefenceNodess;
            }
        }

        public ABMonitor()
        {
            m_RefenceNodess = new List<ABMonitorNode>();
            m_CallerTime = DateTime.Now;
            m_ReferenceTime = DateTime.Now;
        }

        //public void AddCaller(object obj)
        //{
        //    m_WakeCaller = new ABMonitorNode(obj);
        //    m_CallerTime = DateTime.Now;
        //}


        public void Clear()
        {
            this.m_WakeCaller = null;
            this.m_RefenceNodess.Clear();
            FireUpdateEvent();
        }

        public void Retain(object obj)
        {
            var item = FindItem(this.m_RefenceNodess, obj);

            if (item != null)
            {
                item.Retain();
            }
            else
            {
                bool isAwake = (this.m_RefenceNodess.Count == 0);
                ABMonitorNode node = new ABMonitorNode(obj);
                this.m_RefenceNodess.Add(node);
                if (isAwake)
                {
                    m_CallerTime = DateTime.Now;
                    m_WakeCaller = node;
                }
            }
            m_ReferenceTime = DateTime.Now;
            FireUpdateEvent();
        }

        public void Release(object obj)
        {
            var item = FindItem(this.m_RefenceNodess, obj);

            if (item != null)
            {
                if (item.RefCount > 0)
                {
                    item.Release();
                }
                else
                {
                    this.m_RefenceNodess.Remove(item);
                }

                m_ReferenceTime = DateTime.Now;
            }
            FireUpdateEvent();
        }

        private ABMonitorNode FindItem(List<ABMonitorNode> items, object obj)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Target == obj)
                {
                    return items[i];
                }
            }

            return null;
        }

        private void FireUpdateEvent()
        {
            if (OnMonitorUpdate != null)
            {
                OnMonitorUpdate();
            }
        }
    }
}
