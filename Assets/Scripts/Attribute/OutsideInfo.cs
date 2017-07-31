using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideInfo : MonoBehaviour {
	public Actor actor;

	public List<DynamicObject> nearDynamicObjList = new List<DynamicObject>();

	public List<Actor> nearActorObjList = new List<Actor>(); 
	public List<InteractableObject> nearInteractableObjList = new List<InteractableObject> ();
	public List<Bullet> nearBulletObjList = new List<Bullet> ();

	public void Start ()
	{
		actor = Actor.GetActor <OutsideInfo> (this);
	}

	public void OnTriggerEnter (Collider col)
	{
		var dynamicObj = DynamicObject.GetDynamicObject (col);
		if (null != dynamicObj)
		{
			if (!nearDynamicObjList.Contains (dynamicObj)) {
				IdentifyAndAddDynamicObjects (dynamicObj);
			}
		}
	}

	public void OnTriggerStay (Collider col)
	{
		var dynamicObj = DynamicObject.GetDynamicObject (col);
		if (null != dynamicObj)
		{
			if (!nearDynamicObjList.Contains (dynamicObj)) {
				IdentifyAndAddDynamicObjects (dynamicObj);
			}
		}
	}

	public void OnTriggerExit (Collider col)
	{
		var dynamicObj = DynamicObject.GetDynamicObject (col);
		if (null != dynamicObj)
		{
			if (nearDynamicObjList.Contains (dynamicObj)) {
				IdentifyAndRemoveDynamicObjects (dynamicObj);
			}
			return;
		}
	}

	void SortDynamicListByDistance ()
	{
		nearDynamicObjList.Sort (delegate (DynamicObject x, DynamicObject y) {
			if (null == x)
			{
				nearDynamicObjList.Remove(x);
				return 0;
			}
			if (null == y)
			{
				nearDynamicObjList.Remove (y);
				return 0;
			}

			var dis01 = Vector3.Distance (transform.position, x.transform.position);
			var dis02 = Vector3.Distance (transform.position, y.transform.position);

			if (dis01 > dis02) {
				return 1;
			}
			else
			{
				if (dis01 < dis02) 
				{
					return -1;
				}
			}
			return 0;
		});
	}

	void SortActorListByDistance ()
	{
		nearActorObjList.Sort (delegate (Actor x, Actor y) {
			if (null == x)
			{
				nearActorObjList.Remove (x);
				return 0;
			}
			if (null == y)
			{
				nearActorObjList.Remove (y);
				return 0;
			}

			var dis01 = Vector3.Distance (transform.position, x.transform.position);
			var dis02 = Vector3.Distance (transform.position, y.transform.position);

			if (dis01 > dis02) 
			{
				return 1;
			}
			else
			{
				if (dis01 < dis02) 
				{
					return -1;
				}
			}
			return 0;
		});
	}

	void SortInteractableObjectListByDistance ()
	{
		nearInteractableObjList.Sort (delegate(InteractableObject x, InteractableObject y) {
			if (null == x)
			{
				nearInteractableObjList.Remove (x);
				return 0;
			}
			if (null == y)
			{
				nearInteractableObjList.Remove (y);
				return 0;
			}

			var dis01 = Vector3.Distance (transform.position, x.transform.position);
			var dis02 = Vector3.Distance (transform.position, y.transform.position);

			if (dis01 > dis02) 
			{
				return 1;
			}
			else
			{
				if (dis01 < dis02) 
				{
					return -1;
				}
			}
			return 0;
		});
	}

	private void IdentifyAndAddDynamicObjects (DynamicObject dynamicObj)
	{
		nearDynamicObjList.Add (dynamicObj);
		SortDynamicListByDistance ();

		switch (dynamicObj.objectType) {
		case DynamicObjectType.Actor:
			if ((Actor)dynamicObj != actor) {
				nearActorObjList.Add ((Actor)dynamicObj);
				SortActorListByDistance ();
			}
			break;
		case DynamicObjectType.InteractableObject:
			nearInteractableObjList.Add ((InteractableObject)dynamicObj);
			SortInteractableObjectListByDistance ();
			break;
		case DynamicObjectType.Bullet:
			nearBulletObjList.Add ((Bullet)dynamicObj);
			break;
		}
	}

	private void IdentifyAndRemoveDynamicObjects (DynamicObject dynamicObj)
	{
		nearDynamicObjList.Remove (dynamicObj);
		SortDynamicListByDistance ();

		switch (dynamicObj.objectType) {
		case DynamicObjectType.Actor:
			nearActorObjList.Remove ((Actor)dynamicObj);
			SortActorListByDistance ();
			break;
		case DynamicObjectType.InteractableObject:
			nearInteractableObjList.Remove ((InteractableObject)dynamicObj);
			break;
		case DynamicObjectType.Bullet:
			nearBulletObjList.Remove ((Bullet)dynamicObj);
			break;
		}
	}

	public DynamicObject FindNearestDynamicObject ()
	{
		if (nearDynamicObjList.Count != 0)
		{
			return nearDynamicObjList [0];
		}
		return null;
	}

	public InteractableObject FindNearestObstacle ()
	{
		
		for (int i = 0; i < nearInteractableObjList.Count; i++)
		{
			if (nearInteractableObjList [i].interactableObjectType == InteractableObjectType.Obstacle)
			{
				return nearInteractableObjList [i];
			}
		}

		return null;
	}

	public InteractableObject FindNearestItem ()
	{
		for (int i = 0; i < nearInteractableObjList.Count; i++)
		{
			if (nearInteractableObjList [i].interactableObjectType == InteractableObjectType.Item)
			{
				return nearInteractableObjList [i];
			}
		}
		return null;
	}

	public Actor FindNearestEnemyTypeObject ()
	{
		for (int i = 0; i < nearActorObjList.Count; i++)
		{
			if (nearActorObjList [i].humanInfo.humanType == HumanType.Enemy)
			{
				return nearActorObjList [i];
			}
		}
		return null;
	}

	public Actor FindNearestNPCTypeObject ()
	{
		for (int i = 0; i < nearActorObjList.Count; i++)
		{
			if (nearActorObjList [i].humanInfo.humanType == HumanType.NPC)
			{
				return nearActorObjList [i];
			}
		}
		return null;
	}
}
