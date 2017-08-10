using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMaths : MonoBehaviour {

	public static Vector3 GetMaxValueFromVector (Vector3 target)
	{
		var absX = Mathf.Abs (target.x);
		var absY = Mathf.Abs (target.y);
		var absZ = Mathf.Abs (target.z);

		if (absX > absY) {
			if (absX > absZ)
				return new Vector3 (target.x, 0f, 0f);
			else
				return new Vector3 (0f, 0f, target.z);
		}
		else {
			if (absY > absZ)
				return new Vector3 (0f, target.y, 0f);
		}
		return new Vector3 (0f, 0f, target.z);
	}
}
