using UnityEngine;

public class ProjectileController : GameObjectPool
{
    public Transform StartTransform;

    public int ProjectileAmount;
    public int ProjectileLifetime;
    public float InitialAngle;

    public MinMaxFloat MinMaxSpeed;
    public MinMaxFloat MinMaxAngle;

    public void SpawnSequence(int amount, float initialAngle)
    {
        var angleStep = MinMaxAngle.Delta / amount;
        var position = (Vector2) StartTransform.position;

        var angle = initialAngle + MinMaxAngle.Min;

        for (int i = 0; i < amount; i++)
        {
            SpawnProjectile(angle, position);
            angle += angleStep;
        }
    }

    public void SpawnProjectiles(float initialAngle, int amount, bool randomAngle = false)
    {
        SpawnProjectiles(transform.position, initialAngle, amount, randomAngle);
    }

    public void SpawnProjectiles(Vector3 position, float initialAngle, int amount, bool randomAngle = false)
    {
        for (int i = 0; i < amount; i++)
        {
            var angle = initialAngle + (randomAngle ? MinMaxAngle.RandomRange : 0);
            SpawnProjectile(angle, position);
        }
    }

    private void SpawnProjectile(float angle, Vector3 position)
    {
        var dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
        var dirY = Mathf.Sin(angle * Mathf.Deg2Rad);

        var moveDir = new Vector2(dirX, dirY);

        var obj = Get(position, Quaternion.Euler(0, 0, angle));
        var projectile = obj.GetComponent<Projectile>();


        projectile.Velocity = moveDir * MinMaxSpeed.RandomRange;
        projectile.Lifetime = ProjectileLifetime;
    }
}

