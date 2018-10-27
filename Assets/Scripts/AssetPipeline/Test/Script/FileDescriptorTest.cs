using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

namespace XWorld.Test
{
    public class FileDescriptorTest : MonoBehaviour
    {
        private List<FileStream> files = new List<FileStream>();
        private string error = string.Empty;
        private Matrix4x4 guiMatrix;

        private void Start()
        {
            guiMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 2);

            try
            {
                for (int i = 0; ; i++)
                {
                    string filepath = Path.Combine(Application.temporaryCachePath, "filetest" + i);
                    var fs = new FileStream(filepath, FileMode.OpenOrCreate);
                    files.Add(fs);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("files count: " + files.Count);

                error = e.ToString();
            }
        }

        private void OnGUI()
        {
            GUI.matrix = guiMatrix;

            GUILayout.Label("files count: " + files.Count);
            GUILayout.Label(error);
        }
    }
}
