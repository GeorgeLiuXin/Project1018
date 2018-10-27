using Galaxy.DataNode;
using System;

namespace Galaxy.AssetPipeline
{
    [System.Serializable]
    internal class BundleHistoryVariable : Variable
    {
        DataNodeManager m_Data;

        internal BundleHistoryVariable(DataNodeManager data)
        {
            m_Data = data;
        }

        public override Type Type
        {
            get
            {
                return typeof(DataNodeManager);
            }
        }

        public override object GetValue()
        {
            return m_Data;
        }

        public T GetValue<T>() where T : class
        {
            return m_Data as T;
        }

        public override void Reset()
        {
            //
        }

        public override void SetValue(object value)
        {
            m_Data = (DataNodeManager)value;
        }
    }
}
