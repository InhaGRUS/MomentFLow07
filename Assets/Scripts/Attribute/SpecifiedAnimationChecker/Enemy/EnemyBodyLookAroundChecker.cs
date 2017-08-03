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

	private int loopCount = 0;

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
		if (eActor.disToSuspiciousPoint <= 0.1f)
			return true;
		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		isActionEnd = false;
		nowActivated = false;
	}
	public override void DoSpecifiedAction ()
	{
		if (!nowActivated) {
			originViewDirection = eActor.GetEnemyOutsideInfo ().lookDirection;

			if (lookPriority == EnemyLookAroundPriority.Left)
			{
				eActor.GetEnemyOutsideInfo ().SetViewDirection (Quaternion.Euler (Vector3.down * rotateAngle) * originViewDirection); 
			} 
			else 
			{
				eActor.GetEnemyOutsideInfo ().SetViewDirection (Quaternion.Euler (Vector3.up * rotateAngle) * originViewDirection); 
			}
		}

		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		isActionEnd = false;
		nowActivated = false;
	}
	#endregion
}
