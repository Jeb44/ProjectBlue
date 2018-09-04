using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class KeyboardTracker : DeviceTracker {

	protected override void UpdateAxis() {
		//base.UpdateAxis();

		for (int i = 0; i < axisKeys.Length; i++) {
			float value = 0f;

			//Evaluate current value
			//annotation: if positive & negative is pressed the value is 0
			if (Input.GetKey(axisKeys[i].positive)) {
				value += 1f;
				hasNewData = true;
			}
			if (Input.GetKey(axisKeys[i].negative)) {
				value -= 1f;
				hasNewData = true;
			}
			
			inputData.axes[i].Value = value;
		}
	}

	
}
