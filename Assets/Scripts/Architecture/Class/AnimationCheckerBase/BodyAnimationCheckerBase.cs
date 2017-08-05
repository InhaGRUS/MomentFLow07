using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BodyAnimationCheckerBase : AnimationCheckerBase {
	
	public List<BodyAnimationCheckerBase> canTransitionActionList;

	public List<string> bodyOnlyAnimationTriggerName;

	protected override IEnumerator TransitionAction ()
	{
		for (int i = canTransitionActionList.Count - 1; i >= 0; i--)
		{
			if (null != canTransitionActionList[i] && canTransitionActionList [i].IsSatisfiedToAction())
			{
				if (actor.nowBodyAction != canTransitionActionList [i]) {
					actor.nowBodyAction = canTransitionActionList [i];
					BeforeTransitionAction ();
					yield return new WaitForEndOfFrame ();
					actor.nowBodyAction.DoSpecifiedAction ();
				}
				break;
			}
		}
	}

	public void SetAnimationTrigger ()
	{
		if (animationIndex == -1) {
			if (actor.useShoulder)
				animationIndex = Random.Range (0, setAnimationTriggerName.Count - 1);
			else
				animationIndex = Random.Range (0, bodyOnlyAnimationTriggerName.Count - 1);
		} 
		if (actor.useShoulder) {
			if (setAnimationTriggerName [animationIndex].bodyAnimationName != "")
				actor.bodyAnimator.SetTrigger (setAnimationTriggerName [animationIndex].bodyAnimationName);
			if (setAnimationTriggerName [animationIndex].shoulderAnimationName != "")
				actor.shoulderAnimator.SetTrigger (setAnimationTriggerName [animationIndex].shoulderAnimationName);
		} else {
			if (setAnimationTriggerName [animationIndex].bodyAnimationName != "")
				actor.bodyAnimator.SetTrigger (bodyOnlyAnimationTriggerName [animationIndex]);
		}
	}

	public void SetAnimationTrigger (int index)
	{
		if (actor.useShoulder) {
			if (setAnimationTriggerName [index].bodyAnimationName != "")
				actor.bodyAnimator.SetTrigger (setAnimationTriggerName [index].bodyAnimationName);
			if (setAnimationTriggerName [index].shoulderAnimationName != "")
				actor.shoulderAnimator.SetTrigger (setAnimationTriggerName [index].shoulderAnimationName);
		} else {
			if (setAnimationTriggerName [index].bodyAnimationName != "")
				actor.bodyAnimator.SetTrigger (bodyOnlyAnimationTriggerName [index]);
		}
	}
}
