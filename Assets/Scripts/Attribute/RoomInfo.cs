using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RoomInfo : MonoBehaviour {
	public string roomName;
	public RoomState roomState;

	public Vector3 combatStartPoint;

	public List<DynamicObject> dynamicObjectsInRoom = new List<DynamicObject>();
	public List<Actor> actorsInRoom = new List<Actor>();
	public List<InteractableObject> interactableObjectInRoom = new List<InteractableObject>();
	public Collider roomRectCollider;

	public void Start ()
	{
		roomRectCollider = GetComponent <Collider> ();
	}

	public void OnTriggerEnter (Collider col)
	{
		IdentifyAndAddObject (col);
	}

	public void OnTriggerStay (Collider col)
	{
		IdentifyAndAddObject (col);
	}
	public void OnTriggerExit (Collider col)
	{
		IdentifyAndRemoveObject (col);
		Debug.Log ("Exit");
	}

	public void SetRoomStateToCombatState (Vector3 combatPoint)
	{
		roomState = RoomState.Combat;
		combatStartPoint = combatPoint;
	}

	void IdentifyAndAddObject (Collider col)
	{
		var obj = DynamicObject.GetDynamicObject<Collider> (col);
		if (null != obj) {
			dynamicObjectsInRoom.Remove (null);
			actorsInRoom.Remove (null);
			interactableObjectInRoom.Remove (null);

			if (!dynamicObjectsInRoom.Contains (obj)) {
				dynamicObjectsInRoom.Add (obj);
				switch (obj.objectType) {
				case DynamicObjectType.Actor:
					actorsInRoom.Add ((Actor)obj);
					((Actor)obj).roomInfo = this;
					break;
				case DynamicObjectType.InteractableObject:
					interactableObjectInRoom.Add ((InteractableObject)obj);
					break;
				}
			}
		}
	}

	void IdentifyAndRemoveObject (Collider col)
	{
		var obj = DynamicObject.GetDynamicObject<Collider> (col);
		if (null != obj) {
			dynamicObjectsInRoom.Remove (null);
			actorsInRoom.Remove (null);
			interactableObjectInRoom.Remove (null);

			if (dynamicObjectsInRoom.Contains (obj)) {
				dynamicObjectsInRoom.Remove (obj);
				switch (obj.objectType) {
				case DynamicObjectType.Actor:
					actorsInRoom.Remove ((Actor)obj);
					break;
				case DynamicObjectType.InteractableObject:
					interactableObjectInRoom.Remove ((InteractableObject)obj);
					break;
				}
			}
		}
	}

	public static RoomInfo FindRoomByName (string roomName)
	{
		var roomObjs = GameObject.FindObjectsOfType<RoomInfo> ();
		for (int i = 0; i < roomObjs.Length; i++)
		{
			if (roomObjs [i].roomName == roomName)
			{
				return roomObjs [i];
			}
		}
		return null;
	}
}
