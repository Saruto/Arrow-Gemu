using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
	// ----------------------------------- Fields and Properties ----------------------------------- //

	// The rigidbody attached to this gameobject
	Rigidbody rb;

	// Has this arrow hit something yet?
	bool hitAnything = false;

	//  --------- Serialized Fields ---------  //

	// Amount of damage an arrow deals
	[SerializeField] int ArrowDamage;

	// Should Arrows stick to enemies?
	enum StickToEnemies { No, Random, Yes }
	[SerializeField] StickToEnemies ArrowsStick;

	// ------------------------------------------ Methods ------------------------------------------ //


	//  --------- Start ---------  //
	void Start () {
		Destroy(gameObject, 5f);
	}
	
	//  --------- Update ---------  //
	void Update () {

		// Rotate until it hits something
		if(!hitAnything) {
			//transform.Rotate(rb.velocity.normalized, 100f * Time.deltaTime);
			
		}
	}

	//  --------- Fixed Update ---------  //
	void FixedUpdate() {
		// Apply extra gravity after hitting an object.

	}

	// OnCollisionEnter - Deal damage to enemies.
	void OnCollisionEnter(Collision collision) {
		// Hitting an enemy.
		if(collision.collider.tag == "Enemy" && !hitAnything) {
			// Deal damage
			collision.collider.GetComponent<EnemyStats>().TakeDamage(ArrowDamage);

			// Stick to the enemy.
			if(ArrowsStick == StickToEnemies.Yes || (ArrowsStick == StickToEnemies.Random && Random.value > 0.5f)){
				rb.velocity = Vector3.zero;
				// push the arrow a bit inside the enemy
				//transform.position += 0.2f * (collision.contacts[0].point - transform.position).normalized;
				Destroy(rb);
				Destroy(GetComponent<Collider>());
				transform.SetParent(collision.collider.transform);
			}
		}
		// Mark as hitting something
		hitAnything = true;
	}



}
