using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBase : MonoBehaviour {
	public bool nowActivated = false;
	public Actor actor;
	public List<AnimationSet> setAnimationTriggerName;
	public int animationIndex = -1;

	// Use this for initialization
	protected void Start () {
		actor = GetComponentInParent<Actor> ();
	}

	protected abstract bool CanTransition ();

	protected abstract bool IsSatisfiedToAction ();

	protected abstract void BeforeTransitionAction (); // Transition이 시작되기 전 무조건적으로 실행됨 

	protected virtual IEnumerator TransitionAction ()
	{
		yield return null;
	}

	public abstract void DoSpecifiedAction ();

	public abstract void CancelSpecifiedAction ();

	public void TryAction ()
	{
		if (IsSatisfiedToAction ()) {
			DoSpecifiedAction ();
			if (CanTransition())
				StartCoroutine (TransitionAction ());
		} else {
			if (CanTransition ()) {
				CancelSpecifiedAction();
				StartCoroutine (TransitionAction ());
			} else {
				CancelSpecifiedAction();
			}
		}
	}
}
