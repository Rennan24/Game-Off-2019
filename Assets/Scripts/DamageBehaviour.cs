using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBehaviour : MonoBehaviour
{
    public int damageAmount;
    public LayerMask HitLayer;
    public bool DestroyOnHit;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (HitLayer.HasLayer(other))
        {
            var health = other.GetComponent<HealthBehaviour>();
            if (health == null)
                return;

            health.Damage(damageAmount);

            if(DestroyOnHit)
                Destroy(gameObject);
        }
    }
}
