using UnityEngine;

/// <summary>
/// Defined points, where (Collision) Raycast should originate form. 
/// </summary>
public struct RaycastOrigin {
	#region SkinWidth
	const float skinWidth = 0.015f;

	/// <summary>
	/// To avoid weird collision between objects, 
	/// we start our raycasts from inside our collider.
	/// Keep in mind, that you have to add this value again
	/// when using any of the RaycastOrigins.
	/// </summary>
	public float SkinWidth {
		get { return skinWidth; }
	}
	#endregion

	#region Origins
	Vector2 topLeft, topRight;
	Vector2 bottomLeft, bottomRight;

	public Vector2 TopLeft {
		get { return topLeft; }
	}
	public Vector2 TopRight {
		get { return topRight; }
	}
	public Vector2 BottomLeft {
		get { return bottomLeft; }
	}
	public Vector2 BottomRight {
		get { return bottomRight; }
	}
	#endregion

	/// <summary>
	/// Recalculate the origin vectors from the given bounds.
	/// Has to be used every time the character moves.
	/// </summary>
	/// <param name="bounds">Current collider bounds (usually collder.bounds)</param>
	public void Update(Bounds bounds) {
		bounds.Expand(skinWidth * -2f); //2 -> both sides

		bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		topLeft = new Vector2(bounds.min.x, bounds.max.y);
		topRight = new Vector2(bounds.max.x, bounds.max.y);
	}
}

/// <summary>
/// Calculate the number and the distance between rays based on the objects collider size.
/// </summary>
public struct RaycastSpacing { //NOTE RacastSpacing -> better name for spacing?
	#region Distance between Rays
	const float dstBetweenRays = 0.25f;
	public float DistanceBetweenRays {
		get { return dstBetweenRays; }
	}
	#endregion

	#region RayCount
	int horizontalRayCount;
	int verticalRayCount;

	public int HorizontalRayCount {
		get { return horizontalRayCount; }
	}
	public int VerticalRayCount {
		get { return verticalRayCount; }
	}
	#endregion

	#region RaySpacing
	float horizontalRaySpacing;
	float verticalRaySpacing;

	public float HorizontalRaySpacing {
		get { return horizontalRaySpacing; }
	}
	public float VerticalRaySpacing {
		get { return verticalRaySpacing; }
	}
	#endregion

	/// <summary>
	/// Calculate number and distance between rays. Call this function, whenever the collider size changes.
	/// </summary>
	/// <param name="bounds">Current collider bounds (usually collder.bounds)</param>
	/// <param name="skinWidth">Thin protection layer so collider don't space out when directly contacting ground/walls.</param>
	public void Set(Bounds bounds, float skinWidth = 0.015f) {
		bounds.Expand(skinWidth * -2f);

		float boundsHeight = bounds.size.y;
		float boundsWidth = bounds.size.x;

		horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
		verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = boundsHeight / (horizontalRayCount - 1);
		verticalRaySpacing = boundsWidth / (verticalRayCount - 1);
	}
}

/// <summary>
/// Stores the current collisions directions.
/// </summary>
public struct CollisionInfo {
	public bool above, below;
	public bool left, right;

	//NOTE CollisionsInfo: are slopeUp/slopeDown needed as Information in other classes?
	public bool slopeUp, slopeDown;

	/// <summary>
	/// Set all the bool to false. Please call this every frame.
	/// </summary>
	public void Reset() {
		above = below = false;
		left = right = false;
		slopeUp = slopeDown = false;
	}
}