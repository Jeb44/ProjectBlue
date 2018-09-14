using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState {

	protected PlayerController player;
	protected CharacterMotor motor;

	protected Vector2 move;

	public virtual void enter(GameObject character) {
		player = character.GetComponent<PlayerController>();
		motor = character.GetComponent<CharacterMotor>();
	}
	public virtual void enter(PlayerController player) {
		this.player = player;
		motor = player.motor;
	}

	public virtual BaseState update(InputData input) {
		//ResetGravityOnHorizontalCollision();
		//AddGravityToMovement();
		//MoveWithMotor();
		return null;
	}

	public virtual void exit() {}

	public virtual string stateName() {
		return this.ToString().Replace("State", "");
	}

	protected virtual void ResetGravityOnVerticalCollision() {
		if (motor.collision.above || motor.collision.below) {
			move.y = 0f;
		}
	}

	//TODO acces gravity from outside this class
	protected virtual void AddGravityToMovement() {
		move.y += player.gravity * Time.deltaTime;
	}

	protected virtual void MoveWithMotor() {
		motor.Move(move * Time.deltaTime);
	}
}
