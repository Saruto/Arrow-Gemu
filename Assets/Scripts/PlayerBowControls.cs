using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBowControls : MonoBehaviour {
	// ----------------------------------- Fields and Properties ----------------------------------- //

	// The playermovement script
	PlayerMovement playerMovement;

	// The current state of the player's bow.
	enum State { Normal, Focused }
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

	// Movement Speed Multiplier when Focused
	[SerializeField] float ZoomedMoveSpeedMultiplier;

	// The targeting redicle used when Focused.
	[SerializeField] GameObject TargetingRedicle;


	// ------------------------------------------ Methods ------------------------------------------ //


	//  --------- Start ---------  //
	void Start () {
		playerMovement = GetComponent<PlayerMovement>();
		CurrentState = State.Normal;
		TargetingRedicle.SetActive(false);
	}
	
	//  --------- Update ---------  //
	void Update () {
		switch(CurrentState) {
		case State.Normal:
			// --- Input --- //
			// Fire an arrow based on where the camera is looking but planar to the horizontal.
			if(Input.GetMouseButtonDown(0)) {
				FireArrow(false);
			}

			// Change to the Focused State
			if(Input.GetMouseButtonDown(1)) {
				CurrentState = State.Focused;
				playerMovement.MovementSpeed *= ZoomedMoveSpeedMultiplier;
				playerMovement.LookWithCamera = true;
				TargetingRedicle.SetActive(true);
			}
			break;


		case State.Focused:
			// --- Input --- //
			// If the player isn't holding Right Click, transition back to the Neutral State.
			if(!Input.GetMouseButton(1)) {
				CurrentState = State.Normal;
				playerMovement.MovementSpeed /= ZoomedMoveSpeedMultiplier;
				playerMovement.LookWithCamera = false;
				TargetingRedicle.SetActive(false);
			}

			// Fire an arrow directly towards where the player is looking.
			if(Input.GetMouseButtonDown(0)) {
				FireArrow(true);
			}

			break;
		}



	}



	// Fires an arrow based on where the camera is looking.
	// If TrueAim is false, then it will fire parallel with the ground.
	void FireArrow(bool trueAim) {
		const float startForwardsOffset = 1f;	// amount forwards the arrow should spawn in in.
		const float startUpOffset = 0.7f;
		Quaternion arrowRot;
		Vector3 forceDir;
		if(!trueAim) {
			forceDir = cameraPlanarForwards;
			arrowRot = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
		} else {
			// Calculates the angle the player needs to shoot at by finding the target spot via raycasting.
			Vector3 targetPoint;
			RaycastHit hit;
			const float maxDist = 200f;
			if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDist)) {
				targetPoint = hit.point;
			} else {
				// If there's no hit, create a "pretend" target spot at maxDist units away.
				targetPoint = maxDist * Camera.main.transform.forward + Camera.main.transform.position;
			}
			Vector3 arrowStartPos = transform.position + startUpOffset * transform.up;
			forceDir = (targetPoint - arrowStartPos).normalized;
			arrowRot = Quaternion.LookRotation(forceDir);
		}
		// Spawn the arrow in, set its starting position.
		GameObject arrow = Instantiate(ArrowPrefab, transform.position, arrowRot);
		arrow.transform.position += startForwardsOffset * forceDir;
		arrow.transform.position += startUpOffset * transform.up;
		// Push it forwads.
		arrow.GetComponentsInChildren<Rigidbody>()[1].AddForce(ArrowForce * forceDir, ForceMode.Impulse);
		Debug.DrawRay(arrow.transform.position, forceDir * 500, Color.green, 2f);
	}

}
