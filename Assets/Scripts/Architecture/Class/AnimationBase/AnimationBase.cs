using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationBase : StateMachineBehaviour {
	public bool isInitted = false;
	public Actor actor;
	public int animationIndex = -1;

	public void Init (Animator animator,  AnimatorStateInfo stateInfo)
	{
		if (null == actor)
			actor = Actor.GetActor <Animator> (animator);
		isInitted = true;
	}

	protected abstract void OnAnimationEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex);

	protected abstract void OnAnimationStay (Animator animator, AnimatorStateInfo stateInfo, int layerIndex);

	protected abstract void OnAnimationExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex);

	// OnStateEnter is called before OnStateEnter is called on any state inside this state machine
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (!isInitted)
			Init (animator, stateInfo);
		OnAnimationEnter (animator, stateInfo, layerIndex);
	}

	// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		OnAnimationStay (animator, stateInfo, layerIndex);
	}

	// OnStateExit is called before OnStateExit is called on any state inside this state machine
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		OnAnimationExit (animator, stateInfo, layerIndex);
	}

	// OnStateMove is called before OnStateMove is called on any state inside this state machine
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called before OnStateIK is called on any state inside this state machine
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

}
