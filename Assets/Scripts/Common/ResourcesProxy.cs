using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
	public static class ResourcesProxy
	{

		public static string LoadTextString(string path)
		{
			return null;
		}

	}

	public class StreamingAssetsProxy : MonoBehaviour
	{
		public string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "MyFile");
		public string result = "";

		public void LoadFromPath()
		{

		}

		IEnumerator LoadFromStreamingAssets()
		{
			if (filePath.Contains("://"))
			{
				UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
				yield return www.SendWebRequest();
				result = www.downloadHandler.text;
			}
			else
				result = System.IO.File.ReadAllText(filePath);
		}
	}

}
