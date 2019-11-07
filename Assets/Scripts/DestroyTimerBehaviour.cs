using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimerBehaviour : MonoBehaviour
{
    [SerializeField]
    private float time = 1f;

    private void Start()
    {
        Destroy(gameObject, time);
    }
}
