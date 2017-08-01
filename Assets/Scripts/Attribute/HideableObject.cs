using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideableObject : InteractableObject {

	private Collider col;
	public Vector3 colliderScale;
	[SerializeField]
	private List<HideableFace> hideableFace;

	public void Start ()
	{
		col = GetComponent<Collider> ();
		colliderScale = col.bounds.extents * 2f;
	}
		
	#region implemented abstract members of DynamicObject
	public override void SaveObject ()
	{
		
	}
	public override void LoadObject ()
	{
		
	}
	#endregion

	#region implemented abstract members of InteractableObject
	public override bool IsSatisfied (Actor challenger)
	{
		return false;	
	}
	public override void DoInteract (Actor challenger)
	{
		
	}
	public override void CancelInteract (Actor challenger)
	{
		
	}
	#endregion

	public bool GetFaceHideableState (HideableFaceName faceName)
	{
		for (int i = 0; i < hideableFace.Count; i++)
		{
			if (hideableFace [i].faceName == faceName)
			{
				return hideableFace [i].hideable;
			}
		}
		return false;
	}

	public HideableFace GetHideableFaceByName (HideableFaceName faceName)
	{
		for (int i = 0; i< hideableFace.Count; i++)
		{
			if (hideableFace [i].faceName == faceName)
			{
				return hideableFace [i];
			}
		}
		return new HideableFace();
	}
}
