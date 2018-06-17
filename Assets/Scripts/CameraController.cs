using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	// ----------------------------------- Fields and Properties ----------------------------------- //

	// The position of the player last frame.
	Vector3 playerPosLastFrame;

	// The yaw (left-right) and pitch (up-down) of the camera
	float yaw;
	float pitch;

	// The ideal distance from the player the camera wants to be at.
	float distanceFromPlayer = 9.275f;
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
		// --- Handle Camera Rules/Automatic Movement --- //
		//CameraRules();


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

		Debug.DrawRay(transform.position, transform.forward * 500, Color.red);
	}



	// Random Camera Rules
	void CameraRules() {
		// If a raycast from the player to the camera is blocked, jump to the closest unoccluded position
		Vector3 playerToCamera = transform.position - Player.transform.position;
		float dist = playerToCamera.magnitude;
		//print(dist);
		Vector3 dir = playerToCamera.normalized;
		int mask = ~LayerMask.GetMask("Player");
		RaycastHit hitInfo;
		if(Physics.Raycast(Player.transform.position, playerToCamera, out hitInfo, distanceFromPlayer, mask)) {
			print(hitInfo.transform.name);
			transform.position = hitInfo.point;
		} else {
			transform.position = Player.transform.position + distanceFromPlayer * dir;
		}

	}
}
