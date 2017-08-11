using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyShootWithWalkChecker : BodyAnimationCheckerBase {

	public EnemyActor eActor;

	public float disToShoot = 1f;
	public float disToChase = 0.5f;

	[Header ("StateMaintainTimer")]
	public float stateMaintainDuration = 5f;
	public float stateMaintainTimer = 0f;

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
		if (null != eActor.targetActor &&
			eActor.equipmentInfo.nowEquipWeaponType == EquipWeaponType.Gun &&
			eActor.disToTarget >= disToShoot &&
			stateMaintainTimer <= stateMaintainDuration)
		{
			return true;
		}
		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		stateMaintainTimer = 0f;
		attackTimer = 0f;

		actor.aimTarget.AimToForward ();
		actor.bodyAnimator.SetBool ("BoolAim", false);
		actor.shoulderAnimator.SetBool ("BoolAim", false);

		nowActivated = false;
	}
	public override void DoSpecifiedAction ()
	{
		stateMaintainTimer += actor.customDeltaTime;

		eActor.GetEnemyOutsideInfo ().SetViewDirection (eActor.targetActor.transform.position);

		//Aim State
		if (attackTimer <= attackDelay) {
			attackTimer += actor.customDeltaTime;
			if (eActor.disToTarget >= disToChase)
				eActor.customAgent.SetDestination (eActor.targetActor.transform.position);
			else
				eActor.customAgent.SetDestination (eActor.transform.position);
			actor.aimTarget.AimToObject (eActor.targetActor.bodyCollider.bounds.center);
			actor.aimTarget.nowShootVector = (eActor.targetActor.bodyCollider.bounds.center - actor.aimTarget.shootPoint.position).normalized;
			actor.bodyAnimator.SetBool ("BoolAim", true);
			actor.shoulderAnimator.SetBool ("BoolAim", true);
		}
		else { // Shoot State
			SetAnimationTrigger ();
			attackTimer = 0f;
		}

		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		stateMaintainTimer = 0f;
		attackTimer = 0f;

		actor.aimTarget.AimToForward ();
		actor.bodyAnimator.SetBool ("BoolAim", false);
		actor.shoulderAnimator.SetBool ("BoolAim", false);

		nowActivated = false;
	}
	#endregion
}
