using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyRunAnimation : BodyAnimationBase {

	public float accel;
	public float maxRunSpeed;
	public Vector3 runDirection;

	#region implemented abstract members of AnimationBase
	protected override void OnAnimationEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		ChangeAnimationClipByRandom ();
	}
	protected override void OnAnimationStay (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Run ();
	}
	protected override void OnAnimationExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		
	}
	#endregion	

	void Run ()
	{
		if (Input.GetAxis ("Horizontal") < 0) {
			runDirection.x = -1;
			actor.SetLookDirection (true);
		} else if (Input.GetAxis ("Horizontal") > 0) {
			runDirection.x = 1;
			actor.SetLookDirection (false);
		}
		if (Input.GetAxis ("Vertical") < 0) {
			runDirection.z = -1;
		} else if (Input.GetAxis ("Vertical") > 0) {
			runDirection.z = 1;
		}

		runDirection =	runDirection.normalized;

		float tmpMaxWalkSpeedX = Mathf.Abs (maxRunSpeed * runDirection.x);
		float tmpMaxWalkSpeedZ = Mathf.Abs (maxRunSpeed * runDirection.z);
		actor.actorRigid.AddForce (runDirection * accel * actor.customDeltaTime, ForceMode.Force);
		actor.actorRigid.velocity = new Vector3 (Mathf.Clamp (actor.actorRigid.velocity.x, -tmpMaxWalkSpeedX, tmpMaxWalkSpeedX), actor.actorRigid.velocity.y, Mathf.Clamp (actor.actorRigid.velocity.z, -tmpMaxWalkSpeedZ, tmpMaxWalkSpeedZ));
		runDirection = Vector3.zero;
	}
}
