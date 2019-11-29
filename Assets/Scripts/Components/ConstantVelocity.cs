using UnityEngine;

public class ConstantVelocity : MonoBehaviour
{
    public Vector3 Value;

    public void FixedUpdate()
    {
        transform.position += Value * Time.deltaTime;
    }
}
