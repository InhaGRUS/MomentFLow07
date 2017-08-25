using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyHideChecker : BodyAnimationCheckerBase {
	public EnemyActor eActor;
	public EnemyOutsideInfo eOutsideInfo;
	public float autoBreakDistance;

	[Header ("StateMaintainOption")]
	public float stateMaintainDuration = 5f;
	public float stateMaintainMinDuration = 5f;
	public float stateMaintainMaxDuration = 10f;
	public float stateMaintainTimer = 0f;
	[Range (0,1)]
	public float tensionThreshold = 0.5f;

	// Use this for initialization
	protected new void Start () {
		base.Start ();
		eActor = EnemyActor.GetEnemyActor<Actor> (actor);
		eOutsideInfo = eActor.GetEnemyOutsideInfo ();
		if (autoBreakDistance == 0)
		{
			autoBreakDistance = eActor.bodyCollider.bounds.extents.x;
		}
	}

	#region implemented abstract members of AnimationCheckerBase
	protected override bool CanTransition ()
	{
		return true;
	}
	protected override bool IsSatisfiedToAction ()
	{		
		if (eActor.tensionGauge < tensionThreshold)
			return false;
		if (actor.stateInfo.isCrouhcing &&
		    stateMaintainTimer <= stateMaintainDuration) {
			return true;
		}
		eOutsideInfo.SortFoundedHideableObjectList (actor.transform.position);	
		var foundHideableObj = GetHideableObject () as HideableObject;

		if (null != foundHideableObj &&
			stateMaintainTimer <= stateMaintainDuration
		)
		{
			eActor.targetHideableObj = foundHideableObj;
			return true;
		}
		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		stateMaintainTimer = 0f;

		if (actor.stateInfo.isHiding)
		{
			actor.stateInfo.isHiding = false;
			stateMaintainDuration = Random.Range (stateMaintainMinDuration, stateMaintainMaxDuration);
		}
		nowActivated = false;
	}
	public override void DoSpecifiedAction ()
	{
		if (!eActor.stateInfo.isHiding) {
			if (RunToPoint (eActor.targetHideableObj.transform.position + eActor.targetFace.point)) {
				eActor.stateInfo.isHiding = true;
				eActor.targetFace.hideable = false;
				eActor.previousFace = eActor.targetFace;
				eActor.SetToCrouch ();
			}
			else {
				eActor.GetEnemyOutsideInfo ().SetViewDirection (eActor.customAgent.agent.destination);
				eActor.GetSpecificAction<EnemyBodyChaseChecker> ().SetAnimationTrigger ();

			}
		} else {
			SetAnimationTrigger ();
			actor.DecreaseTension ();
			stateMaintainTimer += actor.customDeltaTime;
		}
		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{

		stateMaintainTimer = 0f;


		if (actor.stateInfo.isHiding)
		{
			eActor.targetFace.hideable = true;
			actor.stateInfo.isHiding = false;
		}
		nowActivated = false;
	}
	#endregion

	private InteractableObject GetHideableObject ()
	{
		HideableObject hideableObj = null;
		for (int i = 0; i < eOutsideInfo.foundedHideableObjList.Count; i++)
		{
			hideableObj = eOutsideInfo.foundedHideableObjList [i];

			if (null == hideableObj)
				continue;
			
			var face = GetHideableFace (hideableObj, eActor.damagedDirection);
			if (null != face && face.hideable) {
				/*eActor.previousFace = eActor.targetFace;
				eActor.targetFace = face;

				if (eActor.previousFace != eActor.targetFace) {
					eActor.previousFace.hideable = true;
					Debug.Log ("Move TO Next HOBJ");
				}*/
				eActor.targetFace = face;
				return hideableObj;
			}
		}
		return hideableObj;
	}

	private HideableFace GetHideableFace (HideableObject hideableObj, Vector3 damagedDir)
	{
		HideableFace face = null;

		if (damagedDir.x == CustomMaths.GetMaxValueFromVector (damagedDir).x) {
			if (damagedDir.x > 0) {
				face = hideableObj.GetHideableFaceByName (FaceName.rightFace);
				if (face.hideable)
					return face;
			}
			else {
				face = hideableObj.GetHideableFaceByName (FaceName.leftFace);
				if (face.hideable)
					return face;
			}
		}
		else if (damagedDir.y == CustomMaths.GetMaxValueFromVector (damagedDir).y)
		{
			if (damagedDir.y > 0) {
				face = hideableObj.GetHideableFaceByName (FaceName.downFace);
				if (face.hideable)
					return face;
			} else {
				face = hideableObj.GetHideableFaceByName (FaceName.upFace);
				if (face.hideable)
					return face;
			}
		}
		else if (damagedDir.z == CustomMaths.GetMaxValueFromVector (damagedDir).z)
		{
			if (damagedDir.z > 0) {
				face = hideableObj.GetHideableFaceByName (FaceName.frontFace);
				if (face.hideable)
					return face;
			} else {
				face = hideableObj.GetHideableFaceByName (FaceName.backFace);
				if (face.hideable)
					return face;
			}
		}
		return face;
	}

	//  만약 point에 근접하면 return true, 아니면  return false
	public bool RunToPoint (Vector3 obstaclePoint)
	{
		obstaclePoint.y = eActor.transform.position.y;
		eActor.customAgent.SetDestination (obstaclePoint);
//		Debug.Log (eActor.name + " : " + eActor.targetFace.hideableObj.name + " . " + eActor.targetFace.faceName.ToString() + " * " + eActor.targetHideableObj.name);
		if (Vector3.Distance (actor.transform.position, obstaclePoint) <= autoBreakDistance) {
			return true;
		}
		return false;
	}
}
