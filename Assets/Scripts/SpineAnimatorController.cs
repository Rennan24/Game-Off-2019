using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SpineAnimatorController : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] private Animator animator;
    [SerializeField] private SkeletonAnimation skeletonAnimation;

    private void Start()
    {
        SMB<SpineAnimatorController>.Initialize(animator, this);
    }

    public TrackEntry SetAnimation(int track, Spine.Animation anim, bool loop = false, float timeScale = 1.0f)
    {
        return skeletonAnimation.state.SetAnimation(track, anim, loop, timeScale);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        animator = GetComponent<Animator>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }
#endif
}

public abstract class MonoSMB : StateMachineBehaviour
{
    public static void Initialize(Animator animator)
    {
        foreach (var smb in animator.GetBehaviours<MonoSMB>())
            smb.Init(animator, animator.gameObject);
    }

    protected void Init(Animator animator, GameObject go)
    {
        OnInit(animator, go);
    }

    protected abstract void OnInit(Animator animator, GameObject go);
}

public class SpineAnimSMB : MonoSMB
{
    private SkeletonAnimation skeletonAnimation;

    protected override void OnInit(Animator animator, GameObject go)
    {
        skeletonAnimation = animator.GetComponentInChildren<SkeletonAnimation>();
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}

public abstract class SMB<TAgent> : StateMachineBehaviour where TAgent : MonoBehaviour
{
    protected TAgent Agent;

    public static void Initialize(Animator animator, TAgent agent)
    {
        var smbs = animator.GetBehaviours<SMB<TAgent>>();

        foreach (var smb in smbs)
            smb.InitState(animator, agent);
    }

    private void InitState(Animator animator, TAgent agent)
    {
        Agent = agent;
        OnInit(animator);
    }

    protected virtual void OnInit(Animator animator) { }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
