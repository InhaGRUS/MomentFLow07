using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneGeneralInfo : MonoBehaviour {
	public string SceneName;

	public static SceneGeneralInfo instance;
	public static SceneGeneralInfo GetInstance
	{
		get{
			if (null == instance)
				instance = GameObject.FindObjectOfType<SceneGeneralInfo> ();
			return instance;
		}
		set{
			instance = value;
		}
	}

	public List<RoomInfo> roomsInScene = new List<RoomInfo>();

	public delegate void SceneData ();
	public event SceneData OnInitScene;
	public event SceneData OnStartScene;
	public event SceneData OnPauseScene;
	public event SceneData OnSaveScene;
	public event SceneData OnLoadScene;

	// Use this for initialization
	void Awake () {
		instance = this;
		SceneName = SceneManager.GetActiveScene ().name;
	}

	void Start ()
	{
		OnInitScene ();
		if (null != OnLoadScene) {
			OnLoadScene ();
			OnStartScene ();
		}
		else {
			OnStartScene ();
		}
	}

	public void SaveScene ()
	{

	}

	public void LoadScene ()
	{

	}
}
