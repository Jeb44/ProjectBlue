using UnityEngine;

/// <summary>
/// Struct for current Input. Fill the values and pass it
/// to the Controller.
/// </summary>
public struct InputData {
	public InputAxisData[] axes;
	public InputButtonData[] buttons;

	/// <summary>
	/// Constructor, pass in current number of axes and buttons.
	/// </summary>
	/// <param name="axesCount">Current number of axes.</param>
	/// <param name="buttonsCount">Current number of buttons.</param>
	public InputData(int axesCount, int buttonsCount) {
		axes = new InputAxisData[axesCount];
		buttons = new InputButtonData[buttonsCount];
	}

	/// <summary> Set all input data to it's standard value. (float = 0f, bool = false) </summary>
	public void Reset() {
		for(int i = 0; i < axes.Length; i++) {
			axes[i].Reset();
		}

		for(int i = 0; i < buttons.Length; i++) {
			buttons[i].Reset();
		}
	}
}

/// <summary>
/// Define mapped keys for InputAxisData at the start of the game,
/// while playing and in the Inspector.
/// </summary>
[System.Serializable]
public struct InputAxisKeys {
	//InputStructs InputAxisKeys -> Custom Inspector
	//InputStructs InputAxisKeys -> Add 'advanced' features (description, alternative buttons, etc.)

	/// <summary> Representive name. Do not use to compare values. </summary>
	public string name;
	/// <summary> Represents changing the value to 1 </summary>
	public KeyCode positive;
	/// <summary> Represents changing the value to -1 </summary>
	public KeyCode negative;
	//public KeyCode altnativePositive;
	//public KeyCode altnativeNegative;
	//gravity, sensitivity, etc.
}

/// <summary> Saves values from axis inputs. </summary>
public struct InputAxisData {
	/// <summary> Axis value between -1 (negative), 0 (neutral) and 1 (postive) </summary>
	private float value;
	public float Value {
		get { return value; }
		set { this.value = Mathf.Clamp(value, -1f, 1f);	}
	}

	/// <summary> Set all input data to it's standard value. (float = 0f) </summary>
	public void Reset() {
		Value = 0f;
	}
}

/// <summary>
/// Define mapped keys for InputAxisData at the start of the game,
/// while playing and the Inspector
/// </summary>
[System.Serializable]
public struct InputButtonKeys {
	//InputStructs InputButtonKeys -> Custom Inspector
	//InputStructs InputButtonKeys -> Add 'advanced' features (description, alternative buttons, etc.)

	/// <summary> Representive name. Do not use to compare values. </summary>
	public string name;
	/// <summary> Represents changing the value to true </summary>
	public KeyCode button;
}

/// <summary> Saves values from button inputs. </summary>
public struct InputButtonData {
	/// <summary> Button is pressed. </summary>
	public bool isButtonDown;
	/// <summary> Button is on hold. </summary>
	public bool isButton;
	/// <summary> Button is realeased. </summary>
	public bool isButtonUp;

	/// <summary> Set all input data to it's standard value. (bool = false) </summary>
	public void Reset() {
		isButtonDown = false;
		isButton = false;
		isButtonUp = false;
	}
}
