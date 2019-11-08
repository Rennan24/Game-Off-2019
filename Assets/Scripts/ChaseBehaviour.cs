using System;
using UnityEngine;

public class ChaseBehaviour : MonoBehaviour
{
    public float ChaseSpeed;
    public float ChaseAcceleration;
    public float ChaseRadius;
    public Transform Target;

    public Vector2 Velocity;

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        var color = new Color(1, 0, 0, 1f);
        WHandles.DrawWireDisk(transform.position, ChaseRadius, color);
    }
#endif

    private void Update()
    {
        var targetVec = Target.transform.position - transform.position;
        var distsqr = targetVec.sqrMagnitude;
        var targetDir = targetVec.normalized;

        //Chasing to the right
        var scale = transform.localScale;
        if (targetVec.x > 0.1f)
        {
            transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
        }
        else if (targetVec.x < 0.1f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);
        }

        if (distsqr < ChaseRadius * ChaseRadius)
        {
            Velocity = Vector2.MoveTowards(Velocity, targetDir * ChaseSpeed, ChaseAcceleration * Time.deltaTime);
        }
        else
        {
            Velocity = Vector2.MoveTowards(Vector2.zero, Velocity, ChaseAcceleration * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Velocity * Time.deltaTime);
    }
}
