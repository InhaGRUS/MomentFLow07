using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : DynamicObject {
	[Header ("ActorInfo")]
	public RoomInfo roomInfo;
	public HumanInfo humanInfo = new HumanInfo();
	[HideInInspector]
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
	[Header ("Shoulder")]
	public Animator shoulderAnimator;
	public AimTarget aimTarget;
	public float armLength;

	private SpriteRenderer shoulderSpriteRenderer;

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

	public void DamagedFrom (Actor fromActor, float damagedAmount, Vector3 damagedDir)
	{
		stateInfo.isDamaged = true;
		humanInfo.hp -= damagedAmount;
		damagedDirection = damagedDir;
		stateInfo.isDamaged = true;
		Debug.Log ("Actor : " + damagedDirection);
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
