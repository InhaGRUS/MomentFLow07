using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyLookAroundChecker : BodyAnimationCheckerBase {
	public EnemyActor eActor;
	public EnemyLookAroundPriority lookPriority;
	public Vector3 originViewDirection;
	public float rotateAngle = 80f;
	public float rotateDuration = 1.5f;
	public float rotateTimer = 0f;
	public int loopNum = 1;

	public int loopCount = 0;

	public bool isActionEnd = false;

	// Use this for initialization
	protected new void Start () {
		base.Start ();
		eActor = EnemyActor.GetEnemyActor<Actor> (actor);
	}

	#region implemented abstract members of AnimationCheckerBase
	protected override bool CanTransition ()
	{
		return true;
	}
	protected override bool IsSatisfiedToAction ()
	{
		if (eActor.disToSuspiciousPoint <= 0.1f &&
		    null == eActor.targetActor &&
		    loopCount < loopNum) {
			return true;
		}
		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		Debug.Log ("Before");
		isActionEnd = false;
		nowActivated = false;
		loopCount = 0;
	}
	public override void DoSpecifiedAction ()
	{
		if (!nowActivated) {
			if (loopCount == 0) {
				originViewDirection = eActor.GetEnemyOutsideInfo ().lookDirection;
			}

			if (lookPriority == EnemyLookAroundPriority.Left)
			{
				eActor.GetEnemyOutsideInfo ().SetViewDirection (originViewDirection - Vector3.right * rotateAngle); 
				lookPriority = EnemyLookAroundPriority.Right;
			} 
			else 
			{
				eActor.GetEnemyOutsideInfo ().SetViewDirection (originViewDirection + Vector3.right * rotateAngle); 
				lookPriority = EnemyLookAroundPriority.Left;
			}
		}
			
		nowActivated = true;

		rotateTimer += actor.customDeltaTime;
		if (rotateTimer >= rotateDuration) {
			rotateTimer = 0f;
			loopCount += 1;
			nowActivated = false;
		}
		if (loopCount >= loopNum)
		{
			isActionEnd = true;
		}
	}
	public override void CancelSpecifiedAction ()
	{
		Debug.Log ("Cancel");
		isActionEnd = false;
		nowActivated = false;
		loopCount = 0;
	}
	#endregion
}
