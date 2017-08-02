using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(EnemyBodyHideChecker))]
public class EnemyHideStateRenderer : Editor {

	private EnemyBodyHideChecker selectedChekcerInfo;

	public static float handleSize = 0.5f;
	public static float pickSize = 0.5f;

	private void OnSceneGUI ()
	{
		#if UNITY_EDITOR

		selectedChekcerInfo = target as EnemyBodyHideChecker;

		if (null == selectedChekcerInfo)
			return;

		selectedChekcerInfo.actor = selectedChekcerInfo.GetComponentInParent<Actor> ();
		selectedChekcerInfo.eActor = selectedChekcerInfo.GetComponentInParent<EnemyActor>();
		selectedChekcerInfo.eActor.agent = selectedChekcerInfo.eActor.GetComponent<NavMeshAgent> ();

		EnemyViewRenderer.DrawViewableRect (selectedChekcerInfo.eActor.GetEnemyOutsideInfo());
		EnemyViewRenderer.DrawRecognizableRect (selectedChekcerInfo.eActor.GetEnemyOutsideInfo());

		float size = HandleUtility.GetHandleSize (selectedChekcerInfo.eActor.agent.destination);

		Handles.color = Color.red;
		Handles.Button (selectedChekcerInfo.eActor.agent.destination, Quaternion.Euler(Vector3.back), size * handleSize, pickSize, Handles.ArrowHandleCap);

		#endif
	}
}
