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
	public Dictionary <SurfaceTypePair, AudioClip[]> bulletImpactDic = new Dictionary<SurfaceTypePair, AudioClip[]>();

	[Header ("Path")]
	public string pathOfBulletImapactOnWall;
	public string pathOfBulletImpactOnMetal;
	public string pathOfBulletImpactOnGlass;
	public string pathOfBulletFlyClips;

	[Header ("Cashing Clips")]
	public const int CATEGORY_OF_BULLETIMPACT_COUNT = 3;
	public List<AudioClip[]> listOfBulletImpactClip = new List<AudioClip[]> ();
	private List<AudioClip> listOfBulletFlyClip = new List<AudioClip> ();

	private GameObject audioSourcePool;
	public List<CustomAudioResource> audioSourceList = new List<CustomAudioResource>();
	public int nowSourceTopIndex = 0;


	void Awake ()
	{
		instance = this;
		SceneGeneralInfo.GetInstance.OnStartScene += PoolingDatas;
		SceneGeneralInfo.GetInstance.OnStartScene += SettingAudioDictionary;
		SceneGeneralInfo.GetInstance.OnStartScene += LoadRoomsInfo;
	}

	void PoolingDatas ()
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

		var clipsOfBulletImpactOnWall = Resources.LoadAll (pathOfBulletImapactOnWall).Cast<AudioClip>().ToArray();
		var clipsOfBulletImpactOnMetal = Resources.LoadAll (pathOfBulletImpactOnMetal).Cast<AudioClip>().ToArray();
		var clipsOfBulletImpactOnGlass = Resources.LoadAll (pathOfBulletImpactOnGlass).Cast<AudioClip>().ToArray();

		listOfBulletFlyClip = Resources.LoadAll (pathOfBulletFlyClips).Cast<AudioClip> ().ToList();

		// Align By Enum SoundCategory Priority 
		listOfBulletImpactClip.Add (clipsOfBulletImpactOnWall);
		listOfBulletImpactClip.Add (clipsOfBulletImpactOnMetal);
		listOfBulletImpactClip.Add (clipsOfBulletImpactOnGlass);
	}

	void SettingAudioDictionary ()
	{
		var doc = XmlManager.GetXmlDocument ("SurfaceAudioInfoXml");
		if (null == doc) {
			Debug.LogError ("Surface Doc is Null");
			return;
		}
		var audioNodes = doc.SelectNodes ("SurfaceAudioInfoSet/BulletImpact");

		foreach (XmlNode node in audioNodes) {
			var firstType = (SurfaceType)Enum.Parse (typeof(SurfaceType), node.SelectSingleNode ("FirstSurface").InnerText);
			var secondType = (SurfaceType)Enum.Parse (typeof(SurfaceType), node.SelectSingleNode ("SecondSurface").InnerText);

			var tmpAudioPair = new SurfaceTypePair (firstType, secondType);
			string category = node.SelectSingleNode ("Category").InnerText;
			var intCategory = (int)Enum.Parse (typeof(SoundCategory), category);
			bulletImpactDic.Add (tmpAudioPair, listOfBulletImpactClip[intCategory]);
			var reversedPair = tmpAudioPair.GetReversePair ();
			bulletImpactDic.Add (reversedPair, listOfBulletImpactClip[intCategory]);
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

	public CustomAudioResource BorrowAudioSource ()
	{
		for (int i = nowSourceTopIndex; i < audioSourceList.Count; i++)
		{
			if (!audioSourceList [i].IsPlaying)
			{
				nowSourceTopIndex = i;
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
		return audioSourceList [nowSourceTopIndex];
	}


	public void ReturnAudioSource (int index)
	{
		nowSourceTopIndex = index;
	}

	public void ReturnAudioSource (CustomAudioResource source)
	{
		var index = audioSourceList.IndexOf (source);
		ReturnAudioSource (index);
	}
		
	public SurfaceTypePair GetKeyValueInAudioDictionary (SurfaceTypePair tmpKey)
	{
		var result = keyList.Find (element => (element.first == tmpKey.first && element.second == tmpKey.second));
		return result;
	}

	public AudioClip GetCollideAudioClip (SurfaceTypePair keyPair)
	{
		AudioClip[] clip;
		var getKey = GetKeyValueInAudioDictionary (keyPair);
		if (null == getKey)
			return null;
		if (bulletImpactDic.TryGetValue (getKey, out clip))
		{
			return clip[UnityEngine.Random.Range (0, clip.Length - 1)]; 
		}
		return null;
	}

	public AudioClip GetBulletFlyAudioClip ()
	{
		return listOfBulletFlyClip[UnityEngine.Random.Range(0, listOfBulletFlyClip.Count - 1)];
	}
}
