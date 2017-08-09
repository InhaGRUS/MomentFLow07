using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyCrouchAnimation : BodyAnimationBase {
	public EnemyActor eActor;

	#region implemented abstract members of AnimationBase
	protected override void OnAnimationEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (null == eActor)
			eActor = EnemyActor.GetEnemyActor<Actor> (actor);
		eActor.agent.acceleration = 0f;
		eActor.agent.speed = 0f;
	}
	protected override void OnAnimationStay (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		
	}
	protected override void OnAnimationExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		
	}
	#endregion
	
}
