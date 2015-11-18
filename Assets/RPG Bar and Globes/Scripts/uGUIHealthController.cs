using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class uGUIHealthController : MonoBehaviour {

	public List<Image> healthSprites;
	public List<Image> manaSprites;

	public Player player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	public void UpdateVitals() {
		for(int i = 0; i < healthSprites.Count; i++) {
			healthSprites[i].fillAmount = player.curHealth/player.maxHealth;
		}
		for(int i = 0; i < manaSprites.Count; i++) {
			manaSprites[i].fillAmount = player.curMana/player.maxMana;
		}
	}
}
