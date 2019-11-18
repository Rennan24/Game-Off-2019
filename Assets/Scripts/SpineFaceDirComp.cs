using Spine.Unity;
using UnityEngine;

public class SpineFaceDirComp : MonoBehaviour
{
    [SerializeField]
    private TopdownController controller;

    [SerializeField]
    private SkeletonAnimation animator;

    private void LateUpdate()
    {
        var scaleX = animator.skeleton.ScaleX;
        animator.skeleton.ScaleX = Mathf.Abs(scaleX) * Mathf.Sign(controller.Velocity.x);
    }
}
