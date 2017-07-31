using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyIdleChecker : BodyAnimationCheckerBase {
	EnemyActor eActor;
	// Use this for initialization
	void Start () {
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
		if (null == eActor.targetActor)
		{
			return true;
		}
		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		nowActivated = false;
	}
	public override void DoSpecifiedAction ()
	{
		SetAnimationTrigger ();
		FindSuspiciousObject ();
		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		nowActivated = false;
	}
	#endregion

	public void FindSuspiciousObject ()
	{
		for (int i = 0; i < ((EnemyOutsideInfo)(eActor.outsideInfo)).actorListInVeiw.Count; i++)
		{
			var element = ((EnemyOutsideInfo)(eActor.outsideInfo)).actorListInVeiw [i];
			if (Vector3.Distance (element.transform.position, eActor.transform.position) > ((EnemyOutsideInfo)(eActor.outsideInfo)).viewRecognizeDistance ||
			    element.roomInfo.roomName != eActor.roomInfo.roomName) 
			{
				continue;
			}
			
			if (element.humanInfo.humanType == HumanType.Player)
			{
				eActor.targetActor = element;
				break;
			}
		}
	}


}
