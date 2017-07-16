using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBase : MonoBehaviour {
	public Actor actor;
	public string[] animationNames;
	public List<ActionBase> needTransitionActionList;
	public List<ActionBase> coincideActionList;

	// Use this for initialization
	protected void Start () {
		actor = GetComponentInParent<Actor> ();
	}

	public abstract bool IsNeedTransition ();

	public abstract bool IsSatisfiedToActions ();

	public abstract void BeforeTransitionActions (); // Transition이 시작되기 전 무조건적으로 실행됨 

	public abstract void TransitionAction (); // Transition이 실행되는 동안 실행됨

	public abstract void DoActions (); // coincideActionList에 있는 각 Element의 DoSpecifedAction을 실행한다.

	public abstract void DoSpecifiedAction ();

	public abstract void CancelActions (); // BeforeTransitionAction과 TransitionAction 사이에 실행됨

	public abstract void CancelSpecifiedAction ();

}
