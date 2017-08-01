using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyHideChecker : BodyAnimationCheckerBase {
	EnemyActor eActor;
	public HideableObject targetHideableObj = null; 
	public HideableFace targetFace = new HideableFace(); 
	public float autoBreakDistance;

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
		if (null == targetHideableObj)
			targetHideableObj = GetHideableObject () as HideableObject;
		if (actor.humanInfo.maxHp != actor.humanInfo.hp &&
			null != targetHideableObj &&
			!eActor.stateInfo.isHiding
		)
		{
			return true;
		}
		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		targetHideableObj = null;
		targetFace = new HideableFace();
		nowActivated = false;
	}
	public override void DoSpecifiedAction ()
	{
		if (!eActor.stateInfo.isHiding)
		{
			RunToPoint (targetFace.point.position);
		}
		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		targetHideableObj = null;
		targetFace = new HideableFace();
		nowActivated = false;
	}
	#endregion

	public InteractableObject GetHideableObject ()
	{
		HideableObject hideableObj = null;
		for (int i = 0; i < ((EnemyOutsideInfo)eActor.outsideInfo).hideableObjectInViewCount; i++)
		{
			hideableObj = ((EnemyOutsideInfo)eActor.outsideInfo).GetRankedObstacleByDistance (i) as HideableObject;

			if (null == hideableObj)
				continue;
			var face = GetHideableFace (hideableObj, eActor.damagedDirection);
			if (null != face.point) {
				targetFace = face;
				return hideableObj;
			}
		}
		return hideableObj;
	}

	public HideableFace GetHideableFace (HideableObject hideableObj, Vector3 damagedDir)
	{
		var absX = Mathf.Abs (damagedDir.x);
		var absY = Mathf.Abs (damagedDir.y);
		var absZ = Mathf.Abs (damagedDir.z);

		HideableFace face = new HideableFace();

		if (absX > absY) {
			if (absX > absZ) {
				if (damagedDir.x > 0) {
					face = hideableObj.GetHideableFaceByName (HideableFaceName.leftFace);
					if (face.hideable)
						return face;
				} else {
					face = hideableObj.GetHideableFaceByName (HideableFaceName.rightFace);
					if (face.hideable)
						return face;
				}
			} else {
				if (damagedDir.z > 0) {
					face = hideableObj.GetHideableFaceByName (HideableFaceName.backFace);
					if (face.hideable)
						return face;
				} else {
					face = hideableObj.GetHideableFaceByName (HideableFaceName.forwardFace);
					if (face.hideable)
						return face;
				}
			}
		}
		else {
			if (absY > absZ) {
				if (damagedDir.y > 0) {
					face = hideableObj.GetHideableFaceByName (HideableFaceName.downFace);
					if (face.hideable)
						return face;
				} else {
					face = hideableObj.GetHideableFaceByName (HideableFaceName.upFace);
					if (face.hideable)
						return face;
				}
			} else {
				if (damagedDir.z > 0) {
					face = hideableObj.GetHideableFaceByName (HideableFaceName.backFace);
					if (face.hideable)
						return face;
				} else {
					face = hideableObj.GetHideableFaceByName (HideableFaceName.forwardFace);
					if (face.hideable)
						return face;
				}
			}
		}
		return face;
	}

	//  만약 point에 근접하면 return true, 아니면  return false
	public bool RunToPoint (Vector3 obstaclePoint)
	{
		obstaclePoint.y = eActor.transform.position.z;
		var dis = Vector3.Distance (eActor.transform.position, obstaclePoint);
		if (dis > autoBreakDistance)
		{
			eActor.agent.SetDestination (obstaclePoint);
			eActor.GetSpecificAction<EnemyBodyChaseChecker> ().SetAnimationTrigger ();
			return false;
		}

		SetAnimationTrigger ();
		return true;
	}
}
