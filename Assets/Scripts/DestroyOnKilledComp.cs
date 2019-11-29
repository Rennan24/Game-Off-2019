using System;
using UnityEngine;

public class DestroyOnKilledComp : MonoBehaviour
{
    [SerializeField]
    private HealthBehaviour healthRef;

    private void Start()
    {
        healthRef.Killed += OnKilled;
    }

    private void OnKilled(Vector3 hitPos, Vector2 hitDir)
    {
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        healthRef = GetComponent<HealthBehaviour>();
    }
#endif
}
