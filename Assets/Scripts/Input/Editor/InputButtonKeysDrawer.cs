using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(InputButtonKeys))]
public class InputButtonKeysDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);

		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		Rect nameField = new Rect(position.x, position.y, 70f, position.height);
		Rect keycodeField = new Rect(position.x + 75f, position.y, 50f, position.height);
		//Rect negLabel = new Rect(position.x + 75f, position.y, 15f, position.height);

		
		EditorGUI.PropertyField(nameField, property.FindPropertyRelative("name"), GUIContent.none);
		EditorGUI.PropertyField(keycodeField, property.FindPropertyRelative("button"), GUIContent.none);

		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}
