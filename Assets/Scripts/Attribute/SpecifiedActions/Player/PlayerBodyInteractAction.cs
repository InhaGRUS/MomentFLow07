using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyInteractAction : BodyActionBase {
	
	public KeyCode interactKey = KeyCode.F;

	// Use this for initialization
	void Start () {
		base.Start ();
		canTransition = false;
	}

	#region implemented abstract members of ActionBase

	protected override bool CanTransition ()
	{
		return canTransition;
	}

	protected override bool IsSatisfiedToAction ()
	{
		throw new System.NotImplementedException ();
	}

	protected override void BeforeTransitionAction ()
	{
		throw new System.NotImplementedException ();
	}

	public override void DoSpecifiedAction ()
	{
		throw new System.NotImplementedException ();
	}

	public override void CancelSpecifiedAction ()
	{
		throw new System.NotImplementedException ();
	}

	#endregion
}
