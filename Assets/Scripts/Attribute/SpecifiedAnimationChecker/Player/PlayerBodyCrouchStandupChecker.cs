using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyCrouchStandupChecker : BodyAnimationCheckerBase {

	public KeyCode standUpKey = KeyCode.C;

	// Use this for initialization
	protected new void Start () {
		base.Start ();
	}
		
	#region implemented abstract members of ActionBase
	protected override bool CanTransition ()
	{
		return true; // Animation Behaviour에서 true로 만듦
	}
	protected override bool IsSatisfiedToAction ()
	{
		if (Input.GetKeyDown (standUpKey) && actor.stateInfo.isCrouhcing)
			return true;
		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		nowActivated = false;
	}
	public override void DoSpecifiedAction ()
	{
		actor.ReleaseCrouch ();
		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		nowActivated = false;
	}
	#endregion
}
