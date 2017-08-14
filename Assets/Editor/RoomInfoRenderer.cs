using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor (typeof (RoomInfo))]
public class RoomInfoRenderer : Editor {

	private RoomInfo selectedRoomInfo;
	private static float buttonSize = 0.5f;

	private void OnSceneGUI ()
	{
		#if UNITY_EDITOR
		selectedRoomInfo = target as RoomInfo;

		if (null == selectedRoomInfo)
			return;

		if (null == selectedRoomInfo.roomRectCollider)
		{
			Debug.Log ("Null COllider");
			selectedRoomInfo.roomRectCollider = selectedRoomInfo.GetComponent<Collider>();
			if (null == selectedRoomInfo.roomRectCollider)
			{
				selectedRoomInfo.roomRectCollider = selectedRoomInfo.gameObject.AddComponent <BoxCollider> ();
				selectedRoomInfo.roomRectCollider.isTrigger = true;
			}
			return;
		}
		DrawRoomColliderRect (selectedRoomInfo);

		#endif
	}

	public static int selectedIndex = -1;
	public static CustomCameraButtonType buttonType;

	private static void MakeButton (int index, Vector3 point, float size, CustomCameraButtonType bType)
	{
		if (Handles.Button (point, Quaternion.identity, size * EnemyViewRenderer.handleSize, EnemyViewRenderer.pickSize, Handles.DotHandleCap)) {
			selectedIndex = index;
			buttonType = bType;
		}
	}

	private static List <Vector3[]> DrawRectangularWithOutline (Vector3 center,Vector3 expand, CustomCameraButtonType bType)
	{
		List<Vector3[]> faceList = new List<Vector3[]> ();
		Vector3[] vertices = new Vector3[8];
		vertices [0] = center + Vector3.forward * expand.z + Vector3.down * expand.y + Vector3.left * expand.x; // 왼쪽 아래 앞
		vertices [1] = center + Vector3.forward * expand.z + Vector3.down * expand.y + Vector3.right * expand.x; // 오른쪽 아래 앞
		vertices [2] = center + Vector3.forward * expand.z + Vector3.up * expand.y + Vector3.right * expand.x;
		vertices [3] = center + Vector3.forward * expand.z + Vector3.up * expand.y + Vector3.left * expand.x;
		vertices [4] = center + Vector3.back * expand.z + Vector3.up * expand.y + Vector3.left * expand.x;
		vertices [5] = center + Vector3.back * expand.z + Vector3.up * expand.y + Vector3.right * expand.x;
		vertices [6] = center + Vector3.back * expand.z + Vector3.down * expand.y + Vector3.right * expand.x;
		vertices [7] = center + Vector3.back * expand.z + Vector3.down * expand.y + Vector3.left * expand.x;

		Vector3[] front = new Vector3[4];
		front [0] = vertices [0];
		front [1] = vertices [1];
		front [2] = vertices [2];
		front [3] = vertices [3];

		Vector3[] back = new Vector3[4];
		back [0] = vertices [4];
		back [1] = vertices [5];
		back [2] = vertices [6];
		back [3] = vertices [7];

		Vector3[] leftSide = new Vector3[4];
		leftSide [0] = vertices [0];
		leftSide [1] = vertices [3];
		leftSide [2] = vertices [4];
		leftSide [3] = vertices [7];

		Vector3[] rightSide = new Vector3[4];
		rightSide [0] = vertices [1];
		rightSide [1] = vertices [2];
		rightSide [2] = vertices [5];
		rightSide [3] = vertices [6];

		Vector3[] top = new Vector3[4];
		top [0] = vertices [2];
		top [1] = vertices [3];
		top [2] = vertices [4];
		top [3] = vertices [5];

		Vector3[] down = new Vector3[4];
		down [0] = vertices [0];
		down [1] = vertices [1];
		down [2] = vertices [6];
		down [3] = vertices [7];

		Handles.DrawSolidRectangleWithOutline (
			front,
			new Color (0.6f, 0.721f, 0.596f, 0.05f),
			new Color (0.91f, 0.2901f, 0.3725f, 0.05f)
		);

		Handles.DrawSolidRectangleWithOutline (
			back,
			new Color (0.6f, 0.721f, 0.596f, 0.05f),
			new Color (0.91f, 0.2901f, 0.3725f, 0.05f)
		);

		Handles.DrawSolidRectangleWithOutline (
			leftSide,
			new Color (0.6f, 0.721f, 0.596f, 0.05f),
			new Color (0.91f, 0.2901f, 0.3725f, 0.05f)
		);

		Handles.DrawSolidRectangleWithOutline (
			rightSide,
			new Color (0.6f, 0.721f, 0.596f, 0.05f),
			new Color (0.91f, 0.2901f, 0.3725f, 0.05f)
		);

		Handles.DrawSolidRectangleWithOutline (
			top,
			new Color (0.6f, 0.721f, 0.596f, 0.05f),
			new Color (0.91f, 0.2901f, 0.3725f, 0.05f)
		);
			
		Handles.DrawSolidRectangleWithOutline (
			down,
			new Color (0.6f, 0.721f, 0.596f, 0.05f),
			new Color (0.91f, 0.2901f, 0.3725f, 0.05f)
		);

		faceList.Add (front);
		faceList.Add (back);
		faceList.Add (rightSide);
		faceList.Add (leftSide);
		faceList.Add (top);
		faceList.Add (down);

		return faceList;
	}

