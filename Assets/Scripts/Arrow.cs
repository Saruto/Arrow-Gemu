using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
	// ----------------------------------- Fields and Properties ----------------------------------- //

	// The rigidbody attached to this gameobject
	Rigidbody rb;

	// The player GO
	GameObject player;

	// Has this arrow hit something yet?
	bool hitAnything = false;

	//  --------- Serialized Fields ---------  //

	// Amount of damage an arrow deals
	[SerializeField] int ArrowDamage;

	// ------------------------------------------ Methods ------------------------------------------ //


	//  --------- Start ---------  //
	void Start () {
		rb = GetComponent<Rigidbody>();
		player = GameObject.FindWithTag("Player");
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

			// Push the enemy back.
			Vector3 dir = collider.transform.position - player.transform.position;
			dir.y = 0f;
			dir.Normalize();
			dir = Quaternion.AngleAxis(30f, collider.transform.right) * dir;
			collider.GetComponent<Enemy>().PushBack(dir);

			// Destroy the arrow immediately
			Destroy(gameObject);
		}
		// Mark as hitting something
		hitAnything = true;
	}



}
