using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoulderIdleAnimation : ShoulderAnimationBase {
	public bool isSlacking = false;
	public float slackTimeOffset = 2f;
	public float slackTimer = 0f;

	#region implemented abstract members of AnimationBase
	protected override void OnAnimationEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		slackTimer = 0f;
		isSlacking = false;
	}
	protected override void OnAnimationStay (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!isSlacking)
		{
			slackTimer += actor.customDeltaTime;
			if (slackTimer >= slackTimeOffset) {
				ChangeAnimationClipByRandom ();
				isSlacking = true;
			}
		}
	}
	protected override void OnAnimationExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		
	}
	#endregion
	
}
