using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class PlayerController : MonoBehaviour {

	public float moveSpeed = 6f;
	public float jumpHeight = 4f;
	public float timeToJumpApex = .4f;
	public float lowJumpMultiplier = 2f;
	public float fallMultiplier = 2.5f;

	float gravity;
	float jumpVelocity;

	Vector2 move;

	CharacterMotor motor;

	void Start () {
		motor = GetComponent<CharacterMotor>();

		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

		Debug.Log("Gravity: " + gravity + " | Jump Velocity: " + jumpVelocity);
	}
	
	void Update () {
		//InputManager Data?!
		//Idea: Create Interface/Functions for specific input
		//ex. OnJumpInputDown() do jump
		//combine this with an state machine where every state
		//has for each button an function, even if its empty

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		//Stop y movement, when hitting ground or a ceiling
		if (motor.collision.below || motor.collision.above) {
			move.y = 0;
		}

		//Jump
		if(Input.GetKeyDown(KeyCode.Space) && motor.collision.below) {
			move.y = jumpVelocity;
		}

		move.x = input.x * moveSpeed;
		move.y += gravity * Time.deltaTime;
		motor.Move(move * Time.deltaTime);
	}

	//public void TakeInput(InputData inputData) {
	//	Vector2 input = new Vector2(
	//		inputData.axes[0].Value,
	//		inputData.axes[1].Value
	//	);

	//	move.x = input.x * moveSpeed;
	//	move.y += gravity * Time.deltaTime;
	//	motor.Move(move * Time.deltaTime);
	//}
}
