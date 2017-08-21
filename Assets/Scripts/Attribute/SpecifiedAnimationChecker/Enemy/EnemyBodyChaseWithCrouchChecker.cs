using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyChaseWithCrouchChecker : BodyAnimationCheckerBase {

	public EnemyActor eActor;
	public EnemyBodyHideChecker hideChecker;

	private HideableFace tmpFace;

	public float disToChase = 2f;

	public EnemyOutsideInfo eOutsideInfo;
	public float autoBreakDistance;

	[Header ("StateDelayTimer")]
	public float stateDelay = 5f;
	public float stateMaxDelay = 10f;
	public float stateMinDelay = 5f;
	public float stateDelayTimer = 0f;

	[Range (0,1)]
	public float tensionThreshold = 0.5f;

	// Use this for initialization
	protected new void Start () {
		base.Start ();
		eActor = EnemyActor.GetEnemyActor <Actor> (actor);
		eOutsideInfo = eActor.GetEnemyOutsideInfo ();
		hideChecker = eActor.GetSpecificAction <EnemyBodyHideChecker> ();

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
		if (null == eActor.targetActor || !eActor.stateInfo.isCrouhcing)
		{
			return false;
		}

		if (eActor.targetFace != eActor.previousFace && eActor.targetFace.hideable)
			return true;
		var foundHideableObj = GetHideableObject () as HideableObject;

		if (null == foundHideableObj) {
			return false;
		}
		
		var hObjPoint = foundHideableObj.transform.position + eActor.targetFace.point;
		hObjPoint.y = eActor.transform.position.y;
		var disToHObjPoint = Vector3.Distance (hObjPoint, eActor.transform.position);

		if (stateDelayTimer >= stateDelay &&
			eActor.roomInfo.roomName == eActor.targetActor.roomInfo.roomName)
		{
			if (eActor.targetFace != tmpFace && tmpFace.hideable)
			{
				eActor.previousFace = eActor.targetFace;
				eActor.targetFace = tmpFace;
				eActor.previousFace.hideable = true;
				eActor.targetHideableObj = eActor.targetFace.hideableObj;
				return true;
			}
			return false;
		}
		stateDelayTimer += actor.customDeltaTime;
		return false;
	}

	protected override void BeforeTransitionAction ()
	{
		stateDelayTimer = 0f;
		nowActivated = false;
	}

	public override void DoSpecifiedAction ()
	{
		if (RunToPoint (eActor.targetHideableObj.transform.position + eActor.targetFace.point)) {
			eActor.customAgent.SetDestination(eActor.transform.position);
			SetAnimationTrigger (1);
			eActor.GetEnemyOutsideInfo ().SetViewDirection (eActor.targetFace.hideableObj.transform.position);
			eActor.targetFace.hideable = false;
			eActor.previousFace = eActor.targetFace;
			stateDelayTimer = 0f;
		}
		else {
			SetAnimationTrigger (0);
			eActor.GetEnemyOutsideInfo ().SetViewDirection (eActor.customAgent.agent.destination);
		}
		nowActivated = true;
	}

	public override void CancelSpecifiedAction ()
	{
		stateDelayTimer = 0f;
		nowActivated = false;
	}

	#endregion

	private InteractableObject GetHideableObject ()
	{
		HideableObject hideableObj = null;
		int index = -1;
		float disOfFaceToTarget = 0f;

		for (int i = 0; i < eOutsideInfo.foundedHideableObjList.Count; i++)
		{
			hideableObj = eOutsideInfo.foundedHideableObjList [i];

			if (null == hideableObj) {
				//Debug.Log ("NO Obj");
				continue;
			}

			var face = GetHideableFace (hideableObj, (hideableObj.transform.position - eActor.targetActor.transform.position));

			if (null != face && face.hideable) {
				if (index == -1) {
					disOfFaceToTarget = Vector3.Distance (eActor.targetActor.transform.position, hideableObj.transform.position + face.point);
					index = i;
					tmpFace = face;
					continue;
				}
					
				var tmpDis = Vector3.Distance (eActor.targetActor.transform.position, hideableObj.transform.position + face.point);

				if (tmpDis <= disOfFaceToTarget) {
					disOfFaceToTarget = tmpDis;
					index = i;
					tmpFace = face;
				}
			} 
		}
		if (index == -1) {
			return null;
		}

		return eOutsideInfo.foundedHideableObjList [index];
	}

	private HideableFace GetHideableFace (HideableObject hideableObj, Vector3 damagedDir)
	{
		HideableFace face = null;

		if (damagedDir.x == CustomMaths.GetMaxValueFromVector (damagedDir).x) {
			if (damagedDir.x > 0) {
				face = hideableObj.GetHideableFaceByName (FaceName.rightFace);
			} else {
				face = hideableObj.GetHideableFaceByName (FaceName.leftFace);
			}
		} else if (damagedDir.y == CustomMaths.GetMaxValueFromVector (damagedDir).y) {
			if (damagedDir.y > 0) {
				face = hideableObj.GetHideableFaceByName (FaceName.downFace);
			} else {
				face = hideableObj.GetHideableFaceByName (FaceName.upFace);
			}
		} else if (damagedDir.z == CustomMaths.GetMaxValueFromVector (damagedDir).z) {
			if (damagedDir.z > 0) {
				face = hideableObj.GetHideableFaceByName (FaceName.frontFace);
			} else {
				face = hideableObj.GetHideableFaceByName (FaceName.backFace);
			}
		} else {
			Debug.Log ("??");
		}
		if (null != face && face.hideable) {
			return face;
		}
		return null;
	}
		 
	//  만약 point에 근접하면 return true, 아니면  return false
	public bool RunToPoint (Vector3 obstaclePoint)
	{
		obstaclePoint.y = eActor.transform.position.y;
		eActor.customAgent.SetDestination (obstaclePoint);
		if (Vector3.Distance (actor.transform.position, obstaclePoint) <= autoBreakDistance) {
			return true;
		}
		return false;
	}
}
