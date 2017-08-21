using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor (typeof(HideableObject))]
public class HideableObjectRenderer :  Editor{

	private HideableObject hideableObj;

	public static float handleSize = 0.07f;
	public static float pickSize = 0.08f;

	public static int selectedIndex = -1;

	public void OnSceneGUI ()
	{
		#if UNITY_EDITOR
		hideableObj = target as HideableObject;
		if (null == hideableObj)
			return;

		InitHideableFaces ();


		for (int index = 0; index < 6; index++)
		{
			var faceElement = hideableObj.hideableFaceList [index];
			var size = HandleUtility.GetHandleSize (hideableObj.transform.position + faceElement.point);
			if (faceElement.hideable)
			{
				Handles.color = Color.green;
				if (Handles.Button (hideableObj.transform.position + faceElement.point, Quaternion.identity, handleSize, pickSize, Handles.SphereHandleCap))
				{
					selectedIndex = index;
					faceElement.hideable = false;
				}
			}
			else
			{
				Handles.color = Color.red;
				if (Handles.Button (hideableObj.transform.position + faceElement.point, Quaternion.identity, handleSize, pickSize, Handles.CubeHandleCap))
				{
					selectedIndex = index;
					faceElement.hideable = true;
				}
			}
			if (selectedIndex == index) {
				EditorGUI.BeginChangeCheck ();
				faceElement.point = Handles.DoPositionHandle (faceElement.point, Quaternion.identity);
				if (EditorGUI.EndChangeCheck ())
				{
				Undo.RecordObject (hideableObj, "Change Radius");
				EditorUtility.SetDirty (hideableObj);
				}
			}
		}

		#endif
	}

	void InitHideableFaces ()
	{
		if (hideableObj.hideableFaceList.Count < 6) {
			var count = hideableObj.hideableFaceList.Count;
			for (int i = count; i < 6; i++) {
				hideableObj.hideableFaceList.Add (new HideableFace ());
				hideableObj.hideableFaceList [i].faceName = (FaceName)i;
			}
		}
		else
			if (hideableObj.hideableFaceList.Count > 6) {
				var count = hideableObj.hideableFaceList.Count;
				for (int i = count - 1; i >= 6; i--) {
					hideableObj.hideableFaceList.RemoveAt (i);
				}
			}
	}
}
