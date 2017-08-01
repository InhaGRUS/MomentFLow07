using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(EnemyBodyChaseChecker))]
public class EnemyChaseStateRenderer : Editor {

	private EnemyBodyChaseChecker selectedChekcerInfo;

	private void OnSceneGUI ()
	{
		#if UNITY_EDITOR
		selectedChekcerInfo = target as EnemyBodyChaseChecker;
		if (null == selectedChekcerInfo)
			return;
		if (null == selectedChekcerInfo.eActor)
		{
			selectedChekcerInfo.actor = selectedChekcerInfo.GetComponentInParent<Actor> ();
			selectedChekcerInfo.eActor = selectedChekcerInfo.GetComponentInParent<EnemyActor>();
		}
		
		EnemyViewRenderer.DrawViewableRect (selectedChekcerInfo.eActor.GetEnemyOutsideInfo());
		EnemyViewRenderer.DrawRecognizableRect (selectedChekcerInfo.eActor.GetEnemyOutsideInfo());
		DrawChaseableRect (selectedChekcerInfo);
		#endif
	}

	public static int selectedIndex = -1;

	private static void MakeButton (int index, Vector3 point, float size)
	{
		if (Handles.Button (point, Quaternion.identity, size * EnemyViewRenderer.handleSize, EnemyViewRenderer.pickSize, Handles.DotHandleCap)) {
			selectedIndex = index;
		}
	}

	private static void CheckChange (int index, EnemyBodyChaseChecker chaseChecker, Vector3 point)
	{
		if (selectedIndex == index) {
			EditorGUI.BeginChangeCheck ();
			point = Handles.DoPositionHandle (point, Quaternion.identity);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (chaseChecker, "Change Radius");
				EditorUtility.SetDirty (chaseChecker);
				chaseChecker.disToChase = Vector3.Distance (chaseChecker.transform.position, point);
			}
		}
	}

	public static void DrawChaseableRect (EnemyBodyChaseChecker chaseChecker)
	{
		Handles.color = new Color (0.5f,0.5f, 0f, 0.05f);
		Handles.DrawSolidDisc (
			chaseChecker.actor.transform.position,
			Vector3.down,
			chaseChecker.disToChase);

		Handles.color = Color.white;

		var discPoint = chaseChecker.actor.transform.position + Vector3.right * chaseChecker.disToChase;
		var size = HandleUtility.GetHandleSize (discPoint);
		MakeButton (1, discPoint, size);
		CheckChange (1, chaseChecker, discPoint);

		discPoint = chaseChecker.actor.transform.position + Vector3.left * chaseChecker.disToChase;
		MakeButton (2, discPoint, size);
		CheckChange (2, chaseChecker, discPoint);

		discPoint = chaseChecker.actor.transform.position + Vector3.forward * chaseChecker.disToChase;
		MakeButton (3, discPoint, size);
		CheckChange (3, chaseChecker, discPoint);

		discPoint = chaseChecker.actor.transform.position + Vector3.back * chaseChecker.disToChase;
		MakeButton (4, discPoint, size);
		CheckChange (4, chaseChecker, discPoint);

	}
}
