using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public Vector3 Value;

    public void FixedUpdate()
    {
        transform.Rotate(Value * Time.fixedDeltaTime);
    }
}
