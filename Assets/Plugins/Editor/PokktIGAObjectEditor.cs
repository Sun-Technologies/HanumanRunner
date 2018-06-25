using UnityEngine;
using UnityEditor;
using System.Collections;
using Pokkt;

[CustomPropertyDrawer(typeof(PokktIGAContainer))]
public class PokktIGAObjectEditor : PropertyDrawer // TODO: rename
{
	override public void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		
		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		Rect pnameRect = new Rect(position.x, position.y, 30, position.height);
		Rect adTypeRect = new Rect(position.x, position.y, 60, position.height);
		Rect isHDRect = new Rect(position.x, position.y, 90, position.height);

		EditorGUI.PropertyField(pnameRect, property.FindPropertyRelative("PlacementName"), GUIContent.none);
		EditorGUI.PropertyField(adTypeRect, property.FindPropertyRelative("AdType"), GUIContent.none);
		EditorGUI.PropertyField(isHDRect, property.FindPropertyRelative("IsHD"), GUIContent.none);

		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}
