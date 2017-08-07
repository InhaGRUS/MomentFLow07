using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyCrouchIdleAnimation : BodyAnimationBase {
	#region implemented abstract members of AnimationBase
	protected override void OnAnimationEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		ChangeAnimationClipToIndex (0);
		if (actor.stateInfo.isHiding) {
			ChangeAnimationClipToIndex (1);
			Debug.Log (animationIndex);
		} 
	}
	protected override void OnAnimationStay (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		
	}
	protected override void OnAnimationExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		
	}
	#endregion	
}
