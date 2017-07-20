using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShoulderAnimationBase : AnimationBase {
	public List<AnimationClip> shoulderOnlyAnimClips = new List<AnimationClip>();

	// 이 때는 ShoulderOnlyAnimationClip을 사용 합니다.
	public void ChangeAnimationClipByRandom ()
	{
		if (shoulderOnlyAnimClips.Count != 0)
		{
			var randomIndex = Random.Range (0, shoulderOnlyAnimClips.Count - 1);
			ChangeAnimationClipToIndex (randomIndex);
		}
	}

	public void ChangeAnimationClipToIndex (int index)
	{
		AnimatorOverrideController aoc = new AnimatorOverrideController (actor.shoulderAnimator.runtimeAnimatorController);
		var anims = new List<KeyValuePair<AnimationClip, AnimationClip>> ();

		actor.nowBodyAnimationName = shoulderOnlyAnimClips [index].name;
		foreach (var a in aoc.animationClips) {
			if (a.name == actor.nowShoulderAnimationName) {	
				anims.Add (new KeyValuePair<AnimationClip, AnimationClip> (a, shoulderOnlyAnimClips [index]));
			} else {
				anims.Add (new KeyValuePair<AnimationClip, AnimationClip> (a, a));
			}
		}
		aoc.ApplyOverrides (anims);
		actor.shoulderAnimator.runtimeAnimatorController = aoc;
	}
}
