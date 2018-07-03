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
	CapsuleCollider col;

	//  --------- Serialized Fields ---------  //

	

	// ------------------------------------------ Methods ------------------------------------------ //


	//  --------- Start ---------  //
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		col = GetComponent<CapsuleCollider>();
		Player = GameObject.FindWithTag("Player");
	}
	
	//  --------- Update ---------  //
	void Update () {
		// Follow the player
		if(Player != null && agent != null && agent.enabled) {
			agent.SetDestination(Player.transform.position);
		}
		
		// If we got hit, raycast to the floor to see when we're back on the ground.
		if(agent != null && !agent.enabled) {
			RaycastHit hit;
			LayerMask mask = ~LayerMask.GetMask("Player", "Enemy");
			if(Physics.Raycast(transform.position, Vector3.down, out hit, col.bounds.extents.y + 0.05f, mask)) {
				GetComponent<Rigidbody>().isKinematic = true;
				agent.enabled = true;
			}
		}

	}

	//  --------- OnTriggerEnter ---------  //
	void OnTriggerEnter(Collider other) {
		// Hit the player when we get close enough to them
		if(other.tag == "Player") {
			// Deal damage
			other.GetComponent<Player>().TakeDamage(1);

			// Push the enemy back.
			Vector3 dir = other.transform.position - transform.position;
			dir.y = 0f;
			dir.Normalize();
			dir = Quaternion.AngleAxis(30f, other.transform.right) * dir;
			other.GetComponent<PlayerMovement>().PushBack(dir);
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
		if(agent != null) agent.enabled = false;
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().AddForce(direction * mag, ForceMode.Impulse);
	}
}
