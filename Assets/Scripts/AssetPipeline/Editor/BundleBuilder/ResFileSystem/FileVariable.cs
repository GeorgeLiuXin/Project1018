using Galaxy.DataNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galaxy.AssetPipeline
{
    public class FileVariable : Variable
    {
        private CrudeAssetNode m_AssetNode;
        public override Type Type
        {
            get
            {
                return typeof(CrudeAssetNode);
            }
        }

        public override object GetValue()
        {
            return m_AssetNode;
        }

        public override void Reset()
        {
            m_AssetNode = new CrudeAssetNode();
        }

        public override void SetValue(object value)
        {
            m_AssetNode = (value as CrudeAssetNode) ?? new CrudeAssetNode();
        }
    }
}
