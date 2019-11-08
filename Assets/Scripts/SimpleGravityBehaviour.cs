using UnityEngine;

public class SimpleGravityBehaviour : MonoBehaviour
{
    public Vector2 Velocity;
    public float Gravity = -9.81f;

    public float horizontalDrag;

    private void Update()
    {
        Velocity.y += Gravity * Time.deltaTime;
        Velocity.x = Mathf.MoveTowards(Velocity.x, 0f, horizontalDrag * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(Velocity * Time.deltaTime);

    }
}

