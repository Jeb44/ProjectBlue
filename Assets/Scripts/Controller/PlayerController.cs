using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class PlayerController : MonoBehaviour {

	public float moveSpeed = 6f;
	public float jumpHeight = 4f;
	public float timeToJumpApex = .4f;

	[HideInInspector] public float gravity;
	[HideInInspector] public float jumpVelocity;

	Vector2 move;

	[HideInInspector] public CharacterMotor motor;
	BaseState currentState;

	void Start () {
		motor = GetComponent<CharacterMotor>();

		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		Debug.Log("Gravity: " + gravity + " | Jump Velocity: " + jumpVelocity);

		currentState = new IdleState();
		currentState.enter(this);
	}

	public void TakeInput(InputData inputData) {
		BaseState tempState = currentState.update(inputData);
		if (tempState != null) {
			//Debug.Log("PlayerController switches State!");
			currentState.exit();
			currentState = tempState;
			currentState.enter(this);
		}
	}
}