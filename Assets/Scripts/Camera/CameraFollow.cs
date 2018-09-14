using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	//idea:	base offset in x and y (x is probably always 0)
	//		look Ahead "offset" for x and y
	//		-> x is used most of the time
	//		-> y is used when falling from high places

	public CharacterMotor target;
	public Vector2 camBoxSize;
	public float zOffset = -10f;

	public float verticalOffset;
	public float verticalSmoothTime;
	public float horizontalOffset;
	public float horizontalSmoothTime;

	CamBox camBox;
	float currentLookAheadX;
	float targetLookAheadX;
	float dirX;
	//float dirY;
	float smoothLookVelocityX;
	float smoothLookVelocityY;
	
	void Start() {
		camBox = new CamBox(target.collider.bounds, camBoxSize);
	}

	void LateUpdate() {
		//take the players bounds to recalculate the camBox edges
		camBox.update(target.collider.bounds);

		//Calculate current position
		Vector2 focusPosition = camBox.centre + Vector2.up * verticalOffset;

		//X Direction
		if (camBox.move.x != 0f) {
			dirX = Mathf.Sign(camBox.move.x);
		}

		//Calculate the horizontal Smoothing
		targetLookAheadX = dirX * horizontalOffset;
		currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, horizontalSmoothTime);

		//Y Direction
		//if (camBox.move.y != 0f) {
		//	dirY = Mathf.Sign(camBox.move.x);
		//}

		//Add vertical Smoothing
		focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothLookVelocityY, verticalSmoothTime);

		//Set Camera position
		focusPosition += Vector2.right * currentLookAheadX;
		transform.position = (Vector3)focusPosition + Vector3.forward * zOffset;
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color(0.7f, 0.7f, 0.7f, 0.5f);
		Gizmos.DrawCube(transform.position + (Vector3)camBox.centre, camBoxSize);
	}

}
