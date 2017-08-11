using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatrolNode 
{
	public bool isInitted = false;
	public int nodeId;
	[SerializeField]
	public PatrolNode fromNode;
	public Vector3 nodePoint;
	public float autoStopDistance;

	public PatrolNode (int id, PatrolNode fNode, Vector3 point, float stopDis)
	{
		nodeId = id;
		fromNode = fNode;
		nodePoint = point;
		autoStopDistance = stopDis;
	}
}

[ExecuteInEditMode]
public class EnemyBodyPatrolChecker : BodyAnimationCheckerBase {
	public EnemyActor eActor;

	[Range (0f, 1f)]
	public float slackProbability;
	public PatrolNode nowNode;
	public int maxNodeIndex;
	public float disToNode;

	[Header ("PatrolDuration")]
	public float stateDuration = 15f;
	public float stateDurationTimer = 0f;

	[Header ("StateDelayTimer")]
	public float stateDelay = 2.5f;
	public float stateDelayTimer = 0f;

	public List<PatrolNode> patrolNodeList = new List<PatrolNode>();

	public void OnEnable ()
	{
		for (int i = 0; i < patrolNodeList.Count; i++)
		{
			var element = patrolNodeList [i];
			if (!element.isInitted) {
				element.nodeId = i;
				element.nodePoint = new Vector3 (
					element.nodePoint.x,
					actor.transform.position.y,
					element.nodePoint.z
				);
				if (element.nodeId != 0) {
					element.fromNode = patrolNodeList [i - 1];
				}
				element.autoStopDistance = 0.1f;
				element.isInitted = true;
			}
		}
		maxNodeIndex = patrolNodeList.Count;
	}

	// Use this for initialization
	protected new void Start () {
		base.Start ();
		eActor = EnemyActor.GetEnemyActor <Actor> (actor);
	}

	#region implemented abstract members of AnimationCheckerBase
	protected override bool CanTransition ()
	{
		return true;
	}
	protected override bool IsSatisfiedToAction ()
	{
		if (null == eActor.targetActor &&
			stateDelayTimer >= stateDelay &&
			stateDurationTimer <= stateDuration
		)
		{	
			return true;
		}
		stateDelayTimer += actor.customDeltaTime;
		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		stateDelayTimer = 0f;
		stateDurationTimer = 0f;
		nowNode = null;
		nowActivated = false;
	}
	public override void DoSpecifiedAction ()
	{
		eActor.ReleaseCrouch ();

		if (!nowActivated) {
			ReIndexPatrolNodesByDistance ();
			nowNode = GetPatrolNodeByIndex (0);
		}

		stateDurationTimer += actor.customDeltaTime;

		if (null != nowNode) {
			disToNode = Vector3.Distance (actor.transform.position, nowNode.nodePoint);

			if (disToNode <= nowNode.autoStopDistance) {
				nowNode = GetPatrolNodeByIndex (nowNode.nodeId + 1);
				if (stateDurationTimer >= stateDuration) {
					if (Random.Range (0f, 1f) > slackProbability) {
						stateDurationTimer = 0f;
					}
				}
			} else {
				eActor.customAgent.SetDestination (nowNode.nodePoint);
			}

			if (null == nowNode) {
				ReIndexPatrolNodesByDistance ();
				nowNode = GetPatrolNodeByIndex (0);
			}
		} 
		eActor.FindSuspiciousObject ();
		eActor.GetEnemyOutsideInfo ().SetViewDirection (nowNode.nodePoint);

		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		stateDelayTimer = 0f;
		stateDurationTimer = 0f;
		nowNode = null;
		nowActivated = false;
	}
	#endregion

	public PatrolNode GetPatrolNodeByIndex (int index)
	{
		if (index < 0 || patrolNodeList.Count <= index)
			return null;
		return patrolNodeList [index];
	}

	public void ReIndexPatrolNodesByDistance ()
	{
		patrolNodeList.Sort (delegate(PatrolNode x, PatrolNode y) {
			var dis01 = Vector3.Distance (x.nodePoint, actor.transform.position);
			var dis02 = Vector3.Distance (y.nodePoint, actor.transform.position);

			if (dis01 < dis02)
			{
				return -1;
			}
			else if(dis01 > dis02)
			{
				return 1;
			}
			else
			{
				return 0;
			}
		});

		for (int i  = 0; i < patrolNodeList.Count; i++)
		{
			patrolNodeList [i].nodeId = i;
			if (patrolNodeList [i].nodeId != 0) {
				patrolNodeList [i].fromNode = patrolNodeList [i - 1];
			}
		}
	}
}
