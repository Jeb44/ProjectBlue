using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour {

	/// <summary>
	/// To avoid weird collision between player and objects, we have a 'skin' between it.
	/// This value is subtracted on the RaycastOrigin points and should be added again
	/// on your calculations when working with those points.
	/// </summary>
	public const float skinWidth = 0.015f;
	const float dstBetweenRays = 0.25f;

	[HideInInspector] public int horizontalRayCount = 4;
	[HideInInspector] public int verticalRayCount = 4;
	public LayerMask collisionMask;

	[HideInInspector] public float horizontalRaySpacing;
	[HideInInspector] public float verticalRaySpacing;

	[HideInInspector] public BoxCollider2D coll;
	[HideInInspector] public RaycastOrigin raycastOrigin;

	/// <summary>
	/// Unity-defined Start Function. Get BoxCollider2D and calculate RaySpacing.
	/// </summary>
	public virtual void Awake() {
		coll = GetComponent<BoxCollider2D>();
	}

	public virtual void Start() {
		CalculateRaySpacing(); //unless the raySpacing needs to be defined on the run, this can stay here
	}

	/// <summary>
	/// Calculate the distance between each ray.
	/// Change horizontalRayCount and/or vericalRayCount to modify these values and call this function again.
	/// </summary>
	public void CalculateRaySpacing() {
		Bounds bounds = coll.bounds;
		bounds.Expand(skinWidth * -2f);

		float boundsWidth = bounds.size.x;
		float boundsHeight = bounds.size.y;

		horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
		verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	/// <summary>
	/// Sets the data on the RaycastOrigin struct. 
	/// If the character is moving, this need to refreshed.
	/// </summary>
	public void UpdateRaycastOrigin() {
		Bounds bounds = coll.bounds;
		bounds.Expand(skinWidth * -2f);

		raycastOrigin.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigin.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigin.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigin.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}
}
