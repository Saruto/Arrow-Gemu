using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBowControls : MonoBehaviour {
	// ----------------------------------- Fields and Properties ----------------------------------- //

	// The current state of the player's bow.
	enum State { Neutral, ZoomedIn }
	State CurrentState;

	// The camera's forwards and right vectors, parallel to the XZ plane
	Vector3 cameraPlanarForwards {
		get {
			Vector3 camFwd = Camera.main.transform.forward;
			camFwd.y = 0;
			return camFwd.normalized;
		}
	}

	
	//  --------- Serialized Fields ---------  //

	// The arrow projectile prefab
	[SerializeField] GameObject ArrowPrefab;

	// How much force the player shoots the arrow at.
	[SerializeField] float ArrowForce;


	// ------------------------------------------ Methods ------------------------------------------ //


	//  --------- Start ---------  //
	void Start () {
		CurrentState = State.Neutral;
	}
	
	//  --------- Update ---------  //
	void Update () {
		switch(CurrentState) {
		case State.Neutral:
			// --- Input --- //
			// Fire an arrow based on where the camera is looking.
			if(Input.GetMouseButtonDown(0)) {
				// Spawn the arrow in.
				const float startForwardsOffset = 1f;	// amount forwards the arrow should spawn in in.
				const float startUpOffset = 1f;
				GameObject arrow = Instantiate(ArrowPrefab, transform.position, Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f));
				arrow.transform.position += startForwardsOffset * cameraPlanarForwards;
				arrow.transform.position += startUpOffset * transform.up;
				// Push it forwads.
				//arrow.GetComponent<Rigidbody>().velocity += GetComponent<PlayerMovement>().charController.velocity;
				arrow.GetComponent<Rigidbody>().AddForce(ArrowForce * cameraPlanarForwards, ForceMode.Impulse);
				Destroy(arrow, 5f);
			}

			// Change to the ZoomedIn State
			if(Input.GetMouseButtonDown(1)) {
				CurrentState = State.ZoomedIn;
			}
			break;


		case State.ZoomedIn:
			// --- Input --- //
			// If the player isn't holding Right Click, transition back to the Neutral State.
			if(!Input.GetMouseButton(1)) {
				CurrentState = State.Neutral;
			}

			break;
		}



	}

}
