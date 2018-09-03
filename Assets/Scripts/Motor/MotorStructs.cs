using UnityEngine;

/// <summary>
/// Defined points, where (Collision) Raycast should originate form. 
/// These points should be set in a seperate function every frame.
/// </summary>
public struct RaycastOrigin {
	public Vector2 topLeft, topRight;
	public Vector2 bottomLeft, bottomRight;

	//Maybe put a Set(Bounds) function and skinWidth variable
	//Make a property out of the Vector2 with only a get;
}

public struct CollisionInfo {
	//General
	public bool above, below;
	public bool left, right;

	//TODO: create struct SlopeInfo
	public bool slopeUp;
	public bool slopeDown;
	
	//Put this inside CharacterMotor
	public float slopeAngle;
	public float slopeAngleOld;

	public void Reset() {
		above = below = false;
		left = right = false;

		slopeUp = slopeDown = false;
		slopeAngleOld = slopeAngle;
		slopeAngle = 0f;
	}
}