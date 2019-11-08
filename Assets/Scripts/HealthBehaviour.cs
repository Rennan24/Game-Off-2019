using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBehaviour : MonoBehaviour
{
    public int MaxHealth;

    public bool MaxHealthOnStart;

    public DamagedEvent OnDamaged;
    public HealedEvent OnHealed;
    public UnityEvent OnDeath;

    [SerializeField]
    private int curHealth;

    private void Start()
    {
        curHealth = MaxHealthOnStart ? MaxHealth : 0;
    }

    public void Damage(int amount)
    {
        curHealth = Mathf.Max(0, curHealth - amount);

        OnDamaged?.Invoke(transform.position, amount, curHealth);

        if (curHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        curHealth = Mathf.Min(MaxHealth, curHealth + amount);

        OnHealed?.Invoke(amount, curHealth);
    }
}

[System.Serializable]
public class DamagedEvent : UnityEvent<Vector3, int, int>
{
}

[System.Serializable]
public class HealedEvent : UnityEvent<int, int>
{
}
