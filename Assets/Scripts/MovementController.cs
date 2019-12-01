using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body2DRef;

    public Vector2 Velocity { get; private set; }

    [HideInInspector]
    public float VelocitySqrMagnitude;

    private Vector2 movement;
    private Vector2 currentPos;
    private Vector2 previousPos;

    public void Move(Vector2 move)
    {
        movement += Time.deltaTime * move;
    }

    public Vector2 MoveTowards(Vector2 pos, float speed)
    {
        var dir = (pos - (Vector2)transform.position).normalized;
        movement += Time.deltaTime * speed * dir;
        return dir;
    }

    public void Teleport(Vector2 pos)
    {
        body2DRef.position = pos;
    }

    private void FixedUpdate()
    {
        previousPos = body2DRef.position;
        currentPos = previousPos + movement;
        Velocity = (currentPos - previousPos) / Time.fixedDeltaTime;
        VelocitySqrMagnitude = Velocity.sqrMagnitude;

        body2DRef.MovePosition(currentPos);
        movement = Vector2.zero;
    }

#if UNITY_EDITOR
    private void Reset()
    {
        body2DRef = GetComponent<Rigidbody2D>();
        body2DRef.constraints = RigidbodyConstraints2D.FreezeRotation;
        body2DRef.gravityScale = 0;

    }
#endif
}
