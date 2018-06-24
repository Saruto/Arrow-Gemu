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
		rb = GetComponent<Rigidbody>();
		Destroy(gameObject, 5f);
	}
	
	//  --------- Update ---------  //
	void Update () {

		// Rotate until it hits something
		if(!hitAnything) {
			//rb.AddTorque(rb.velocity.normalized * 10f);
		}
	}

	//  --------- Fixed Update ---------  //
	void FixedUpdate() {
		// Apply extra gravity after hitting an object.

	}

	// OnTriggerEnter - Deal damage to enemies.
	void OnTriggerEnter(Collider collider) {
		// Hitting an enemy.
		if(collider.tag == "Enemy" && !hitAnything) {
			// Deal damage
			collider.GetComponent<Enemy>().TakeDamage(ArrowDamage);

			// Stick to the enemy.
			if(ArrowsStick == StickToEnemies.Yes || (ArrowsStick == StickToEnemies.Random && Random.value > 0.5f)){
				Destroy(gameObject);
				//rb.velocity = Vector3.zero;
				//transform.position += 0.2f * (collision.contacts[0].point - transform.position).normalized;
				//rb.Sleep();
				//Destroy(rb);
				//Destroy(GetComponentInChildren<Collider>());
				//print(transform.position);
				//transform.SetParent(collider.transform);
			}
		}
		// Mark as hitting something
		hitAnything = true;
	}



}
