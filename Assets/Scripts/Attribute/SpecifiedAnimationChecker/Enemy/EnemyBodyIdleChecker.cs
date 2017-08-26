using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyIdleChecker : BodyAnimationCheckerBase {
	EnemyActor eActor;
	// Use this for initialization
	protected new void Start () {
		base.Start ();
		eActor = EnemyActor.GetEnemyActor<Actor> (actor);
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
		eActor.ReleaseCrouch ();
		SetAnimationTrigger ();
		eActor.customAgent.StopMove ();
		if (null != eActor.targetActor)
		{
			eActor.GetEnemyOutsideInfo ().SetViewDirection (eActor.targetActor.bodyCollider.bounds.center);
		}
		//eActor.FindSuspiciousObject ();
		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		nowActivated = false;
	}
	#endregion


}
