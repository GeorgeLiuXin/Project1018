using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
	public class InputManager : MonoBehaviour
	{

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.T))
			{
				GMCommand.Instance.HandleGMCommand("test 5 2 1.5");
			}
		}
	}

}
