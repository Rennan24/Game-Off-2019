using System;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset = new Vector3(0, 0, -10);
    public float SmoothTime;

    private Vector3 position;
    private Vector3 velocity;

    private void Awake()
    {
        position = transform.position;
        transform.position = Target.position + Offset;
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        position = Vector3.SmoothDamp(transform.position, Target.position + Offset, ref velocity, SmoothTime);
        transform.position = position;
        //Test
    }
}
