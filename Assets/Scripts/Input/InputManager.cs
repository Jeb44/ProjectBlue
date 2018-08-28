using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	[Range(0, 4)] public int axisCount;
	[Range(0, 20)] public int buttonCount;
	
	public void RefreshTracker() {
		DeviceTracker[] tracker = GetComponents<DeviceTracker>();
		if (tracker != null) {
			for(int i = 0; i < tracker.Length; i++) {
				tracker[i].Refresh();
			}
		}
	}

	public void TakeInput(InputData inputData) {
		//DebugInput(inputData);
	}

	void DebugInput(InputData inputData) {
		for (int i = 0; i < inputData.axes.Length; i++) {
			Debug.Log("Axis" + i + " - Value: " + inputData.axes[i].Value);
		}
		for (int i = 0; i < inputData.buttons.Length; i++) {
			Debug.Log(
				"Button" + i + "  - Up/Pressed/Released: "
				+ inputData.buttons[i].isButtonDown.ToString() + " / "
				+ inputData.buttons[i].isButton.ToString() + " / "
				+ inputData.buttons[i].isButtonUp.ToString()
			);
		}
	}
}
