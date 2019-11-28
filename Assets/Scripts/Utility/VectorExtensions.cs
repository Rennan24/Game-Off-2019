
using UnityEngine;

public static class Vector2Extensions
{
    public static void MoveTowards(ref this Vector2 vec, Vector2 target, float maxDistanceDelta)
    {
        vec = Vector2.MoveTowards(vec, target, maxDistanceDelta);
    }

    public static void MoveTowards(this Vector2 vec, out Vector2 outVec, Vector2 target, float maxDistanceDelta)
    {
        outVec = Vector2.MoveTowards(vec, target, maxDistanceDelta);
    }
}

public static class Vector3Extensions
{
    public static Vector3 WithX(this Vector3 vec, float newX)
        => new Vector3(newX, vec.y, vec.z);

    public static Vector3 WithY(this Vector3 vec, float newY)
        => new Vector3(vec.x, newY, vec.z);

    public static Vector3 WithZ(this Vector3 vec, float newZ)
        => new Vector3(vec.x, vec.y, newZ);
}

public static class TransformExtensions
{
    public static Vector3 GetDelta(this Transform t, Transform other)
        => other.position - t.position;

    public static Vector3 GetDelta(this Transform t, Vector3 other)
        => other - t.position;

    public static void ScaleX(this Transform t, float newX)
    {
        var scale = t.localScale;
        t.localScale = new Vector3(newX, scale.y, scale.z);
    }

    public static void ScaleY(this Transform t, float newY)
    {
        var scale = t.localScale;
        t.localScale = new Vector3(scale.z, newY, scale.z);
    }

    public static void ScaleZ(this Transform t, float newZ)
    {
        var scale = t.localScale;
        t.localScale = new Vector3(scale.x, scale.y, newZ);
    }
}
