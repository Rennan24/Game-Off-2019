using UnityEngine;

public static class Physics2DHelper
{
    public static RaycastHit2D[] HitBuffer { get; } = new RaycastHit2D[4];
    public static Collider2D[] HitColliders { get; } = new Collider2D[4];
    private static ContactFilter2D filter2D = new ContactFilter2D {
        useLayerMask = true,
        useTriggers = false,
    };

    public static bool Cast(Collider2D collider, Vector2 dir, LayerMask mask, float dist)
    {
        filter2D.layerMask = mask;
        return collider.Cast(dir, filter2D, HitBuffer, dist) > 0;
    }

    public static bool OverlapCollider(Collider2D collider, LayerMask mask, bool useTriggers = false)
    {
        filter2D.layerMask = mask;
        filter2D.useTriggers = useTriggers;
        return collider.OverlapCollider(filter2D, HitColliders) > 0;
    }

    public static Collider2D FirstOverlapCollider(Collider2D collider, LayerMask mask, bool useTriggers = false)
    {
        filter2D.layerMask = mask;
        filter2D.useTriggers = useTriggers;
        var hitCount = collider.OverlapCollider(filter2D, HitColliders);

        return hitCount >= 1 ? HitColliders[0] : null;
    }

    public static int OverlapColliderCount(Collider2D collider, LayerMask mask, bool useTriggers = false)
    {
        filter2D.layerMask = mask;
        filter2D.useTriggers = useTriggers;
        return collider.OverlapCollider(filter2D, HitColliders);
    }
}
