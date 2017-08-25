using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCloth : MonoBehaviour {

	private Cloth targetCloth;
	public RoomInfo roomInfo;

	private bool isSet = false;

	// Use this for initialization
	void Start () {
		Setting ();
		if (isSet) {
			roomInfo.onRoomObjectAdded += AddCapsuleColliderToList;
			roomInfo.onRoomObjectAdded += AddSphereColliderToList;
			roomInfo.onRoomObjectRemoved += RemoveCapsuleColliderInList;
			roomInfo.onRoomObjectRemoved += RemoveSphereColliderInList;
		}
	}

	void AddCapsuleColliderToList (DynamicObject obj)
	{
		var element = obj.GetComponentInChildren <CapsuleCollider>();
		if (null != element)
		{
			List<CapsuleCollider> nowColList = new List<CapsuleCollider> (targetCloth.capsuleColliders);
			nowColList.Add (element);
			targetCloth.capsuleColliders = nowColList.ToArray ();
		}
	}

	void AddSphereColliderToList (DynamicObject obj)
	{
		var element = obj.GetComponentInChildren <SphereCollider>();
		if (null != element)
		{
			List<ClothSphereColliderPair> nowColList = new List<ClothSphereColliderPair> (targetCloth.sphereColliders);
			var newPair = new ClothSphereColliderPair ();
			newPair.first = element;
			nowColList.Add (newPair);
			targetCloth.sphereColliders = nowColList.ToArray ();
		}
	}
		
	void RemoveCapsuleColliderInList (DynamicObject obj)
	{
		var element = obj.GetComponentInChildren <CapsuleCollider>();
		if (null != element)
		{
			List<CapsuleCollider> nowColList = new List<CapsuleCollider> (targetCloth.capsuleColliders);
			nowColList.Remove (element);
			targetCloth.capsuleColliders = nowColList.ToArray ();
		}
	}

	void RemoveSphereColliderInList (DynamicObject obj)
	{
		var element = obj.GetComponentInChildren <SphereCollider>();
		if (null != element)
		{
			List<ClothSphereColliderPair> nowColList = new List<ClothSphereColliderPair> (targetCloth.sphereColliders);
			for (int i = 0; i < nowColList.Count; i++)
			{
				if (nowColList [i].first == element)
				{
					nowColList.RemoveAt (i);
					break;
				}
			}
			targetCloth.sphereColliders = nowColList.ToArray ();
		}
	}

	void Setting ()
	{
		if (null == targetCloth)
			targetCloth = GetComponent<Cloth> ();
		if (null != targetCloth && null != roomInfo)
			isSet = true;
	}
}
