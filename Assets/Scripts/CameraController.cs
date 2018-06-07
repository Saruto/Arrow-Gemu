using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	// ----------------------------------- Fields and Properties ----------------------------------- //

	// The position of the player last frame.
	Vector3 playerPosLastFrame;

	//  --------- Serialized Fields ---------  //

	// The player gameobject
	[SerializeField] GameObject Player;

	// Should we lock the cursor?
	[SerializeField] bool LockCursor;

	// Mouse Rotation Speeds
	[SerializeField] float HorizontalSpeed = 2f;
	[SerializeField] float VerticalSpeed = 2f;

	// ------------------------------------------ Methods ------------------------------------------ //


	//  --------- Start ---------  //
	void Start () {
		playerPosLastFrame = Player.transform.position;
		if(LockCursor) Cursor.lockState = CursorLockMode.Locked;
	}
	
	//  --------- LateUpdate ---------  //
	void LateUpdate () {
		// Move the camera's position if they moved.
		if(playerPosLastFrame != Player.transform.position) {
			transform.position += Player.transform.position - playerPosLastFrame;
		}
		playerPosLastFrame = Player.transform.position;

		// Rotate the camera based on the player's mouse movements.
		float h = HorizontalSpeed * Input.GetAxis("Mouse X");
		transform.RotateAround(Player.transform.position, Vector3.up, h);
	}
}
