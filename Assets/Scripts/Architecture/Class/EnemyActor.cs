using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyActor : Actor {
	[HideInInspector]
	public NavMeshAgent agent;

	public Actor targetActor;
	public Vector3 suspiciousPoint;
	public Vector3 lastTargetPoint;
	public Vector3 damagedDirection;

	public void OnEnable ()
	{
		outsideInfo = GetComponentInChildren<EnemyOutsideInfo> ();
		equipmentInfo = GetComponentInChildren <EquipmentInfo> ();
		agent = GetComponent<NavMeshAgent> ();
	}

	// Use this for initialization
	protected new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected new void Update () {
		base.Update ();
	}
		
	public void FindSuspiciousObject ()
	{
		for (int i = 0; i < GetEnemyOutsideInfo().actorListInVeiw.Count; i++)
		{
			var element = GetEnemyOutsideInfo().actorListInVeiw [i];
			var dis = Vector3.Distance (element.transform.position, transform.position);
			if (dis > GetEnemyOutsideInfo().viewMaxDistance ||
				element.roomInfo.roomName != roomInfo.roomName) 
			{
				if (null != targetActor)
					lastTargetPoint = targetActor.transform.position;
				targetActor = null;
				continue;
			}

			if (element.humanInfo.humanType == HumanType.Player)
			{
				if (dis < GetEnemyOutsideInfo().viewMaxDistance)
				{
					suspiciousPoint = element.transform.position;
				}

				if (dis < GetEnemyOutsideInfo ().viewRecognizeDistance) {
					roomInfo.roomState = RoomState.Combat;
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
