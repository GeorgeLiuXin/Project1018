using System;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld.DataConfig
{
    public abstract class TableBase : MonoBehaviour
    {
        public string  tableName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public virtual void LoadData(string content)
        {

        }
    }
}