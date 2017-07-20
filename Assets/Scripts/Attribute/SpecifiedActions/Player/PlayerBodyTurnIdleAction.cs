using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyTurnIdleAction : BodyActionBase {

	public bool turnFinish = false;

	// Use this for initialization
	protected new void Start () {
		base.Start ();
	}

	#region implemented abstract members of ActionBase
	protected override bool CanTransition ()
	{
		return turnFinish;
	}
	protected override bool IsSatisfiedToAction ()
	{
		var signOfHorizontalInput = Mathf.Sign (Input.GetAxis ("Horizontal"));
		Debug.Log (signOfHorizontalInput);
		if (!actor.stateInfo.isCrouhcing && signOfHorizontalInput == Mathf.Sign (actor.transform.localScale.x) && Input.GetAxis ("Horizontal") != 0) {
			return true;	
		}
		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		turnFinish = false;
		nowActivated = false;
	}
	public override void DoSpecifiedAction ()
	{
		SetAnimationTrigger ();
		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		turnFinish = false;
		nowActivated = false;
	}
	#endregion
}
