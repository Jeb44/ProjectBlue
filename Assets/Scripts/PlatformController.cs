using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController {

	public LayerMask passengerMask;
	public Vector3[] localWaypoints;
	Vector3[] globalWaypoints;

	public float speed;
	public bool isCyclic;
	public float waitTime;
	[Range(0,2)] public float easeAmount;

	int fromWaypointIndex;
	float percentBetweenWaypoint;
	float nextMoveTime;

	List<PassengerMovement> passengerMovements;
	Dictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();

	public override void Start () {
		base.Start();

		globalWaypoints = new Vector3[localWaypoints.Length];
		for(int i = 0; i < localWaypoints.Length; i++) {
			globalWaypoints[i] = localWaypoints[i] + transform.position;
		}
	}
	
	void Update () {
		UpdateRaycastOrigin();
		Vector3 velocity = CalculatePlatformMovement();

		CalculatePassengerMovement(velocity);

		MovePassengers(true);
		transform.Translate(velocity);
		MovePassengers(false);
	}

	void MovePassengers(bool beforeMovePlatform) {
		foreach(PassengerMovement passenger in passengerMovements) {
			if(!passengerDictionary.ContainsKey(passenger.transform)) {
				passengerDictionary.Add(passenger.transform.transform, passenger.transform.GetComponent<Controller2D>());
			}
			if(passenger.moveBeforePlatform == beforeMovePlatform) {
				passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
			}
		}
	}

	void CalculatePassengerMovement(Vector3 velocity) {
		HashSet<Transform> movedPassengers = new HashSet<Transform>();
		passengerMovements = new List<PassengerMovement>();

		float directionX = Mathf.Sign(velocity.x);
		float directionY = Mathf.Sign(velocity.y);

		//vertical moving platform
		if(velocity.y != 0f) {
			float rayLength = Mathf.Abs(velocity.y) + skinWidth;

			for(int i = 0; i < verticalRayCount; i++) {
				Vector2 rayOrigin = (directionY == -1) ? raycastOrigin.bottomLeft : raycastOrigin.topLeft;
				rayOrigin += Vector2.right * (verticalRaySpacing * i);

				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

				if(hit && hit.distance != 0f) {
					if(!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);

						float pushX = (directionY == 1) ? velocity.x : 0;
						float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

						passengerMovements.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
					}					
				}				
			}
		}

		//horizontal moving platform
		if(velocity.x != 0f) {
			float rayLength = Mathf.Abs(velocity.x) + skinWidth;

			for(int i = 0; i < horizontalRayCount; i++) {
				Vector2 rayOrigin = (directionX == -1) ? raycastOrigin.bottomLeft : raycastOrigin.bottomRight;
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);

				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

				if(hit && hit.distance != 0f) {
					if(!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);

						float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
						float pushY = -skinWidth; //if 0 -> cant jump

						passengerMovements.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
					}
				}
			}
		}

		//passenger on top of a horizontally or downward moving platform
		if(directionY == -1 || velocity.y == 0 && velocity.x != 0) {
			float rayLength = skinWidth * 2;

			for(int i = 0; i < verticalRayCount; i++) {
				Vector2 rayOrigin = raycastOrigin.topLeft + Vector2.right * (verticalRaySpacing * i);

				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

				if(hit && hit.distance != 0f) {
					if(!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);

						float pushX = velocity.x;
						float pushY = velocity.y;

						passengerMovements.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
					}
				}
			}
		}
	}

	Vector3 CalculatePlatformMovement() {

		if(Time.time < nextMoveTime) {
			return Vector3.zero;
		}

		fromWaypointIndex %= globalWaypoints.Length;
		int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
		float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
		percentBetweenWaypoint += Time.deltaTime * speed/distanceBetweenWaypoints;
		percentBetweenWaypoint = Mathf.Clamp01(percentBetweenWaypoint);
		float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoint);

		Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

		if(percentBetweenWaypoint >= 1) {
			percentBetweenWaypoint = 0f;
			fromWaypointIndex++;
			if(!isCyclic) {
				if(fromWaypointIndex >= globalWaypoints.Length - 1) {
					fromWaypointIndex = 0;
					System.Array.Reverse(globalWaypoints);
				}
			}
			nextMoveTime = Time.time + waitTime;
		}

		return newPos - transform.position;
	}

	float Ease(float x) {
		//Ease occurs for a = 1;
		float a = easeAmount + 1;
		return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
	}

	struct PassengerMovement {
		public Transform transform;
		public Vector3 velocity;
		public bool standingOnPlatform;
		public bool moveBeforePlatform;

		public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform) {
			transform = _transform;
			velocity = _velocity;
			standingOnPlatform = _standingOnPlatform;
			moveBeforePlatform = _moveBeforePlatform;
		}
	}

	void OnDrawGizmos() {
		if(localWaypoints != null) {
			Gizmos.color = Color.red;
			float size = .3f;

			for(int i = 0; i < localWaypoints.Length; i++) {
				Vector3 globalWaypointPos = (Application.isPlaying)? globalWaypoints[i] : localWaypoints[i] + transform.position;
				Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
				Gizmos.DrawLine(globalWaypointPos - Vector3.right * size, globalWaypointPos + Vector3.right * size);
			}
		}
	}
}
