using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor (typeof(EnemyBodyPatrolChecker))]
public class EnemyPatrolStateRenderer : Editor {

	private EnemyBodyPatrolChecker selectedCheckerInfo;

	private void OnSceneGUI ()
	{
		#if UNITY_EDITOR
		selectedCheckerInfo = target as EnemyBodyPatrolChecker;
		if (null == selectedCheckerInfo)
			return;
		if (null == selectedCheckerInfo.eActor)
		{
			selectedCheckerInfo.actor = selectedCheckerInfo.GetComponentInParent<Actor> ();
			selectedCheckerInfo.eActor = selectedCheckerInfo.GetComponentInParent<EnemyActor>();
		}

		EnemyViewRenderer.DrawViewableRect (selectedCheckerInfo.eActor.GetEnemyOutsideInfo ());
		EnemyViewRenderer.DrawRecognizableRect (selectedCheckerInfo.eActor.GetEnemyOutsideInfo ());
		DrawPatrolNodes (selectedCheckerInfo);
		#endif
	}

	public static int selectedIndex = -1;

	private static void MakeButton (int index, Vector3 point, float size)
	{
		Handles.Label (point, index.ToString ());
		if (Handles.Button (point, Quaternion.identity, size * EnemyViewRenderer.handleSize, EnemyViewRenderer.pickSize, Handles.DotHandleCap)) {
			selectedIndex = index;
		}
	}

	private static void CheckChange (int index, EnemyBodyPatrolChecker patrolChecker, Vector3 point)
	{
		if (selectedIndex == index) {
			EditorGUI.BeginChangeCheck ();
			point = Handles.DoPositionHandle (point, Quaternion.identity);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (patrolChecker, "Change Radius");
				EditorUtility.SetDirty (patrolChecker);
				patrolChecker.patrolNodeList[index].nodePoint = point;
			}
		}
	}

	public static void DrawPatrolNodes (EnemyBodyPatrolChecker patrolChecker)
	{
		Handles.color = Color.blue;

		for (int i = 0; i < patrolChecker.patrolNodeList.Count; i++)
		{
			var element = patrolChecker.patrolNodeList [i];
			var size = HandleUtility.GetHandleSize (patrolChecker.patrolNodeList [i].nodePoint);
			MakeButton (element.nodeId, patrolChecker.patrolNodeList [i].nodePoint, size);
			CheckChange (element.nodeId, patrolChecker, patrolChecker.patrolNodeList [i].nodePoint);

			if (null != element.fromNode) {
				if (element == patrolChecker.nowNode) {
					Handles.DrawBezier (element.fromNode.nodePoint, element.nodePoint, Vector3.zero, Vector3.one, Color.green, null, 1.5f);
				} else {
					Handles.DrawBezier (element.fromNode.nodePoint, element.nodePoint, Vector3.zero, Vector3.one, Color.blue, null, 1.2f);
				}
			}
			else {
				Handles.DrawBezier (patrolChecker.actor.transform.position, element.nodePoint, Vector3.zero, Vector3.one, Color.red, null, 1.2f);
			}
		}
	}
}
