﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoulderRunChecker : ShoulderAnimationCheckerBase {

	// Use this for initialization
	protected new void Start () {
		base.Start ();
	}

	#region implemented abstract members of ActionBase
	protected override bool CanTransition ()
	{
		return true;
	}
	protected override bool IsSatisfiedToAction ()
	{
		if (actor.useShoulder && actor.nowBodyAction.GetType() == typeof (PlayerBodyRunChecker))
		{
			return true;
		}
		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		nowActivated = false;
	}
	public override void DoSpecifiedAction ()
	{
		nowActivated = true;
	}
	public override void CancelSpecifiedAction ()
	{
		nowActivated = false;
	}
	#endregion
}
