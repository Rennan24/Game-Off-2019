using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBehaviour : MonoBehaviour
{
    public int MaxHealth;

    public bool MaxHealthOnStart;

    public HealthChangedEvent OnDamaged;
    public HealthChangedEvent OnHealed;
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

        OnDamaged?.Invoke(amount, curHealth);

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
public class HealthChangedEvent : UnityEvent<int, int>
{
}
