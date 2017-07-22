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

	public float maxAngle = 90f;
	public float minAngle = 25f;

	public float defaultAngle = 45f;

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
		Debug.Log ("Aim");
		var mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		switch (aimState) {
		case AimState.Normal:
			List<Vector3> points = new List<Vector3> ();
			points.Add (aimStartPoint.position);
			if (Physics.Raycast (mouseRay, out hit, maxGunAimDistance)) {
				var tmpPoint = hit.point;
				if (Physics.Raycast (aimStartPoint.position, (hit.point - aimStartPoint.position).normalized, out hit, maxGunAimDistance)) {
					points.Add (hit.point);
					AimToObject (hit.point);
				}
			}
			else {
				var point = aimStartPoint.position + mouseRay.direction * maxGunAimDistance;
				points.Add (point);
				AimToObject (hit.point);
			}
			lineRenderer.positionCount = points.Count;
			lineRenderer.SetPositions (points.ToArray ());
			if (timer < aimColorFadeInDuration) {
				timer += actor.customDeltaTime;
				lineRenderer.material.color = new Color (aimColor.r, aimColor.g, aimColor.b, aimColor.a * timer / aimColorFadeInDuration);
				lineRenderer.startWidth = 0.02f * timer / aimColorFadeInDuration;
				lineRenderer.endWidth = lineRenderer.startWidth;
			}
			if (Mathf.Sign (mouseRay.direction.x) != -Mathf.Sign (actor.transform.localScale.x)) {
				if (Mathf.Sign (mouseRay.direction.x) < 0)
					actor.SetLookDirection (true, 1);
				else
					actor.SetLookDirection (false, 1);
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
		actor.ResetSetLookDirectionPriority ();
		StopCoroutine ("EraseAimLine");
		StartCoroutine ("EraseAimLine");
		nowActivated = false;
	}
	#endregion

	IEnumerator EraseAimLine ()
	{
		while (timer < aimColorFadeInDuration) {
			timer += actor.customDeltaTime;
			lineRenderer.material.color = new Color (aimColor.r, aimColor.g, aimColor.b, lineRenderer.material.color.a * (1 - timer / aimColorFadeOutDurtaion));
			lineRenderer.startWidth = 0.02f * (1 - timer / aimColorFadeOutDurtaion);
			lineRenderer.endWidth = lineRenderer.startWidth;
			yield return new WaitForEndOfFrame ();
		}
		lineRenderer.material.color = Color.clear;
		lineRenderer.startWidth = 0;
		lineRenderer.endWidth = 0;
		timer = 0;
	}

	public void AimToObject (Vector3 targetPos)
	{
		float dis_x;
		float dis_y;
		float degree;

		if (targetPos.x > actor.shoulderAnimator.transform.position.x)
		{
			dis_x = (actor.shoulderAnimator.transform.position.x - targetPos.x);
			dis_y = targetPos.y - actor.shoulderAnimator.transform.position.y;
			degree = Mathf.Atan2(dis_x, -dis_y) * Mathf.Rad2Deg;
			actor.SetLookDirection (false);
		}
		else
		{
			dis_x = -(actor.shoulderAnimator.transform.position.x - targetPos.x);
			dis_y = targetPos.y - actor.shoulderAnimator.transform.position.y;
			degree = Mathf.Atan2(dis_x, -dis_y) * Mathf.Rad2Deg;
			actor.SetLookDirection (true);
		}
		degree = Mathf.Clamp(degree, -maxAngle, -minAngle);

		actor.shoulderAnimator.SetFloat("AimAngleRatio", Mathf.Abs(degree / 180));

		actor.shoulderAnimator.transform.localRotation = Quaternion.Euler(0, 0, degree + defaultAngle);
	}

	public void AimToForward()
	{
		if (actor.shoulderAnimator.CompareTag ("Player")) {
			var inputX = Input.GetAxis("Horizontal");
			if (inputX < 0)
			{
				actor.SetLookDirection (false);
			}
			else if (inputX > 0)
			{
				actor.SetLookDirection (true);
			}
		}

		actor.shoulderAnimator.SetFloat("AimAngleRatio", 0.5f);

		actor.shoulderAnimator.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}
}
