using UnityEngine;

public class ChaseBehaviour : MonoBehaviour
{
    public float ChaseSpeed;
    public float ChaseRadius;
    public Transform Target;

    [Header("References:")]
    [SerializeField]
    private MovementController controllerRef;

    private void Awake()
    {
        var health = GetComponent<HealthBehaviour>();
        if (health != null)
        {
            health.Killed += StopChaseOnKilled;
        }

        Target = FindObjectOfType<PlayerController>().transform;
    }

    private void StopChaseOnKilled(Vector3 hitpos, Vector2 hitdir)
    {
        enabled = false;
    }

    private void FixedUpdate()
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
            controllerRef.Move(targetDir * ChaseSpeed);
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        var color = new Color(1, 0, 0, 1f);
        WHandles.DrawWireDisk(transform.position, ChaseRadius, color);
    }

    private void Reset()
    {
        controllerRef = GetComponentInChildren<MovementController>();
    }
#endif
}
