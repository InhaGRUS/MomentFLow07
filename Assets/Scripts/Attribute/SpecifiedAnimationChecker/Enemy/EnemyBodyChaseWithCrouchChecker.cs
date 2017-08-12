using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyChaseWithCrouchChecker : BodyAnimationCheckerBase {

	public EnemyActor eActor;
	public EnemyBodyHideChecker hideChecker;

	public float disToChase = 2f;

	public EnemyOutsideInfo eOutsideInfo;
	public float autoBreakDistance;

	[Header ("StateDelayTimer")]
	public float stateDelay = 2.5f;
	public float stateMaxDelay = 5f;
	public float stateMinDelay = 2.5f;
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

		var foundHideableObj = GetHideableObject () as HideableObject;

		if (null == foundHideableObj)
			return false;
		
		var hObjPoint = foundHideableObj.transform.position + eActor.targetFace.point;
		hObjPoint.y = eActor.transform.position.y;
		var disToHObjPoint = Vector3.Distance (hObjPoint, eActor.transform.position);

		if (stateDelayTimer >= stateDelay &&
			eActor.roomInfo.roomName == eActor.targetActor.roomInfo.roomName)
		{
			eActor.targetHideableObj = foundHideableObj;
			return true;
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
			eActor.customAgent.agent.destination = eActor.transform.position;
			SetAnimationTrigger (1);
			eActor.GetEnemyOutsideInfo ().SetViewDirection (eActor.targetActor.transform.position);
			eActor.targetFace.hideable = false;
			stateDelayTimer = 0f;
			Debug.Log ("Arrived");
		}
		else {
			SetAnimationTrigger (0);
			eActor.GetEnemyOutsideInfo ().SetViewDirection (eActor.customAgent.agent.destination);
			Debug.Log ("CrouchWalking Now");
			if (!eActor.previousFace.hideable)
				eActor.previousFace.hideable = true;
		}
		nowActivated = true;
	}

	public override void CancelSpecifiedAction ()
	{
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

			if (null == hideableObj)
				continue;

			var face = GetHideableFace (hideableObj, (hideableObj.transform.position - eActor.targetActor.transform.position));

			if (null != face && face.hideable) {
				if (index == -1) {
					disOfFaceToTarget = Vector3.Distance (eActor.targetActor.transform.position, hideableObj.transform.position + face.point);
					index = i;
					eActor.previousFace = eActor.targetFace;
					eActor.targetFace = face;
					continue;
				}
					
				var tmpDis = Vector3.Distance (eActor.targetActor.transform.position, hideableObj.transform.position + face.point);

				if (tmpDis <= disOfFaceToTarget) {
					disOfFaceToTarget = tmpDis;
					index = i;
					eActor.previousFace = eActor.targetFace;
					eActor.targetFace = face;
				
				}
			}
			else if (null == face && null != eActor.targetFace){

			}
		}
		if (index == -1)
			return null;

		return eOutsideInfo.foundedHideableObjList [index];
	}

	private HideableFace GetHideableFace (HideableObject hideableObj, Vector3 damagedDir)
	{
		var absX = Mathf.Abs (damagedDir.x);
		var absY = Mathf.Abs (damagedDir.y);
		var absZ = Mathf.Abs (damagedDir.z);

		HideableFace face = null;

		if (absX > absY) {
			if (absX > absZ) {
				if (damagedDir.x > 0) {
					face = hideableObj.GetHideableFaceByName (HideableFaceName.rightFace);
					if (face.hideable)
						return face;
				} else {
					face = hideableObj.GetHideableFaceByName (HideableFaceName.leftFace);
					if (face.hideable)
						return face;
				}
			} else {
				if (damagedDir.z > 0) {
					face = hideableObj.GetHideableFaceByName (HideableFaceName.forwardFace);
					if (face.hideable)
						return face;
				} else {
					face = hideableObj.GetHideableFaceByName (HideableFaceName.backFace);
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
			}
			else {
				if (damagedDir.z > 0) {
					face = hideableObj.GetHideableFaceByName (HideableFaceName.forwardFace);
					if (face.hideable)
						return face;
				} else {
					face = hideableObj.GetHideableFaceByName (HideableFaceName.backFace);
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
		obstaclePoint.y = eActor.transform.position.y;
		eActor.customAgent.SetDestination (obstaclePoint);

		if (Vector3.Distance (actor.transform.position, obstaclePoint) <= autoBreakDistance) {
			return true;
		}
		return false;
	}
}
