using UnityEngine;

//TODO create DeviceManager: track available devices, store current device, notify on device changes

public abstract class DeviceTracker : MonoBehaviour {

	public InputAxisKeys[] axisKeys;
	public InputButtonKeys[] buttonKeys;

	protected InputManager im;
	protected InputData inputData;
	protected bool hasNewData;

	protected virtual void Awake() {
		hasNewData = false;
	}

	protected virtual void Start() {
		im = GetComponent<InputManager>();
		inputData = new InputData(im.axisCount, im.buttonCount);
	}

	protected virtual void Reset() {
		im = GetComponent<InputManager>();
		axisKeys = new InputAxisKeys[im.axisCount];
		buttonKeys = new InputButtonKeys[im.buttonCount];
	}

	/// <summary>
	/// Update the axisKeys & buttonsKeys values to represent the InputManager correctly.
	/// </summary>
	public virtual void Refresh() {
		im = GetComponent<InputManager>();

		InputAxisKeys[] newAxisKeys = new InputAxisKeys[im.axisCount];
		InputButtonKeys[] newButtonKeys = new InputButtonKeys[im.buttonCount];

		//Put already existing Key Information into newAxisKeys/newButtonKeys
		if (axisKeys != null) {
			int min = Mathf.Min(newAxisKeys.Length, axisKeys.Length);
			for (int i = 0; i < min; i++) {
				newAxisKeys[i] = axisKeys[i];
			}
		}
		axisKeys = newAxisKeys;

		if (buttonKeys != null) {
			int min = Mathf.Min(newButtonKeys.Length, buttonKeys.Length);
			for (int i = 0; i < min; i++) {
				newButtonKeys[i] = buttonKeys[i];
			}
		}
		buttonKeys = newButtonKeys;
	}

	protected virtual void Update() {
		UpdateAxis();
		UpdateButton();
		SendData();
	}

	/// <summary> Update all Axis Inputs. Has to be overwritten for specific Trackers. </summary>
	protected virtual void UpdateAxis() {

	}

	/// <summary> Update all Buttons Inputs. Can be overwritten for specific Trackers. </summary>
	protected virtual void UpdateButton() {
		for (int i = 0; i < buttonKeys.Length; i++) {
			if (Input.GetKeyDown(buttonKeys[i].button)) {
				inputData.buttons[i].isButtonDown = true;
				hasNewData = true;
			}
			if (Input.GetKey(buttonKeys[i].button)) {
				inputData.buttons[i].isButton = true;
				hasNewData = true;
			}
			if (Input.GetKeyUp(buttonKeys[i].button)) {
				inputData.buttons[i].isButtonUp = true;
				hasNewData = true;
			}
		}
	}

	/// <summary>
	/// When new data is there, send it to the InputManager.
	/// </summary>
	protected void SendData() {
		if (hasNewData) {
			im.TakeInput(inputData, this);
			inputData.Reset();
			hasNewData = false;
		}
	}

	
}
