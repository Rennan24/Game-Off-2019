using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public struct MinMaxFloat
{
    public float Min, Max;

    public MinMaxFloat(float min, float max)
    {
        Min = min;
        Max = max;
    }

    public float Delta => Max - Min;

    public float RandomRange => Random.Range(Min, Max);

    public float Clamp(float value) => Mathf.Clamp(value, Min, Max);

}

public class MinMaxSliderAttribute : PropertyAttribute
{

    public readonly float Min;
    public readonly float Max;

    public MinMaxSliderAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MinMaxFloat))]
public class MinMaxDrawer : PropertyDrawer
{
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        if (property.serializedObject.isEditingMultipleObjects) return;

        var minProperty = property.FindPropertyRelative("Min");
        var maxProperty = property.FindPropertyRelative("Max");

        label = EditorGUI.BeginProperty(rect, label, property);
        rect = EditorGUI.PrefixLabel(rect, GUIUtility.GetControlID(FocusType.Passive), label);

        var halfWidth = rect.width * 0.5f;
        float tempLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 28f;
        var start = new Rect(rect.x, rect.y, halfWidth - 10, rect.height);
        var end = new Rect(rect.x + halfWidth, rect.y, halfWidth, rect.height);
        EditorGUI.PropertyField(start, minProperty, new GUIContent("Min:"));
        EditorGUI.PropertyField(end, maxProperty, new GUIContent("Max:"));
        EditorGUIUtility.labelWidth = tempLabelWidth;
        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
public class MinMaxSliderDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.serializedObject.isEditingMultipleObjects) return 0f;
        return base.GetPropertyHeight(property, label) + 16f;
    }

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        if (property.serializedObject.isEditingMultipleObjects) return;

        var minProperty = property.FindPropertyRelative("Min");
        var maxProperty = property.FindPropertyRelative("Max");
        var minmax = attribute as MinMaxSliderAttribute ?? new MinMaxSliderAttribute(0, 1);
        rect.height -= 16f;

        label = EditorGUI.BeginProperty(rect, label, property);
        rect = EditorGUI.PrefixLabel(rect, GUIUtility.GetControlID(FocusType.Passive), label);
        var min = minProperty.floatValue;
        var max = maxProperty.floatValue;

        var left = new Rect(rect.x, rect.y, rect.width / 2 - 11f, rect.height);
        var right = new Rect(rect.x + rect.width - left.width, rect.y, left.width, rect.height);
        var mid = new Rect(left.xMax, rect.y, 22, rect.height);
        min = Mathf.Clamp(EditorGUI.FloatField(left, min), minmax.Min, max);
        EditorGUI.LabelField(mid, " to ");
        max = Mathf.Clamp(EditorGUI.FloatField(right, max), min, minmax.Max);

        rect.y += 16f;
        EditorGUI.MinMaxSlider(rect, GUIContent.none, ref min, ref max, minmax.Min, minmax.Max);

        minProperty.floatValue = min;
        maxProperty.floatValue = max;
        EditorGUI.EndProperty();
    }
}
#endif
