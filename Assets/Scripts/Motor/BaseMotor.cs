using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RequireComponent should not only be BoxCollider2D
//When implementing the character/objects,
//please revisit this script

[RequireComponent(typeof(BoxCollider2D))]
public class BaseMotor : MonoBehaviour {

	public LayerMask collisionMask;
	public bool drawDebugRaycastOriginCountSpacing = false;

	/// <summary>
	/// To avoid weird collision between objects, 
	/// we start our raycasts from inside our collider.
	/// Keep in mind, that you have to add this value again
	/// when using any of the RaycastOrigins.
	/// </summary>
	public const float skinWidth = 0.015f;
	protected const float dstBetweenRays = 0.25f;

	protected int horizontalRayCount;
	protected int verticalRayCount;

	protected float horizontalRaySpacing;
	protected float verticalRaySpacing;

	protected new BoxCollider2D collider;
	protected RaycastOrigin raycastOrigin;

	protected virtual void Awake() {
		collider = GetComponent<BoxCollider2D>();
	}

	protected virtual void Start() {
		CalculateRaySpacing();
	}

	protected virtual void Update() {
		if (drawDebugRaycastOriginCountSpacing) {
			DebugRaycastOriginCountSpacing();
		}			
		UpdateRaycastOrigin();
	}

	/// <summary>
	/// Calculate rayCount and raySpacing for horizontal and vertical Raycast.
	/// If the size of the object is changing, please call this function.
	/// </summary>
	private void CalculateRaySpacing() {
		Bounds bounds = collider.bounds;
		bounds.Expand(skinWidth * -2f); //2 -> both sides

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
	/// Update the RaycastOrigin struct Vector2 data.
	///	This may be moved to the struct itself in the future.
	/// </summary>
	private void UpdateRaycastOrigin() {
		Bounds bounds = collider.bounds;
		bounds.Expand(skinWidth * -2f); //2 -> both sides

		raycastOrigin.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigin.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigin.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigin.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	/// <summary>
	/// Draw RaycastOrigin and RaycastCount with RaycastSpacing.
	/// </summary>
	protected void DebugRaycastOriginCountSpacing() {
		Color color = new Color(0f, 1f, 0f, 0.5f);
		//Vertical RaycastOrigin
		Debug.DrawLine(raycastOrigin.bottomLeft + Vector2.down / 2, raycastOrigin.topLeft + Vector2.up / 2, color);
		Debug.DrawLine(raycastOrigin.bottomRight + Vector2.down / 2, raycastOrigin.topRight + Vector2.up / 2, color);
		//Horizontal RaycastOrigin
		Debug.DrawLine(raycastOrigin.topLeft + Vector2.left / 2, raycastOrigin.topRight + Vector2.right / 2, color);
		Debug.DrawLine(raycastOrigin.bottomLeft + Vector2.left / 2, raycastOrigin.bottomRight + Vector2.right / 2, color);

		color = new Color(0f, 1f, 0f, 0.35f);

		//Vertical RaycastCount + RaycastSpacing
		for (int i = 1; i < verticalRayCount - 1; i++) {
			Debug.DrawRay(raycastOrigin.bottomLeft + Vector2.right * (verticalRaySpacing * i), Vector2.down / 4, color);
			Debug.DrawRay(raycastOrigin.topLeft + Vector2.right * (verticalRaySpacing * i), Vector2.up / 4, color);
		}
		//Horizontal RaycastCount + RaycastSpacing
		for (int i = 1; i < horizontalRayCount - 1; i++) {
			Debug.DrawRay(raycastOrigin.bottomLeft + Vector2.up * (horizontalRaySpacing * i), Vector2.left / 4, color);
			Debug.DrawRay(raycastOrigin.bottomRight + Vector2.up * (horizontalRaySpacing * i), Vector2.right / 4, color);
		}			
	}
}
