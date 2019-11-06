using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 3;
    public float MovementAcceleration;
    public float DashSpeed;
    public float DashTime;
    public float DashAcceleration;
    public int MaxDashCount;

    public Vector2 Velocity;
    private InputMapping input;
    private Vector2 dashDir;
    private float dashTimer;
    private int curDashCount;

    [Header("References:")]
    [SerializeField]
    private Rigidbody2D body;

    private void Awake()
    {
        input = new InputMapping();
    }

    private void Update()
    {
        var dt = Time.deltaTime;
        var dir = input.Player.Move.ReadValue<Vector2>();
        var boost = input.Player.Boost.triggered;

        if (boost)
        {
            dashDir = dir;
            dashTimer = DashTime;
            curDashCount -= 1;
        }

        if (dashTimer > 0f)
        {
            Velocity = Vector2.MoveTowards(Velocity, dashDir * DashSpeed, DashAcceleration * dt);
        }
        else
        {
            Velocity = Vector2.MoveTowards(Velocity, dir * MovementSpeed, MovementAcceleration * dt);
        }

        dashTimer -= dt;
    }

    private void FixedUpdate()
    {
        body.velocity = Velocity;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
