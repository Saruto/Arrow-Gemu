using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
	// ----------------------------------- Fields and Properties ----------------------------------- //

	// Has this arrow already dealt damage to something before?
	bool dealtDamage = false;

	//  --------- Serialized Fields ---------  //

	// Amount of damage an arrow deals
	[SerializeField] int ArrowDamage;

	// Should Arrows stick to enemies?
	enum StickToEnemies { No, Random, Yes }
	[SerializeField] StickToEnemies ArrowsStick;

	// ------------------------------------------ Methods ------------------------------------------ //


	//  --------- Start ---------  //
	void Start () {
		
	}
	
	//  --------- Update ---------  //
	void Update () {
		
	}

	// OnCollisionEnter - Deal damage to enemies.
	void OnCollisionEnter(Collision collision) {
		if(collision.collider.tag == "Enemy" && !dealtDamage) {
			// Deal damage
			collision.collider.GetComponent<EnemyStats>().TakeDamage(ArrowDamage);
			dealtDamage = true;

			// Stick to the enemy.
			if(ArrowsStick == StickToEnemies.Yes || (ArrowsStick == StickToEnemies.Random && Random.value > 0.5f)){
				Rigidbody rb = GetComponent<Rigidbody>();
				rb.velocity = Vector3.zero;
				// push the arrow a bit inside the enemy
				//transform.position += 0.2f * (collision.contacts[0].point - transform.position).normalized;
				Destroy(rb);
				Destroy(GetComponent<Collider>());
				transform.SetParent(collision.collider.transform);
			}

		}
	}

}
