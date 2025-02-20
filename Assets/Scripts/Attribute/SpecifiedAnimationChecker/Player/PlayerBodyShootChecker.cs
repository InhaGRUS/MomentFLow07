﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyShootChecker : BodyAnimationCheckerBase {
	
	#region implemented abstract members of AnimationCheckerBase
	protected override bool CanTransition ()
	{
		return true;
	}
	protected override bool IsSatisfiedToAction ()
	{
		if (Input.GetAxis ("Horizontal") == 0 && Input.GetAxis ("Vertical") == 0 &&
			Input.GetMouseButton (0)
		)
		{
			return true;
		}
		return false;
	}
	protected override void BeforeTransitionAction ()
	{
		
	}
	public override void DoSpecifiedAction ()
	{
		
	}
	public override void CancelSpecifiedAction ()
	{
		
	}
	#endregion
	
}
