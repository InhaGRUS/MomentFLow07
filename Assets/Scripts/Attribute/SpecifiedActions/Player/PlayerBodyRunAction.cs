using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyRunAction : BodyActionBase {
	public KeyCode crouchKey = KeyCode.C;

	public KeyCode runKey;
	public float accel;
	public float maxRunSpeed;
	public Vector3 runDirection;

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
		if ((Input.GetKey (runKey) && (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0)) &&
			!actor.stateInfo.isCrouhcing
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

		Run ();

		if (Input.GetKeyDown (crouchKey))
			actor.stateInfo.isCrouhcing = true;

		nowActivated = true;
	}

	public override void CancelSpecifiedAction ()
	{
		nowActivated = false;
	}

	#endregion

	void Run ()
	{
		if (Input.GetAxis ("Horizontal") < 0) {
			runDirection.x = -1;
			actor.SetLookDirection (true);
		}
		else
			if (Input.GetAxis ("Horizontal") > 0) {
				runDirection.x = 1;
				actor.SetLookDirection (false);
			}
		if (Input.GetAxis ("Vertical") < 0) {
			runDirection.z = -1;
		}
		else
			if (Input.GetAxis ("Vertical") > 0) {
				runDirection.z = 1;
			}
		runDirection = runDirection.normalized;
		float tmpMaxRunSpeedX = Mathf.Abs (maxRunSpeed * runDirection.x);
		float tmpMaxRunSpeedZ = Mathf.Abs (maxRunSpeed * runDirection.z);
		actor.actorRigid.AddForce (runDirection * accel * actor.customDeltaTime, ForceMode.Force);
		actor.actorRigid.velocity = new Vector3 (Mathf.Clamp (actor.actorRigid.velocity.x, -tmpMaxRunSpeedX, tmpMaxRunSpeedX), actor.actorRigid.velocity.y, Mathf.Clamp (actor.actorRigid.velocity.z, -tmpMaxRunSpeedZ, tmpMaxRunSpeedZ));
	}
}
