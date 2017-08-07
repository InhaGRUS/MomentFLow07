using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BodyAnimationBase : AnimationBase {
	public List<AnimationClip> bodyOnlyAnimClips = new List<AnimationClip>();
	public List<AnimationClip> bodyAnimClips = new List<AnimationClip>();
	public List<AnimationClip> shoulderAnimClips = new List<AnimationClip> ();

	public void ChangeAnimationClipByRandom ()
	{
		if (actor.useShoulder) {
			if (bodyAnimClips.Count != 0) {
				animationIndex = Random.Range (0, bodyAnimClips.Count - 1);
				ChangeAnimationClipToIndex (animationIndex);
			}
		} else {
			if (bodyOnlyAnimClips.Count != 0) {
				animationIndex = Random.Range (0, bodyOnlyAnimClips.Count - 1);
				ChangeAnimationClipToIndex (animationIndex);
			}
		}
	}
		
	public void ChangeAnimationClipToIndex (int index)
	{
		AnimatorOverrideController aoc = new AnimatorOverrideController (actor.bodyAnimator.runtimeAnimatorController);
		var anims = new List<KeyValuePair<AnimationClip, AnimationClip>> ();

		if (!actor.useShoulder) {
			actor.nowBodyAnimationName = bodyOnlyAnimClips [index].name;
			foreach (var a in aoc.animationClips) {
				if (a.name == actor.nowBodyAnimationName) {	
					anims.Add (new KeyValuePair<AnimationClip, AnimationClip> (a, bodyOnlyAnimClips [index]));
					actor.nowBodyAnimationName = bodyOnlyAnimClips [index].name;
				}
				else {
					anims.Add (new KeyValuePair<AnimationClip, AnimationClip> (a, a));
				}
			}
			aoc.ApplyOverrides (anims);
			actor.bodyAnimator.runtimeAnimatorController = aoc;

			actor.bodyAnimator.Play (actor.nowBodyAnimationName);
		} 
		else {
			
			actor.nowBodyAnimationName = bodyAnimClips [animationIndex].name;
			actor.nowShoulderAnimationName = shoulderAnimClips [animationIndex].name;

			foreach (var a in aoc.animationClips) {
				if (a.name == actor.nowBodyAnimationName) {	
					anims.Add (new KeyValuePair<AnimationClip, AnimationClip> (a, bodyAnimClips [index]));
				}
				else {
					anims.Add (new KeyValuePair<AnimationClip, AnimationClip> (a, a));
				}
			}
			aoc.ApplyOverrides (anims);
			actor.bodyAnimator.runtimeAnimatorController = aoc;

			actor.bodyAnimator.Play (actor.nowBodyAnimationName);

			anims.Clear ();
			aoc = new AnimatorOverrideController (actor.shoulderAnimator.runtimeAnimatorController);
			foreach (var a in aoc.animationClips) {
				if (a.name == actor.nowShoulderAnimationName) {	
					anims.Add (new KeyValuePair<AnimationClip, AnimationClip> (a, shoulderAnimClips [index]));
				} 
				else {
					anims.Add (new KeyValuePair<AnimationClip, AnimationClip> (a, a));
				}
			}
			aoc.ApplyOverrides (anims);
			actor.shoulderAnimator.runtimeAnimatorController = aoc;
			actor.shoulderAnimator.Play (actor.nowShoulderAnimationName);
		}
	}

}
