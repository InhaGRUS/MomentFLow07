using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoulderAimChecker : ShoulderAnimationCheckerBase {
	public Transform aimStartPoint;
	public AimState aimState = AimState.Normal;

	public LayerMask aimableLayerMask;

	public float maxGunAimDistance;

	public LineRenderer lineRenderer;
	public LineRenderer playerAdditionalLineRenderer;

	public float aimColorFadeInDuration = 0.5f;
	public float aimColorFadeOutDurtaion = 0.5f;

	public float timer = 0;

	public Material aimMat;
	public Material additionalAimMat;

	public Color aimColor;
	public Color additionalAimColor;

	public void Update ()
	{
		lineRenderer.SetPosition (0, aimStartPoint.position);
		playerAdditionalLineRenderer.SetPosition (0, aimStartPoint.position);
	}

	// Use this for initialization
	protected new void Start () {
		base.Start ();
		lineRenderer = GetComponent<LineRenderer> ();
		aimMat = new Material (lineRenderer.material);
		additionalAimMat = new Material (playerAdditionalLineRenderer.material);
		lineRenderer.material = aimMat;
		playerAdditionalLineRenderer.material = additionalAimMat;
	}

	#region implemented abstract members of ActionBase
	protected override bool CanTransition ()
	{
		return true;
	}
	protected override bool IsSatisfiedToAction ()
	{
		if (actor.useShoulder && Input.GetMouseButton (1) && !actor.GetSpecificAction<PlayerBodyRunChecker>().nowActivated)
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
		SetAnimationTrigger ();
		PlayerAimRender ();

		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		timer = 0;
		actor.ResetSetLookDirectionPriority ();
		StopCoroutine ("EraseAimLine");
		StartCoroutine ("EraseAimLine");
		actor.aimTarget.AimToForward ();
		nowActivated = false;
	}
	#endregion

	public void PlayerAimRender ()
	{
		var mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		switch (aimState) {
		case AimState.Normal:
			List<Vector3> points = new List<Vector3> ();
			points.Add (aimStartPoint.position);

			List<Vector3> playerAimPoints = new List<Vector3> ();
			playerAimPoints.Add (aimStartPoint.position);

			// MouseRay 
			if (Physics.Raycast (mouseRay, out hit, maxGunAimDistance, aimableLayerMask)) {
				var tmpPoint = hit.point;
				if (Physics.Raycast (aimStartPoint.position, (hit.point - aimStartPoint.position).normalized, out hit, maxGunAimDistance, aimableLayerMask)) {
					points.Add (hit.point);
					actor.aimTarget.AimToObject (hit.point);
				}
			} else {
				var point = aimStartPoint.position + mouseRay.direction * maxGunAimDistance;
				points.Add (point);
				actor.aimTarget.AimToObject (hit.point);
			}

			lineRenderer.positionCount = points.Count;
			lineRenderer.SetPositions (points.ToArray ());

			// PlayerAimRay
			var dir = actor.aimTarget.nowAimVector;

			if (Physics.Raycast (aimStartPoint.position, dir, out hit, maxGunAimDistance, aimableLayerMask)) {
				playerAimPoints.Add (hit.point);
			}
			else {
				playerAimPoints.Add (aimStartPoint.position + dir * maxGunAimDistance);
			}

			playerAdditionalLineRenderer.positionCount = playerAimPoints.Count;
			playerAdditionalLineRenderer.SetPositions (playerAimPoints.ToArray ());

			if (timer < aimColorFadeInDuration) {
				timer += actor.customDeltaTime;
			
				lineRenderer.material.color = new Color (aimColor.r, aimColor.g, aimColor.b, aimColor.a * timer / aimColorFadeInDuration);
				lineRenderer.startWidth = 0.02f * timer / aimColorFadeInDuration;
				lineRenderer.endWidth = lineRenderer.startWidth;

				playerAdditionalLineRenderer.material.color = new Color (additionalAimColor.r, additionalAimColor.g, additionalAimColor.b, additionalAimColor.a * timer / aimColorFadeInDuration);
				playerAdditionalLineRenderer.startWidth = 0.02f * timer / aimColorFadeInDuration;
				playerAdditionalLineRenderer.endWidth = lineRenderer.startWidth;
			}

			break;
		case AimState.Bounce:
			break;
		}
	}

	public IEnumerator EraseAimLine ()
	{
		while (timer < aimColorFadeInDuration) {
			timer += actor.customDeltaTime;
			lineRenderer.material.color = new Color (aimColor.r, aimColor.g, aimColor.b, lineRenderer.material.color.a * (1 - timer / aimColorFadeOutDurtaion));
			lineRenderer.startWidth = 0.02f * (1 - timer / aimColorFadeOutDurtaion);
			lineRenderer.endWidth = lineRenderer.startWidth;

			playerAdditionalLineRenderer.material.color = new Color (additionalAimColor.r, additionalAimColor.g, additionalAimColor.b, lineRenderer.material.color.a * (1 - timer / aimColorFadeOutDurtaion));
			playerAdditionalLineRenderer.startWidth = 0.02f * (1 - timer / aimColorFadeOutDurtaion);
			playerAdditionalLineRenderer.endWidth = lineRenderer.startWidth;
			yield return new WaitForEndOfFrame ();
		}
		lineRenderer.material.color = Color.clear;
		lineRenderer.startWidth = 0;
		lineRenderer.endWidth = 0;

		playerAdditionalLineRenderer.material.color = Color.clear;
		playerAdditionalLineRenderer.startWidth = 0;
		playerAdditionalLineRenderer.endWidth = 0;
		timer = 0;
	}
}
