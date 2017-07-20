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

[System.Serializable]
public struct AimLinePoints
{
	public List<Vector3> points;

	public AimLinePoints (List<Vector3> points)
	{
		this.points = points;
	}
}

public class StructPool  {
	
}
