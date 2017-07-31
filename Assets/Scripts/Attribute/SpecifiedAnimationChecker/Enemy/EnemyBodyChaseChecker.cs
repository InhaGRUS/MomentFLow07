using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyChaseChecker : BodyAnimationCheckerBase {
	EnemyActor eActor;
	public float disToChase = 2f;
	public float disToTarget;
	// Use this for initialization
	void Start () {
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
		if (null == eActor.targetActor)
			return false;
		disToTarget = Vector3.Distance (eActor.targetActor.transform.position, eActor.transform.position);
		if (disToTarget > disToChase &&
			eActor.roomInfo.roomName == eActor.targetActor.roomInfo.roomName
		)
		{
			return true;
		}
		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		eActor.agent.SetDestination (eActor.transform.position);
		nowActivated = false;
	}
	public override void DoSpecifiedAction ()
	{
		SetAnimationTrigger ();
		eActor.agent.SetDestination (eActor.targetActor.transform.position);
	}
	public override void CancelSpecifiedAction ()
	{
		eActor.agent.SetDestination (eActor.transform.position);
		nowActivated = false;
	}
	#endregion
}
