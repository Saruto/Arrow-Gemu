using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
	// ----------------------------------- Fields and Properties ----------------------------------- //

	// Has this arrow already dealt damage to something before?
	bool dealtDamage = false;

	//  --------- Serialized Fields ---------  //

	[SerializeField] int ArrowDamage;

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
		}
	}

}
