using UnityEditor;
using UnityEngine;

/// <summary> 
///特性：Inspector窗口变量仅显示
/// </summary>
public class ShowOnlyAttribute : PropertyAttribute {

}
[CustomPropertyDrawer(typeof(ShowOnlyAttribute))]//需要using UnityEditor;
public class ShowOnlyDrawer : PropertyDrawer {
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		return EditorGUI.GetPropertyHeight(property, label, true);
	}
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		GUI.enabled = false;
		EditorGUI.PropertyField(position, property, label, true);
		GUI.enabled = true;
	}
}
