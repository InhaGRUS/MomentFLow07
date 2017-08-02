using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HideableFace {
	[HideInInspector]
	public HideableObject hideableObj;
	public HideableFaceName faceName;
	public bool hideable;
	public Vector3 point;
}
