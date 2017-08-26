using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodySuspiciousChecker : BodyAnimationCheckerBase {
	public bool isFoundSuspiciousPoint = false;
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

		/*if (null != eActor.suspiciousActor &&
			null == eActor.targetActor &&
			eActor.disToSuspiciousPoint > 0.1f
		)
		{
			return true;
		}*/

		if (null == eActor.targetActor &&
			isFoundSuspiciousPoint
		) {
			return true;
		}

		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		//isFoundSuspiciousPoint = false;
		nowActivated = false;
	}
	public override void DoSpecifiedAction ()
	{
		if (eActor.disToSuspiciousPoint > 0.1f) {
			eActor.ReleaseCrouch ();
			SetAnimationTrigger ();
			//eActor.FindSuspiciousObject ();
			eActor.customAgent.SetDestination (eActor.suspiciousPoint);
			eActor.GetEnemyOutsideInfo ().SetViewDirection (eActor.customAgent.agent.destination);
		} else {
			isFoundSuspiciousPoint = false;
		}
		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		//isFoundSuspiciousPoint = false;
		nowActivated = false;
	}
	#endregion
}
