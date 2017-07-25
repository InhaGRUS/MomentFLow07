using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : DynamicObject {

	public int interactCount = 0;

	// Use this for initialization
	void Start (){
	}

	public abstract bool IsSatisfied (Actor challenger);

	public virtual void TryInteract (Actor challenger)
	{
		if (IsSatisfied (challenger))
		{
			DoInteract (challenger);
			return;
		}
		CancelInteract (challenger);
	}

	public abstract void DoInteract (Actor challenger);

	public abstract void CancelInteract (Actor challenger);
}
