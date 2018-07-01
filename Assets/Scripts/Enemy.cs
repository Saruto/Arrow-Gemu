using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
	// ----------------------------------- Fields and Properties ----------------------------------- //

	// The enemy's health.
	public int Health;

	// The navmesh agent on this enemy.
	NavMeshAgent agent;

	// The player GO
	GameObject Player;

	//  --------- Serialized Fields ---------  //

	

	// ------------------------------------------ Methods ------------------------------------------ //


	//  --------- Start ---------  //
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		Player = GameObject.FindWithTag("Player");
	}
	
	//  --------- Update ---------  //
	void Update () {
		if(agent != null && agent.enabled) {
			agent.SetDestination(Player.transform.position);
		}
		

	}


	// Called by a projectile when the enemy takes damage.
	public void TakeDamage(int damage) {
		Health -= damage;
		if(Health <= 0) {
			Destroy(gameObject);
		}
	}

	// Pushes the enemy back some direction. Used when hit with normal arrows.
	public void PushBack(Vector3 direction, float mag = 10f) {
		StartCoroutine(push(direction * mag));
	}

	IEnumerator push(Vector3 force) {
		agent.enabled = false;
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
		float accum = 0f;
		while(accum < 1f) {
			yield return null;
			accum += Time.deltaTime;
		}
		GetComponent<Rigidbody>().isKinematic = true;
		agent.enabled = true;
	}
}
