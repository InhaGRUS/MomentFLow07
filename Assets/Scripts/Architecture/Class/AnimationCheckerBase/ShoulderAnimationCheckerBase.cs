using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShoulderAnimationCheckerBase : AnimationCheckerBase {

	public List<string> shoulderOnlyAnimationTriggerName;
	public List<ShoulderAnimationCheckerBase> canTransitionActionList;


	protected override IEnumerator TransitionAction ()
	{
		for (int i = canTransitionActionList.Count - 1; i >= 0; i--)
		{
			if (canTransitionActionList [i].IsSatisfiedToAction())
			{
				if (actor.nowShoulderAction != canTransitionActionList [i]) {
					actor.nowShoulderAction = canTransitionActionList [i];
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
			animationIndex = Random.Range (0, setAnimationTriggerName.Count - 1);
		} 

		actor.bodyAnimator.SetTrigger (setAnimationTriggerName [animationIndex].bodyAnimationName);
		actor.shoulderAnimator.SetTrigger (setAnimationTriggerName [animationIndex].shoulderAnimationName);
	}
}
