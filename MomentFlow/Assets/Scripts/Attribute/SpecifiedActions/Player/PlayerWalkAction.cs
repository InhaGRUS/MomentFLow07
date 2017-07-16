using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkAction : ActionBase {

	// Use this for initialization
	protected new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	#region implemented abstract members of ActionBase

	public override bool IsNeedTransition ()
	{
		throw new System.NotImplementedException ();
	}

	public override bool IsSatisfiedToActions ()
	{
		throw new System.NotImplementedException ();
	}

	public override void BeforeTransitionActions ()
	{
		throw new System.NotImplementedException ();
	}

	public override void TransitionAction ()
	{
		throw new System.NotImplementedException ();
	}

	public override void DoActions ()
	{
		throw new System.NotImplementedException ();
	}

	public override void DoSpecifiedAction ()
	{
		throw new System.NotImplementedException ();
	}

	public override void CancelActions ()
	{
		throw new System.NotImplementedException ();
	}

	public override void CancelSpecifiedAction ()
	{
		throw new System.NotImplementedException ();
	}

	#endregion
}
