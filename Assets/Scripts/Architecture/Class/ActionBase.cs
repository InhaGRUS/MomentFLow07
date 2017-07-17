using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBase : MonoBehaviour {
	public bool nowActivated = false;
	public Actor actor;
	public string[] bodyAnimationTriggerName;
	public string[] shoulderAnimationTriggerName;
	public List<ActionBase> canTransitionActionList;

	// Use this for initialization
	protected void Start () {
		actor = GetComponentInParent<Actor> ();
	}

	protected abstract bool CanTransition ();

	protected abstract bool IsSatisfiedToAction ();

	protected abstract void BeforeTransitionAction (); // Transition이 시작되기 전 무조건적으로 실행됨 

	protected void TransitionAction ()
	{
		for (int i = canTransitionActionList.Count - 1; i >= 0; i--)
		{
			if (canTransitionActionList [i].IsSatisfiedToAction())
			{
				actor.nowAction = canTransitionActionList [i];
				break;
			}
		}
		BeforeTransitionAction ();
		CancelSpecifiedAction ();
	}

	public abstract void DoSpecifiedAction ();

	public abstract void CancelSpecifiedAction ();

	public void TryAction ()
	{
		if (IsSatisfiedToAction ()) {
			DoSpecifiedAction ();
			TransitionAction ();
		} else {
			if (CanTransition ()) {
				CancelSpecifiedAction();
				BeforeTransitionAction ();
				TransitionAction ();
			} else {
				CancelSpecifiedAction();
			}
		}
	}

}
