using UnityEngine;

public class VelocityBehaviour : MonoBehaviour
{
    public Vector3 Value;

    public void FixedUpdate()
    {
        transform.position += Value * Time.deltaTime;
    }
}