	public static void DrawRoomColliderRect (RoomInfo roomInfo)
	{
		var faces = DrawRectangularWithOutline (roomInfo.roomRectCollider.bounds.center, roomInfo.roomRectCollider.bounds.extents, CustomCameraButtonType.CameraRect);
		var tmpVector = Vector3.zero;
		for (int i = 0; i < 6; i++)
		{
			var midPoint = Vector3.zero;
			for (int j = 0; j < 4; j++)
			{
				midPoint += faces [i] [j];
			}
			midPoint /= 4f;
			MakeButton (i, midPoint, buttonSize, CustomCameraButtonType.RoomRect);
			if (selectedIndex == i && buttonType == CustomCameraButtonType.RoomRect)
			{
				EditorGUI.BeginChangeCheck ();
				midPoint = Handles.DoPositionHandle (midPoint, Quaternion.identity);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (roomInfo, "Change Collider Rect");
					EditorUtility.SetDirty (roomInfo);
					if (i == 0 || i == 1) {// Front or Back
						tmpVector =  new Vector3(
							roomInfo.roomRectCollider.bounds.extents.x,
							roomInfo.roomRectCollider.bounds.extents.y,
							Mathf.Abs (midPoint.z)
						);
						roomInfo.roomRectCollider.bounds.Expand (tmpVector - roomInfo.roomRectCollider.bounds.extents);
					} 
					else if (i == 2 || i == 3) { // right side or left side
						tmpVector =  new Vector3(
							Mathf.Abs (midPoint.x),
							roomInfo.roomRectCollider.bounds.extents.y,
							roomInfo.roomRectCollider.bounds.extents.z
						);
						roomInfo.roomRectCollider.bounds.Expand (tmpVector - roomInfo.roomRectCollider.bounds.extents);
					} 
					else if (i == 4 || i == 5) { // top or down
						tmpVector =  new Vector3(
							roomInfo.roomRectCollider.bounds.extents.x,
							Mathf.Abs (midPoint.z),
							roomInfo.roomRectCollider.bounds.extents.z
						);
						roomInfo.roomRectCollider.bounds.Expand (tmpVector - roomInfo.roomRectCollider.bounds.extents);
					}
				}
			}
		}
	}

	public static void DrawRoomCameraRect (RoomInfo roomInfo)
	{

	}

}
