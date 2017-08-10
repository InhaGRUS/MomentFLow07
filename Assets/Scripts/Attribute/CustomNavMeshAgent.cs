using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomNavMeshAgent : MonoBehaviour {

	public EnemyActor eActor;
	private NavMeshAgent agent;
	public LayerMask obstacleMask;

	public float comfortableDistance = 1f;

	public int nowDestinationIndex = 0;
	public List<Vector3> cornerPointList = new List<Vector3>();

	// Use this for initialization
	void Start () {
		eActor = EnemyActor.GetEnemyActor <CustomNavMeshAgent> (this);
		agent = eActor.agent;
	}

	public void SetDestination (Vector3 targetPoint)
	{
		Ray ray;
		ray.origin = eActor.transform.position;
		ray.direction = (targetPoint - eActor.transform.position).normalized;
		RaycastHit hit;
		var dir = (targetPoint - agent.transform.position).normalized;

		if (eActor.bodyCollider.Raycast (ray, out hit, Vector3.Distance (eActor.transform.position, targetPoint))) {
			var bodyToHitDir = (hit.point - targetPoint).normalized;
			if (CustomMaths.GetMaxValueFromVector (bodyToHitDir).x == bodyToHitDir.x)
			{
				if (bodyToHitDir.x > 0) {
					
				}
				else {

				}
			}
			else if (CustomMaths.GetMaxValueFromVector (bodyToHitDir).y == bodyToHitDir.y)
			{
				if (bodyToHitDir.y > 0) {

				}
				else {

				}
			}
			else if (CustomMaths.GetMaxValueFromVector (bodyToHitDir).z == bodyToHitDir.z)
			{
				if (bodyToHitDir.z > 0) {

				}
				else {

				}
			}
		}
	}

	public Vector3 GetNearestCorner (Collider col, Vector3 point)
	{

	}

	// Update is called once per frame
	void Update () {
		
	}
}
