using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE i should not be able to move my dir while moving

public class DashState : BaseState {

	float curDashTime;
	float maxDashTime;

	float dirX;

	public override void enter() {
		//TODO change character colliders to be flatter or sumthin owo

		//Setup timer
		curDashTime = 0f;
		maxDashTime = variables.dashTime;

		//Turn current direction into math value
		dirX = (references.direction.Left)? -1f : 1f;
	}

	public override BaseState update(InputData input) {
		ResetGravityOnVerticalCollision();

		//Dash 
		move.x = variables.dashSpeed * dirX;

		//Switch to idle after defined time passed
		curDashTime += Time.deltaTime;
		if (curDashTime >= maxDashTime) {
			return new IdleState();
		}

		//In case of falling off a platform, the character continues the dash the usual way
		//and is pulled down through the gravity
		AddGravityToMovement();
		MoveWithMotor();
		return null;
	}
}
