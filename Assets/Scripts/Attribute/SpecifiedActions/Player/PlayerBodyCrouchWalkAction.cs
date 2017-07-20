using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyCrouchWalkAction : BodyActionBase {
	public KeyCode standUpKey;

	public float accel;
	public float maxWalkSpeed;
	public Vector3 walkDirection;

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
		if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) &&
			actor.stateInfo.isCrouhcing
		)	
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
		Walk ();

		if (Input.GetKeyDown (standUpKey)) {
			actor.stateInfo.isCrouhcing = false;
		}

		nowActivated = true;
	}

	public override void CancelSpecifiedAction ()
	{
		nowActivated = false;
	}

	#endregion

	void Walk ()
	{
		if (Input.GetAxis ("Horizontal") < 0) {
			walkDirection.x = -1;
			actor.SetLookDirection (true);
		}
		else
			if (Input.GetAxis ("Horizontal") > 0) {
				walkDirection.x = 1;
				actor.SetLookDirection (false);
			}
		if (Input.GetAxis ("Vertical") < 0) {
			walkDirection.z = -1;
		}
		else
			if (Input.GetAxis ("Vertical") > 0) {
				walkDirection.z = 1;
			}
		
		walkDirection = walkDirection.normalized;

		float tmpMaxWalkSpeedX = Mathf.Abs (maxWalkSpeed * walkDirection.x);
		float tmpMaxWalkSpeedZ = Mathf.Abs (maxWalkSpeed * walkDirection.z);
		actor.actorRigid.AddForce (walkDirection * accel * actor.customDeltaTime, ForceMode.Force);
		actor.actorRigid.velocity = new Vector3 (Mathf.Clamp (actor.actorRigid.velocity.x, -tmpMaxWalkSpeedX, tmpMaxWalkSpeedX), actor.actorRigid.velocity.y, Mathf.Clamp (actor.actorRigid.velocity.z, -tmpMaxWalkSpeedZ, tmpMaxWalkSpeedZ));
		walkDirection = Vector3.zero;
	}
}
