
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class WHandles
{
    public static void DrawWireDisk(Vector3 center, Vector3 normal, float radius, Color color)
    {
        var oldColor = Handles.color;
        Handles.color = color;
        Handles.DrawWireDisc(center, normal, radius);
        Handles.color = oldColor;
    }

    public static void DrawWireDisk(Vector3 center, float radius, Color color)
    {
        var oldColor = Handles.color;
        Handles.color = color;
        Handles.DrawWireDisc(center, Vector3.forward, radius);
        Handles.color = oldColor;
    }
}
#endif
