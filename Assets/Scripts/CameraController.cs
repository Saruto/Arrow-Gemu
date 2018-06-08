using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	// ----------------------------------- Fields and Properties ----------------------------------- //

	// The position of the player last frame.
	Vector3 playerPosLastFrame;

	// The ideal distance from the player the camera wants to be at.
	float distanceFromPlayer;



	// Maximum/Minimum values for the camera.
	const float MinDistance = 1f;
	const float MaxDistance = 10f;


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
		// --- Follow the player --- //
		if(playerPosLastFrame != Player.transform.position) {
			transform.position += Player.transform.position - playerPosLastFrame;
		}
		playerPosLastFrame = Player.transform.position;

		// --- Horizontal Rotation Controls --- //
		float h = HorizontalSpeed * Input.GetAxis("Mouse X");
		transform.RotateAround(Player.transform.position, Vector3.up, h);

		// --- Vertical Rotation Controls --- //
		float v = VerticalSpeed * Input.GetAxis("Mouse Y");
		transform.RotateAround(Player.transform.position, transform.right, -v);
		//print(v);

	}
}
