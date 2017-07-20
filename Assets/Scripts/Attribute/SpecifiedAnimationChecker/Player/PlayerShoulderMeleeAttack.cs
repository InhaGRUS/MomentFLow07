using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerShoulderMeleeAttack : ShoulderAnimationCheckerBase {
	public Collider attackRectCollider;
	public MeleeWeaponInfo nowEquipMeleeWeapon;

	public ParticleSystem attackParticle;

	private IEnumerator maintainCombo;

	public float comboDuration = 1;
	public int comboIndex = 0;
	public float comboTimer = 0;
	// Use this for initialization
	protected new void Start () {
		base.Start ();
		if (attackRectCollider == null)
			attackRectCollider = GetComponent <Collider> ();
		maintainCombo = MaintainCombo();
	}

	#region implemented abstract members of ActionBase

	protected override bool CanTransition ()
	{
		return true;
	}

	protected override bool IsSatisfiedToAction ()
	{
		if (Input.GetMouseButtonDown(0) &&
			actor.equipmentInfo.nowEquipWeaponType == EquipWeaponType.MeleeWeapon)
		{
			Debug.Log ("Satisfied for MeleeAttack");
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
		Debug.Log ("In MeleeAttack DoAction");
		attackRectCollider.enabled = true;
		if (nowEquipMeleeWeapon != (MeleeWeaponInfo)actor.equipmentInfo.nowEquipWeaponInfo)
		{
			nowEquipMeleeWeapon = (MeleeWeaponInfo)actor.equipmentInfo.nowEquipWeaponInfo;
			comboDuration = nowEquipMeleeWeapon.attackDelay * 2;
			comboTimer = 0;
			comboIndex = 0;
		}
			
		StopCoroutine (maintainCombo);
		maintainCombo = MaintainCombo ();
		StartCoroutine (maintainCombo);

		//After Init Animations
		//SetAnimationTrigger ();

		comboTimer = 0;
		nowActivated = true;
	}

	public override void CancelSpecifiedAction ()
	{
		attackRectCollider.enabled = false;
		nowActivated = false;
	}

	#endregion

	public void OnTriggerEnter (Collider col)
	{
		var actor = Actor.GetActor<Collider> (col);
		if (null != actor && actor.bodyCollider == col)
		{
			DamageTo <Actor> (actor);
		}
	}

	public bool DamageTo <T> (T to) where T : DynamicObject
	{
		if (typeof (T) == typeof (Actor))
		{
			var toObj = (Actor)Convert.ChangeType(to, typeof (Actor));
			toObj.humanInfo.hp -= nowEquipMeleeWeapon.damage;
			Debug.Log (this.actor.name + " Damage To " + to.name);
			attackParticle.Play ();
			comboIndex++;
			return true;
		}
		return false;
	}

	public IEnumerator MaintainCombo ()
	{
		while (comboTimer < comboDuration) {
			comboTimer += actor.customDeltaTime;
			yield return new WaitForEndOfFrame();
		}
		comboTimer = 0;
		comboIndex = 0;
	}
}
