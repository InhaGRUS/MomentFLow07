using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunAction : ActionBase {
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
		if (Input.GetKey (runKey) && Input.GetAxis ("Horizontal") != 0)
		{
			return true;
		}
		return false;
	}

	protected override void BeforeTransitionAction ()
	{
		
	}

	public override void DoSpecifiedAction ()
	{
		if (Input.GetAxis("Horizontal") < 0)
		{
			runDirection = Vector3.left;
			actor.SetLookDirection (true);
		}
		else if (Input.GetAxis("Horizontal") > 0)
		{
			runDirection = Vector3.right;
			actor.SetLookDirection (false);
		}

		actor.actorRigid.AddForce (runDirection * accel * actor.customDeltaTime, ForceMode.Force);

		actor.actorRigid.velocity = new Vector3 (Mathf.Clamp(actor.actorRigid.velocity.x, -maxRunSpeed, maxRunSpeed),
			actor.actorRigid.velocity.y,
			actor.actorRigid.velocity.z);

		nowActivated = true;
	}

	public override void CancelSpecifiedAction ()
	{
		nowActivated = false;
	}

	#endregion
}
