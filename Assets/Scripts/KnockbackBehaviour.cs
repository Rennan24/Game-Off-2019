using System;
using System.Collections;
using UnityEngine;

public class KnockbackBehaviour : MonoBehaviour
{
    public float DamageKnockbackSpeed = 15;
    public float DeathKnockbackSpeed = 5;
    public float DeathHoldTime = 1;
    public int Score = 10;
    public ParticleSystem DeathParticles;

    public LootTable Loot;

    [SerializeField]
    private Collider2D[] colliders;

    [SerializeField]
    private HealthBehaviour healthRef;

    [SerializeField]
    private MovementController controllerRef;

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
        foreach (var coll in colliders)
            coll.enabled = false;

        for (float t = 0; t < 1f; t += Time.deltaTime / DeathHoldTime)
        {
            Knockback(hitDir * Mathf.Lerp(DeathKnockbackSpeed, 0, t));
            yield return null;
        }

        GameManager.AddScore(Score);
        DamageTextManager.Inst.SpawnText(transform.position, $"+{Score:N0}", 1f);

        var drop = Loot.GetDrop();
        if (drop != null)
        {
            Instantiate(drop, transform.position, Quaternion.identity);
        }


        Instantiate(DeathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Knockback(Vector2 dir)
    {
        controllerRef.Move(dir);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        healthRef = GetComponent<HealthBehaviour>();
        controllerRef = GetComponent<MovementController>();
    }
#endif
}
