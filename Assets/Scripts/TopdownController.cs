using UnityEngine;

public class TopdownController : MonoBehaviour
{
    public float MovementAcceleration;

    [Header("References:")]
    [SerializeField]
    private Rigidbody2D body2DRef;

    public Vector2 Velocity { get; private set; }

    private Vector2 movement;
    private Vector2 currentPos;
    private Vector2 previousPos;

    public void Move(Vector2 move)
    {
        movement += move * Time.deltaTime;
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

        body2DRef.MovePosition(currentPos);
        movement = Vector2.zero;
    }

#if UNITY_EDITOR
    private void Reset()
    {
        body2DRef = GetComponent<Rigidbody2D>();
    }
#endif
}
