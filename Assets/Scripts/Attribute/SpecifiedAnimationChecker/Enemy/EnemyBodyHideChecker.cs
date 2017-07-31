using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyHideChecker : BodyAnimationCheckerBase {
	EnemyActor eActor;
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
		if (actor.humanInfo.maxHp != actor.humanInfo.hp &&
			null != ((EnemyOutsideInfo)eActor.outsideInfo).GetNearestObstacle()
		)
		{
			return true;
		}
		return false;
	}
	protected override void BeforeTransitionAction ()
	{

	}
	public override void DoSpecifiedAction ()
	{
		
	}
	public override void CancelSpecifiedAction ()
	{

	}
	#endregion
}
