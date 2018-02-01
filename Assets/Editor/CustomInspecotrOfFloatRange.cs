using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(FloatRangeAttribute))]
public class FRangeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + 16;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.type != "FloatRange")
            Debug.LogWarning("Use only with FRange type");
        else
        {
            var range = attribute as FloatRangeAttribute;
            var minValue = property.FindPropertyRelative("min");
            var maxValue = property.FindPropertyRelative("max");
            var newMin = minValue.floatValue;
            var newMax = maxValue.floatValue;

            var xDivision = position.width * 0.33f;
            var yDivision = position.height * 0.5f;
            EditorGUI.LabelField(new Rect(position.x, position.y, xDivision, yDivision), label);

            // EditorGUI.LabelField(new Rect(position.x, position.y + yDivision, position.width, yDivision), range.min.ToString("0.##"));
            EditorGUI.LabelField(new Rect(position.x, position.y + yDivision, position.width, yDivision), range.min.ToString("0.##"));
            EditorGUI.LabelField(new Rect(position.x + position.width - 28f, position.y + yDivision, position.width, yDivision), range.max.ToString("0.##"));
            EditorGUI.MinMaxSlider(
                new Rect(position.x + 24f, position.y + yDivision, position.width - 48f, yDivision),
                ref newMin, ref newMax, range.min, range.max);
            
            EditorGUI.LabelField(new Rect(position.x + xDivision, position.y, xDivision, yDivision), "From: ");
            
            // seems you cant really change the formatting of the number :/
            newMin = EditorGUI.FloatField(new Rect(position.x + xDivision + 30, position.y, xDivision - 30, yDivision), newMin);
            newMin = Mathf.Clamp(newMin, range.min, newMax);
            EditorGUI.LabelField(new Rect(position.x + xDivision * 2f, position.y, xDivision, yDivision), "To: ");
            newMax = EditorGUI.FloatField(new Rect(position.x + xDivision * 2f + 24, position.y, xDivision - 24, yDivision), newMax);
            newMax = Mathf.Clamp(newMax, newMin, range.max);

            minValue.floatValue = newMin;
            maxValue.floatValue = newMax;
        }
    }
}
