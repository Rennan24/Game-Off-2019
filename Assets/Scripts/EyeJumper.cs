using System;
using System.Collections;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class EyeJumper : MonoBehaviour
{
    public DelayTimer JumpTime;
    public float MoveSpeed;
    public float JumpHeight;
    public float JumpLength;
    public Transform Target;

    private Vector3 startPos;
    private Vector3 offset;

    [SerializeField, SpineAnimation]
    private string JumpAnim;

    [SerializeField, SpineAnimation]
    private string LandAnim;

    [SerializeField]
    private Transform visualtf;

    [SerializeField]
    private TopdownController controller;

    [SerializeField]
    private SkeletonAnimation animator;

//    private float t;

    private void Start()
    {
        startPos = transform.position;
        StartCoroutine(JumpVisual());
    }


    public void Jump(Vector3 pos)
    {
        var targetDist = (pos - transform.position);
        var targetDir = targetDist.normalized;

        var stopPos = targetDir * JumpLength;
    }

    public IEnumerator JumpVisual()
    {
        while (true)
        {
            animator.state.SetAnimation(0, JumpAnim, false);
            yield return new WaitForSpineEvent(animator, "Jumped");

            yield return new DOTweenCYInstruction.WaitForCompletion(visualtf.DOLocalMoveY(JumpHeight, 0.25f).SetEase(Ease.InOutSine));

            visualtf.DOLocalMoveY(0, 0.25f).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(0.2f);

            animator.state.SetAnimation(0, LandAnim, false);
        }
    }

    public void FixedUpdate()
    {


        var targetDist = (Target.position - transform.position);
        var targetDir = targetDist.normalized;

        controller.Move(targetDir * MoveSpeed);
    }
}
