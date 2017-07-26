using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AimTarget : MonoBehaviour
{
	public Actor actor;
    private Animator animator;
    private SpriteRenderer shoulderSpriteRenderer;

    public Transform shoulder;
	public Transform shootPoint;

	public Vector3 targetAimVector;
	public Vector3 nowAimVector;

    public float maxAngle;
    public float minAngle;

    public float defaultAngle = 45f;

    void Start()
    {
		actor = Actor.GetActor <AimTarget> (this);
		animator = actor.shoulderAnimator;
        shoulderSpriteRenderer = shoulder.GetComponent<SpriteRenderer>();
    }

	public void AimToObject (Vector3 targetPos)
	{
		float disY_x;
		float disY_y;
		float degreeY;

		float disZ_x;
		float disZ_z;
		float degreeZ;

		if (targetPos.x > actor.shoulderAnimator.transform.position.x)
		{
			disY_x = (actor.shoulderAnimator.transform.position.x - targetPos.x);
			disY_y = targetPos.y - actor.shoulderAnimator.transform.position.y;
			degreeY = Mathf.Atan2(disY_x, -disY_y) * Mathf.Rad2Deg;

			disZ_x = (actor.shoulderAnimator.transform.position.x - targetPos.x);
			disZ_z = targetPos.z - actor.shoulderAnimator.transform.position.z;
			degreeZ = Mathf.Atan2(disZ_x, -disZ_z) * Mathf.Rad2Deg;

			actor.SetLookDirection (false, 1);
		}
		else
		{
			disY_x = -(actor.shoulderAnimator.transform.position.x - targetPos.x);
			disY_y = targetPos.y - actor.shoulderAnimator.transform.position.y;
			degreeY = Mathf.Atan2(disY_x, -disY_y) * Mathf.Rad2Deg;

			disZ_x = (actor.shoulderAnimator.transform.position.x - targetPos.x);
			disZ_z = targetPos.z - actor.shoulderAnimator.transform.position.z;
			degreeZ = Mathf.Atan2(disZ_x, -disZ_z) * Mathf.Rad2Deg;

			actor.SetLookDirection (true, 1);
		}
		degreeY = Mathf.Clamp(degreeY, -maxAngle, -minAngle);
		degreeZ = degreeZ + 360f;

		if (degreeZ >= 360f)
			degreeZ -= 360f;

		targetAimVector = (targetPos - shootPoint.position).normalized;

		nowAimVector = Vector3.Lerp (nowAimVector, targetAimVector, actor.customDeltaTime * 3f);

		actor.shoulderAnimator.SetFloat ("AimYAngleRatio", Mathf.Abs(degreeY / 180));
		actor.bodyAnimator.SetFloat ("AimZAngleRatio", degreeZ / 360f);
		actor.shoulderAnimator.transform.localRotation = Quaternion.Euler(0, 0, degreeY + defaultAngle);

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

		actor.shoulderAnimator.SetFloat("AimYAngleRatio", 0.5f);

		actor.shoulderAnimator.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}
}