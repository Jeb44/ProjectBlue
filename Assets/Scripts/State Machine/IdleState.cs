using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState {

	public override BaseState update(InputData input) {
		ResetGravityOnVerticalCollision();

		//Just make sure we aren't moveing!
		move.x = 0f;

		//Walking
		if (input.axes[0].Value != 0f) {
			return new WalkState();
		}
		//Dash
		if (input.buttons[2].isButtonDown) {
			return new DashState();
		}
		//Jumping
		if (input.buttons[3].isButtonDown && references.motor.collision.below) {
			return new JumpState();
		}
		
		//Falling off a platform (ex. floor disappears)
		if (!references.motor.collision.below) {
			return new FallingState();
		}

		AddGravityToMovement();
		MoveWithMotor();
		return null;
	}
}
