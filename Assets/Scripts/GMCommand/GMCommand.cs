using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public class GMCommand : Singleton<GMCommand>
    {
        public delegate void GMCommandFunction(params object[] values);

        public static Dictionary<string, EventsDelegate.EventFunction> m_GMCommandDict;

        public GMCommand()
        {
            m_GMCommandDict = new Dictionary<string, EventsDelegate.EventFunction>();
            InitGM();
        }

        public void InitGM()
        {

        }

    }
}