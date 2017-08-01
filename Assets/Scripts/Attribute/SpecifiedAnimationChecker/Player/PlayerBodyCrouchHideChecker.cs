using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyCrouchHideChecker : BodyAnimationCheckerBase {

	// Use this for initialization
	protected new void Start () {
		base.Start ();
	}

	#region implemented abstract members of AnimationCheckerBase
	protected override bool CanTransition ()
	{
		return true;
	}
	protected override bool IsSatisfiedToAction ()
	{
		if (actor.stateInfo.isCrouhcing && 
			null != actor.outsideInfo.FindNearestObstacle() &&
			Input.GetAxis ("Horizontal") == 0 && Input.GetAxis ("Vertical") == 0)
		{
			return true;
		}
		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		actor.stateInfo.isHiding = false;
		nowActivated = false;
	}
	public override void DoSpecifiedAction ()
	{
		SetAnimationTrigger ();

		if (!actor.stateInfo.isHiding)
		{
			actor.stateInfo.isHiding = true;
		}
		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		actor.stateInfo.isHiding = false;
		nowActivated = false;
	}
	#endregion
}
