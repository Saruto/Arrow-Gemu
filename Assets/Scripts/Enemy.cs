using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
	// ----------------------------------- Fields and Properties ----------------------------------- //

	// The enemy's health.
	public int Health;

	// The player GO
	GameObject Player;

	// Components that this gameobject has.
	NavMeshAgent agent;
	CapsuleCollider collider;

	// Variables for controlling the push-back part of enemies
	float pushBackTimer = 0f;
	bool pushBackRunning = false;

	//  --------- Serialized Fields ---------  //

	

	// ------------------------------------------ Methods ------------------------------------------ //


	//  --------- Start ---------  //
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		collider = GetComponent<CapsuleCollider>();
		Player = GameObject.FindWithTag("Player");
	}
	
	//  --------- Update ---------  //
	void Update () {
		// Follow the player
		if(agent != null && agent.enabled) {
			agent.SetDestination(Player.transform.position);
		}
		
		// If we got hit, raycast to the floor to see when we're back on the ground.
		if(agent != null && !agent.enabled) {
			RaycastHit hit;
			LayerMask mask = ~LayerMask.GetMask("Player", "Enemy");
			if(Physics.Raycast(transform.position, Vector3.down, out hit, collider.bounds.extents.y + 0.05f, mask)) {
				GetComponent<Rigidbody>().isKinematic = true;
				agent.enabled = true;
			}
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
		// Push the enemy and deactivate the agent
		agent.enabled = false;
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().AddForce(direction * mag, ForceMode.Impulse);
	}
}
