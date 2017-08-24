using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TransparentByDistance : MonoBehaviour {
	private string rectName = "OcculusionRect";
	public Transform posCompareTarget;
	public MeshRenderer[] meshRenderers;
	public SkinnedMeshRenderer[] skinnedRenderers;
	public Rect3D ratioRect;

	private bool isSet = false;
	public bool useNoiseTranslate;
	[Range (0f, 1f)]
	public float nowAlpha = 1f;
	[Range (0f, 1f)]
	public float minAlpha = 0f;
	[Range (0f, 1f)]
	public float maxAlpha = 1f;

	void Start ()
	{
		CheckSetting ();
		if (isSet) {
			for (int i = 0; i < meshRenderers.Length; i++)
			{
				var element = meshRenderers [i];
				element.sharedMaterial = new Material (element.sharedMaterial);
			}

			for (int i = 0; i < skinnedRenderers.Length; i++)
			{
				var element = skinnedRenderers [i];
				element.sharedMaterial = new Material (element.sharedMaterial);
			}
		}
	}

	void OnEnable ()
	{
		if (null != ratioRect)
			ratioRect.rectName = rectName;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isSet)
			return;
		if (ratioRect.IsContainPoint (posCompareTarget.position)) {
			StopCoroutine ("SetToNormal");
			StartCoroutine ("SetToTransparent");
		}
		else {
			StopCoroutine ("SetToTransparent");
			StartCoroutine ("SetToNormal");
		}
	}

	public IEnumerator SetToTransparent ()
	{
		while (nowAlpha > minAlpha) {
			nowAlpha -= Time.deltaTime;
			for (int i = 0; i < meshRenderers.Length; i++)
			{
				var element = meshRenderers [i];
				var newColor = element.sharedMaterial.color;
				newColor.a = nowAlpha;
				element.sharedMaterial.color = newColor;
			}
			for (int i = 0; i < skinnedRenderers.Length; i++)
			{
				var element = skinnedRenderers [i];
				var newColor = element.sharedMaterial.color;
				newColor.a = nowAlpha;
				element.sharedMaterial.color = newColor;
			}
			yield return new WaitForEndOfFrame ();
		}

		if (nowAlpha != minAlpha) {
			nowAlpha = Mathf.Clamp (nowAlpha, minAlpha, maxAlpha);
			for (int i = 0; i < meshRenderers.Length; i++)
			{
				var element = meshRenderers [i];
				var newColor = element.sharedMaterial.color;
				newColor.a = nowAlpha;
				element.sharedMaterial.color = newColor;
			}
			for (int i = 0; i < skinnedRenderers.Length; i++)
			{
				var element = skinnedRenderers [i];
				var newColor = element.sharedMaterial.color;
				newColor.a = nowAlpha;
				element.sharedMaterial.color = newColor;
			}
		}
	}

	public IEnumerator SetToNormal ()
	{
		while (nowAlpha < maxAlpha) {
			nowAlpha += Time.deltaTime;
			for (int i = 0; i < meshRenderers.Length; i++)
			{
				var element = meshRenderers [i];
				var newColor = element.sharedMaterial.color;
				newColor.a = nowAlpha;
				element.sharedMaterial.color = newColor;
			}
			for (int i = 0; i < skinnedRenderers.Length; i++)
			{
				var element = skinnedRenderers [i];
				var newColor = element.sharedMaterial.color;
				newColor.a = nowAlpha;
				element.sharedMaterial.color = newColor;
			}
			yield return new WaitForEndOfFrame ();
		}

		if (nowAlpha != maxAlpha) {
			nowAlpha = Mathf.Clamp (nowAlpha, minAlpha, maxAlpha);
			for (int i = 0; i < meshRenderers.Length; i++)
			{
				var element = meshRenderers [i];
				var newColor = element.sharedMaterial.color;
				newColor.a = nowAlpha;
				element.sharedMaterial.color = newColor;
			}
			for (int i = 0; i < skinnedRenderers.Length; i++)
			{
				var element = skinnedRenderers [i];
				var newColor = element.sharedMaterial.color;
				newColor.a = nowAlpha;
				element.sharedMaterial.color = newColor;
			}
		}
	}

	void CheckSetting ()
	{
		isSet = false;
		if (null == posCompareTarget)
			return;
		if (0 == skinnedRenderers.Length && meshRenderers.Length == 0)
			return;
		if (null == ratioRect)
			return;
		isSet = true;
	}
}
