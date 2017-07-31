using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyActor : Actor {
	[HideInInspector]
	public NavMeshAgent agent;

	public Actor targetActor;
	public Vector3 lastTargetPoint;
	public Vector3 damagedDirection;

	// Use this for initialization
	protected new void Start () {
		base.Start ();
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	protected new void Update () {
		base.Update ();
		outsideInfo = GetComponentInChildren<EnemyOutsideInfo> ();
	}
		
	public static EnemyActor GetEnemyActor <T> (T component) where T : Component
	{
		return ((EnemyActor)Actor.GetActor (component));
	}
}
