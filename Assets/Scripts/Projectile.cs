using UnityEngine;

public class Projectile: MonoBehaviour, IPoolable
{
    public Vector3 Velocity;
    public float Lifetime;

    public void FixedUpdate()
    {
        var dt = Time.fixedDeltaTime;
        transform.position += Velocity * dt;
        Lifetime -= dt;

        if(Lifetime < 0)
            Pool.ReturnToPool(gameObject);
    }

    public GameObjectPool Pool { get; set; }
}
