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

    public float maxAngle;
    public float minAngle;

    public float defaultAngle = 45f;

    void Start()
    {
		actor = Actor.GetActor <AimTarget> (this);
		animator = actor.shoulderAnimator;
        shoulderSpriteRenderer = shoulder.GetComponent<SpriteRenderer>();
    }

    public void AimToObject(Vector3 targetPos)
    {
        float dis_x;
        float dis_y;
        float degree;

        if (targetPos.x > transform.position.x)
        {
            dis_x = (shoulder.transform.position.x - targetPos.x);
            dis_y = targetPos.y - shoulder.transform.position.y;
            degree = Mathf.Atan2(dis_x, -dis_y) * Mathf.Rad2Deg;
			actor.SetLookDirection (false);
        }
        else
        {
            dis_x = -(shoulder.transform.position.x - targetPos.x);
            dis_y = targetPos.y - shoulder.transform.position.y;
            degree = Mathf.Atan2(dis_x, -dis_y) * Mathf.Rad2Deg;
			actor.SetLookDirection (true);
        }
        degree = Mathf.Clamp(degree, -maxAngle, -minAngle);

        animator.SetFloat("AimAngleRatio", Mathf.Abs(degree / 180));

        shoulder.transform.localRotation = Quaternion.Euler(0, 0, degree + defaultAngle);
    }

	public void AimToForward()
	{
		if (transform.CompareTag ("Player")) {
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

		animator.SetFloat("AimAngleRatio", 0.5f);

		shoulder.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	public void AimToForward(bool isReverse)
    {
		if (transform.CompareTag ("Player")) {
			var inputX = Input.GetAxis("Horizontal");
			if (inputX < 0)
			{
				if (isReverse)
					actor.SetLookDirection (true);
				else
					actor.SetLookDirection (false);
			}
			else if (inputX > 0)
			{
				if (isReverse)
					actor.SetLookDirection (false);
				else
					actor.SetLookDirection (true);
			}
		}

        animator.SetFloat("AimAngleRatio", 0.5f);

        shoulder.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
}