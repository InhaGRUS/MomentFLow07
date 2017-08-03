using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodySuspiciousChecker : BodyAnimationCheckerBase {
	public bool foundSuspiciousObject = false;
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
		if (foundSuspiciousObject && 
			null == eActor.targetActor &&
			eActor.disToSuspiciousPoint > 0.1f
		)
		{
			return true;
		}
		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		foundSuspiciousObject = false;
		nowActivated = false;
	}
	public override void DoSpecifiedAction ()
	{
		SetAnimationTrigger ();
		eActor.FindSuspiciousObject ();
		eActor.agent.SetDestination (eActor.suspiciousPoint);
		eActor.GetEnemyOutsideInfo ().SetViewDirection (eActor.agent.destination);
		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		foundSuspiciousObject = false;
		nowActivated = false;
	}
	#endregion
}
