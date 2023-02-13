#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LabelAttribute))]
public class LabelAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        LabelAttribute attr = attribute as LabelAttribute;
        if (attr.Name.Length > 0)
        {
            label.text = attr.Name;
        }
        EditorGUI.PropertyField(position, property, label);
    }
}
#endif