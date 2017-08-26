using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyActor : Actor {
	[HideInInspector]
	public CustomNavMeshAgent customAgent;

	public LayerMask targetableMask;
	public Actor targetActor;
	public Actor suspiciousActor;
	public Vector3 suspiciousPoint;
	public Vector3 lastTargetPoint;

	public float disToTarget;
	public float disToSuspiciousPoint;

	public HideableObject targetHideableObj;
	public HideableFace targetFace;
	public HideableFace previousFace;

	public void OnEnable ()
	{
		outsideInfo = GetComponentInChildren<EnemyOutsideInfo> ();
		equipmentInfo = GetComponentInChildren <EquipmentInfo> ();
		customAgent = GetComponent<CustomNavMeshAgent> ();
	}

	// Use this for initialization
	protected new void Start () {
		base.Start ();
		// Set Handlers
		GetEnemyOutsideInfo ().onViewObjectAdded += HandlerObjectInViewAdded;
		GetEnemyOutsideInfo ().onViewObjectRemoved += HandlerObjectInViewRemoved;
	}
	
	// Update is called once per frame
	protected new void Update () {
		base.Update ();
		if (null != targetActor) {
			disToTarget = Vector3.Distance (transform.position, targetActor.transform.position);
		}
		else {
			disToSuspiciousPoint = Vector3.Distance (transform.position, suspiciousPoint);
		}
	}

	public override void DamagedFrom (Actor fromActor, float damagedAmount, Vector3 damagedDir)
	{
		humanInfo.hp = Mathf.Max (humanInfo.hp - damagedAmount, 0f);
		damagedDirection = damagedDir;
		GetEnemyOutsideInfo ().SetViewDirection (fromActor.bodyCollider.bounds.center);
		IncreaseTension ();
	}

	public override void DamagedFrom (Actor fromActor, float damagedAmount, Vector3 damagedDir, float tensionInc)
	{
		humanInfo.hp = Mathf.Max (humanInfo.hp - damagedAmount, 0f);
		damagedDirection = damagedDir;
		GetEnemyOutsideInfo ().SetViewDirection (fromActor.bodyCollider.bounds.center);
		IncreaseTension (tensionInc);
	}

	public void SetToCrouch ()
	{
		if (!stateInfo.isCrouhcing) {
			stateInfo.isCrouhcing = true;
			bodyAnimator.SetBool ("BoolCrouch", true);
			shoulderAnimator.SetBool ("BoolCrouch", true);
		}
	}

	public void ReleaseCrouch ()
	{
		if (stateInfo.isCrouhcing) {
			stateInfo.isCrouhcing = false;
			bodyAnimator.SetBool ("BoolCrouch", false);
			shoulderAnimator.SetBool ("BoolCrouch", false);
			stateInfo.isHiding = false;
			if (null != targetFace)
				targetFace.hideable = true;
		}
	}

	public void OnDrawGizmos ()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawCube (suspiciousPoint, Vector3.one * 0.25f);
	}

	//OutsideInfo Handler Block
	public void HandlerObjectInViewAdded (DynamicObject obj)
	{
		switch (obj.objectType) {
		case DynamicObjectType.Actor:
			//Identify Object, Is Object Target?
			if (((1<<obj.gameObject.layer) & targetableMask) != 0)
			{
				var tmpActor = obj as Actor;
				var disToActor = Vector3.Distance (tmpActor.bodyCollider.bounds.center, bodyCollider.bounds.center);
				// Can i Recognize this Object?
				if (disToActor <=  GetEnemyOutsideInfo().viewRecognizeDistance) {
					targetActor = tmpActor;
					suspiciousActor = targetActor;
					suspiciousPoint = tmpActor.bodyCollider.bounds.center;
					StopCoroutine ("ObserveSuspiciousActor");
				}
				else { // If i can't recognize this object, but i know there is suspicious object in my view
					suspiciousActor = tmpActor;
					suspiciousPoint = tmpActor.bodyCollider.bounds.center;
					suspiciousPoint.y = transform.position.y;
					StartCoroutine ("ObserveSuspiciousActor");
				}
				GetSpecificAction<EnemyBodySuspiciousChecker> ().isFoundSuspiciousPoint = true;
			}
			break;
		case DynamicObjectType.Bullet:

			break;
		case DynamicObjectType.InteractableObject:

			break;
		}
	}
	public void HandlerObjectInViewRemoved (DynamicObject obj)
	{
		// obj on out of view now
		switch (obj.objectType) {
		case DynamicObjectType.Actor:
			if ((Actor)obj == targetActor) // this means targetActor isn't null
			{
				if (targetActor.roomInfo != roomInfo) {
					Debug.Log ("Target Enter Other Room");
					suspiciousActor = targetActor;
					suspiciousPoint = suspiciousActor.bodyCollider.bounds.center;
					suspiciousPoint.y = transform.position.y;
					StartCoroutine ("ObserveSuspiciousActor");
				}
				else {
					Debug.Log ("Target Out Of View");
					suspiciousPoint = targetActor.bodyCollider.bounds.center;
					suspiciousPoint.y = transform.position.y;
					lastTargetPoint = suspiciousPoint;
					//suspiciousActor = null;
					targetActor = null;
					StopCoroutine ("ObserveSuspiciousActor");
					StartCoroutine ("LostSuspiciousTarget");
				}

			}
			break;
		case DynamicObjectType.Bullet:

			break;
		case DynamicObjectType.InteractableObject:

			break;
		}
	}
	//End of Handler Block

	public IEnumerator ObserveSuspiciousActor ()
	{
		while (true)
		{
			if (null != suspiciousActor) {
				var disToActor = Vector3.Distance (suspiciousActor.bodyCollider.bounds.center, bodyCollider.bounds.center);
				if (disToActor > GetEnemyOutsideInfo ().viewRecognizeDistance) {
					suspiciousPoint = suspiciousActor.bodyCollider.bounds.center;
					suspiciousPoint.y = transform.position.y;
					GetSpecificAction<EnemyBodySuspiciousChecker> ().isFoundSuspiciousPoint = true;
				} 
				else {
					targetActor = suspiciousActor;
					suspiciousPoint = suspiciousActor.bodyCollider.bounds.center;
					suspiciousPoint.y = transform.position.y;
				}
			}
			yield return new WaitForEndOfFrame ();
		}
	}

	public IEnumerator LostSuspiciousTarget ()
	{
		yield return new WaitForSeconds (2.5f);
		suspiciousActor = null;
		StopCoroutine ("LostSuspiciousTarget");
	}

	/*public void FindSuspiciousObject ()
	{
		for (int i = 0; i < GetEnemyOutsideInfo().actorListInVeiw.Count; i++)
		{
			var element = GetEnemyOutsideInfo().actorListInVeiw [i];
			var dis = Vector3.Distance (element.transform.position, transform.position);

			if (null == element.roomInfo)
				return;

			if (dis > GetEnemyOutsideInfo().viewMaxDistance ||
				element.roomInfo.roomName != roomInfo.roomName) 
			{
				if (element == targetActor) {
					lastTargetPoint = targetActor.transform.position;
					suspiciousPoint = element.transform.position;
					targetActor = null;
				}
				continue;
			}

			if (element.humanInfo.humanType == HumanType.Player)
			{
				if (dis < GetEnemyOutsideInfo ().viewMaxDistance) {
					suspiciousPoint = element.transform.position;
					GetSpecificAction<EnemyBodySuspiciousChecker> ().foundSuspiciousObject = true;
				} else {
					GetSpecificAction<EnemyBodySuspiciousChecker> ().foundSuspiciousObject = false;
				}

				if (dis < GetEnemyOutsideInfo ().viewRecognizeDistance) {
					roomInfo.roomState = RoomState.Combat;
					lastTargetPoint = suspiciousPoint;
					targetActor = element;
				}
				break;
			}
		}
	}*/

	public EnemyOutsideInfo GetEnemyOutsideInfo ()
	{
		return ((EnemyOutsideInfo)outsideInfo);
	}

	public static EnemyActor GetEnemyActor <T> (T component) where T : Component
	{
		return ((EnemyActor)Actor.GetActor (component));
	}
}
