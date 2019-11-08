using UnityEngine;

public static class LayerMaskHelper
{
    public static bool HasLayer(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static bool HasLayer(this LayerMask mask, Collider2D collider)
    {
        return mask == (mask | (1 << collider.gameObject.layer));
    }

    public static bool IsLayer(this Collider2D collider, int layer)
    {
        return collider.gameObject.layer == layer;
    }
}

public static class PhysicLayers
{
    public const int Default       = 0;
    public const int TransparentFX = 1;
    public const int IgnoreRaycast = 2;
    public const int Water         = 4;
    public const int UI            = 5;
    public const int Player        = 8;
    public const int Enemy         = 9;
    public const int Projectile    = 10;
}

public static class PhysicMasks
{
    public const int Default       = 1 << PhysicLayers.Default;
    public const int TransparentFX = 1 << PhysicLayers.TransparentFX;
    public const int IgnoreRaycast = 1 << PhysicLayers.IgnoreRaycast;
    public const int Water         = 1 << PhysicLayers.Water;
    public const int UI            = 1 << PhysicLayers.UI;
    public const int Player        = 1 << PhysicLayers.Player;
    public const int Enemy         = 1 << PhysicLayers.Enemy;
    public const int Projectile    = 1 << PhysicLayers.Projectile;
}
