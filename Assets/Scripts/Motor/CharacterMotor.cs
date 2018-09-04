using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BUG: CharacterMotor: jump doesn't always work when switching from slope up to down

[RequireComponent(typeof(RaycastMotor))]
public class CharacterMotor : RaycastMotor, IMotor {
	//CharacterMotor Debug Functions -> DrawRay via boolean and seperate function

	public CollisionInfo collision;
	public float maxSlopeAngle;

	float currentSlopeAngle;
	float lastSlopeAngle;
	Vector3 moveOld; //stored in case we detect a slope up while walking a slope down 

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
		//Reset/recalculate values
		raycastOrigin.Update(collider.bounds);
		collision.Reset();
		lastSlopeAngle = currentSlopeAngle;
		currentSlopeAngle = 0f;
		moveOld = move;

		if(move.y < 0f) {
			DescendSlope(ref move);
		}

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
		float rayLength = Mathf.Abs(move.x) + raycastOrigin.SkinWidth;

		for(int i = 0; i < raycastSpacing.HorizontalRayCount; i++) {
			Vector2 rayOrigin = (dirX == -1) ? raycastOrigin.BottomLeft
											: raycastOrigin.BottomRight;
			rayOrigin += Vector2.up * (raycastSpacing.HorizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, 
				Vector2.right * dirX, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * dirX * rayLength, Color.red);

			if (hit) {

				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

				//Only check slopes on bottom raycast
				if(i == 0 && slopeAngle <= maxSlopeAngle){

					if (collision.slopeDown) {
						collision.slopeDown = false;
						move = moveOld;
					}

					//remove 'hit.distance' when walking on slope (!not standing)
					//hit.distance is usually really small, but you are 'jumping' down
					//on the slope when you stop moving
					float distanceToSlopeStart = 0f;
					if(slopeAngle != lastSlopeAngle) {
						//the != occurs when start walking on a slope
						//because the slopeAngleOld is 0f, while slopeAngle is changed
						//it also occurs when you go from a slope to another slope
						distanceToSlopeStart = hit.distance - raycastOrigin.SkinWidth;
						move.x -= distanceToSlopeStart * dirX;
					}
					ClimbingSlope(ref move, slopeAngle);
					move.x += distanceToSlopeStart * dirX;
				}

				//if not on a slope (standard case)
				//OR detecting a "wall" (while also on a slope)
				if(!collision.slopeUp || slopeAngle > maxSlopeAngle) {
					move.x = (hit.distance - raycastOrigin.SkinWidth) * dirX;
					rayLength = hit.distance;

					if (collision.slopeUp) {
						move.y = Mathf.Tan(currentSlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(move.x);
					}

					collision.left = (dirX == -1);
					collision.right = (dirX == 1);
				}				
			}
		}
	}

	void VerticalCollisions(ref Vector2 move) {
		float dirY = Mathf.Sign(move.y);
		float rayLength = Mathf.Abs(move.y) + raycastOrigin.SkinWidth;

		for (int i = 0; i < raycastSpacing.VerticalRayCount; i++) {
			Vector2 rayOrigin = (dirY == -1) ? raycastOrigin.BottomLeft
											: raycastOrigin.TopLeft;
			rayOrigin += Vector2.right * (raycastSpacing.VerticalRaySpacing * i + move.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin,
				Vector2.up * dirY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * dirY * rayLength, Color.red);

			if (hit) {
				move.y = (hit.distance - raycastOrigin.SkinWidth) * dirY;
				rayLength = hit.distance;

				//if climbingSlope
				//tan(a) = y / x <=> x = y / tan(a)
				//velocity.x = velocity.y / tan(slopeAngle * deg2Rad) * dirY

				if (collision.slopeUp) {
					move.x = move.y / Mathf.Tan(currentSlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(move.x);
				}

				collision.below = (dirY == -1);
				collision.above = (dirY == 1);
			}
		}

		//a bug occurs when we switch between two slopes, so we are 'stuck' for a frame in a platform
		//so we try to detect other slopes beforehand and adjust our character accordingly
		if (collision.slopeUp) {
			float dirX = Mathf.Sign(move.x);
			rayLength = Mathf.Abs(move.x) + raycastOrigin.SkinWidth;
			//case the ray from the new height
			Vector2 rayOrigin = ((dirX == -1) ? raycastOrigin.BottomLeft : raycastOrigin.BottomRight) + Vector2.up * move.y;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * dirX, rayLength, collisionMask);

			if (hit) {
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				//if new slope is not our current slope
				if(slopeAngle != currentSlopeAngle) {
					move.x = (hit.distance - raycastOrigin.SkinWidth) * dirX;
					currentSlopeAngle = slopeAngle;
				}
			}
		}
	}

	void ClimbingSlope(ref Vector2 move, float slopeAngle){
		float distance = Mathf.Abs(move.x);

		float slopeMoveY = distance * Mathf.Sin(slopeAngle * Mathf.Deg2Rad);

		//if y is smaller then calculated move then change it
		//if y is higher, that the player is jumping (or otherwise changing the y move)
		if(move.y <= slopeMoveY){
			move.y = slopeMoveY;
			move.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * distance * Mathf.Sign(move.x);

			collision.below = true; //we can assume, that when we walk on a slope we are grounded
			collision.slopeUp = true;
			currentSlopeAngle = slopeAngle;
		}
	}

	void DescendSlope(ref Vector2 move) {
		float dirX = Mathf.Sign(move.x);
		Vector2 rayOrigin = (dirX == -1) ? raycastOrigin.BottomRight : raycastOrigin.BottomLeft;
		RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);
		Debug.DrawRay(rayOrigin, Vector2.down, Color.blue);

		if (hit) {
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			//maybe bug occurs when faliing on a angled platform? 
			if (slopeAngle != 0f && slopeAngle <= maxSlopeAngle) {
				//are you actually walking downwards? (
				if(Mathf.Sign(hit.normal.x) == dirX) {
					//if our detected ground is smaller/egual than our actual move (calculated thourgh y = tan(a) * x)
					if (hit.distance - raycastOrigin.SkinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(move.x)) {
						float distance = Mathf.Abs(move.x);

						move.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * distance * dirX;
						move.y -= distance * Mathf.Sin(slopeAngle * Mathf.Deg2Rad);

						currentSlopeAngle = slopeAngle;
						collision.slopeDown = true;
						collision.below = true;

					}
				}
			}
		}
	}
}
