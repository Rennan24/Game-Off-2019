using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DashBehaviour : MonoBehaviour
{
    [Header("Movement Vars:")]
    public float Speed;
    public float Length;
    public float Acceleration;

    [Header("Trail Vars:")]
    public float SpawnTime;
    public float FadeTime;

    [SerializeField]
    private Rigidbody2D body2D;

    [SerializeField]
    private SpriteRenderer spriteRef;

    private Vector2 curVel;
    private Vector2 targetVel;
    private float dashTimer;
    private float trailTimer;

    public bool IsDashing => dashTimer > 0f;

    private void Update()
    {
        if (dashTimer <= 0f)
            return;

        curVel = Vector2.MoveTowards(curVel, targetVel, Acceleration * Time.deltaTime);
        dashTimer -= Time.deltaTime;
        trailTimer += Time.deltaTime;

        if (trailTimer >= SpawnTime)
        {
            DashTrailManager.SpawnTrail(spriteRef, FadeTime, transform);
            trailTimer = 0;
        }
    }

    private void FixedUpdate()
    {
        if (dashTimer <= 0f)
            return;

        body2D.velocity = curVel;
    }

    public void Dash(Vector2 dir)
    {
        targetVel = dir * Speed;
        curVel = body2D.velocity;
        dashTimer = Length;
        trailTimer = 0;
    }
}
