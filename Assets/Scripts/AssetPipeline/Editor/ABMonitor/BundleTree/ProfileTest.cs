
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnityEditor
{
    internal class ABProfile
    {
        Dictionary<int, HashSet<string>> filter = new Dictionary<int, HashSet<string>>()
        {
            {1, new HashSet<string>() {"Assets",}}, //"Other", "Not Saved" 
            { 2, new HashSet<string>()
                { "SerializedFile", "Texture2D",
                }
            },
        };
        public void Test()
        {
          //  Type type = Assembly.Load("UnityEditor").GetType("MemoryElement");
          //  MemoryElement root = new UnityEditor.MemoryElement();

           // MemoryElement root = ProfilerWindow.GetMemoryDetailRoot(filter);
        }
    }
}
