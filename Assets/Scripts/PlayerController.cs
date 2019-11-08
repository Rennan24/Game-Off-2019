using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 3;
    public float MovementAcceleration;
    public float DashSpeed;
    public float DashTime;
    public float DashAcceleration;

    public int MaxDashCount;

    public GunBehaviour Gun;
    public Vector3 GunOffset;

    public Vector2 Velocity;
    private InputMapping input;
    private Vector2 dashDir;
    private float dashTimer;

    private int curDashCount;

    [Header("References:")]
    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private SpriteRenderer playerRenderer;

    [SerializeField]
    private SpriteRenderer gunRenderer;

    private Camera camera;

    private void Awake()
    {
        input = new InputMapping();

        camera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        var dt = Time.deltaTime;
        var dir = input.Player.Move.ReadValue<Vector2>();
        var boost = input.Player.Boost.triggered;
        var fired = input.Player.Fire.ReadValue<float>() > 0.1f;

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -camera.transform.position.z;
        var mouseWorldPos = camera.ScreenToWorldPoint(mouseScreenPos);

        var mouseTargetDir = mouseWorldPos - (transform.position + GunOffset);

        var gunRotZ = Mathf.Atan2(mouseTargetDir.y, mouseTargetDir.x) * Mathf.Rad2Deg;
        var gunPos = transform.position + GunOffset + mouseTargetDir.normalized;
        var gunQuat = Quaternion.Euler(0, 0, gunRotZ);

        Gun.transform.SetPositionAndRotation(gunPos, gunQuat);
//        Gun.transform.rotation = gunQuat;

        // flip renderer if mouse is to the left of the player
        playerRenderer.flipX = mouseTargetDir.x < 0;
        gunRenderer.flipY = mouseTargetDir.x < 0;

        if (fired)
        {
            Gun.Fire(mouseTargetDir.normalized);
        }

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
