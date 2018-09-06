using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INPROGRESS Jump State -> add horizontal movement
//TODO test movement -> bugs and other weird stuff

public class JumpUpState : BaseState {

	public override void enter(PlayerController player) {
		base.enter(player);

		move.y = player.jumpVelocity;
		motor.Move(move * Time.deltaTime);
	}

	public override BaseState update(InputData input) {
		base.earlyUpdate();
		if (move.y < 0f) {
			return new JumpDownState();
		}

		//NOTE this if statement might be useless
		//if (motor.collision.below) {
		//	return new IdleState();
		//}
		base.lateUpdate();
		return null;
	}
}

public class JumpDownState : BaseState {

	public override BaseState update(InputData input) {
		base.update(input);
		if (motor.collision.below) {
			return new IdleState();
		}

		return null;
	}
}