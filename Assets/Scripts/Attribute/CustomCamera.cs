using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour {
	private Camera thisCamera;
	public RoomInfo nowFocusingRoom;
	public Actor followingTarget;

	[Header ("Use Setting")]
	public bool useRoomAnchor = true;
	public bool useMaxDistance = true;
	public bool useMinDistance = true;

	[Header ("Camera Setting")]
	public float followSpeed;
	public Vector3 offset;
	public float bobAmount = 1f;
	public float bobMultiply = 0.5f;

	public float maxDistance = 0.5f;
	public float minDistance = 0.5f;

	private float xRatio;
	private float yRatio;
	private float zRatio;

	private float disToFollowingTarget = 0f;

	void Start ()
	{
		thisCamera = GetComponent<Camera> ();
	}

	// Update is called once per frame
	void Update () {
		if (null != followingTarget && null != followingTarget.roomInfo)
		{
			nowFocusingRoom = followingTarget.roomInfo;
			disToFollowingTarget = Vector3.Distance (transform.position, followingTarget.bodyCollider.bounds.center);

			if (useMaxDistance) {
				var tmpPos = transform.position;	
				var tmpVec = tmpPos - followingTarget.bodyCollider.bounds.center;
				var dir = tmpVec.normalized;

				if (disToFollowingTarget >= maxDistance) {
					tmpPos =  tmpPos - (disToFollowingTarget - maxDistance) * dir;
					transform.position = Vector3.Lerp (transform.position, tmpPos, Time.deltaTime);
				}
			}	

			if (useMinDistance) {
				var tmpPos = transform.position;
				var tmpVec = tmpPos - followingTarget.bodyCollider.bounds.center;
				var dir = tmpVec.normalized;

				if (disToFollowingTarget < minDistance) {
					tmpPos = tmpPos + (minDistance - disToFollowingTarget) * dir;
					transform.position = Vector3.Lerp (transform.position, tmpPos, Time.deltaTime);
				}
			}

			if (useRoomAnchor) {
				LockToCameraRect ();
			}
		}
	}

	public void SetFousingRoom (RoomInfo newRoom)
	{

	}

	void LockToCameraRect ()
	{
		xRatio = (Mathf.Abs (followingTarget.bodyCollider.bounds.center.x + offset.x - nowFocusingRoom.roomRectCollider.bounds.min.x)) / nowFocusingRoom.roomRectCollider.bounds.size.x;
		if (followingTarget.stateInfo.isCrouhcing) {
			yRatio = bobMultiply * bobAmount + (Mathf.Abs (followingTarget.bodyCollider.bounds.center.y + offset.y - nowFocusingRoom.roomRectCollider.bounds.min.y)) / nowFocusingRoom.roomRectCollider.bounds.size.y;
		} else {
			yRatio = (Mathf.Abs (followingTarget.bodyCollider.bounds.center.y + offset.y - nowFocusingRoom.roomRectCollider.bounds.min.y)) / nowFocusingRoom.roomRectCollider.bounds.size.y;
		}
		zRatio = (Mathf.Abs (followingTarget.bodyCollider.bounds.center.z + offset.z - nowFocusingRoom.roomRectCollider.bounds.min.z)) / nowFocusingRoom.roomRectCollider.bounds.size.z;

		var tmpPos = transform.position;

		tmpPos.x = Mathf.Lerp (nowFocusingRoom.cameraRectCollider.min.x, nowFocusingRoom.cameraRectCollider.max.x, xRatio);
		tmpPos.y = Mathf.Lerp (nowFocusingRoom.cameraRectCollider.min.y, nowFocusingRoom.cameraRectCollider.max.y, yRatio);
		tmpPos.z = Mathf.Lerp (nowFocusingRoom.cameraRectCollider.min.z, nowFocusingRoom.cameraRectCollider.max.z, zRatio);

		transform.position = Vector3.Lerp (transform.position, tmpPos, followSpeed * Time.deltaTime);
	}
}
