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
	public List<HideableObject> foundedHideableObjList = new List<HideableObject> ();

	[HideInInspector]
	public int itemInViewCount;
	[HideInInspector]
	public int hideableObjectInViewCount;

	public Vector3 lookDirection = Vector3.forward;

	public LayerMask viewableMask;

	public delegate void OnViewObjectChanged (DynamicObject obj);

	public event OnViewObjectChanged onViewObjectAdded;
	public event OnViewObjectChanged onViewObjectRemoved;

	// Use this for initialization
	protected new void Start () {
		base.Start ();
		actor = Actor.GetActor <EnemyOutsideInfo> (this);
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (FindDynamicObjectsInView ());
	}

	private IEnumerator RotateViewDirection (Vector3 targetDir)
	{
		var deltaDir = Vector3.Distance (targetDir, lookDirection);
		while (deltaDir >= 0.01f) {
			yield return new WaitForEndOfFrame ();
			lookDirection = Vector3.Lerp (lookDirection, targetDir, actor.customDeltaTime * 3f);
			if (lookDirection.x > 0)
				actor.SetLookDirection (false, 1);
			else if (lookDirection.x < 0)
				actor.SetLookDirection (true, 1);
		}
		actor.ResetSetLookDirectionPriority ();
	}

	public void SetViewDirection (Vector3 targetPos)
	{
		var dir = (targetPos - actor.transform.position).normalized;
		StopCoroutine ("RotateViewDirection");
		StartCoroutine ("RotateViewDirection", dir);
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

			if (Mathf.Abs (degreeZ01 - degreeZ02) <= viewAngle * 0.5f ||
				Mathf.Abs (degreeZ01 -degreeZ02) >= 360 - viewAngle * 0.5f) 
			{
				RaycastHit hit;
				Vector3 origin = actor.bodyCollider.bounds.center + Vector3.up * actor.bodyCollider.bounds.extents.y * 0.8f;
				if (Physics.Raycast (origin , (objPos - origin).normalized, out hit, viewMaxDistance, viewableMask))
				{
					var hitDynamicObj = DynamicObject.GetDynamicObject (hit.collider);
					if (hitDynamicObj == obj) {
						IdentifyAndAddDynamicObject (obj);
					} else if (null == hitDynamicObj) {
						IdentifyAndRemoveDynamicObject (obj);
					} else {
						Debug.Log (obj.name + " : " + hitDynamicObj.name);
					}
				}
			} 
			else
			{
				IdentifyAndRemoveDynamicObject (obj);
			}
			yield return new WaitForEndOfFrame ();
		}
	}

	public InteractableObject GetNearestObstacle ()
	{
		return GetRankedObstacleByDistance (0);
	}

	public InteractableObject GetRankedObstacleByDistance(int rank)
	{
		int index = 0;
		for (int i = 0; i < interactableObjectListInView.Count; i++)
		{
			var element = interactableObjectListInView [i];
			if (element.interactableObjectType == InteractableObjectType.HideableObject) {
				if (index != rank)
					index++;
				else
					return element;
			}
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
			switch (obj.objectType) {
			case DynamicObjectType.Actor:
				actorListInVeiw.Add ((Actor)obj);
				SortActorObjectListByDistance ();
				break;
			case DynamicObjectType.InteractableObject:
				interactableObjectListInView.Add ((InteractableObject)obj);
				if (((InteractableObject)obj).interactableObjectType == InteractableObjectType.HideableObject) {
					hideableObjectInViewCount++;
					foundedHideableObjList.Add ((HideableObject)obj);
					SortFoundedHideableObjectList (actor.transform.position);
				}
				else if (((InteractableObject)obj).interactableObjectType == InteractableObjectType.Item)
					itemInViewCount++;
				SortInteractableObjectListDistance ();
				break;
			}
			dynamicObjectListInView.Add (obj);
			if (null != onViewObjectAdded)
				onViewObjectAdded (obj);
			SortDynamicObjectListByDistance ();
		}
	}

	void IdentifyAndRemoveDynamicObject (DynamicObject obj)
	{
		if (dynamicObjectListInView.Contains (obj)) {
			switch (obj.objectType) {
			case DynamicObjectType.Actor:
				actorListInVeiw.Remove ((Actor)obj);
				SortActorObjectListByDistance ();
				break;
			case DynamicObjectType.InteractableObject:
				
				interactableObjectListInView.Remove ((InteractableObject)obj);

				if (((InteractableObject)obj).interactableObjectType == InteractableObjectType.HideableObject) {
					hideableObjectInViewCount--;
				}
				else if (((InteractableObject)obj).interactableObjectType == InteractableObjectType.Item)
					itemInViewCount--;
				SortInteractableObjectListDistance ();
				break;
			}
			dynamicObjectListInView.Remove (obj);
			if (null != onViewObjectRemoved)
				onViewObjectRemoved (obj);
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

	public void SortFoundedHideableObjectList (Vector3 point)
	{
		foundedHideableObjList.Sort (delegate(HideableObject x, HideableObject y) {
			var dis01 = Vector3.Distance (point, x.transform.position);
			var dis02 = Vector3.Distance (point, y.transform.position);
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
