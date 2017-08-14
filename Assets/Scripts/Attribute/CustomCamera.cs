using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour {
	private Camera thisCamera;
	public RoomInfo nowFocusingRoom;
	public Actor followingTarget;
	[Header ("Camera Setting")]
	public bool useRoomAnchor = true;
	public bool useMaxDistance = true;

	[Header ("Following Setting")]
	public float followSpeed;
	public Vector3 offset;
	public float bobMultiply = 1.5f;
	public float maxDistance = 0.5f;

	private float xRatio;
	private float yRatio;
	private float zRatio;

	void Start ()
	{
		thisCamera = GetComponent<Camera> ();
	}

	// Update is called once per frame
	void Update () {
		if (null != followingTarget && null != followingTarget.roomInfo)
		{
			nowFocusingRoom = followingTarget.roomInfo;
			if (useRoomAnchor) {
				xRatio = (Mathf.Abs (followingTarget.bodyCollider.bounds.center.x + offset.x - nowFocusingRoom.roomRectCollider.bounds.min.x)) / nowFocusingRoom.roomRectCollider.bounds.size.x;
				if (followingTarget.stateInfo.isCrouhcing) {
					yRatio = (Mathf.Abs (followingTarget.bodyCollider.bounds.center.y + offset.y - nowFocusingRoom.roomRectCollider.bounds.min.y * bobMultiply)) / nowFocusingRoom.roomRectCollider.bounds.size.y;
				} else {
					yRatio = (Mathf.Abs (followingTarget.bodyCollider.bounds.center.y + offset.y - nowFocusingRoom.roomRectCollider.bounds.min.y)) / nowFocusingRoom.roomRectCollider.bounds.size.y;
				}
				zRatio = (Mathf.Abs (followingTarget.bodyCollider.bounds.center.z + offset.z - nowFocusingRoom.roomRectCollider.bounds.min.z)) / nowFocusingRoom.roomRectCollider.bounds.size.z;
					
				var tmpPos = transform.position;
					
				tmpPos.x = Mathf.Lerp (nowFocusingRoom.cameraRectCollider.bounds.min.x, nowFocusingRoom.cameraRectCollider.bounds.max.x, xRatio);
				tmpPos.y = Mathf.Lerp (nowFocusingRoom.cameraRectCollider.bounds.min.y, nowFocusingRoom.cameraRectCollider.bounds.max.y, yRatio);
				tmpPos.z = Mathf.Lerp (nowFocusingRoom.cameraRectCollider.bounds.min.z, nowFocusingRoom.cameraRectCollider.bounds.max.z, zRatio);
					
				transform.position = Vector3.Lerp (transform.position, tmpPos, followSpeed * Time.deltaTime);
				LockToCameraRect ();
			}
			if (useMaxDistance) {
				var tmpPos = transform.position;
				
				tmpPos.x = Mathf.Clamp (tmpPos.x, followingTarget.bodyCollider.bounds.center.x - maxDistance,followingTarget.bodyCollider.bounds.center.x + maxDistance );
				tmpPos.y = Mathf.Clamp (tmpPos.y, followingTarget.bodyCollider.bounds.center.y - maxDistance,followingTarget.bodyCollider.bounds.center.y + maxDistance );
				tmpPos.z = Mathf.Clamp (tmpPos.z, followingTarget.bodyCollider.bounds.center.z - maxDistance,followingTarget.bodyCollider.bounds.center.z + maxDistance );
				transform.position = tmpPos;
			}
			
		}

	}

	void LockToCameraRect ()
	{
		var tmpPos = transform.position;
		tmpPos.x = Mathf.Clamp (tmpPos.x, nowFocusingRoom.cameraRectCollider.bounds.min.x, nowFocusingRoom.cameraRectCollider.bounds.max.x);
		tmpPos.y = Mathf.Clamp (tmpPos.y, nowFocusingRoom.cameraRectCollider.bounds.min.y, nowFocusingRoom.cameraRectCollider.bounds.max.y);
		tmpPos.z = Mathf.Clamp (tmpPos.z, nowFocusingRoom.cameraRectCollider.bounds.min.z, nowFocusingRoom.cameraRectCollider.bounds.max.z);
		transform.position = tmpPos;
	}
}
