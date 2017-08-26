using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : DynamicObject {
	[Header ("ActorInfo")]
	public RoomInfo roomInfo;
	public HumanInfo humanInfo = new HumanInfo();

	public OutsideInfo outsideInfo;
	[HideInInspector]
	public EquipmentInfo equipmentInfo;
	public SpecificActorStateInfo stateInfo;

	public Rigidbody actorRigid;
	public Collider bodyCollider;

	public Vector3 damagedDirection;

	public bool useShoulder = false;

	[Header ("Body")]
	public Animator bodyAnimator;
	public float listenableDistance = 2f;

	[Header ("Shoulder")]
	public Animator shoulderAnimator;
	public AimTarget aimTarget;
	public float armLength;

	private SpriteRenderer shoulderSpriteRenderer;

	[Header ("Crouch Setting")]
	public float normalHeight;
	public float normalCenterY;
	public float crouchHeight;
	public float crouchCenterY;

	[Header ("TensionGauge")]
	[Range (0,1)]
	public float tensionGauge = 0f;
	public float tensionIncreaseAmount = 1f;
	public float tensionDecreaseAmount = 1f;

	[Header ("Action")]
	public AnimationCheckerBase[] actions;
	public AnimationCheckerBase nowBodyAction;
	public AnimationCheckerBase nowShoulderAction;

	public string nowBodyAnimationName;
	public string nowShoulderAnimationName;


	public Vector3 actorVelocity;

	public virtual void OnEnable ()
	{
		outsideInfo = GetComponentInChildren <OutsideInfo> ();
		equipmentInfo = GetComponentInChildren <EquipmentInfo> ();
	}

	// Use this for initialization
	protected void Start () {
		actorRigid = GetComponent<Rigidbody> ();

		if (useShoulder) {
			shoulderSpriteRenderer = shoulderAnimator.GetComponent<SpriteRenderer> ();
			aimTarget = GetComponent<AimTarget> ();
			armLength = Vector3.Distance (aimTarget.shootPoint.position, shoulderAnimator.transform.position);
		}

		actions = GetComponentsInChildren<AnimationCheckerBase> ();
	}
	
	// Update is called once per frame
	protected new void Update () {
		base.Update ();
		if (null != nowBodyAction)
			nowBodyAction.TryAction ();

		if (null != nowShoulderAction)
			nowShoulderAction.TryAction ();
	}

	private void FixedUpdate ()
	{
		actorVelocity = actorRigid.velocity;
	}

	private int nowSetLookDirectionPriority = 0;

	public void SetLookDirection (bool toLeft)
	{
		SetLookDirection (toLeft, 0);
	}

	public void SetLookDirection (bool toLeft, int priority)
	{
		if (priority < nowSetLookDirectionPriority)
			return;
		nowSetLookDirectionPriority = priority;
		if (toLeft)
			transform.localScale = new Vector3 (1, 1, 1);
		else
			transform.localScale = new Vector3 (-1, 1, 1);
	}

	public void ResetSetLookDirectionPriority ()
	{
		nowSetLookDirectionPriority = 0;
	}

	public void SetToCrouch ()
	{
		bodyAnimator.SetBool ("BoolCrouch", true);
		shoulderAnimator.SetBool ("BoolCrouch", true);
		stateInfo.isCrouhcing = true;
		var col = bodyCollider as CapsuleCollider;
		col.center = new Vector3 (
			col.center.x,
			crouchCenterY,
			col.center.z
		);
		col.height = crouchHeight;
	}

	public void ReleaseCrouch ()
	{
		bodyAnimator.SetBool ("BoolCrouch", false);
		shoulderAnimator.SetBool ("BoolCrouch", false);
		stateInfo.isCrouhcing = false;
		var col = bodyCollider as CapsuleCollider;
		col.center = new Vector3 (
			col.center.x,
			normalCenterY,
			col.center.z
		);
		col.height = normalHeight;
	}

	public void HideShoulder (bool hideValue)
	{
		if (hideValue) {
			shoulderSpriteRenderer.enabled = false;
			shoulderAnimator.enabled = false;
			return;
		}
		shoulderSpriteRenderer.enabled = true;
		shoulderAnimator.enabled = true;

	}

	public virtual void DamagedFrom (Actor fromActor, float damagedAmount, Vector3 damagedDir)
	{
		humanInfo.hp = Mathf.Max (humanInfo.hp - damagedAmount, 0f);
		damagedDirection = damagedDir;
		IncreaseTension ();
		SetToUnBeatable (0.5f);
	}

	public virtual void DamagedFrom (Actor fromActor, float damagedAmount, Vector3 damagedDir, float tensionInc)
	{
		humanInfo.hp = Mathf.Max (humanInfo.hp - damagedAmount, 0f);
		damagedDirection = damagedDir;
		IncreaseTension (tensionInc);
		SetToUnBeatable (0.5f);
	}
		
	public IEnumerator SetToUnBeatable (float maintainTime)
	{
		float timer = 0f;
		stateInfo.isUnbeatable = true;
		while (timer <= maintainTime) 
		{
			timer += customDeltaTime;
			yield return new WaitForEndOfFrame ();
		}
		stateInfo.isUnbeatable = false;
		yield return null;
	}

	public void IncreaseTension (float tensionInc)
	{
		tensionGauge = Mathf.Min (tensionGauge + customDeltaTime * tensionInc, 1f);
	}

	public void IncreaseTension ()
	{
		IncreaseTension (tensionIncreaseAmount);
	}

	public void DecreaseTension (float tensionDec)
	{
		tensionGauge = Mathf.Max (tensionGauge - customDeltaTime * tensionDec, 0f);
	}

	public void DecreaseTension ()
	{
		DecreaseTension (tensionDecreaseAmount);
	}

	public static Actor FindActorByHumanName (string humanName)
	{
		Actor[] actors = GameObject.FindObjectsOfType<Actor> ();

		for (int i = 0; i < actors.Length; i++)
		{
			if (actors[i].humanInfo.humanName == humanName)
			{
				return actors [i];
			}
		}
		return null;
	}

	public static bool IsHaveActorComponent<T> (T obj) where T : Component
	{
		if (null != obj.GetComponent<Actor>())
			return true;
		if (null != obj.GetComponentInChildren<Actor>())
			return true;
		if (null != obj.GetComponentInParent<Actor> ())
			return true;
		return false;
	}

	public static Actor GetActor<T> (T obj) where T : Component
	{
		if (null != obj.GetComponent<Actor>())
			return obj.GetComponent<Actor>();
		if (null != obj.GetComponentInChildren<Actor>())
			return obj.GetComponentInChildren<Actor>();
		if (null != obj.GetComponentInParent<Actor> ())
			return obj.GetComponentInParent<Actor> ();
		return null;
	}

	public T GetSpecificAction <T> () where T : AnimationCheckerBase
	{
		for (int i = 0; i < actions.Length; i++)
		{
			if ((actions[i]).GetType() == typeof(T))
			{
				return (T)actions [i];
			}
		}
		return null;
	}

	public bool PlaySpecificBodyAction <T> () where T : BodyAnimationCheckerBase
	{
		var newAction = GetSpecificAction<T> ();
		if (null == newAction)
			return false;
		nowBodyAction = newAction;
		return true;
	}

	public bool PlaySpecificShoulderAction <T> () where T : ShoulderAnimationCheckerBase
	{
		var newAction = GetSpecificAction<T> ();
		if (null == newAction)
			return false;
		nowShoulderAction = newAction;
		return true;
	}

	public bool EnterRoom (RoomInfo newInfo)
	{
		if (null == roomInfo) {
			roomInfo = newInfo;
			return true;	
		}
		if (newInfo == roomInfo)
		{
			return false;
		}
		if (roomInfo.actorsInRoom.Contains (this)) {
			return false;
		}
		Debug.Log ("Change Room Before : " + roomInfo.name + "  TO : " + newInfo.name);
		roomInfo = newInfo;
		return true;
	}

	public bool ExitRoom (RoomInfo prevRoom)
	{
		
		//if (prevRoom == roomInfo)
		//{
		//	roomInfo = null;
		//	return true;
		//}
		return false;
	}

	#region implemented abstract members of DynamicObject

	public override void SaveObject ()
	{
		
	}

	public override void LoadObject ()
	{
		throw new System.NotImplementedException ();
	}

	#endregion
}
