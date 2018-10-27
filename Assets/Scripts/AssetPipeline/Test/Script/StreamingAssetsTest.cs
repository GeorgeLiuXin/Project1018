using UnityEngine;
using System.Collections.Generic;
using XWorld;

namespace XWorld.Test
{
    public class StreamingAssetsTest : MonoBehaviour
    {
        private Matrix4x4 guiMatrix;
        private List<string> fileNames = new List<string>();

        private void Start()
        {
            guiMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 2);

           // StreamingAssets.CopyStreamingAssets();

            if (System.IO.Directory.Exists(GlobalAssetSetting.PersistentBundlePath))
            {
                fileNames.AddRange(System.IO.Directory.GetFiles(GlobalAssetSetting.PersistentBundlePath));
            }
        }

        private void OnGUI()
        {
            GUI.matrix = guiMatrix;

            GUILayout.Label("dataPath: " + Application.dataPath);
            GUILayout.Label("cachePath: " + Application.temporaryCachePath);
            GUILayout.Label("streamingAssetsPath: " + Application.streamingAssetsPath);

            GUILayout.Space(40f);

            for (int i = 0; i < fileNames.Count; i++)
            {
                GUILayout.Label(fileNames[i]);
            }
        }
    }
}
