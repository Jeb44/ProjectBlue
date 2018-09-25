using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : BaseState {

	public override BaseState update(InputData input) {
		ResetGravityOnVerticalCollision();

		//Evaluate left & right input
		float inputX = input.axes[0].Value;
		move.x = inputX * variables.moveSpeed;

		//Jumping
		if (input.buttons[3].isButtonDown && references.motor.collision.below) {
			return new JumpState();
		}

		//Dash
		if (input.buttons[2].isButtonDown) {
			return new DashState();
		}

		//Falling off a platform
		if (!references.motor.collision.below) {
			return new FallingState();
		}

		//When not moving go to idle state
		if (inputX == 0f) {
			return new IdleState();
		}

		AddGravityToMovement();
		MoveWithMotor();
		return null;
	}
}
