using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {
	// ----------------------------------- Fields and Properties ----------------------------------- //

	// The enemy's health.
	[SerializeField] int Health;


	//  --------- Serialized Fields ---------  //



	// ------------------------------------------ Methods ------------------------------------------ //


	//  --------- Start ---------  //
	void Start () {
		
	}
	
	//  --------- Update ---------  //
	void Update () {
		
	}


	// Called by a projectile when the enemy takes damage.
	public void TakeDamage(int damage) {
		Health -= damage;
		if(Health <= 0) {
			Destroy(gameObject);
		}
	}
}
