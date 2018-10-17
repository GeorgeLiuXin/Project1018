using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;
using System;

public class OneKeyBuild : EditorWindow
{
	private BuildTarget buildTarget
	{
		get
		{
#if UNITY_IOS
			return BuildTarget.iOS;
#elif UNITY_ANDROID
			return BuildTarget.Android;
#else
			return BuildTarget.StandaloneWindows64;
#endif
		}
	}
	//private bool isLocalBundle = true;
	private string wwwPath = "";
	private string exportPath;

	void OnGUI ()
	{
		GUILayout.BeginHorizontal ();
		//isLocalBundle = GUILayout.Toggle (isLocalBundle, "是否是本地AssetBundle");
		GUILayout.EndHorizontal ();

		GUILayout.BeginVertical ();

		/*if (GUILayout.Button ("拷贝txt文件到Unity目录")) 
		{
			CopyAllTxts ();
		}*/

		if (GUILayout.Button ("自动创建CSharp配置文件")) 
		{
			excuteBuildConfig ();	
		}

        if(GUILayout.Button ("还原服务器txt文件制表格式"))
        {
            NewTxtConfig();
        }
			
		GUILayout.EndVertical ();
	}

	void CopyAllTxts()
	{
		AutoCSharpTools.CopyAllTxt ();
	}

	void excuteBuildConfig()
	{
		if (AutoCSharpTools.IsBuilding)
			return;

		AutoCSharpTools.InitAutoClassBuilder ();
		AutoCSharpTools.DoAuto ();
	}

    void NewTxtConfig()
    {
        ServerConfig.AutoNew();
    }
}
