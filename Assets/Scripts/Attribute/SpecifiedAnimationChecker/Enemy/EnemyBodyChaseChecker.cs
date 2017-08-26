using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyChaseChecker : BodyAnimationCheckerBase {
	public EnemyActor eActor;
	public float disToChase = 2f;

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
		if (null == eActor.targetActor) {
			return false;
		} 
		if (eActor.disToTarget > disToChase
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
		eActor.ReleaseCrouch ();

		SetAnimationTrigger ();
		//eActor.FindSuspiciousObject ();
		if (null != eActor.targetActor) {
			eActor.customAgent.SetDestination (eActor.targetActor.transform.position);
		}
		else {
			eActor.customAgent.SetDestination (eActor.lastTargetPoint);
		}
		eActor.GetEnemyOutsideInfo ().SetViewDirection (eActor.customAgent.agent.destination);
		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		nowActivated = false;
	}
	#endregion
}
