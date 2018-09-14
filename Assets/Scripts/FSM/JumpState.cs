using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BUG Jump State -> sometimes movement is looked mid-air
//TODO Jump State -> add gravity modifier
//TODO test movement -> bugs and other weird stuff

public class JumpUpState : BaseState {

	public override void enter(PlayerController player) {
		base.enter(player);

		move.y = player.jumpVelocity;
		motor.Move(move * Time.deltaTime);
	}

	public override BaseState update(InputData input) {
		ResetGravityOnVerticalCollision();

		float inputX = input.axes[0].Value;
		move.x = inputX * player.moveSpeed;

		if (move.y <= 0f) {
			return new JumpDownState();
		}

		AddGravityToMovement();
		MoveWithMotor();
		return null;
	}
}

public class JumpDownState : BaseState {

	public override BaseState update(InputData input) {
		ResetGravityOnVerticalCollision();

		float inputX = input.axes[0].Value;
		move.x = inputX * player.moveSpeed;

		if (motor.collision.below) {
			return new IdleState();
		}

		AddGravityToMovement();
		MoveWithMotor();
		return null;
	}
}