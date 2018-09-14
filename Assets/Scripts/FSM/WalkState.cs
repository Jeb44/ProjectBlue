using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : BaseState {

	public override BaseState update(InputData input) {
		ResetGravityOnVerticalCollision();

		//Left & Right movement
		float inputX = input.axes[0].Value;
		move.x = inputX * player.moveSpeed;

		//Jumping
		if (input.buttons[3].isButtonDown && motor.collision.below) {
			return new JumpUpState();
		}

		//Falling off a platform
		if (!motor.collision.below) {
			return new JumpDownState();
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
