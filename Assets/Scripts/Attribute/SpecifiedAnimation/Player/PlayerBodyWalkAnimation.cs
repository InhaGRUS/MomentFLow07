using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyWalkAnimation : BodyAnimationBase {

	public float accel;
	public float maxWalkSpeed;
	public Vector3 walkDirection;

	#region implemented abstract members of AnimationBase

	protected override void OnAnimationEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		ChangeAnimationClipByRandom ();
	}

	protected override void OnAnimationStay (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Walk ();
	}

	protected override void OnAnimationExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		
	}

	#endregion

	void Walk ()
	{
		if (Input.GetAxis ("Horizontal") < 0) {
			walkDirection.x = -1;
			actor.SetLookDirection (true);

			if (Mathf.Sign (walkDirection.x) != -Mathf.Sign (actor.transform.localScale.x))
				actor.bodyAnimator.SetFloat ("WalkDirection", -1f);
			else
				actor.bodyAnimator.SetFloat ("WalkDirection", 1f);
		} else if (Input.GetAxis ("Horizontal") > 0) {
			walkDirection.x = 1;
			actor.SetLookDirection (false);

			if (Mathf.Sign (walkDirection.x) != -Mathf.Sign (actor.transform.localScale.x))
				actor.bodyAnimator.SetFloat ("WalkDirection", -1f);
			else
				actor.bodyAnimator.SetFloat ("WalkDirection", 1f);
		}
		if (Input.GetAxis ("Vertical") < 0) {
			walkDirection.z = -1;
		} else if (Input.GetAxis ("Vertical") > 0) {
			walkDirection.z = 1;
		}
			
		walkDirection =	walkDirection.normalized;

		float tmpMaxWalkSpeedX = Mathf.Abs (maxWalkSpeed * walkDirection.x);
		float tmpMaxWalkSpeedZ = Mathf.Abs (maxWalkSpeed * walkDirection.z);
		actor.actorRigid.AddForce (walkDirection * accel * actor.customDeltaTime, ForceMode.Force);
		actor.actorRigid.velocity = new Vector3 (Mathf.Clamp (actor.actorRigid.velocity.x, -tmpMaxWalkSpeedX, tmpMaxWalkSpeedX), actor.actorRigid.velocity.y, Mathf.Clamp (actor.actorRigid.velocity.z, -tmpMaxWalkSpeedZ, tmpMaxWalkSpeedZ));
		walkDirection = Vector3.zero;
	}
}
