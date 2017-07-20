using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : DynamicObject {
	public HumanInfo humanInfo = new HumanInfo();
	[HideInInspector]
	public OutsideInfo outsideInfo;
	[HideInInspector]
	public EquipmentInfo equipmentInfo;

	public Rigidbody actorRigid;
	public Collider bodyCollider;

	public bool useShoulder = false;

	public Animator bodyAnimator;
	public Animator shoulderAnimator;

	public AnimationCheckerBase[] actions;
	public AnimationCheckerBase nowBodyAction;
	public AnimationCheckerBase nowShoulderAction;

	public string nowBodyAnimationName;
	public string nowShoulderAnimationName;

	public SpecificActorStateInfo stateInfo;

	public Vector3 actorVelocity;

	// Use this for initialization
	void Start () {
		outsideInfo = GetComponentInChildren <OutsideInfo> ();
		equipmentInfo = GetComponentInChildren <EquipmentInfo> ();
		actorRigid = GetComponent<Rigidbody> ();

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

	public void SetLookDirection (bool toLeft)
	{
		if (toLeft)
			transform.localScale = new Vector3 (1, 1, 1);
		else
			transform.localScale = new Vector3 (-1, 1, 1);
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
