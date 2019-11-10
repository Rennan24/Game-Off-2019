using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CameraFollower : MonoBehaviourSingleton<CameraFollower>
{
    public Transform Target;
    public Vector3 Offset = new Vector3(0, 0, -10);
    public float SmoothTime;

    private Transform parent;
    private Vector3 position;
    private Vector3 velocity;

    public override void Awake()
    {
        base.Awake();
        parent = new GameObject("Camera Follower").transform;
        transform.SetParent(parent);

        position = parent.position = Target.position + Offset;
        transform.localPosition = Vector3.zero;
    }

    private void FixedUpdate()
    {
        position = Vector3.SmoothDamp(parent.position, Target.position + Offset, ref velocity, SmoothTime * Time.deltaTime);
        parent.position = position;
    }

    public void Shake(Vector3 punch, float time)
    {
        var randomX = Random.Range(-punch.x, punch.x);
        var randomY = Random.Range(-punch.y, punch.y);
        transform.DOPunchPosition(new Vector3(randomX, randomY), time);
    }
}
