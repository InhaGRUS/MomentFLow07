using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyRunAnimation : BodyAnimationBase {
	public EnemyActor eActor;
	public float accel;
	public float maxRunSpeed;
	public Vector3 desireDir;

	protected override void OnAnimationEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (null == eActor)
			eActor = EnemyActor.GetEnemyActor<Actor> (actor);
		eActor.agent.acceleration = accel;
		eActor.agent.speed = maxRunSpeed;

	}
	protected override void OnAnimationStay (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		desireDir = (eActor.targetActor.transform.position - actor.transform.position).normalized;
		if (desireDir.x > 0) {
			actor.SetLookDirection (false);
		} else if (desireDir.x < 0) {
			actor.SetLookDirection (true);
		}
	}
	protected override void OnAnimationExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

	}
}
