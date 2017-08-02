using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyChaseChecker : BodyAnimationCheckerBase {
	public EnemyActor eActor;
	public float disToChase = 2f;
	public float disToTarget;

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
			if (!eActor.roomInfo.roomRectCollider.bounds.Contains (eActor.lastTargetPoint)) {
				return false;
			}
			disToTarget = Vector3.Distance (eActor.lastTargetPoint, eActor.transform.position);
			if (Mathf.Approximately (disToTarget, 0))
				return false;
			if (eActor.roomInfo.roomState == RoomState.Combat)
				return true;
		} 
		else {
			disToTarget = Vector3.Distance (eActor.targetActor.transform.position, eActor.transform.position);
			if (disToTarget > disToChase &&
				eActor.roomInfo.roomName == eActor.targetActor.roomInfo.roomName
			)
			{
				return true;
			}
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
		Debug.Log ("Chase");
		SetAnimationTrigger ();
		eActor.FindSuspiciousObject ();
		if (null != eActor.targetActor) {
			eActor.agent.SetDestination (eActor.targetActor.transform.position);
		}
		else {
			eActor.agent.SetDestination (eActor.lastTargetPoint);
		}
		eActor.GetEnemyOutsideInfo ().SetViewDirection (eActor.agent.destination);
	}
	public override void CancelSpecifiedAction ()
	{
		eActor.agent.SetDestination (eActor.transform.position);
		nowActivated = false;
	}
	#endregion
}
