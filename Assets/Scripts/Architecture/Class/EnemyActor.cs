using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : Actor {

	public float viewAngle;
	public float viewDistance;
	public List<DynamicObject> dynamicObjectListInView = new List<DynamicObject>();
	public List<Actor> actorListInVeiw = new List<Actor>();
	public List<InteractableObject> interactableObjectListInView = new List<InteractableObject>();
	public Vector3 lookDirection = Vector3.forward;

	// Use this for initialization
	protected new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected new void Update () {

		StartCoroutine (FindDynamicObjectsInView ());
		base.Update ();
	}

	public IEnumerator FindDynamicObjectsInView ()
	{
		for (int i = 0; i < roomInfo.dynamicObjectsInRoom.Count; i++) {
			var obj = roomInfo.dynamicObjectsInRoom [i];
			if (obj == ((DynamicObject)this)) {
				continue;
			}
			var objPos = obj.transform.position;
			var dis = Vector3.Distance (transform.position, obj.transform.position);
			if (dis > viewDistance) {
				if (dynamicObjectListInView.Contains (obj)) {
					dynamicObjectListInView.Remove (obj);
					switch (obj.objectType) {
					case DynamicObjectType.Actor:
						actorListInVeiw.Remove ((Actor)obj);
						break;
					case DynamicObjectType.InteractableObject:
						interactableObjectListInView.Remove ((InteractableObject)obj);
						break;
					}
				}
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
				if (!dynamicObjectListInView.Contains (obj)) {
					dynamicObjectListInView.Add (obj);
					switch (obj.objectType) {
					case DynamicObjectType.Actor:
						actorListInVeiw.Add ((Actor)obj);
						break;
					case DynamicObjectType.InteractableObject:
						interactableObjectListInView.Add ((InteractableObject)obj);
						break;
					}
				}
			} else {
				if (dynamicObjectListInView.Contains (obj)) {
					dynamicObjectListInView.Remove (obj);
					switch (obj.objectType) {
					case DynamicObjectType.Actor:
						actorListInVeiw.Remove ((Actor)obj);
						break;
					case DynamicObjectType.InteractableObject:
						interactableObjectListInView.Remove ((InteractableObject)obj);
						break;
					}
				}
			}
			yield return new WaitForEndOfFrame ();
		}
	}

	public IEnumerator FindSuspiciousObject ()
	{
		for (int i = 0; i < actorListInVeiw.Count; i++)
		{
			var actorObj = actorListInVeiw [i];
			if (actorObj.humanInfo.humanType == HumanType.Player)
			{

			}
		}
		yield return null;
	}

	public Vector3 ListenSuspiciousSound ()
	{
		return Vector3.zero;
	}
}
