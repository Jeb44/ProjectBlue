using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public abstract class BaseState {

	protected CharacterStateReferences references;
	protected CharacterStateVariables variables;

	protected Vector2 move;

	/* FINITES STATE MACHINE - Applied for Character Movement
	 * The way a character is currently moving can be tought as multiple states.
	 * For ex. no movement -> idle, going left/right -> walk, vertical movement -> jump/falling...
	 * 
	 * A SM works typically like this: enter a state => update your movement => exit the state => enter a state => ... 
	 * Between exit and enter occurs a transition, called by updateing the movement.
	 * 
	 * Additionally we use seperate structs to save our Refereces (StateReferences) and (for now) movement-related variables (CharacterStateVariables)
	 * In our code this typically looks like this:
	 * 
	 * start:
	 *		create start State						||		currentState = new IdleState();
	 *		set references							||		currentState.SetReferences(this, motor, new FacingDirection(true));
	 *		set variables							||		currentState.SetVariables(variables);
	 *		enter the state							||		currentState.enter();
	 * 
	 * updating:
	 *		update variables (ex. slow effects)		||		currentState.SetVariables(variables);
	 *		give input to state						||		BaseState tempState = currentState.update(inputData);
	 *		if state changes, go trough the cycle	||		if (tempState != null) {
	 *		exit the current state					||			currentState.exit();
	 *		copy references							||			tempState.SetReferences(currentState.GetReferences());
	 *		copy variables							||			tempState.SetVariables(currentState.GetVariables());
	 *		set current state to new state			||			currentState = tempState;
	 *		enter the new state						||			currentState.enter(); }
	 */

	#region Reference Functions
	public void SetReferences(PlayerController player, CharacterMotor motor, FacingDirection direction) {
		references.player = player;
		references.motor = motor;
		references.direction = direction;
	}
	public void SetReferences(CharacterStateReferences references) {
		this.references = references;
	}
	public CharacterStateReferences GetReferences() {
		return references;
	}
	#endregion

	#region Variable Functions
	public void SetVariables(CharacterStateVariables variables) {
		this.variables = variables;
	}
	public CharacterStateVariables GetVariables() {
		return variables;
	}
	#endregion

	#region Virtual State Functions -> Override for each state specific purpose!
	public virtual void enter() {}

	public virtual BaseState update(InputData input) {
		//ResetGravityOnHorizontalCollision();
		//AddGravityToMovement();
		//MoveWithMotor();
		return null;
	}

	public virtual void exit() {}
	#endregion

	#region Virtual Movement Functions
	/// <summary>
	/// Set vertical movement to 0f, when hitting a ceiling or the ground
	/// </summary>
	protected virtual void ResetGravityOnVerticalCollision() {
		if (references.motor.collision.above || references.motor.collision.below) {
			move.y = 0f;
		}
	}

	//TODO get gravity & jumpVelocity from the physics controller
	protected virtual void AddGravityToMovement() {
		float gravity = -(2 * variables.jumpHeight) / Mathf.Pow(variables.timeToJumpApex, 2);
		move.y += gravity * Time.deltaTime;
	}

	/// <summary>
	/// Set FacingDirection, move with the accumulated value
	/// </summary>
	protected virtual void MoveWithMotor() {
		//Check in which direction we are walking, if not moving, stay with the current direction
		if(move.x > 0f){
			references.direction.Right = true;
		} else if (move.x < 0f){
			references.direction.Left = true;
		}

		//Move with accumulated move amount
		references.motor.Move(move * Time.deltaTime);
	}
	#endregion

	#region Debug
	public virtual string stateName() {
		return this.ToString().Replace("State", "");
	}

	public virtual string directionName() {

		if (references.direction.Left) {
			return "<-";
		} else if (references.direction.Right) {
			return "->";
		}

		return "<>";
	}

	public virtual string moveName() {
		return move.ToString();
	}
	#endregion
}