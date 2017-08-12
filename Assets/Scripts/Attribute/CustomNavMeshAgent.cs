using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomNavMeshAgent : MonoBehaviour {

	public EnemyActor eActor;
	public NavMeshAgent agent;
	public LayerMask obstacleMask;

	public float comfortableDistance = 1f;

	public int maxDepth = 3;
	public int nowDestinationIndex = 0;
	public float remainingDistance;
	public List<Vector3> destCornerPointList = new List<Vector3>();

	public float cornerMaintainTimer = 0f;
	public float cornerMaintainDuration = 5f;

	// Use this for initialization
	void Start () {
		eActor = EnemyActor.GetEnemyActor <CustomNavMeshAgent> (this);
		agent = eActor.GetComponent<NavMeshAgent> ();
		comfortableDistance = Vector3.Distance (eActor.bodyCollider.bounds.center, eActor.bodyCollider.bounds.max) * 1.25f;
	}

	public void Update ()
	{
		if (eActor.stateInfo.isCrouhcing) {
			comfortableDistance = Vector3.Distance (eActor.bodyCollider.bounds.center, eActor.bodyCollider.bounds.max) * 0.6f;
		} else {
			Vector3.Distance (eActor.bodyCollider.bounds.center, eActor.bodyCollider.bounds.max);
		}
		remainingDistance = agent.remainingDistance;

		if (nowDestinationIndex != 0) {
			if (cornerMaintainTimer >= cornerMaintainDuration) {
				nowDestinationIndex = 0;
				cornerMaintainTimer = 0f;
			}
			cornerMaintainTimer += eActor.customDeltaTime;
			SetDestination (destCornerPointList [nowDestinationIndex]);
		}

		if (remainingDistance <= 0.05f) {
			if (nowDestinationIndex - 1 >= 0)
				nowDestinationIndex -= 1;
		}
	}

	public void SetDestination (Vector3 targetPoint)
	{		
		if (nowDestinationIndex != 0) {
			destCornerPointList [0] = targetPoint;
			return;
		}
		destCornerPointList.Clear ();
		destCornerPointList.Add (targetPoint);

		if (AddDestination (0, eActor.transform.position, targetPoint)) {
			nowDestinationIndex = destCornerPointList.Count - 1;
			agent.SetDestination (destCornerPointList [nowDestinationIndex]);
		}
		else {
			nowDestinationIndex = 0;
			agent.SetDestination (targetPoint);
		}
	}

	public void StopMove ()
	{
		nowDestinationIndex = 0;
		destCornerPointList.Clear ();
		SetDestination (eActor.transform.position);
	}

	public bool AddDestination (int depth, Vector3 startPoint, Vector3 targetPoint)
	{
		startPoint.y = eActor.transform.position.y;
		targetPoint.y = eActor.transform.position.y;

		var dir = (targetPoint - startPoint).normalized;
		var dis = Vector3.Distance (startPoint,targetPoint);

		var hits = eActor.actorRigid.SweepTestAll (dir, dis);

		for (int i = 0; i < hits.Length; i++)
		{
			var hitAgent = CustomNavMeshAgent.GetCustomNavMeshAgent <Collider> (hits [i].collider);
			if (null != hitAgent &&
				hitAgent != this &&
				hitAgent.eActor.bodyCollider == hits [i].collider) {
				var closestPoint = GetNearestCorner (hits [i].collider, hits[i].point);
				var tmpDir = (closestPoint - new Vector3 (hits [i].collider.bounds.center.x, closestPoint.y, hits [i].collider.bounds.center.z)).normalized;
				closestPoint += tmpDir * comfortableDistance;
				if (depth < maxDepth)
					return AddDestination (depth + 1, startPoint, closestPoint);
				return false;
			}
			else if (null != eActor.targetActor && eActor.targetActor.bodyCollider == hits [i].collider)
			{
				destCornerPointList[0] = targetPoint;
				return true;
			}
		}

		if (targetPoint != destCornerPointList [0])
			destCornerPointList.Add (targetPoint);
		return true;
	}

	public Vector3 GetNearestCorner (Collider col, Vector3 point)
	{
		point.y = eActor.transform.position.y;
		Vector3[] points = new Vector3[4];
		points [0] = new Vector3 (col.bounds.max.x, eActor.transform.position.y, col.bounds.max.z);
		points [1] = new Vector3 (col.bounds.min.x, eActor.transform.position.y, col.bounds.max.z);
		points [2] = new Vector3 (col.bounds.max.x, eActor.transform.position.y, col.bounds.min.z);
		points [3] = new Vector3 (col.bounds.min.x, eActor.transform.position.y, col.bounds.min.z);

		int index = 0;
		float tmpDis = Vector3.Distance (points[0], point);
		for (int i = 1; i < 4; i++)
		{
			if (Vector3.Distance (points[i], point) <= tmpDis)
			{
				tmpDis = Vector3.Distance (points [i], point);
				index = i;
			}
		}
		return points [index];
	}

	public static CustomNavMeshAgent GetCustomNavMeshAgent<T> (T obj) where T : Component
	{
		if (null != obj.GetComponent<CustomNavMeshAgent>())
			return obj.GetComponent<CustomNavMeshAgent>();
		if (null != obj.GetComponentInChildren<CustomNavMeshAgent>())
			return obj.GetComponentInChildren<CustomNavMeshAgent>();
		if (null != obj.GetComponentInParent<CustomNavMeshAgent> ())
			return obj.GetComponentInParent<CustomNavMeshAgent> ();
		return null;
	}
}
