using Spine.Unity;
using UnityEngine;

public class SetSpineAnimSMB : SMB<SpineAnimatorController>
{
    [SerializeField] private AnimationReferenceAsset animation;
    [SerializeField] private int track = 0;
    [SerializeField] private bool loop = false;
    [SerializeField] private float timeScale = 1.0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Agent.SetAnimation(track, animation, loop, timeScale);
    }
}
