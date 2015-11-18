using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TextureAnimator : MonoBehaviour {
	
	public Vector2 Speed;
	public int frameSkip = 0;
	public bool zeroOffset = false;
	public bool updateInEditor = false;
	int frameCounter = 0;
	Renderer _renderer;
	Material _material;
	void Start () {
		if (zeroOffset)
			GetComponent<Renderer>().sharedMaterial.mainTextureOffset = Vector2.zero;
#if UNITY_EDITOR
		_material = GetComponent<Renderer>().sharedMaterial;
#else
		_material = renderer.material;
#endif
		GetComponent<Renderer>().material = _material;
		_renderer = GetComponent<Renderer>();
	}

	void Update () {
#if UNITY_EDITOR
		if (!updateInEditor)
			return;
#endif
		frameCounter++;
		if (frameSkip > 0)
		{
			if (frameCounter < frameSkip)
				return;
			else
				frameCounter = 0;
		}
		if (Speed == Vector2.zero)
		{
			if (_renderer)
				_material.mainTextureOffset = Vector2.zero;
		}
		else
		{
			if (_renderer && _material)
				_material.mainTextureOffset += Speed;
		}

	}


}

