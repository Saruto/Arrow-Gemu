using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	// ----------------------------------- Fields and Properties ----------------------------------- //

	
	// Components on this Character Gameobject
	Rigidbody rb;
	CharacterController charController;

	Vector3 moveDirection = Vector3.zero;

	// The camera's forwards and right vectors, parallel to the XZ plane
	Vector3 cameraPlanarForwards {
		get {
			Vector3 camFwd = Camera.transform.forward;
			camFwd.y = 0;
			return camFwd.normalized;
		}
	}
	Vector3 cameraPlanarRight {
		get {
			Vector3 camRight = Camera.transform.right;
			camRight.y = 0;
			return camRight.normalized;
		}
	}


	//  --------- Serialized Fields ---------  //

	// The camera gameobject.
	[SerializeField] GameObject Camera;

	// Speed of movement
	[SerializeField] float MovementSpeed;

	// Speed of movement
	[SerializeField] float JumpingSpeed;

	// Speed of rotation
	[SerializeField] float RotationSpeed;

	// Gravity Scale
	[SerializeField] float Gravity;

	// ------------------------------------------ Methods ------------------------------------------ //


	//  --------- Start ---------  //
	void Start () {
		rb = GetComponent<Rigidbody>();
		charController = GetComponent<CharacterController>();
	}
	
	//  --------- Update ---------  //
	void Update () {
		// --- Simple Movement --- //
		if(charController.isGrounded){
			moveDirection = Vector3.zero;
		} else {
			moveDirection = new Vector3(0f, moveDirection.y, 0f);
		}

		// Forwards/Backwards
		if(Input.GetKey(KeyCode.W)) {
			moveDirection += cameraPlanarForwards;
		} else if(Input.GetKey(KeyCode.S)) {
			moveDirection += -cameraPlanarForwards;
		}

		// Right/Left
		if(Input.GetKey(KeyCode.D)) {
			moveDirection += cameraPlanarRight;
		} else if(Input.GetKey(KeyCode.A)) {
			moveDirection += -cameraPlanarRight;
		}

		// Set the magnitude
		moveDirection.x *= MovementSpeed;
		moveDirection.z *= MovementSpeed;

		// --- Jumping --- //
		if(Input.GetKeyDown(KeyCode.Space) && charController.isGrounded) {
			moveDirection.y = JumpingSpeed;
		}

		// --- Applying Movement --- //
		moveDirection.y -= Gravity * Time.deltaTime;
		charController.Move(moveDirection * Time.deltaTime);

		// --- Rotation --- //
		RotateTowardsMovement();
	}

	// Rotates the player towards the direction they're moving.
	void RotateTowardsMovement() {
		float step = RotationSpeed * Time.deltaTime;
		
		// Get movement direction vector. Based on velocity and input direction.
		Vector3 facingDirection = Vector3.zero;
		// X and Z components based on input
		facingDirection += Input.GetKey(KeyCode.W) ? cameraPlanarForwards : Input.GetKey(KeyCode.S) ? -cameraPlanarForwards : Vector3.zero;
		facingDirection += Input.GetKey(KeyCode.D) ? cameraPlanarRight : Input.GetKey(KeyCode.A) ? -cameraPlanarRight : Vector3.zero;
		// Y component based on velocity
		facingDirection.y = charController.velocity.y;
		facingDirection.y /= 15f;

		// If the character isn't moving, make them face the direction they're already facing without the y component.
		if(facingDirection == Vector3.zero || (facingDirection.x == 0f && facingDirection.z == 0f)) {
			facingDirection = transform.forward;
			facingDirection.y = 0f;
		}

		// If the character is grounded, get rid of the y rotation.
		else if(charController.isGrounded) {
			facingDirection.y = 0f;
		}

		Vector3 newDir = Vector3.RotateTowards(transform.forward, facingDirection, RotationSpeed, 0f);
		transform.rotation = Quaternion.LookRotation(newDir);
	}



}
