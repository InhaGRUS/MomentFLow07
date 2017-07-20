using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadow : MonoBehaviour {
	public Renderer spriteRenderer;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<Renderer> ();

		spriteRenderer.receiveShadows = true;
		spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
	}
}
