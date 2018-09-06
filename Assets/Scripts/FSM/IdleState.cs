using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState {

	public override BaseState update(InputData input) {
		base.update(input);

		if (input.hasNewData) {

			//Walking
			if (input.axes[0].Value != 0f) {
				return new WalkState();
			}
			//Jumping
			if (input.buttons[3].isButtonDown && motor.collision.below) {
				return new JumpUpState();
			}
		}
		
		//Falling off a platform
		if (!motor.collision.below) {
			return new JumpDownState();
		}

		return null;
	}
}
