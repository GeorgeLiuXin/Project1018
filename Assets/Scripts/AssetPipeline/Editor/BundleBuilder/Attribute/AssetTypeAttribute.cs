using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galaxy.AssetPipeline
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class AssetBuildAttribute : Attribute
    {
        private string[] m_suffix;
        private Type[] m_type;
        private string[] m_tagPaths;

        public AssetBuildAttribute(Type[] type, params string[] suffix)
        {
            m_suffix = suffix;
            m_type = type;
        }

        public AssetBuildAttribute(params string[] tagPaths)
        {
            m_tagPaths = tagPaths;
        }

        public string[] Suffix
        {
            get
            {
                return m_suffix;
            }
        }

        public Type[] Types
        {
            get
            {
                return m_type;
            }
        }

        public string[] TagPaths
        {
            get
            {
                return m_tagPaths;
            }
        }
    }
}
