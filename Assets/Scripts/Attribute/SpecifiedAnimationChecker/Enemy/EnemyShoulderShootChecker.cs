using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoulderShootChecker : ShoulderAnimationCheckerBase {
	public EnemyActor eActor;

	public float shootDistance = 1f;

	public float attackDelay = 1f;
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
			!actor.stateInfo.isHiding &&
			actor.equipmentInfo.nowEquipWeaponType == EquipWeaponType.Gun &&
			Vector3.Distance (actor.transform.position, eActor.targetActor.transform.position) <= shootDistance
		) {
			return true;
		}
		return false;
	}

	protected override void BeforeTransitionAction ()
	{
		actor.shoulderAnimator.ResetTrigger ("TriggerAim");
		nowActivated = false;
	}

	public override void DoSpecifiedAction ()
	{
		if (attackTimer < attackDelay) {
			attackTimer += actor.customDeltaTime;
			eActor.aimTarget.AimToObject (eActor.targetActor.transform.position);
			actor.shoulderAnimator.SetTrigger ("TriggerAim");
		} else {
			attackTimer = 0f;
			SetAnimationTrigger ();
		}
		nowActivated = true;
	}

	public override void CancelSpecifiedAction ()
	{
		actor.shoulderAnimator.ResetTrigger ("TriggerAim");
		nowActivated = false;
	}
	#endregion
}
