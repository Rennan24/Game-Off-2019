using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public struct DelayTimer
{
    public float Delay;
    private float timestamp;

    public DelayTimer(float delay)
    {
        Delay = delay;

        // Causes the time to already be ready
        timestamp = Time.time + delay;
    }

    /// <summary>
    /// Checks if the timer is ready and if so it will reset the timestamp
    /// </summary>
    /// <returns>False if the timer is not ready and True if it is ready</returns>
    public bool AutoReady()
    {
        if (Time.time < timestamp)
            return false;

        timestamp = Time.time + Delay;
        return true;
    }

    public bool IsReady => Time.time > timestamp;

    public void ResetTimestamp()
    {
        timestamp = Time.time + Delay;
    }

    public float TimeLeft()
    {
        var timeLeft = timestamp - Time.time;
        return Mathf.Max(0, timeLeft);
    }
}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(DelayTimer))]
public class DelayTimerDrawer : PropertyDrawer
{
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(rect, label, property);
        EditorGUI.PropertyField(rect, property.FindPropertyRelative("Delay"), label);
        EditorGUI.EndProperty();
    }
}
#endif
