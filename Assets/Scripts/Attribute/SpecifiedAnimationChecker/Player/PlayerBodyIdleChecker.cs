using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyIdleChecker : BodyAnimationCheckerBase {

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
		if (Input.GetAxis ("Horizontal") == 0 && Input.GetAxis ("Vertical") == 0 &&
			!actor.stateInfo.isCrouhcing
		)
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
		nowActivated = true;
	}

	public override void CancelSpecifiedAction ()
	{
		nowActivated = false;
	}

	#endregion
}
