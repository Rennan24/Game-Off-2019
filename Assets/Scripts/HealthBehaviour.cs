using UnityEngine;
using UnityEngine.Events;

public class HealthBehaviour : MonoBehaviour
{
    public int MaxHealth;

    public bool MaxHealthOnStart;

    public delegate void OnDamaged(Vector3 hitPos, Vector2 hitDir, int hitAmount, int curHealth);
    public delegate void OnHealed(int healAmount, int curHealth);
    public delegate void OnKilled(Vector3 hitPos, Vector2 hitDir);

    public event OnDamaged Damaged;
    public event OnHealed Healed;
    public event OnKilled Killed;

    public bool IsKilled = false;

    [SerializeField]
    private int curHealth;

    private void Start()
    {
        curHealth = MaxHealthOnStart ? MaxHealth : 0;
    }

    public void Damage(int amount, Vector3 hitPos, Vector2 hitDir)
    {
        curHealth = Mathf.Max(0, curHealth - amount);

        Damaged?.Invoke(hitPos, hitDir, amount, curHealth);

        if (curHealth <= 0)
        {
            IsKilled = true;
            Killed?.Invoke(hitPos, hitDir);
        }
    }

    public void Heal(int amount)
    {
        curHealth = Mathf.Min(MaxHealth, curHealth + amount);
        Healed?.Invoke(amount, curHealth);
    }
}

[System.Serializable]
public class DamagedEvent : UnityEvent<Vector3, Vector2, int, int>
{
}

[System.Serializable]
public class DeathEvent : UnityEvent<Vector3, Vector2>
{
}


[System.Serializable]
public class HealedEvent : UnityEvent<int, int>
{
}
