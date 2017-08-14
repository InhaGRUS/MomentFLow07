using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyCrouchIdleChecker : BodyAnimationCheckerBase {
	public KeyCode crouchKey = KeyCode.C;

	// Use this for initialization
	protected new void Start () {
		base.Start ();
	}

	public void Update ()
	{
		canTransition = actor.stateInfo.isCrouhcing;
	}

	#region implemented abstract members of ActionBase

	protected override bool CanTransition ()
	{
		return canTransition;
	}

	protected override bool IsSatisfiedToAction ()
	{
		if ((!actor.stateInfo.isCrouhcing && Input.GetKeyDown (crouchKey)) || actor.stateInfo.isCrouhcing)
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
		actor.SetToCrouch ();
		nowActivated = true;
	}

	public override void CancelSpecifiedAction ()
	{
		nowActivated = false;
	}

	#endregion
}
