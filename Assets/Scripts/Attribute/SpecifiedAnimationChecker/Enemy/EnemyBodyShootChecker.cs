using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyShootChecker : BodyAnimationCheckerBase {
	public EnemyActor eActor;

	// Use this for initialization
	protected new void Start () {
		base.Start ();
		eActor = EnemyActor.GetEnemyActor <Actor> (actor);
	}

	#region implemented abstract members of AnimationCheckerBase

	protected override bool CanTransition ()
	{
		return true;
	}

	protected override bool IsSatisfiedToAction ()
	{
		if (null != eActor.targetActor) {
			return true;
		}
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
