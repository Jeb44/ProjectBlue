using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StringReference))]
public class StringReferenceDrawer : PropertyDrawer {
	private readonly string[] popupOptions =
		{ "Use Standard String", "Use String Reference" };

	private GUIStyle popupStyle;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		if (popupStyle == null) {
			popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
			popupStyle.imagePosition = ImagePosition.ImageOnly;
		}

		label = EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, label);

		EditorGUI.BeginChangeCheck();

		//Get properties
		SerializedProperty useStandard = property.FindPropertyRelative("UseStandard");
		SerializedProperty standardValue = property.FindPropertyRelative("StandardText");
		SerializedProperty reference = property.FindPropertyRelative("Reference");

		//Calculate rect for configuration button
		Rect buttonRect = new Rect(position);
		buttonRect.yMin += popupStyle.margin.top;
		buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
		position.xMin = buttonRect.xMax;

		//Store old indent level and set it to 0, the PrefixLabel takes care of it
		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		//Popup Button with selection specified in popupOptions
		int result = EditorGUI.Popup(buttonRect, useStandard.boolValue ? 0 : 1, popupOptions, popupStyle);

		//change variable in Property based on the result of the popup selection
		useStandard.boolValue = result == 0;

		//Draw Field
		EditorGUI.PropertyField(position, useStandard.boolValue ? standardValue : reference, GUIContent.none);

		if (EditorGUI.EndChangeCheck())
			property.serializedObject.ApplyModifiedProperties();

		//Reset indent, End property
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();
	}
}
