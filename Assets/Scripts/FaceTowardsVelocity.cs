using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTowardsVelocity : MonoBehaviour
{
    public Vector3 Normal;

    [SerializeField]
    private VelocityBehaviour velocity;

    private void Reset()
    {
        Normal = transform.up;
        velocity = GetComponent<VelocityBehaviour>();
    }

    private void Update()
    {
        var rotZ = Mathf.Atan2(velocity.Value.y, velocity.Value.x);
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
