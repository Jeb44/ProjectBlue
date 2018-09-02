using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : RaycastMotor, IMotor {

	public CollisionInfo collision;

	protected override void Awake() {
		base.Awake();
	}

	protected override void Start() {
		base.Start();
	}

	/// <summary>
	/// Move character, while also checking for collisions.
	/// </summary>
	/// <param name="move">Direction/Velocity of the Movement</param>
	public void Move(Vector2 move) {
		UpdateRaycastOrigin();
		collision.Reset();

		if(move.x != 0f) {
			HorizontalCollisions(ref move);
		}

		if(move.y != 0f) {
			VerticalCollisions(ref move);
		}

		transform.Translate(move);
	}

	void HorizontalCollisions(ref Vector2 move) {
		float dirX = Mathf.Sign(move.x);
		float rayLength = Mathf.Abs(move.x) + skinWidth;

		for(int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigin = (dirX == -1) ? raycastOrigin.bottomLeft
											: raycastOrigin.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, 
				Vector2.right * dirX, rayLength, collisionMask);

			//TODO: Put Debug Functions seperately
			Debug.DrawRay(rayOrigin, Vector2.right * dirX * rayLength, Color.red);

			if (hit) {
				move.x = (hit.distance - skinWidth) * dirX;
				rayLength = hit.distance;

				collision.left = (dirX == -1);
				collision.right = (dirX == 1);
			}
		}
	}

	void VerticalCollisions(ref Vector2 move) {
		float dirY = Mathf.Sign(move.y);
		float rayLength = Mathf.Abs(move.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = (dirY == -1) ? raycastOrigin.bottomLeft
											: raycastOrigin.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + move.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin,
				Vector2.up * dirY, rayLength, collisionMask);

			//TODO: Put Debug Functions seperately
			Debug.DrawRay(rayOrigin, Vector2.up * dirY * rayLength, Color.red);

			if (hit) {
				move.y = (hit.distance - skinWidth) * dirY;
				rayLength = hit.distance;

				collision.below = (dirY == -1);
				collision.above = (dirY == 1);
			}
		}
	}
}
