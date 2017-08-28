using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SurfaceTypePair
{
	public SurfaceType first;
	public SurfaceType second;

	public SurfaceTypePair (SurfaceType f, SurfaceType s)
	{
		this.first = f;
		this.second = s;
	}

	public SurfaceTypePair GetReversePair ()
	{
		return new SurfaceTypePair (second, first);
	}
}

[System.Serializable]
[RequireComponent (typeof (Collider))]
public class SurfaceInfo : MonoBehaviour{
	public SurfaceType surfaceType;
	public int usingAudioSourceIndex = -1; // 사용중이지 않을 때
	[Range (0, 100)]
	public int audioPriority = 0;
	public AudioClip originCollideClip;
	public bool useOriginSound = false;

	[Header("AudioSetting")]
	[Range (0f, 4f)]
	public float audioPitch = 1f;
	[Range (0f, 1f)]
	public float audioVolume = 1f;

	private CustomAudioResource nowUsingSource;

	public void ReturnAudio ()
	{
		Debug.Log ("Audio Played");
		try{
			nowUsingSource.OnPlayEnd -= ReturnAudio;
		}
		catch (Exception e) {

		}
		SoundManager.instance.ReturnAudioSource (usingAudioSourceIndex);
		usingAudioSourceIndex = -1;
		nowUsingSource = null;
	}

	public void PlayCollideAudio (SurfaceInfo other)
	{
		if (null == other)
			return;
		if (useOriginSound) {
			nowUsingSource = SoundManager.instance.BorrowAudioSource (out usingAudioSourceIndex);
			nowUsingSource.OnPlayEnd += ReturnAudio;
			nowUsingSource.source.pitch = audioPitch;
			nowUsingSource.source.volume = audioVolume;
			nowUsingSource.Clip = ComparePriority (this, other).originCollideClip;
			nowUsingSource.Play ();
		} 
		else {
			nowUsingSource = SoundManager.instance.BorrowAudioSource (out usingAudioSourceIndex);
			nowUsingSource.Clip = SoundManager.instance.GetCollideAudioClip (new SurfaceTypePair(this.surfaceType, other.surfaceType));
			nowUsingSource.OnPlayEnd += ReturnAudio;
			nowUsingSource.source.pitch = audioPitch;
			nowUsingSource.source.volume = audioVolume;
			nowUsingSource.Play ();
		}
	}

	public void PlayCollideAudio (SurfaceInfo other, bool useOrigin)
	{
		if (null == other)
			return;
		if (useOrigin) {
			nowUsingSource = SoundManager.instance.BorrowAudioSource (out usingAudioSourceIndex);
			nowUsingSource.Clip = ComparePriority (this, other).originCollideClip;
			nowUsingSource.OnPlayEnd += ReturnAudio;
			nowUsingSource.source.pitch = audioPitch;
			nowUsingSource.source.volume = audioVolume;
			nowUsingSource.Play ();
		} else {
			nowUsingSource = SoundManager.instance.BorrowAudioSource (out usingAudioSourceIndex);
			nowUsingSource.Clip = SoundManager.instance.GetCollideAudioClip (new SurfaceTypePair(this.surfaceType, other.surfaceType));
			nowUsingSource.OnPlayEnd += ReturnAudio;
			nowUsingSource.source.pitch = audioPitch;
			nowUsingSource.source.volume = audioVolume;
			nowUsingSource.Play ();
		}
	}

	public static SurfaceInfo ComparePriority (SurfaceInfo first, SurfaceInfo second)
	{
		if (null == first || null == second)
			return null;
		if (first.audioPriority > second.audioPriority)
		{
			return first;
		}
		return second;
	}
}
