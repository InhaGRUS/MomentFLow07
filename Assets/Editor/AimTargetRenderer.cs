using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor (typeof (AimTarget))]
public class AimTargetRenderer : Editor {

	private AimTarget selectedAimTarget;

	private void OnSceneGUI ()
	{
		#if UNITY_EDITOR
		selectedAimTarget = target as AimTarget;

		if (null == selectedAimTarget)
			return;

		if (null == selectedAimTarget.actor)
			selectedAimTarget.actor = Actor.GetActor<Transform> (selectedAimTarget.transform);

		Handles.color = new Color (0.5f, 0.25f, 0.25f, 0.15f);
		Handles.DrawSolidArc (
			selectedAimTarget.actor.bodyCollider.bounds.center, 
			Vector3.up,
			selectedAimTarget.targetAimVector,
			45f,
			selectedAimTarget.actor.armLength
		);
		Handles.DrawSolidArc (
			selectedAimTarget.actor.bodyCollider.bounds.center, 
			Vector3.down,
			selectedAimTarget.targetAimVector,
			45f,
			selectedAimTarget.actor.armLength
		);


		Handles.color = new Color (0.25f, 0.25f, 0.5f, 0.15f);
		Handles.DrawSolidArc (
			selectedAimTarget.actor.bodyCollider.bounds.center, 
			Vector3.up,
			selectedAimTarget.nowAimVector,
			45f,
			selectedAimTarget.actor.armLength * 1.5f
		);
		Handles.DrawSolidArc (
			selectedAimTarget.actor.bodyCollider.bounds.center, 
			Vector3.down,
			selectedAimTarget.nowAimVector,
			45f,
			selectedAimTarget.actor.armLength * 1.5f
		);
		#endif
	}
}
