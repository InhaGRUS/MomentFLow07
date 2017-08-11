using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyShootWithStopChecker : BodyAnimationCheckerBase {
	public EnemyActor eActor;

	public float disToShoot = 1f;
	public float disToChase = 0.5f;

	[Header ("StateMaintainTimer")]
	public float stateMaintainDuration = 5f;
	public float stateMaintainTimer = 0f;

	[Header ("StateDelayTimer")]
	public float stateDelay = 2.5f;
	public float stateMaxDelay = 5f;
	public float stateMinDelay = 2.5f;
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
			velocity.magnitude <= eActor.customAgent.agent.speed &&
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
		stateDelay = Random.Range (stateMinDelay, stateMaxDelay);
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

			if (eActor.disToTarget > disToChase) {
				eActor.bodyAnimator.SetTrigger ("TriggerWalk");
				eActor.customAgent.SetDestination (eActor.targetActor.transform.position);
			}
			else {
				eActor.bodyAnimator.SetTrigger ("TriggerIdle");
				eActor.customAgent.StopMove ();
			}

			actor.aimTarget.AimToObject (eActor.targetActor.bodyCollider.bounds.center);
			actor.aimTarget.nowShootVector = (eActor.targetActor.bodyCollider.bounds.center - actor.aimTarget.shootPoint.position).normalized;
			actor.bodyAnimator.SetBool ("BoolAim", true);
			actor.shoulderAnimator.SetBool ("BoolAim", true);
		} else {
			if (eActor.disToTarget > disToChase) {
				eActor.bodyAnimator.SetTrigger ("TriggerWalk");
				eActor.shoulderAnimator.SetTrigger ("TriggerShoot");
				eActor.customAgent.SetDestination (eActor.targetActor.transform.position);
			}
			else {
				eActor.bodyAnimator.SetTrigger ("TriggerShoot");
				eActor.shoulderAnimator.SetTrigger ("TriggerShoot");
				eActor.customAgent.StopMove();
			}
			//SetAnimationTrigger ();
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
