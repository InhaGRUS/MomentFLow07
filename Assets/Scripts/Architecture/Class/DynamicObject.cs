using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DynamicObject : MonoBehaviour, ISaveable, ILoadable {

	public float customDeltaTime;
	public float customTimeScale = 1;

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

	#region ISaveable implementation

	public abstract void SaveObject ();

	#endregion

	#region ILoadable implementation

	public abstract void LoadObject ();

	#endregion
}
