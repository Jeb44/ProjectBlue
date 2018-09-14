using UnityEngine;

//NOTE maybe rearrange RaycastMotor to be a component instead of a base class
//RaycastMotor add functionality for not just BoxColliders

/// <summary>
/// Loads raycast data inside the RaycastOrigin struct.
/// Use this as base class whenever you work with Collider2D.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class RaycastMotor : MonoBehaviour {

	public LayerMask collisionMask;
	public bool drawDebugRays = false;

	[HideInInspector] public new BoxCollider2D collider;
	protected RaycastOrigin raycastOrigin;
	protected RaycastSpacing raycastSpacing;

	protected virtual void Awake() {
		collider = GetComponent<BoxCollider2D>();
		raycastOrigin = new RaycastOrigin();
		raycastSpacing = new RaycastSpacing();
	}

	protected virtual void Start() {
		raycastOrigin.Update(collider.bounds);
		raycastSpacing.Set(collider.bounds, raycastOrigin.SkinWidth);
	}

	protected virtual void Update() {
		if (drawDebugRays) {
			DebugRaycastOriginCountSpacing();
		}
	}

	/// <summary>
	/// Draw RaycastOrigin and RaycastCount with RaycastSpacing.
	/// </summary>
	protected void DebugRaycastOriginCountSpacing() {
		Color color = new Color(0f, 1f, 0f, 0.5f);

		//Vertical RaycastOrigin
		Debug.DrawLine(raycastOrigin.BottomLeft + Vector2.down / 2, raycastOrigin.TopLeft + Vector2.up / 2, color);
		Debug.DrawLine(raycastOrigin.BottomRight + Vector2.down / 2, raycastOrigin.TopRight + Vector2.up / 2, color);
		//Horizontal RaycastOrigin
		Debug.DrawLine(raycastOrigin.TopLeft + Vector2.left / 2, raycastOrigin.TopRight + Vector2.right / 2, color);
		Debug.DrawLine(raycastOrigin.BottomLeft + Vector2.left / 2, raycastOrigin.BottomRight + Vector2.right / 2, color);

		color = new Color(0f, 1f, 0f, 0.35f);

		//Vertical RaycastCount + RaycastSpacing
		for (int i = 1; i < raycastSpacing.VerticalRayCount - 1; i++) {
			Debug.DrawRay(raycastOrigin.BottomLeft + Vector2.right * (raycastSpacing.VerticalRaySpacing * i), Vector2.down / 4, color);
			Debug.DrawRay(raycastOrigin.TopLeft + Vector2.right * (raycastSpacing.VerticalRaySpacing * i), Vector2.up / 4, color);
		}
		//Horizontal RaycastCount + RaycastSpacing
		for (int i = 1; i < raycastSpacing.HorizontalRayCount - 1; i++) {
			Debug.DrawRay(raycastOrigin.BottomLeft + Vector2.up * (raycastSpacing.HorizontalRaySpacing * i), Vector2.left / 4, color);
			Debug.DrawRay(raycastOrigin.BottomRight + Vector2.up * (raycastSpacing.HorizontalRaySpacing * i), Vector2.right / 4, color);
		}
	}
}