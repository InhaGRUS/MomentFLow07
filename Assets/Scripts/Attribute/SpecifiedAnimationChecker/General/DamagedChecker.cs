using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedChecker : BodyAnimationCheckerBase {

	#region implemented abstract members of AnimationCheckerBase
	protected override bool CanTransition ()
	{
		return true;
	}
	protected override bool IsSatisfiedToAction ()
	{
		if (actor.stateInfo.isDamaged)
		{
			return true;
		}
		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		actor.stateInfo.isDamaged = false;
		nowActivated = false;
	}
	public override void DoSpecifiedAction ()
	{
		SetAnimationTrigger ();
		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		actor.stateInfo.isDamaged = false;
		nowActivated = false;
	}
	#endregion
}
