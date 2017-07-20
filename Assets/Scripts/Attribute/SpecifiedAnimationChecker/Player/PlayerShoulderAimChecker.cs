using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoulderAimChecker : ShoulderAnimationCheckerBase {
	public Transform aimStartPoint;
	public AimState aimState = AimState.Normal;
	public float maxGunAimDistance;

	public LineRenderer lineRenderer;

	public float aimColorFadeInDuration = 0.5f;
	public float aimColorFadeOutDurtaion = 0.5f;

	public float timer = 0;

	public Material aimMat;

	public Color aimColor;

	public void Update ()
	{
		lineRenderer.SetPosition (0, aimStartPoint.position);
	}

	// Use this for initialization
	protected new void Start () {
		base.Start ();
		lineRenderer = GetComponent<LineRenderer> ();
		aimMat = new Material (lineRenderer.material);
		lineRenderer.material = aimMat;
	}

	#region implemented abstract members of ActionBase
	protected override bool CanTransition ()
	{
		return true;
	}
	protected override bool IsSatisfiedToAction ()
	{
		if (actor.useShoulder && Input.GetMouseButton (1))
		{
			return true;
		}
		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		nowActivated = false;
	}
	public override void DoSpecifiedAction ()
	{

		var mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		switch (aimState)
		{
		case AimState.Normal:
			List<Vector3> points = new List<Vector3> ();
			points.Add (aimStartPoint.position);

			if (Physics.Raycast (mouseRay, out hit, maxGunAimDistance)) {
				var tmpPoint = hit.point;
				if (Physics.Raycast (aimStartPoint.position, (hit.point - aimStartPoint.position).normalized, out hit, maxGunAimDistance)) {
					points.Add (hit.point);
				}
			} else {
				var point = aimStartPoint.position + mouseRay.direction * maxGunAimDistance;
				points.Add (point);
			}
				
			lineRenderer.positionCount = points.Count;
			lineRenderer.SetPositions (points.ToArray ());

			if (timer < aimColorFadeInDuration) {
				timer += actor.customDeltaTime;
				lineRenderer.material.color = new Color (aimColor.r, aimColor.g, aimColor.b, aimColor.a * timer / aimColorFadeInDuration);
				lineRenderer.startWidth = 0.02f * timer / aimColorFadeInDuration;
				lineRenderer.endWidth = lineRenderer.startWidth;
			}

			if (Mathf.Sign (mouseRay.direction.x) != -Mathf.Sign (actor.transform.localScale.x))
			{
				if (Mathf.Sign (mouseRay.direction.x) < 0)
					actor.SetLookDirection (true);
				else
					actor.SetLookDirection (false);
			}
			break;
		case AimState.Bounce:

			break;
		}


		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		timer = 0;
		StopCoroutine ("EraseAimLine");
		StartCoroutine ("EraseAimLine");
		nowActivated = false;
	}
	#endregion

	IEnumerator EraseAimLine ()
	{
		while (timer < aimColorFadeInDuration) {
			Debug.Log ("Erase");
			timer += actor.customDeltaTime;
			lineRenderer.material.color = new Color (aimColor.r, aimColor.g, aimColor.b, aimColor.a * (1 - timer / aimColorFadeOutDurtaion));
			lineRenderer.startWidth = 0.02f * (1 - timer / aimColorFadeOutDurtaion);
			lineRenderer.endWidth = lineRenderer.startWidth;
			yield return new WaitForEndOfFrame ();
		}
		lineRenderer.material.color = Color.clear;
		timer = 0;
	}
}
