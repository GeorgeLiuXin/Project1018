using UnityEngine;
using System;
using System.Collections;

public class Singleton<T> where T : class
{
	private static T _instance;
	private static readonly object _lockRoot = new object();

	public static T Instance()
	{
		if(_instance == null)
		{
			lock(_lockRoot)
			{
				if(_instance == null)
				{
					_instance = (T)Activator.CreateInstance(typeof(T),true);
				}
			}
		}

		return _instance;
	}
}

