using UnityEngine;
using UnityEngine.Events;

public class HealthBehaviour : MonoBehaviour
{
    public int MaxHealth;
    public DelayTimer DamageDelay;

    public delegate void OnDamaged(Vector3 hitPos, Vector2 hitDir, int hitAmount, int curHealth);
    public delegate void OnHealed(int healAmount, int curHealth);
    public delegate void OnKilled(Vector3 hitPos, Vector2 hitDir);

    public event OnDamaged Damaged;
    public event OnHealed Healed;
    public event OnKilled Killed;

    [HideInInspector]
    public bool IsKilled = false;

    public int CurHealth { get; private set; }

    private void Awake()
    {
        CurHealth = MaxHealth;
    }

    public void Damage(int amount, Vector3 hitPos, Vector2 hitDir)
    {
        if (!DamageDelay.AutoReady())
            return;

        CurHealth = Mathf.Max(0, CurHealth - amount);

        Damaged?.Invoke(hitPos, hitDir, amount, CurHealth);

        if (CurHealth <= 0)
        {
            IsKilled = true;
            Killed?.Invoke(hitPos, hitDir);
        }
    }

    public void Heal(int amount)
    {
        CurHealth = Mathf.Min(MaxHealth, CurHealth + amount);
        Healed?.Invoke(amount, CurHealth);
    }
}

[System.Serializable]
public class DamagedEvent : UnityEvent<Vector3, Vector2, int, int> { }

[System.Serializable]
public class DeathEvent : UnityEvent<Vector3, Vector2> { }

[System.Serializable]
public class HealedEvent : UnityEvent<int, int> { }
