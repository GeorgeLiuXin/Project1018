using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galaxy.AssetPipeline
{
    public class AssetFolderSetting
    {
        public static Dictionary<string, List<string>> s_FolderToIgnoreExtensionDict = new Dictionary<string, List<string>>()
        {
            {"Atlas", new List<string>() { ".meta",".mat",".png" } },

            {"Scenes", new List<string>() { ".meta",".asset",".png", ".exr" } },
        };

        public static bool IsContainsExtension(string rootName, string extension)
        {
            if (s_FolderToIgnoreExtensionDict.ContainsKey(rootName))
            {
                if (!s_FolderToIgnoreExtensionDict[rootName].Contains(extension))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
