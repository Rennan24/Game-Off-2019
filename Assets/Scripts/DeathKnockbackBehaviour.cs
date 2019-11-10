using UnityEngine;

public class DeathKnockbackBehaviour : MonoBehaviour
{
    public float KnockbackSpeed;

    [SerializeField]
    private HealthBehaviour healthRef;

    [SerializeField]
    private TopdownController controllerRef;

    private void Start()
    {
        healthRef.Killed += OnKilled;
    }

    private void OnKilled(Vector3 hitPos, Vector2 hitDir)
    {
        Debug.Log("Death Knockback");
    }
}
