using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor (typeof (NavMeshAgent))]
public class NavMeshAgentPathRenderer : Editor {
	private NavMeshAgent selectedAgent;

	private void OnSceneGUI ()
	{
		#if UNITY_EDITOR
		selectedAgent = target as NavMeshAgent;

		if (null == selectedAgent)
			return;

		Debug.Log (selectedAgent.pathEndPosition);

		Handles.color = Color.red;
		var path = selectedAgent.path;
		for (int i = 0; i < path.corners.Length; i++)
		{
			Handles.DrawSolidDisc (
				path.corners [i],
				Vector3.up,
				0.05f
			);
		}
		#endif
	}
}
