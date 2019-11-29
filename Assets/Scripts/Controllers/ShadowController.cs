using UnityEngine;

public class ShadowController : MonoBehaviour
{
    public Transform Origin;
    public float maxHeight = 3.0f;

    private Vector3 startPos;
    private Vector3 startSize;

    private void Start()
    {
        startPos = transform.position;
        startSize = transform.localScale;
    }

    private void LateUpdate ()
    {
        float height = Vector3.SqrMagnitude(Origin.position - transform.position);
        float ratio = Mathf.Clamp(1.0f - height / (maxHeight * maxHeight), 0.0f, 1.0f);

        transform.localScale = startSize * ratio;
    }
}
