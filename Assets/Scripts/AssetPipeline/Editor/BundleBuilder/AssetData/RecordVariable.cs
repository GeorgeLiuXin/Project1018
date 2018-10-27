using Galaxy.DataNode;
using System;

namespace Galaxy.AssetPipeline
{
    [System.Serializable]
    internal class RecordVariable : Variable
    {
        RefinedAssetNode m_RecordNode;

        internal RecordVariable(RefinedAssetNode node)
        {
            m_RecordNode = node;
        }

        public override Type Type
        {
            get
            {
                return typeof(RefinedAssetNode);
            }
        }

        public override object GetValue()
        {
            return m_RecordNode;
        }

        public T GetValue<T>() where T : class
        {
            return m_RecordNode as T;
        }

        public override void Reset()
        {
            //
        }

        public override void SetValue(object value)
        {
            m_RecordNode = (RefinedAssetNode)value;
        }
    }
}
