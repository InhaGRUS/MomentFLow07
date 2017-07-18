using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AnimationSet
{

	public string bodyAnimationName;
	public string shoulderAnimationName;

	public AnimationSet (string bodyName, string shoulderName)
	{
		bodyAnimationName = bodyName;
		shoulderAnimationName = shoulderName;
	}
}

public class StructPool  {
	
}
