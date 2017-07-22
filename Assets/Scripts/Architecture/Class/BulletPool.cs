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

	public void Awake ()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
