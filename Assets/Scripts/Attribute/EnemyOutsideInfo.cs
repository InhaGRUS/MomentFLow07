using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOutsideInfo : OutsideInfo {

	private RoomInfo roomInfo;

	public float viewAngle;
	public float viewMaxDistance = 10;
	public float viewRecognizeDistance = 5;
	public List<DynamicObject> dynamicObjectListInView = new List<DynamicObject>();
	public List<Actor> actorListInVeiw = new List<Actor>();
	public List<InteractableObject> interactableObjectListInView = new List<InteractableObject>();
	public Vector3 lookDirection = Vector3.forward;

	// Use this for initialization
	void Start () {
		actor = Actor.GetActor <EnemyOutsideInfo> (this);
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (FindDynamicObjectsInView ());
	}

	public IEnumerator FindDynamicObjectsInView ()
	{
		roomInfo = actor.roomInfo;
		for (int i = 0; i < roomInfo.dynamicObjectsInRoom.Count; i++) {
			var obj = roomInfo.dynamicObjectsInRoom [i];
			if (obj == ((DynamicObject)actor)) {
				continue;
			}
			var objPos = obj.transform.position;
			var dis = Vector3.Distance (transform.position, obj.transform.position);
			if (dis > viewMaxDistance) {
				IdentifyAndRemoveDynamicObject (obj);
				continue;
			}
			var adjustedPos = new Vector3 (objPos.x, transform.position.y, objPos.z);
			var disZ_x = adjustedPos.x - transform.position.x;
			var disZ_z = adjustedPos.z - transform.position.z;
			var degreeZ01 = Mathf.Atan2 (disZ_x, disZ_z) * Mathf.Rad2Deg;

			disZ_x = lookDirection.x;
			disZ_z = lookDirection.z;
			var degreeZ02 = Mathf.Atan2 (disZ_x, disZ_z) * Mathf.Rad2Deg;

			if (Mathf.Abs (degreeZ01 - degreeZ02) <= viewAngle ||
				Mathf.Abs (degreeZ01 -degreeZ02) >= 360 - viewAngle) {
				IdentifyAndAddDynamicObject (obj);
			} else {
				IdentifyAndRemoveDynamicObject (obj);
			}
			yield return new WaitForEndOfFrame ();
		}
	}

	public InteractableObject GetNearestObstacle ()
	{
		for (int i = 0; i < interactableObjectListInView.Count; i++)
		{
			var element = interactableObjectListInView [i];
			if (element.interactableObjectType == InteractableObjectType.Obstacle)
				return element;
		}
		return null;
	}

	public Actor GetNearestPlayer ()
	{
		for (int i = 0; i < actorListInVeiw.Count; i++)
		{
			var element = actorListInVeiw [i];
			if (element.humanInfo.humanType == HumanType.Player)
				return element;
		}
		return null;
	}

	public Actor GetNearestEnemy ()
	{
		for (int i = 0; i < actorListInVeiw.Count; i++)
		{
			var element = actorListInVeiw [i];
			if (element.humanInfo.humanType == HumanType.Enemy)
				return element;
		}
		return null;
	}

	void IdentifyAndAddDynamicObject (DynamicObject obj)
	{
		if (!dynamicObjectListInView.Contains (obj)) {
			dynamicObjectListInView.Add (obj);
			switch (obj.objectType) {
			case DynamicObjectType.Actor:
				actorListInVeiw.Add ((Actor)obj);
				SortActorObjectListByDistance ();
				break;
			case DynamicObjectType.InteractableObject:
				interactableObjectListInView.Add ((InteractableObject)obj);
				SortInteractableObjectListDistance ();
				break;
			}
			SortDynamicObjectListByDistance ();
		}
	}

	void IdentifyAndRemoveDynamicObject (DynamicObject obj)
	{
		if (dynamicObjectListInView.Contains (obj)) {
			dynamicObjectListInView.Remove (obj);
			switch (obj.objectType) {
			case DynamicObjectType.Actor:
				actorListInVeiw.Remove ((Actor)obj);
				SortActorObjectListByDistance ();
				break;
			case DynamicObjectType.InteractableObject:
				interactableObjectListInView.Remove ((InteractableObject)obj);
				SortInteractableObjectListDistance ();
				break;
			}
			SortDynamicObjectListByDistance ();
		}
	}

	public void SortDynamicObjectListByDistance ()
	{
		dynamicObjectListInView.Sort (delegate(DynamicObject x, DynamicObject y) {
			var dis01 = Vector3.Distance (transform.position, x.transform.position);
			var dis02 = Vector3.Distance (transform.position, y.transform.position);
			if (dis01 < dis02)	
			{
				return -1;
			}
			else if (dis01 == dis02)
			{
				return 0;
			}
			return 1;
		});
	}

	public void SortActorObjectListByDistance ()
	{
		actorListInVeiw.Sort (delegate(Actor x, Actor y) {
			var dis01 = Vector3.Distance (transform.position, x.transform.position);
			var dis02 = Vector3.Distance (transform.position, y.transform.position);
			if (dis01 < dis02)	
			{
				return -1;
			}
			else if (dis01 == dis02)
			{
				return 0;
			}
			return 1;
		});
	}

	public void SortInteractableObjectListDistance ()
	{
		interactableObjectListInView.Sort (delegate(InteractableObject x, InteractableObject y) {
			var dis01 = Vector3.Distance (transform.position, x.transform.position);
			var dis02 = Vector3.Distance (transform.position, y.transform.position);
			if (dis01 < dis02)	
			{
				return -1;
			}
			else if (dis01 == dis02)
			{
				return 0;
			}
			return 1;
		});
	}
}
