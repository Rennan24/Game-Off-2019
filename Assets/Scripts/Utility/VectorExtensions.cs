
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
