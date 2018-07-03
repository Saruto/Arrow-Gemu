using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The main player script, holds statistics and other player info.
public class Player : MonoBehaviour {
	// ----------------------------------- Fields and Properties ----------------------------------- //

	// The player's health.
	int CurrentHealth;


	//  --------- Serialized Fields ---------  //

	// The max health of the player.
	[SerializeField] int MaxHealth;

	// Life bar 
	[SerializeField] Image LifeSlider;

	// ------------------------------------------ Methods ------------------------------------------ //


	//  --------- Start ---------  //
	void Start () {
		CurrentHealth = MaxHealth;
	}
	
	//  --------- Update ---------  //
	void Update () {
		// --- Life Bar --- //
		LifeSlider.fillAmount = (float)CurrentHealth / MaxHealth;
	}


	//  --------- Public Functions ---------  //
	// Makes the player lose some amount of life.
	public void TakeDamage(int damage) {
		CurrentHealth -= damage;
		if(CurrentHealth <= 0) {
			LifeSlider.fillAmount = 0f;
			Destroy(gameObject);
		}
	}

}
