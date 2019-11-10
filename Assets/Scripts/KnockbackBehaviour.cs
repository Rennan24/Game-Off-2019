using System.Collections;
using UnityEngine;

public class KnockbackBehaviour : MonoBehaviour
{
    public float DamageKnockbackSpeed = 15;
    public float DeathKnockbackSpeed = 5;
    public float DeathHoldTime = 1;
    public ParticleSystem DeathParticles;

    [SerializeField]
    private HealthBehaviour healthRef;

    [SerializeField]
    private TopdownController controllerRef;

    private void Start()
    {
        healthRef.Damaged += OnDamaged;
        healthRef.Killed += OnKilled;
    }

    private void OnDamaged(Vector3 hitPos, Vector2 hitDir, int hitAmount, int curHealth)
    {
        Knockback(hitDir * DamageKnockbackSpeed);
    }

    private void OnKilled(Vector3 hitPos, Vector2 hitDir)
    {
        StartCoroutine(KillKnockbackRoutine(hitDir));
    }

    private IEnumerator KillKnockbackRoutine(Vector2 hitDir)
    {
        for (float t = 0; t < 1f; t += Time.deltaTime / DeathHoldTime)
        {
            Knockback(hitDir * Mathf.Lerp(DeathKnockbackSpeed, 0, t));
            yield return null;
        }

        Instantiate(DeathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Knockback(Vector2 dir)
    {
        controllerRef.Move(dir);
    }
}
