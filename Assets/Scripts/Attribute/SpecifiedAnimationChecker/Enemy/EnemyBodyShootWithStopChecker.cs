using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyShootWithStopChecker : BodyAnimationCheckerBase {
	public EnemyActor eActor;

	public float disToShoot = 1f;

	[Header ("StateMaintainTimer")]
	public float stateMaintainDuration = 5f;
	public float stateMaintainTimer = 0f;

	[Header ("StateDelayTimer")]
	public float stateDelay = 2.5f;
	public float stateDelayTimer = 0f;

	[Header ("AimMaintainTimer")]
	public float attackDelay = 1.5f;
	public float attackTimer = 0f;

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
		var velocity = eActor.actorVelocity;
		velocity.y = 0;

		if (null != eActor.targetActor && 
			!eActor.stateInfo.isHiding &&
			velocity.magnitude <= eActor.agent.speed &&
			eActor.disToTarget <= disToShoot &&
			stateDelayTimer >= stateDelay &&
			eActor.equipmentInfo.nowEquipWeaponType == EquipWeaponType.Gun &&
			stateMaintainTimer <= stateMaintainDuration
		) {
			return true;
		}
		stateDelayTimer += actor.customDeltaTime;
		return false;
	}

	protected override void BeforeTransitionAction ()
	{
		stateMaintainTimer = 0f;
		stateDelayTimer = 0f;
		attackTimer = 0f;

		nowActivated = false;
		actor.aimTarget.AimToForward ();
		actor.bodyAnimator.SetBool ("BoolAim", false);
		actor.shoulderAnimator.SetBool ("BoolAim", false);
	}

	public override void DoSpecifiedAction ()
	{
		stateMaintainTimer += actor.customDeltaTime;

		eActor.GetEnemyOutsideInfo ().SetViewDirection (eActor.targetActor.transform.position);

		if (attackTimer <= attackDelay) {
			attackTimer += actor.customDeltaTime;
			eActor.agent.SetDestination (eActor.transform.position);
			actor.aimTarget.AimToObject (eActor.targetActor.bodyCollider.bounds.center);
			actor.bodyAnimator.SetBool ("BoolAim", true);
			actor.shoulderAnimator.SetBool ("BoolAim", true);
		} else {
			SetAnimationTrigger ();
			attackTimer = 0f;
		}

		nowActivated = true;
	}

	public override void CancelSpecifiedAction ()
	{
		stateMaintainTimer = 0f;
		stateDelayTimer = 0f;
		attackTimer = 0f;

		nowActivated = false;
		actor.aimTarget.AimToForward ();
		actor.bodyAnimator.SetBool ("BoolAim", false);
		actor.shoulderAnimator.SetBool ("BoolAim", false);
	}

	#endregion
}
