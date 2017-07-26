using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideableObject : InteractableObject {

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
	
}
