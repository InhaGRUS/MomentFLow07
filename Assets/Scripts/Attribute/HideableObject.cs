using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HideableObject : InteractableObject {

	public float skinDepth = 0.1f;

	private Collider col;
	public Vector3 colliderExtents;
	public List<HideableFace> hideableFaceList = new List<HideableFace>();

	public LayerMask ignoreHideableCheckLayer;

	public void OnEnable ()
	{
		objectType = DynamicObjectType.InteractableObject;

		col = GetComponent<Collider> ();
		colliderExtents = col.bounds.extents;

		for (int i = 0; i < 6; i++)
		{
			hideableFaceList.Add (new HideableFace ());
			hideableFaceList [i].hideableObj = this;
			hideableFaceList [i].faceName = (HideableFaceName)i;

			if (i == 0){
				hideableFaceList [i].point = Vector3.left * (col.bounds.extents.x + skinDepth);
				continue;
			}
			if (i == 1) {
				hideableFaceList [i].point = Vector3.right * (col.bounds.extents.x + skinDepth);
				continue;
			}
			if (i == 2) {
				hideableFaceList [i].point = Vector3.forward * (col.bounds.extents.z + skinDepth);
				continue;
			}
			if (i == 3) {
				hideableFaceList [i].point = Vector3.back * (col.bounds.extents.z + skinDepth);
				continue;
			}
			if (i == 4) {
				hideableFaceList [i].point = Vector3.up * (col.bounds.extents.y + skinDepth);
				continue;
			}
			if (i == 5) {
				hideableFaceList [i].point = Vector3.down * (col.bounds.extents.y + skinDepth);
				continue;
			}
		}
	}

	public void Start ()
	{
		
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
		for (int i = 0; i < hideableFaceList.Count; i++)
		{
			if (hideableFaceList [i].faceName == faceName)
			{
				return hideableFaceList [i].hideable;
			}
		}
		return false;
	}

	public HideableFace GetHideableFaceByName (HideableFaceName faceName)
	{
		for (int i = 0; i< hideableFaceList.Count; i++)
		{
			if (hideableFaceList [i].faceName == faceName)
			{
				return hideableFaceList [i];
			}
		}
		return new HideableFace();
	}
}
