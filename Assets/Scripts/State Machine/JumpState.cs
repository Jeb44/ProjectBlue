using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Jump State -> add gravity modifier

public class JumpState : BaseState {

	float gravity;
	float jumpVelocity;

	public override void enter() {
		
		//TODO get gravity & jumpVelocity from the physics controller
		gravity = -(2 * variables.jumpHeight) / Mathf.Pow(variables.timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * variables.timeToJumpApex;

		move.y = jumpVelocity;
		MoveWithMotor();
	}

	public override BaseState update(InputData input) {
		ResetGravityOnVerticalCollision();

		//Evaluate left & right input
		float inputX = input.axes[0].Value;
		move.x = inputX * variables.moveSpeed;

		if (move.y <= 0f) {
			return new FallingState();
		}

		if((references.motor.collision.left || references.motor.collision.right) && !references.motor.collision.below){
			return new WallSlideState();
		}

		AddGravityToMovement();
		MoveWithMotor();
		return null;
	}
}

public class FallingState : BaseState {

	public override BaseState update(InputData input) {
		ResetGravityOnVerticalCollision();

		//Evaluate left & right input
		float inputX = input.axes[0].Value;
		move.x = inputX * variables.moveSpeed;

		//Go back to idle state when hitting the ground
		if (references.motor.collision.below) {
			return new IdleState();
		}

		if((references.motor.collision.left || references.motor.collision.right) && !references.motor.collision.below){
			Debug.Log("test!");
			return new WallSlideState();
		}

		AddGravityToMovement();
		MoveWithMotor();
		return null;
	}
}