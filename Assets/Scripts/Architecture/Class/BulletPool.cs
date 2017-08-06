using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour {

	private static BulletPool instance;

	public static BulletPool Instance {
		get {
			if (instance == null) {
				var newBulletPool = new GameObject ("BulletPool");
				instance = newBulletPool.AddComponent <BulletPool> ();
			}
			return instance;
		}
	}

	public Transform bulletPoolParent;
	public List<Transform> bulletPools;

	public void Start ()
	{
		instance = this;
		InitBullets ();
	}

	public void InitBullets()
	{
		var rifles = FindRifleWeapons ();

		for (int i = 0; i < rifles.Count; i++) {
			var rifle = rifles [i];
			GameObject pool = GameObject.Find (rifle.usingBullet.name + "Pool");

			if (null != pool)
			{
				AddAmmoToPool (rifle, pool.transform);
				return;
			}
			// Can't Find Bullet Pool
			GameObject newPool = new GameObject (rifle.usingBullet.name + "Pool");
			newPool.transform.SetParent (bulletPoolParent);
			newPool.transform.position = Vector3.zero;
			bulletPools.Add (newPool.transform);

			AddAmmoToPool (rifle, newPool.transform);
		}
	}

	public void AddAmmoToPool (GunInfo rifle, Transform pool)
	{
		for (int i = 0; i < rifle.maxAmmo; i++)
		{
			var newBullet = Instantiate (rifle.usingBullet, pool) as GameObject;
			newBullet.transform.position = Vector3.zero;
			newBullet.SetActive (false);
		}
	}

	public void AddAmmoToPool (GameObject usingBullet, int addedAmmmo, Transform pool)
	{
		for (int i = 0; i < addedAmmmo; i++)
		{
			var newBullet = Instantiate (usingBullet, pool) as GameObject;
			newBullet.transform.position = Vector3.zero;
			newBullet.SetActive (false);
		}
	}

	public Bullet BorrowBullet (GameObject usingBullet, Actor owner)
	{
		for (int i = 0; i < bulletPools.Count; i++)
		{
			if (bulletPools[i].name.Contains (usingBullet.name))
			{
				var pool = bulletPools [i];
				var index = 0;
				var borrowBullet = new Bullet ();

				while (index < pool.childCount)
				{
					if (pool.GetChild(index).GetComponent<Collider>().enabled)
					{
						index++;
						continue;
					}
					borrowBullet = pool.GetChild (index).GetComponent<Bullet> ();
					borrowBullet.bulletIndex = index;
					borrowBullet.owner = owner;
					Debug.Log ("Bullet Owner : " + owner.name);
					borrowBullet.GetComponent<Collider> ().enabled = true;
					borrowBullet.gameObject.SetActive (true);
					return borrowBullet;
				}
				AddAmmoToPool (usingBullet, 1, pool);
				borrowBullet = pool.GetChild (index).GetComponent<Bullet> ();
				borrowBullet.bulletIndex = index;
				borrowBullet.owner = owner;
				Physics.IgnoreCollision (owner.bodyCollider, borrowBullet.GetComponent<Collider> (), true);
				Debug.Log ("Bullet Owner : " + owner.name);
				borrowBullet.gameObject.SetActive (true);
				borrowBullet.GetComponent<Collider> ().enabled = true;
				return borrowBullet;
			}
		}
		return null;
	}

	public void ReturnBullet (GameObject usedBullet)
	{
		//usedBullet.GetComponent<TrailRenderer> ().Clear ();
		usedBullet.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		usedBullet.GetComponent<Collider>().enabled = false;
	}


	private List<GunInfo> FindRifleWeapons()
	{
		var equiptments = GameObject.FindObjectsOfType<EquipmentInfo> ();
		List<GunInfo> rifles = new List<GunInfo> ();

		foreach (EquipmentInfo equiptment in equiptments) 
		{
			foreach (GunInfo gun in equiptment.nowHaveGunInfo)
			{
				rifles.Add (gun);
			}
		}
		return rifles;
	}
}
