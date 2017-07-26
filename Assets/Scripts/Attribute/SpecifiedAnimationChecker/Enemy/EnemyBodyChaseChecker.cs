using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyChaseChecker : BodyAnimationCheckerBase {

	// Use this for initialization
	void Start () {
		base.Start ();
	}

	#region implemented abstract members of AnimationCheckerBase
	protected override bool CanTransition ()
	{
		return true;
	}
	protected override bool IsSatisfiedToAction ()
	{
		return false;
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
