using System;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform Target;

    private void Awake()
    {
        transform.SetParent(null);
    }

    private void LateUpdate()
    {
        transform.position = Target.position;
    }
}
