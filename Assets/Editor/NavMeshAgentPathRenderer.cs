using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor (typeof (CustomNavMeshAgent))]
public class NavMeshAgentPathRenderer : Editor {
	private CustomNavMeshAgent selectedAgent;

	private void OnSceneGUI ()
	{
		#if UNITY_EDITOR
		selectedAgent = target as CustomNavMeshAgent;

		if (null == selectedAgent)
			return;

		for (int i = 0; i < selectedAgent.destCornerPointList.Count; i++)
		{
			Handles.color = Color.red;
			Handles.DrawSolidDisc (
				selectedAgent.destCornerPointList [i],
				Vector3.up,
				0.05f
			);
		}
		#endif
	}
}
