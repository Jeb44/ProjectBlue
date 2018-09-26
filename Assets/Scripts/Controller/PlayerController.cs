using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterMotor))]
public class PlayerController : MonoBehaviour {

	//State Variable
	public CharacterStateVariables variables;
	[HideInInspector] public float gravity;
	[HideInInspector] public float jumpVelocity;

	//References
	[HideInInspector] public CharacterMotor motor;

	//State Machine
	BaseState currentState;

	[Header("Debug")]
	public TextMeshProUGUI stateText;
	public TextMeshProUGUI collisionText;
	Dictionary<string, bool> debugCollisions;
	public TextMeshProUGUI directionText;
	public TextMeshProUGUI moveText;

	void Start () {
		//TODO get gravity & jumpVelocity from the physics controller
		gravity = -(2 * variables.jumpHeight) / Mathf.Pow(variables.timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * variables.timeToJumpApex;

		//References
		motor = GetComponent<CharacterMotor>();

		//State Machine setup
		currentState = new IdleState();
		currentState.SetReferences(this, motor, new FacingDirection(true));
		currentState.SetVariables(variables);
		currentState.enter();

		//Debug
		debugCollisions = new Dictionary<string, bool> {
			{ "above", false },
			{ "below", false },
			{ "right", false },
			{ "left", false }
		};
	}

	/// <summary>
	/// Main function to let the character move. Handels character states through provided input.
	/// </summary>
	/// <param name="inputData">Provides information for state machine to handle the state.</param>
	public void TakeInput(InputData inputData) {

		//See Explanation on Character FSM on BaseState.cs
		currentState.SetVariables(variables); //update variables in case the values change in-game (ex. slow effect)
		BaseState tempState = currentState.update(inputData);
		if (tempState != null) {
			currentState.exit();

			tempState.SetReferences(currentState.GetReferences());
			tempState.SetVariables(currentState.GetVariables());

			currentState = tempState;
			currentState.enter();
		}

		//Debug
		stateText.text = currentState.stateName();
		collisionText.text = DebugCollision();
		directionText.text = currentState.directionName();
		moveText.text = currentState.moveName();
	}

	private string DebugCollision() {
		//Reset Collision Dictionary
		debugCollisions["above"] = false;
		debugCollisions["below"] = false;
		debugCollisions["right"] = false;
		debugCollisions["left"] = false;

		//Set Collision Dictionary
		if (motor.collision.above)
			debugCollisions["above"] = true;
		if (motor.collision.below)
			debugCollisions["below"] = true;
		if (motor.collision.right)
			debugCollisions["right"] = true;
		if (motor.collision.left)
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