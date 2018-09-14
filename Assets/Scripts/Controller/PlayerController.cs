using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterMotor))]
public class PlayerController : MonoBehaviour {

	public float moveSpeed = 6f;
	public float jumpHeight = 4f;
	public float timeToJumpApex = .4f;

	public TextMeshProUGUI stateText;
	public TextMeshProUGUI collisionText;
	Dictionary<string, bool> debugCollisions;

	[HideInInspector] public float gravity;
	[HideInInspector] public float jumpVelocity;

	[HideInInspector] public CharacterMotor motor;
	BaseState currentState;

	void Start () {
		motor = GetComponent<CharacterMotor>();

		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		//Debug.Log("Gravity: " + gravity + " | Jump Velocity: " + jumpVelocity);

		currentState = new IdleState();
		currentState.enter(this);

		debugCollisions = new Dictionary<string, bool> {
			{ "above", false },
			{ "below", false },
			{ "right", false },
			{ "left", false }
		};
	}

	public void TakeInput(InputData inputData) {
		BaseState tempState = currentState.update(inputData);
		if (tempState != null) {
			//Debug.Log("PlayerController switches State!");
			currentState.exit();
			currentState = tempState;
			currentState.enter(this);
			stateText.text = currentState.stateName();
		}
		collisionText.text = DebugCollision();
	}

	private string DebugCollision() {
		//Reset Collision Dictionary
		debugCollisions["above"] = false;
		debugCollisions["below"] = false;
		debugCollisions["right"] = false;
		debugCollisions["left"] = false;

		//Set Collision Dictionary
		if (motor.collision.above)
			//if(debugCollisions.ContainsKey("above"))
				debugCollisions["above"] = true;
		if (motor.collision.below)
			//if(debugCollisions.ContainsKey("below"))
				debugCollisions["below"] = true;
		if (motor.collision.right)
			//if(debugCollisions.ContainsKey("right"))
				debugCollisions["right"] = true;
		if (motor.collision.left)
			//if(debugCollisions.ContainsKey("left"))
				debugCollisions["left"] = true;

		//Write collision string
		string text = "";
		foreach (KeyValuePair<string, bool> collisionPair in debugCollisions) {
			text += collisionPair.Key + ": " + collisionPair.Value.ToString();
			//collisionPair.ToString()
			text += System.Environment.NewLine;
		}
		return text;
	}
}