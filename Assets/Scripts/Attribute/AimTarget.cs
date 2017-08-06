using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AimTarget : MonoBehaviour
{
	public Actor actor;
    private SpriteRenderer shoulderSpriteRenderer;

    public Transform shoulder;
	public Transform shootPoint;

	public Vector3 targetAimVector;
	public Vector3 nowAimVector;
	public Vector3 nowShootVector;

    public float maxAngle;
    public float minAngle;

    public float defaultAngle = 45f;

    void Start()
    {
		actor = Actor.GetActor <AimTarget> (this);
        shoulderSpriteRenderer = shoulder.GetComponent<SpriteRenderer>();
    }

	public void AimToObject (Vector3 targetPos)
	{
		//float disY_x;
		//float disY_y;
		//float degreeY;

		float disZ_x;
		float disZ_z;
		float degreeZ;

		float nowAimDegreeZ;

		targetAimVector = (targetPos - shoulder.transform.position).normalized;
		nowAimVector = Vector3.MoveTowards (nowAimVector, targetAimVector, actor.customDeltaTime * 3f);

		if (nowAimVector.x > 0) {
			//disY_x = (actor.shoulderAnimator.transform.position.x - nowAimVector.x);
			//disY_y = nowAimVector.y - actor.shoulderAnimator.transform.position.y;
			//degreeY = Mathf.Atan2(disY_x, -disY_y) * Mathf.Rad2Deg;

			disZ_x = (-nowAimVector.x);
			disZ_z = nowAimVector.z;
			degreeZ = Mathf.Atan2 (disZ_x, -disZ_z) * Mathf.Rad2Deg;

			actor.SetLookDirection (false, 1);
		} else{
			//disY_x = -(actor.shoulderAnimator.transform.position.x - nowAimVector.x);
			//disY_y = nowAimVector.y - actor.shoulderAnimator.transform.position.y;
			//degreeY = Mathf.Atan2(disY_x, -disY_y) * Mathf.Rad2Deg;

			disZ_x = (-nowAimVector.x);
			disZ_z = nowAimVector.z;
			degreeZ = Mathf.Atan2 (disZ_x, -disZ_z) * Mathf.Rad2Deg;

			actor.SetLookDirection (true, 1);
		}
		//degreeY = Mathf.Clamp(degreeY, -maxAngle, -minAngle);
		degreeZ = degreeZ + 360f;

		if (degreeZ >= 360f)
			degreeZ -= 360f;

		//var degreeYRatio = Mathf.Abs (degreeY / 180);
		var degreeZRatio = degreeZ / 360f;

		//actor.shoulderAnimator.SetFloat ("AimYAngleRatio", degreeYRatio);
		actor.bodyAnimator.SetFloat ("AimZAngleRatio", degreeZRatio);
		actor.shoulderAnimator.SetFloat ("AimZAngleRatio", actor.bodyAnimator.GetFloat ("AimZAngleRatio"));

		if ((degreeZRatio >= 0.125f && degreeZRatio <= 0.375f) || (degreeZRatio >= 0.625 && degreeZRatio <= 0.875f)) {
			shootPoint.localPosition = new Vector3 (-actor.armLength, 0f, 0f);
			//actor.shoulderAnimator.transform.localRotation = Quaternion.Euler (0, 0, degreeY + defaultAngle);
		} else if (nowAimVector.z > 0) {
			//actor.shoulderAnimator.transform.localRotation = Quaternion.Euler (0, 0, 0);
			shootPoint.localPosition = new Vector3 (0f, 0f, actor.armLength);
		} else if (nowAimVector.z < 0) {
			shootPoint.localPosition = new Vector3 (0f, 0f, -actor.armLength);
		}
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