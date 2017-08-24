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
	public Collider cameraRectCollider;

	[Header ("Editor Setting")]
	public Color roomRectColor = new Color (0.5529f, 0.9921f, 0.6313f, 0.0196f);
	public Color cameraRectColor = new Color (0.4941f, 0.6431f, 1f, 0.0196f);

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
				
				switch (obj.objectType) {
				case DynamicObjectType.Actor:
					if (!((Actor)obj).EnterRoom (this)) {
						return;
					}
					actorsInRoom.Add ((Actor)obj);
					break;
				case DynamicObjectType.InteractableObject:
					interactableObjectInRoom.Add ((InteractableObject)obj);
					break;
				}
				dynamicObjectsInRoom.Add (obj);
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
					((Actor)obj).ExitRoom (this);
					actorsInRoom.Remove ((Actor)obj);
					break;
				case DynamicObjectType.InteractableObject:
					interactableObjectInRoom.Remove ((InteractableObject)obj);
					break;
				}
			}
		}
	}

	public InteractableObject GetRankedObstacleByDistance (Vector3 point, int rank)
	{
		var rankedObstacleList = interactableObjectInRoom;
		rankedObstacleList.Sort (delegate(InteractableObject x, InteractableObject y) {
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
		int index = 0;
		for (int i = 0; i < rankedObstacleList.Count; i++)
		{
			var element = rankedObstacleList [i];
			if (element.interactableObjectType == InteractableObjectType.HideableObject)
			{
				if (index != rank)
					index++;
				else
					return element;
			}
		}
		return null;
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
