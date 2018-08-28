using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(KeyboardTracker))]
public class KeyboardTrackerEditor : Editor {

	public override void OnInspectorGUI() {
		//base.OnInspectorGUI();

		KeyboardTracker tracker = target as KeyboardTracker;

		EditorGUILayout.LabelField("Axes", EditorStyles.boldLabel);
		if (tracker.axisKeys.Length == 0) {
			EditorGUILayout.HelpBox("No axes defined in InputManager.", MessageType.Info);
		} else {
			//draw axisKeys array, which is a SerialitedProperty (see AxisKeyDrawer)
			SerializedProperty prop = serializedObject.FindProperty("axisKeys");
			for (int i = 0; i < tracker.axisKeys.Length; i++) {
				EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i), new GUIContent("Axis " + i));
			}
		}

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel);
		if (tracker.buttonKeys.Length == 0) {
			EditorGUILayout.HelpBox("No axes defined in InputManager.", MessageType.Info);
		} else {
			SerializedProperty prop = serializedObject.FindProperty("buttonKeys");
			for (int i = 0; i < tracker.buttonKeys.Length; i++) {
				//tracker.buttonKeys[i] = (KeyCode)EditorGUILayout.EnumPopup("Button " + i, tracker.buttonKeys[i]);
				EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i), new GUIContent("Button " + i));
			}
		}

		serializedObject.ApplyModifiedProperties();
		serializedObject.Update();
	}
}
