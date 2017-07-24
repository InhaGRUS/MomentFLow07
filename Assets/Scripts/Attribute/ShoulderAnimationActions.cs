using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderAnimationActions : MonoBehaviour {
	private Actor actor;
	private Transform shoulder;
	private Animator animator;

	public void Start ()
	{
		if (null == actor)
			actor = Actor.GetActor <Transform> (transform);
		if (null == shoulder)
			shoulder = transform;
		if (null == animator)
			animator = GetComponent<Animator> ();
	}

	public void Shoot ()
	{
		var bullet = BulletPool.Instance.BorrowBullet (((GunInfo)actor.equipmentInfo.nowEquipWeaponInfo).usingBullet, actor);
		bullet.transform.position = actor.aimTarget.shootPoint.position;
		bullet.transform.localRotation = Quaternion.LookRotation (actor.aimTarget.nowAimVector);
		bullet.originVelocity = actor.aimTarget.nowAimVector * bullet.maxSpeed;
		bullet.GetComponent<ParticleSystem> ().Play (false);
	}

	public void Reload ()
	{
		
	}
}
