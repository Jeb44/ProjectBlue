using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(InputAxisKeys))]
public class InputAxisKeysDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);

		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		Rect nameField = new Rect(position.x, position.y, 70f, position.height);
		Rect posLabel = new Rect(position.x + 75f, position.y, 15f, position.height);
		Rect posField = new Rect(position.x + 95f, position.y, 50f, position.height);
		Rect negLabel = new Rect(position.x + 150f, position.y, 15f, position.height);
		Rect negField = new Rect(position.x + 170f, position.y, 50f, position.height);

		//set labels
		GUIContent posGUI = new GUIContent("+");
		GUIContent negGUI = new GUIContent("-");

		//draw fields
		EditorGUI.PropertyField(nameField, property.FindPropertyRelative("name"), GUIContent.none);
		EditorGUI.LabelField(posLabel, posGUI);
		EditorGUI.PropertyField(posField, property.FindPropertyRelative("positive"), GUIContent.none);
		EditorGUI.LabelField(negLabel, negGUI);
		EditorGUI.PropertyField(negField, property.FindPropertyRelative("negative"), GUIContent.none);

		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}
