using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState {

	protected PlayerController player;
	protected CharacterMotor motor;

	protected Vector2 move;

	public virtual void enter(GameObject character) {
		Debug.Log("Enter: " + this.ToString());
		player = character.GetComponent<PlayerController>();
		motor = character.GetComponent<CharacterMotor>();
	}
	public virtual void enter(PlayerController player) {
		Debug.Log("Enter: " + this.ToString());
		this.player = player;
		motor = player.motor;
	}

	public virtual BaseState update(InputData input) {
		earlyUpdate();
		lateUpdate();	
		return null;
	}

	protected virtual void earlyUpdate() {
		if (motor.collision.above || motor.collision.below) {
			move.y = 0f;
		}
	}
	protected virtual void lateUpdate() {
		move.y += player.gravity * Time.deltaTime;
		motor.Move(move * Time.deltaTime);
	}

	public virtual void exit() {
		//Debug.Log("Exit: " + this.ToString());
	}
}
