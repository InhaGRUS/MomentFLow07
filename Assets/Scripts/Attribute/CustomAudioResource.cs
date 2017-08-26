using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class CustomAudioResource : MonoBehaviour {
	public AudioSource source;

	public bool IsPlaying
	{
		get {
			return source.isPlaying;
		}
	}

	public AudioClip Clip
	{
		get {
			return source.clip;
		}
		set {
			source.clip = value;
		}
	}

	public delegate void AudioFunc ();
	public event AudioFunc OnPlayStart;
	public event AudioFunc OnPlayEnd;
	public event AudioFunc OnAudioPause;
	public event AudioFunc OnAudioUnPause;
	public event AudioFunc OnAudioStop;

	// Use this for initialization
	void OnEnable () {
		if (null == source)
			source = GetComponent<AudioSource> ();
	}

	public IEnumerator IPlay ()
	{
		source.Play ();
		Debug.Log (source.clip.name);
		if (null != OnPlayStart)
			OnPlayStart ();
		while (IsPlaying)
		{
			yield return new WaitForEndOfFrame ();
		}
		if (null != OnPlayEnd)
			OnPlayEnd ();
	}

	public void Play ()
	{
		StopCoroutine ("IPlay");
		StartCoroutine ("IPlay");
	}

	public void Stop ()
	{
		source.Stop ();
		if (null != OnAudioStop)
			OnAudioStop ();
	}

	public void Pause ()
	{
		source.Pause ();
		if (null != OnAudioPause)
			OnAudioPause ();
	}

	public void UnPause ()
	{
		source.UnPause ();
		if (null != OnAudioUnPause)
			OnAudioUnPause ();
	}

}
