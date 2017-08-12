using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderAnimationActions : MonoBehaviour {
	public Actor actor;
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
		bullet.startPosition = bullet.transform.position;
		bullet.transform.rotation = Quaternion.LookRotation (actor.aimTarget.nowShootVector);
		bullet.originVelocity = actor.aimTarget.nowShootVector * bullet.maxSpeed;
		bullet.GetComponent<ParticleSystem> ().Play (false);
	}

	public void Reload ()
	{
		
	}
}
