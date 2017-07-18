using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyCrouchIdleAction : BodyActionBase {
	public KeyCode standUpKey;

	// Use this for initialization
	protected new void Start () {
		base.Start ();
	}

	#region implemented abstract members of ActionBase

	protected override bool CanTransition ()
	{
		return true;
	}

	protected override bool IsSatisfiedToAction ()
	{
		if (actor.stateInfo.isCrouhcing)
		{
			return true;
		}
		return false;
	}

	protected override void BeforeTransitionAction ()
	{
		nowActivated = false;
	}

	public override void DoSpecifiedAction ()
	{
		SetAnimationTrigger ();

		if (Input.GetKeyDown (standUpKey)) {
			actor.stateInfo.isCrouhcing = false;
		}
		
		nowActivated = true;
	}

	public override void CancelSpecifiedAction ()
	{
		nowActivated = false;
	}

	#endregion
}
