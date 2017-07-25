using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DynamicObject : MonoBehaviour, ISaveable, ILoadable {

	public DynamicObjectType objectType;

	public float customDeltaTime;
	public float customTimeScale = 1;

	[HideInInspector]
	public Animator[] animators;
	[HideInInspector]
	public ParticleSystem[] particles;

	public List<float> previousTimeScaleList;

	protected void Update ()
	{
		customDeltaTime = Time.unscaledDeltaTime * customTimeScale;
	}

	public static bool IsHaveDynamicObjectComponent<T> (T obj) where T : Component
	{
		if (null != obj.GetComponent<DynamicObject>())
			return true;
		if (null != obj.GetComponentInChildren<DynamicObject>())
			return true;
		if (null != obj.GetComponentInParent<DynamicObject> ())
			return true;
		return false;
	}

	public static DynamicObject GetDynamicObject<T> (T obj) where T : Component
	{
		if (null != obj.GetComponent<DynamicObject>())
			return obj.GetComponent<DynamicObject>();
		if (null != obj.GetComponentInChildren<DynamicObject>())
			return obj.GetComponentInChildren<DynamicObject>();
		if (null != obj.GetComponentInParent<DynamicObject> ())
			return obj.GetComponentInParent<DynamicObject> ();
		return null;
	}

	public void ChangeTimeScale (float timeScale)
	{
		previousTimeScaleList.Add (customTimeScale);
		customTimeScale = timeScale;
		AffectCustomTimeScale ();
	}

	public void BackToPreviousTimeScale ()
	{
		if (previousTimeScaleList.Count != 0) {
			customTimeScale = previousTimeScaleList [previousTimeScaleList.Count - 1];
			previousTimeScaleList.RemoveAt (previousTimeScaleList.Count - 1);
		} else
			customTimeScale = 1f;
		AffectCustomTimeScale ();
	}

	private void AffectCustomTimeScale ()
	{
		if (null != animators) {
			for (int i = 0; i < animators.Length; i++)
			{
				animators [i].speed = customTimeScale;
			}
		}
		if (null != particles) {
			for (int i = 0; i < particles.Length; i++)
			{
				var ma = particles [i].main;
				ma.simulationSpeed = customTimeScale;
			}
		}
	}

	#region ISaveable implementation

	public abstract void SaveObject ();

	#endregion

	#region ILoadable implementation

	public abstract void LoadObject ();

	#endregion
}
