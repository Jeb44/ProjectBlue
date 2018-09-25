using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Character Facing by only allowing one boolean value to be true;
/// </summary>
public struct FacingDirection{
	//NOTE add up/middle/down if needed in the future

	bool left;
	bool right;

	public bool Left{
		set {
			left = value;
			if(left == true){
				right = false;
			}
		}
		get {return left;}
	} 
	public bool Right{
		set {
			right = value;
			if(right == true){
				left = false;
			}
		}
		get {return right;}
	} 

	public FacingDirection(bool looksToRight = true){
		left = !looksToRight;
		right = looksToRight;
	}
}

/// <summary>
/// References in Character States can't be set static, because it's used for multiple entities (multiple players, enemies, npc, etc.).
/// That's why we need to share the references in a seperate struct.
/// </summary>
public struct CharacterStateReferences {
	public FacingDirection direction;
	public PlayerController player;
	public CharacterMotor motor;
}

/// <summary>
/// Saves all (for now) movement-related variables the Character FSM needs.
/// </summary>
[System.Serializable]
public struct CharacterStateVariables {
	[Header("General")]
	public float moveSpeed;

	[Header("Dash")]
	public float dashSpeed;
	public float dashTime;

	[Header("Jump")]
	public float jumpHeight;
	public float timeToJumpApex;
}