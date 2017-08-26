using System;
using System.Linq;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	public static SoundManager instance;

	private List<RoomInfo> roomsInScene = new List<RoomInfo>();
	public List<SurfaceTypePair> keyList = new List<SurfaceTypePair>();
	public Dictionary <SurfaceTypePair, AudioClip> collideAudioDic = new Dictionary<SurfaceTypePair, AudioClip>();

	private GameObject audioSourcePool;
	public List<CustomAudioResource> audioSourceList = new List<CustomAudioResource>();
	public int nowSourceTopIndex = 0;


	void Awake ()
	{
		instance = this;
		SceneGeneralInfo.GetInstance.OnStartScene += PoolingAudioSource;
		SceneGeneralInfo.GetInstance.OnStartScene += SettingAudioDictionary;
		SceneGeneralInfo.GetInstance.OnStartScene += LoadRoomsInfo;
	}

	void PoolingAudioSource ()
	{
		audioSourcePool = new GameObject ("AudioSourcePool");
		audioSourcePool.transform.parent = transform;
		audioSourcePool.transform.localPosition = Vector3.zero;

		for (int i = 0; i < 20; i++)
		{
			var source = audioSourcePool.AddComponent<AudioSource> ();
			source.playOnAwake = false;
			source.loop = false;
			var customSource = audioSourcePool.AddComponent <CustomAudioResource> ();
			customSource.source = source;
			audioSourceList.Add (customSource);
		}
	}

	void SettingAudioDictionary ()
	{
		var doc = XmlManager.GetXmlDocument ("SurfaceAudioInfoXml");
		if (null == doc) {
			Debug.LogError ("Surface Doc is Null");
			return;
		}
		var audioNodes = doc.SelectNodes ("SurfaceAudioInfoSet/StateSet");

		foreach (XmlNode node in audioNodes) {
			var firstType = (SurfaceType)Enum.Parse (typeof(SurfaceType), node.SelectSingleNode ("FirstSurface").InnerText);
			var secondType = (SurfaceType)Enum.Parse (typeof(SurfaceType), node.SelectSingleNode ("SecondSurface").InnerText);

			var tmpAudioPair = new SurfaceTypePair (firstType, secondType);
			AudioClip clip = Resources.Load<AudioClip> (node.SelectSingleNode ("AudioPath").InnerText + ".wav");
			//Debug.Log (node.SelectSingleNode ("AudioPath").InnerText + " : " + firstType + " , " + secondType);
			collideAudioDic.Add (tmpAudioPair, clip);
			var reversedPair = tmpAudioPair.GetReversePair ();
			collideAudioDic.Add (reversedPair, clip);
			keyList.Add (tmpAudioPair);
			keyList.Add (reversedPair);
		}
	}

	void LoadRoomsInfo ()
	{
		roomsInScene = SceneGeneralInfo.GetInstance.roomsInScene;
	}

	public CustomAudioResource BorrowAudioSource (out int index)
	{
		for (int i = nowSourceTopIndex; i < audioSourceList.Count; i++)
		{
			if (!audioSourceList [i].IsPlaying)
			{
				nowSourceTopIndex = i;
				index = i;
				return audioSourceList [i];
			}
		}
		nowSourceTopIndex = audioSourceList.Count;
		var source = audioSourcePool.AddComponent<AudioSource> ();
		source.playOnAwake = false;
		source.loop = false;
		var customSource = audioSourcePool.AddComponent <CustomAudioResource> ();
		customSource.source = source;
		audioSourceList.Add (customSource);
		index = nowSourceTopIndex;
		return audioSourceList [nowSourceTopIndex];
	}

	public void ReturnAudioSource (int index)
	{
		nowSourceTopIndex = index;
	}

	// Update is called once per frame
	void Update () {
		
	}

	public SurfaceTypePair GetKeyValueInAudioDictionary (SurfaceTypePair tmpKey)
	{
		var result = keyList.First (element => (element.first == tmpKey.first && element.second == tmpKey.second));
		return result;
	}

	public AudioClip GetCollideAudioClip (SurfaceTypePair keyPair)
	{
		AudioClip clip = new AudioClip();
		var getKey = GetKeyValueInAudioDictionary (keyPair);
		if (collideAudioDic.TryGetValue (getKey, out clip))
		{
			Debug.Log (clip.name);
			return clip;
		}
		return null;
	}
}
