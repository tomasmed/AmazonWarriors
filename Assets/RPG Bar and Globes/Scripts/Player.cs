using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float curHealth;
	public float maxHealth;

	public float curMana;
	public float maxMana;

	public float healthRegenSpeed;
	public float manaRegenSpeed;

	private uGUIHealthController healthController;

	// Use this for initialization
	void Start () {
		healthController = GameObject.FindGameObjectWithTag("HealthController").GetComponent<uGUIHealthController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(curHealth < maxHealth) {
			curHealth += Time.deltaTime * healthRegenSpeed;
		}
		else {
			curHealth = maxHealth;
		}

		if(curMana < maxMana) {
			curMana += Time.deltaTime * manaRegenSpeed;
		}
		else {
			curMana = maxMana;
		}

		healthController.UpdateVitals();
	}

	public void CastSpell() {
		curMana -= Random.Range(10,20);
		healthController.UpdateVitals();
	}

	public void TakeDamage() {
		curHealth -= Random.Range(10,20);
		healthController.UpdateVitals();
	}
}
