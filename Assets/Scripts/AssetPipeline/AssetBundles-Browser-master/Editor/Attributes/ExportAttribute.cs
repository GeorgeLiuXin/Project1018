using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace Galaxy.AssetBundleBrowser
{
    [AttributeUsage(AttributeTargets.All)]
    public class ExportAttribute : Attribute
    {
        private readonly string m_exportName;

        public string ExportName
        {
            get
            {
                return m_exportName;
            }
        }

        public ExportAttribute(string name)
        {
            this.m_exportName = name;
        }
    }
}
