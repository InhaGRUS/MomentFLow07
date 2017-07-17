using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saver : MonoBehaviour {
	public static string GetSaveKey (string name, string type)
	{
		return SceneManager.GetActiveScene().name + ":" + name + ":" + type;
	}


	public static void SaveInt (string name, string typeTag, int value)
	{
		PlayerPrefs.SetInt (GetSaveKey (name, typeTag), value);
	}

	public static void SaveFloat (string name, string typeTag, float value)
	{
		PlayerPrefs.SetFloat (GetSaveKey (name, typeTag), value);
	}

	public static void SaveBool (string name, string typeTag, bool value)
	{
		PlayerPrefs.SetString (GetSaveKey (name, typeTag), value.ToString());
	}

	public static void SaveTransform (string name, Transform t)
	{
		PlayerPrefs.SetString (GetSaveKey(name,typeof(Transform).Name), t.position.x + "," + t.position.y + "," + t.position.z + "," +
			t.localRotation.x + "," + t.localRotation.y + "," + t.localRotation.z + "," +
			t.localScale.x + "," + t.localScale.y + "," + t.localScale.z);
	}

}
