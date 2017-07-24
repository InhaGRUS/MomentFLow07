using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoulderShootChecker : ShoulderAnimationCheckerBase {

	// Use this for initialization
	protected new void Start () {
		base.Start ();
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

		actor.GetSpecificAction <PlayerShoulderAimChecker> ().PlayerAimRender ();
		actor.aimTarget.AimToObject (actor.GetSpecificAction<PlayerShoulderAimChecker>().lineRenderer.GetPosition(1));
		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		nowActivated = false;
		actor.ResetSetLookDirectionPriority ();
	}
	#endregion
}
