using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CamBox{
	public Vector2 centre;
	public Vector2 move;
	float top, bot;
	float left, right;

	public CamBox(Bounds bounds, Vector2 size){
			left = bounds.center.x - size.x / 2;
			right = bounds.center.x + size.x / 2;
			bot = bounds.min.y;
			top = bounds.min.y + size.y;

			move = Vector2.zero;
			centre = new Vector2((left + right) / 2f, (top + bot) / 2f);
	}

	///<summary>
	///Take the current bounds and adjust the CamBox accordingly
	///</summary>
	public void update(Bounds bounds){

		//Check if horizontal borders are touched by the bounds
		//If the bounds are outside the border, add it to shift variable
		float shiftX = 0f;
		if(bounds.min.x < left){
			shiftX = bounds.min.x - left;
		} else if (bounds.max.x > right){
			shiftX = bounds.max.x - right;
		}
		//Add the shift variable on the borders
		left += shiftX;
		right += shiftX;

		//Check if vertical borders are touched by the bounds
		//If the bounds are outside the border, add it to shift variable
		float shiftY = 0f;
		if (bounds.min.y < bot){
			shiftY = bounds.min.y - bot;
		} else if (bounds.max.y > top){
			shiftY = bounds.max.y - top;
		}
		//Add the shift variable on the borders
		top += shiftY;
		bot += shiftY;

		//Recalculate the centre
		centre = new Vector2((left + right) / 2, (top + bot) / 2);
		//remember the shifted variables
		move = new Vector2(shiftX, shiftY);
	}
}
