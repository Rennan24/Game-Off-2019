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

    [Header("References:")]
    [SerializeField]
    private TopdownController controllerRef;

    [SerializeField]
    private SpriteRenderer spriteRef;

    private Vector2 dashVel;
    private float dashTimer;
    private float trailTimer;

    public bool IsDashing => dashTimer > 0f;

    private void Update()
    {
        if (dashTimer <= 0f)
            return;

        controllerRef.Move(dashVel);
        dashTimer -= Time.deltaTime;
        trailTimer += Time.deltaTime;

        if (trailTimer >= SpawnTime)
        {
            DashTrailManager.SpawnTrail(spriteRef, FadeTime, transform);
            trailTimer = 0;
        }
    }

    public void Dash(Vector2 dir)
    {
        dashVel = dir * Speed;
        dashTimer = Length;
        trailTimer = 0;
    }
}
