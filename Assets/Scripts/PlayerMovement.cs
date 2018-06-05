using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	// ----------------------------------- Fields and Properties ----------------------------------- //

	
	// Components on this Character Gameobject
	Rigidbody rb;

	// Continuous forces to apply to the character.
	// Set in update and used in fixed update
	Vector3 ForceToApply;


	//  --------- Serialized Fields ---------  //

	// The camera gameobject.
	[SerializeField] GameObject Camera;

	// Speed of movement
	[SerializeField] float MovementForce;

	// Max Velocity
	[SerializeField] float MaxVelocity;

	// Speed of movement
	[SerializeField] float JumpingForce;

	// Speed of rotation
	[SerializeField] float RotationSpeed;

	// Gravity Scale
	[Range(1f, 100f)]
	[SerializeField] float GravityScale;

	// ------------------------------------------ Methods ------------------------------------------ //


	//  --------- Start ---------  //
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	//  --------- Update ---------  //
	void Update () {
		ForceToApply = Vector3.zero;
		// --- Simple Movement --- //

		// Rotates the camera's forwards/right unit vectors so that they're parallel to the XZ plane.
		Vector3 cameraPlanarForwards = Camera.transform.forward;
		cameraPlanarForwards.y = 0;
		cameraPlanarForwards.Normalize();
		Vector3 cameraPlanarRight = Camera.transform.right;
		cameraPlanarRight.y = 0;
		cameraPlanarRight.Normalize();

		// Forwards/Backwards
		if(Input.GetKey(KeyCode.W)) {
			ForceToApply += cameraPlanarForwards;
		} else if(Input.GetKey(KeyCode.S)) {
			ForceToApply += -cameraPlanarForwards;
		}

		// Right/Left
		if(Input.GetKey(KeyCode.D)) {
			ForceToApply += cameraPlanarRight;
		} else if(Input.GetKey(KeyCode.A)) {
			ForceToApply += -cameraPlanarRight;
		}

		// Set the magnitude
		ForceToApply = ForceToApply.normalized * MovementForce;


		// --- Jumping --- //
		if(Input.GetKeyDown(KeyCode.Space)) {
			rb.AddForce(transform.up * JumpingForce, ForceMode.Impulse);
			ForceToApply *= 0.2f;
		}


		// --- Rotation --- //
		//RotateTowardsMovement();
	}

	//  --------- FixedUpdate ---------  //
	void FixedUpdate() {
		// Add Forces
		rb.AddForce(ForceToApply);

		// Add Additional Gravity
		rb.AddForce(-1 * (1f - GravityScale) * (Physics.gravity * rb.mass));

		// Cap Planar Velocities
		Vector3 planeSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
		if(planeSpeed.sqrMagnitude > MaxVelocity * MaxVelocity) {
			planeSpeed = planeSpeed.normalized * MaxVelocity;
			rb.velocity = new Vector3(planeSpeed.x, rb.velocity.y, planeSpeed.z);
		}
	}

	// Rotates the player towards the direction they're moving.
	void RotateTowardsMovement() {
		float step = RotationSpeed * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, rb.velocity, RotationSpeed, 0f);
		transform.rotation = Quaternion.LookRotation(newDir);
	}



}
