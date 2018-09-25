using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Jump State -> add gravity modifier

public class JumpState : BaseState {

	public override void enter() {
		move.y = references.player.jumpVelocity;
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

		AddGravityToMovement();
		MoveWithMotor();
		return null;
	}
}