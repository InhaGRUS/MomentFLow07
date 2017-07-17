using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : ActionBase {

	// Use this for initialization
	protected new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	#region implemented abstract members of ActionBase

	protected override bool CanTransition ()
	{
		return true;
	}

	protected override bool IsSatisfiedToAction ()
	{
		throw new System.NotImplementedException ();
	}

	protected override void BeforeTransitionAction ()
	{
		throw new System.NotImplementedException ();
	}

	public override void DoSpecifiedAction ()
	{
		throw new System.NotImplementedException ();
	}

	public override void CancelSpecifiedAction ()
	{
		throw new System.NotImplementedException ();
	}

	#endregion
}
