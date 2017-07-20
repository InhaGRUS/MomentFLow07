using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyCrouchAnimation : BodyAnimationBase {
	#region implemented abstract members of AnimationBase

	protected override void OnAnimationEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		
	}

	protected override void OnAnimationStay (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		
	}

	protected override void OnAnimationExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		actor.stateInfo.isCrouhcing = true;
	}

	#endregion



}
