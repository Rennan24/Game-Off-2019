using UnityEngine;

public class DamageBehaviour : MonoBehaviour
{
    public int damageAmount;
    public LayerMask HitLayer;
    public bool DestroyOnHit;

    [Header("References:")]
    [SerializeField]
    private Collider2D colliderRef;

    private Vector2 previousPosition;

    public void FixedUpdate()
    {
        previousPosition = transform.position;

        var hitCount = Physics2DHelper.OverlapColliderCount(colliderRef, HitLayer, true);
        if (hitCount <= 0)
            return;

        for (int i = 0; i < hitCount; i++)
        {
            var health = Physics2DHelper.HitColliders[i].GetComponentInParent<HealthBehaviour>();

            if (health != null)
                Damage(health);

            if(DestroyOnHit)
                Destroy(gameObject);
        }
    }

    public void Damage(HealthBehaviour health)
    {
        Vector2 position = transform.position;
        Vector2 hitDir = position - previousPosition;

        health.Damage(damageAmount, position, hitDir);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        colliderRef = GetComponent<Collider2D>();
    }
#endif
}
