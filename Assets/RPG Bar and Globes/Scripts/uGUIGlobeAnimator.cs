using UnityEngine;
using System.Collections;

public class uGUIGlobeAnimator : MonoBehaviour {

	public float scrollSpeed;
	RectTransform trans;

	// Use this for initialization
	void Start () {
		trans = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		trans.localPosition += Vector3.right * Time.deltaTime * scrollSpeed;
		if(trans.anchoredPosition.x >= trans.sizeDelta.x * 0.25f) {
			trans.anchoredPosition = new Vector2(-trans.sizeDelta.x * 0.25f,0);
		}
	}
}
