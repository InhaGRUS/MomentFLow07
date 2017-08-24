using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyActor : Actor {
	[HideInInspector]
	public CustomNavMeshAgent customAgent;

	public Actor targetActor;
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

	public virtual void DamagedFrom (Actor fromActor, float damagedAmount, Vector3 damagedDir)
	{
		humanInfo.hp = Mathf.Max (humanInfo.hp - damagedAmount, 0f);
		damagedDirection = damagedDir;
		IncreaseTension ();
	}

	public virtual void DamagedFrom (Actor fromActor, float damagedAmount, Vector3 damagedDir, float tensionInc)
	{
		humanInfo.hp = Mathf.Max (humanInfo.hp - damagedAmount, 0f);
		damagedDirection = damagedDir;
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

	public void FindSuspiciousObject ()
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
	}

	public EnemyOutsideInfo GetEnemyOutsideInfo ()
	{
		return ((EnemyOutsideInfo)outsideInfo);
	}

	public static EnemyActor GetEnemyActor <T> (T component) where T : Component
	{
		return ((EnemyActor)Actor.GetActor (component));
	}
}
