using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(EnemyBodyShootWithCrouchChecker))]
public class EnemyShootWithCrouchStateRenderer : Editor {

	private EnemyBodyShootWithCrouchChecker selectedCheckerInfo;

	private void OnSceneGUI ()
	{
		#if UNITY_EDITOR
		selectedCheckerInfo = target as EnemyBodyShootWithCrouchChecker;

		if (null == selectedCheckerInfo)
			return;
		if (null == selectedCheckerInfo.actor)
		{
			selectedCheckerInfo.actor = selectedCheckerInfo.GetComponentInParent<Actor>();
			selectedCheckerInfo.eActor = EnemyActor.GetEnemyActor<Actor> (selectedCheckerInfo.actor);
		}

		EnemyViewRenderer.DrawRecognizableRect (selectedCheckerInfo.eActor.GetEnemyOutsideInfo ());
		EnemyViewRenderer.DrawViewableRect (selectedCheckerInfo.eActor.GetEnemyOutsideInfo ());
		DrawShootableRect (selectedCheckerInfo);
		#endif
	}

	public static int selectedIndex = -1;

	private static void MakeButton (int index, Vector3 point, float size)
	{
		if (Handles.Button (point, Quaternion.identity, size * EnemyViewRenderer.handleSize, EnemyViewRenderer.pickSize, Handles.DotHandleCap)) {
			selectedIndex = index;
		}
	}

	private static void CheckChange (int index, EnemyBodyShootWithCrouchChecker shootChecker, Vector3 point)
	{
		if (selectedIndex == index) {
			EditorGUI.BeginChangeCheck ();
			point = Handles.DoPositionHandle (point, Quaternion.identity);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (shootChecker, "Change Radius");
				EditorUtility.SetDirty (shootChecker);
				shootChecker.disToShoot = Vector3.Distance (shootChecker.transform.position, point);
			}
		}
	}

	public static void DrawShootableRect (EnemyBodyShootWithCrouchChecker shootChecker)
	{
		Handles.color = new Color (0f, 0.5f, 0.5f, 0.05f);
		Handles.DrawSolidDisc (
			shootChecker.actor.transform.position,
			Vector3.down,
			shootChecker.disToShoot
		);

		Handles.color = Color.white;

		var discPoint = shootChecker.actor.transform.position + Vector3.right * shootChecker.disToShoot;
		var size = HandleUtility.GetHandleSize (discPoint);
		MakeButton (1, discPoint, size);
		CheckChange (1, shootChecker, discPoint);

		discPoint = shootChecker.actor.transform.position + Vector3.left * shootChecker.disToShoot;
		size = HandleUtility.GetHandleSize (discPoint);
		MakeButton (2, discPoint, size);
		CheckChange (2, shootChecker, discPoint);

		discPoint = shootChecker.actor.transform.position + Vector3.forward * shootChecker.disToShoot;
		size = HandleUtility.GetHandleSize (discPoint);
		MakeButton (3, discPoint, size);
		CheckChange (3, shootChecker, discPoint);

		discPoint = shootChecker.actor.transform.position + Vector3.back * shootChecker.disToShoot;
		size = HandleUtility.GetHandleSize (discPoint);
		MakeButton (4, discPoint, size);
		CheckChange (4, shootChecker, discPoint);
	}
}
