using UnityEditor;
using UnityEngine;

public class MaxLengthAttribute : PropertyAttribute
{
    public int maxLenght;

    public MaxLengthAttribute(int maxLength)
    {
        this.maxLenght = maxLength;
    }
}

[CustomPropertyDrawer(typeof(MaxLengthAttribute))]
public class MaxLengthDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var maxLengthAttribute = (MaxLengthAttribute)attribute;

        if (property.propertyType == SerializedPropertyType.String)
        {
            property.stringValue = EditorGUI.TextField(position, label, property.stringValue);

            if (property.stringValue.Length > maxLengthAttribute.maxLenght)
            {
                property.stringValue = property.stringValue.Substring(0, maxLengthAttribute.maxLenght);
            }
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use MaxLength with string.");
        }
    }
}
