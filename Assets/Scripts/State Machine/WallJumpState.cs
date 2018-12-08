using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideState : BaseState{

	float dirX;

	public override void enter() {
		dirX = (references.motor.collision.left) ? -1f : 1f;
	}

	public override BaseState update(InputData input){
		ResetGravityOnVerticalCollision();

		//Apply small force, collider raycast go into direction of the wall
		move.x = dirX;

		//Jumping
		if (input.buttons[3].isButtonDown) {
			return new JumpState();
		}

		//Hitting the ground
		if (references.motor.collision.below) {
			return new IdleState();
		}

		//When sliding down a wall, but wall disappears
		if (!references.motor.collision.right && !references.motor.collision.left) {
			return new FallingState();
		}

		AddGravityToMovement();
		MoveWithMotor();
		return null;
    }   


}
