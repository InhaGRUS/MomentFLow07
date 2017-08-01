using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoulderShootChecker : ShoulderAnimationCheckerBase {

	private PlayerShoulderAimChecker aimChecker;

	// Use this for initialization
	protected new void Start () {
		base.Start ();
		aimChecker = actor.GetSpecificAction <PlayerShoulderAimChecker> ();
	}

	#region implemented abstract members of ActionBase
	protected override bool CanTransition ()
	{
		return true;
	}
	protected override bool IsSatisfiedToAction ()
	{
		if (actor.useShoulder && Input.GetMouseButton(0) && actor.equipmentInfo.nowEquipWeaponType == EquipWeaponType.Gun)
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
		SetAnimationTrigger ();

		if (actor.roomInfo.roomState != RoomState.Combat)
		{
			actor.roomInfo.SetRoomStateToCombatState (aimChecker.aimStartPoint.position);
		}

		actor.GetSpecificAction <PlayerShoulderAimChecker> ().PlayerAimRender ();
		actor.aimTarget.AimToObject (aimChecker.lineRenderer.GetPosition(1));
		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		nowActivated = false;
		actor.bodyAnimator.ResetTrigger ("TriggerShoot");
		actor.shoulderAnimator.ResetTrigger ("TriggerShoot");
		actor.ResetSetLookDirectionPriority ();
	}
	#endregion
}
