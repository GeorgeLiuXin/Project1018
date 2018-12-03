using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
	public partial class GMCommand
	{
		private void GMCommand_Test(params object[] values)
		{
			int a, b;
			float c;
			a = Convert.ToInt32(values[0]);
			b = Convert.ToInt32(values[1]);
			c = Convert.ToSingle(values[2]);
			float result = (a - b) * c;
			Debug.Log("GMCommand_Test: the result is " + result);
		}
	}

}
