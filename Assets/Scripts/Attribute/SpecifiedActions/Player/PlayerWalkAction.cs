using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkAction : ActionBase {

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
		if (Input.GetAxis("Horizontal") != 0)	
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
			walkDirection = Vector3.left;
			actor.SetLookDirection (true);
		}
		else if (Input.GetAxis("Horizontal") > 0)
		{
			walkDirection = Vector3.right;
			actor.SetLookDirection (false);
		}

		actor.actorRigid.AddForce (walkDirection * accel * actor.customDeltaTime, ForceMode.Force);

		actor.actorRigid.velocity = new Vector3 (Mathf.Clamp(actor.actorRigid.velocity.x, -maxWalkSpeed, maxWalkSpeed),
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
