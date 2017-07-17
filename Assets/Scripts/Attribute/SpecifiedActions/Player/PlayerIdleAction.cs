using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleAction : ActionBase {

	public int animationIndex = -1;

	// Use this for initialization
	protected new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected new void Update () {
		base.Start ();
	}

	#region implemented abstract members of ActionBase

	protected override bool CanTransition ()
	{
		return true;
	}
	protected override bool IsSatisfiedToAction ()
	{
		if (Input.GetAxis ("Horizontal") == 0)
		{
			return true;
		}
		return false;
	}

	protected override void BeforeTransitionAction ()
	{

	}

	public override void DoSpecifiedAction ()
	{
		if (animationIndex == -1) {
			if (actor.useShoulder) {
				animationIndex = Random.Range (0, shoulderAnimationTriggerName.Length - 1);
			} else {
				animationIndex = Random.Range (shoulderAnimationTriggerName.Length - 1, bodyAnimationTriggerName.Length - 1);
			}
		} else {
			actor.bodyAnimator.SetTrigger (bodyAnimationTriggerName[animationIndex]);
			actor.shoulderAnimator.SetTrigger (shoulderAnimationTriggerName[animationIndex]);
		}

		nowActivated = true;
	}

	public override void CancelSpecifiedAction ()
	{
		animationIndex = -1;
		nowActivated = false;
	}

	#endregion
}
