using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(EnemyOutsideInfo))]
public class EnemyViewRenderer :  Editor{

	private EnemyOutsideInfo selectedOutsideInfo;

	private void OnSceneGUI ()
	{
		#if UNITY_EDITOR
		selectedOutsideInfo = target as EnemyOutsideInfo;
		if (null == selectedOutsideInfo)
			return;
		//Draw View Distance
		DrawViewableRect (selectedOutsideInfo);
		//Draw Recognizable Distance
		DrawRecognizableRect (selectedOutsideInfo);
		#endif
	}

	public static float handleSize = 0.04f;
	public static float pickSize = 0.08f;

	public static int selectedIndex = -1;

	public static void DrawViewableRect (EnemyOutsideInfo eOutsideInfo)
	{
		Handles.color = new Color (0.447f,0.992f, 0.416f, 0.05f);
		Handles.DrawSolidArc (
			eOutsideInfo.actor.transform.position,
			Vector3.up,
			eOutsideInfo.lookDirection,
			eOutsideInfo.viewAngle * 0.5f,
			eOutsideInfo.viewMaxDistance);
		Handles.DrawSolidArc (
			eOutsideInfo.actor.transform.position,
			Vector3.down,
			eOutsideInfo.lookDirection,
			eOutsideInfo.viewAngle * 0.5f,
			eOutsideInfo.viewMaxDistance);

		var arcPoint = eOutsideInfo.actor.transform.position + eOutsideInfo.lookDirection * eOutsideInfo.viewMaxDistance;
		var size = HandleUtility.GetHandleSize (arcPoint);

		Handles.color = Color.white;
		if (Handles.Button (arcPoint, Quaternion.identity, size * handleSize, pickSize, Handles.DotHandleCap)) {
			selectedIndex = 1;
		}
		if (selectedIndex == 1) {
			EditorGUI.BeginChangeCheck ();
			arcPoint = Handles.DoPositionHandle (arcPoint, Quaternion.identity);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (eOutsideInfo, "Change Radius");
				EditorUtility.SetDirty (eOutsideInfo);
				eOutsideInfo.lookDirection = (arcPoint - eOutsideInfo.actor.transform.position).normalized;
				eOutsideInfo.viewMaxDistance = Vector3.Distance (eOutsideInfo.transform.position, arcPoint);
			}
		}
	}

	public static void DrawRecognizableRect (EnemyOutsideInfo eOutsideInfo)
	{
		Handles.color = new Color (0.7f,0f,0f, 0.05f);
		Handles.DrawSolidArc (
			eOutsideInfo.actor.transform.position,
			Vector3.up,
			eOutsideInfo.lookDirection,
			eOutsideInfo.viewAngle * 0.5f,
			eOutsideInfo.viewRecognizeDistance);
		Handles.DrawSolidArc (
			eOutsideInfo.actor.transform.position,
			Vector3.down,
			eOutsideInfo.lookDirection,
			eOutsideInfo.viewAngle * 0.5f,
			eOutsideInfo.viewRecognizeDistance);

		var arcPoint = eOutsideInfo.actor.transform.position + eOutsideInfo.lookDirection * eOutsideInfo.viewRecognizeDistance;
		var size = HandleUtility.GetHandleSize (arcPoint);
		Handles.color = Color.white;
		if (Handles.Button (arcPoint, Quaternion.identity, size * handleSize, pickSize, Handles.DotHandleCap)) {
			selectedIndex = 2;
		}

		if (selectedIndex == 2) {
			EditorGUI.BeginChangeCheck ();
			arcPoint = Handles.DoPositionHandle (arcPoint, Quaternion.identity);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (eOutsideInfo, "Change Radius");
				EditorUtility.SetDirty (eOutsideInfo);
				eOutsideInfo.lookDirection = (arcPoint - eOutsideInfo.actor.transform.position).normalized;
				eOutsideInfo.viewRecognizeDistance = Vector3.Distance (eOutsideInfo.transform.position, arcPoint);
			}
		}
	}

}
