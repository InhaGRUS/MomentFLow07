using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : DynamicObject {
	public Actor owner;
	[HideInInspector]
	public Rigidbody rigid;

	[Header ("BulletInfo")]
	public int bulletIndex;
	public float damage;
	public float moveSpeed;
	public float maxSpeed;
	public float accel;
	public float maxFlingDistance;
	private float flingDistance;

	public Vector3 startPosition = Vector3.zero;
	[Header ("Other")]
	public ParticleSystem destroyParticle;
	private List<Collider> ignoreColliders = new List<Collider>();
	public LayerMask collisionMask;
	public Vector3 originVelocity;

	private CustomAudioResource flingSound;

	// Use this for initialization
	void Start () {
		rigid = GetComponent <Rigidbody> ();
		surfaceInfo = GetComponent<SurfaceInfo> ();
		flingSound = GetComponent<CustomAudioResource> ();
		flingSound.OnPlayEnd += ReturnFlingSound;
		if (null == destroyParticle)
			destroyParticle = GetComponentInChildren <ParticleSystem> ();
	}

	void PlayCollideSound (SurfaceInfo otherOne)
	{
		if (null != surfaceInfo)
			surfaceInfo.PlayCollideAudio (otherOne);
	}

	public void PlayFlingSound ()
	{
		if (null == flingSound) {
			flingSound = SoundManager.instance.BorrowAudioSource ();
			flingSound.OnPlayEnd += ReturnFlingSound;
		}
		if (!flingSound.IsPlaying) {
			flingSound.source.pitch = 1f;
			flingSound.source.volume = 1f;
			flingSound.Clip = SoundManager.instance.GetBulletFlyAudioClip ();
			flingSound.Play ();
		}
	}
	
	// Update is called once per frame
	protected new void Update () {
		base.Update ();
		flingDistance += rigid.velocity.magnitude * customDeltaTime;
		rigid.velocity = originVelocity * Mathf.Pow(customTimeScale, 12f);

		if (maxFlingDistance < flingDistance) {
			DestroyBullet ();
		}
	}

	public void ReturnFlingSound ()
	{
		flingSound.OnPlayEnd -= ReturnFlingSound;
	}

	public void OnCollisionEnter (Collision col)
	{
		var colActor = Actor.GetActor<Collider> (col.collider);
		//Debug.Log ("Col : " + col.collider.name);
		if (null != colActor && !colActor.stateInfo.isUnbeatable)
		{
			colActor.actorRigid.velocity = Vector3.zero;
			colActor.DamagedFrom (owner, damage, originVelocity.normalized);
		}
		PlayCollideSound (col.collider.GetComponentInChildren<SurfaceInfo> ());
		DestroyBullet ();
	}

	#region implemented abstract members of DynamicObject

	public override void SaveObject ()
	{
		throw new System.NotImplementedException ();
	}

	public override void LoadObject ()
	{
		throw new System.NotImplementedException ();
	}

	#endregion

	public void DestroyBullet()
	{
		flingDistance = 0;
		originVelocity = Vector3.zero;
		rigid.velocity = Vector3.zero;
		if (0 != ignoreColliders.Count) {
			Debug.Log ("ignore 발동");
			for (int i = 0; i< ignoreColliders.Count; i++)
			{
				Physics.IgnoreCollision (ignoreColliders[i], GetComponent<Collider> (), false);
			}
			ignoreColliders.Clear ();
		}
		customTimeScale = 1;
		previousTimeScaleList.Clear ();

		GetComponent<ParticleSystem> ().Stop ();
		GetComponent<ParticleSystem> ().Clear ();
		GetComponent<TrailRenderer> ().Clear ();
		destroyParticle.transform.parent = transform.parent;
		destroyParticle.transform.position = transform.position;
		destroyParticle.Play ();
		BulletPool.Instance.ReturnBullet (gameObject);
	}
}
