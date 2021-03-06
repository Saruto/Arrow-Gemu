using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	// ----------------------------------- Fields and Properties ----------------------------------- //
		
	// Components on this Character Gameobject
	public CharacterController charController { get; private set; }
	Vector3 moveDirection = Vector3.zero;

	
	// --- Controller Variables --- //
	// External
	// Locks the player's rotation to that of the camera when active.
	// Used by the bow controller.
	[NonSerialized] public bool LookWithCamera = false;

	// Internal
	// Number of available mid-air jumps left for the player.
	int airJumpsLeft = 1;

	// Can turn off the player's ability to control movement.
	bool canMove = true;


	// --- Properties --- //
	// The camera's forwards and right vectors, parallel to the XZ plane
	Vector3 cameraPlanarForwards {
		get {
			Vector3 camFwd = Camera.main.transform.forward;
			camFwd.y = 0;
			return camFwd.normalized;
		}
	}
	Vector3 cameraPlanarRight {
		get {
			Vector3 camRight = Camera.main.transform.right;
			camRight.y = 0;
			return camRight.normalized;
		}
	}


	//  --------- Serialized Fields ---------  //

	// Speed of movement
	public float MovementSpeed;

	// Speed of movement
	[SerializeField] float JumpingSpeed;

	// Speed of rotation
	[SerializeField] float RotationSpeed;

	// Gravity Scale
	[SerializeField] float Gravity;


	// ------------------------------------------ Methods ------------------------------------------ //
	//  --------- Start ---------  //
	void Start () {
		charController = GetComponent<CharacterController>();
	}
	
	//  --------- Update ---------  //
	void Update () {
		// --- Simple Movement --- //
		if(canMove) {
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
			if(Input.GetKeyDown(KeyCode.Space) && (charController.isGrounded || airJumpsLeft > 0)) {
				moveDirection.y = JumpingSpeed;
				if(!charController.isGrounded) --airJumpsLeft;
			}

			// Reset jumpsleft while gounded.
			else if(charController.isGrounded) {
				airJumpsLeft = 1;
			}

			/*
			// Set the player's upwards velocity to 0 when releasing the space key mid jump ("glass ceiling effect")
			if(Input.GetKeyUp(KeyCode.Space) && !charController.isGrounded && moveDirection.y > 0) {
				moveDirection.y = 0f;
			}
			*/
		}

		// --- Applying Movement --- //
		moveDirection.y -= Gravity * Time.deltaTime;
		charController.Move(moveDirection * Time.deltaTime);

		// --- Rotation --- //
		if(LookWithCamera){
			transform.rotation = Quaternion.LookRotation(cameraPlanarForwards);
		} else {
			RotateTowardsMovement();
		}
	}


	//  --------- Public Functions ---------  //
	// Pushes the player back some direction.
	public void PushBack(Vector3 direction, float speed = 100f) {
		canMove = false;
		moveDirection = direction * speed;
		StartCoroutine(GlobalFunctions.Invoke(() => { canMove = true; }, 1f));
	}


	//  --------- Helper Functions ---------  //
	// Rotates the player towards the direction they're moving.
	void RotateTowardsMovement() {
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

		Vector3 newDir = Vector3.RotateTowards(transform.forward, facingDirection, RotationSpeed * Time.deltaTime, 0f);
		transform.rotation = Quaternion.LookRotation(newDir);
	}




}
