using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoulderIdleChecker : ShoulderAnimationCheckerBase {
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
		return true;
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
