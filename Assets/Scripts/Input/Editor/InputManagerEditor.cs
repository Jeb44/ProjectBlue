using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputManager))]
public class InputManagerEditor : Editor {

	public override void OnInspectorGUI() {
		InputManager im = target as InputManager;

		//If something changed in the InputManager
		//then update all DeviceTracker
		EditorGUI.BeginChangeCheck();
		base.OnInspectorGUI();
		if (EditorGUI.EndChangeCheck()) {
			im.RefreshTracker();
		}
	}

	
}
