﻿using UnityEngine;
using UnityEngine.Animations;

public class SceneLinkedSMB<TMonoBehaviour> : SealedSMB where TMonoBehaviour : MonoBehaviour
{
    protected TMonoBehaviour Agent;

    public static void Initialise(Animator animator, TMonoBehaviour monoBehaviour)
    {
        var sceneLinkedSMBs = animator.GetBehaviours<SceneLinkedSMB<TMonoBehaviour>>();

        foreach (var smb in sceneLinkedSMBs)
            smb.InitializeInternal(animator, monoBehaviour);
    }

    private void InitializeInternal(Animator animator, TMonoBehaviour monoBehaviour)
    {
        Agent = monoBehaviour;
        OnInitialize(animator);
    }

    /// <summary>
    /// Called by a MonoBehaviour in the scene during its Start function.
    /// </summary>
    protected virtual void OnInitialize(Animator animator)
    {

    }

//    public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
//    {
//        OnSLStateEnter(animator, stateInfo, layerIndex);
//        OnSLStateEnter(animator, stateInfo, layerIndex, controller);
//    }
//
//    public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
//    {
//        if (!animator.gameObject.activeSelf)
//            return;
//
//
//    }
//
//    public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
//        AnimatorControllerPlayable controller)
//    {
//        m_LastFrameHappened = false;
//
//        OnSLStateExit(animator, stateInfo, layerIndex);
//        OnSLStateExit(animator, stateInfo, layerIndex, controller);
//    }
//
//
//
//    /// <summary>
//    /// Called before Updates when execution of the state first starts (on transition to the state).
//    /// </summary>
//    public virtual void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//    }
//
//    /// <summary>
//    /// Called after OnSLStateEnter every frame during transition to the state.
//    /// </summary>
//    public virtual void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//    }
//
//    /// <summary>
//    /// Called on the first frame after the transition to the state has finished.
//    /// </summary>
//    public virtual void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//    }
//
//    /// <summary>
//    /// Called every frame after PostEnter when the state is not being transitioned to or from.
//    /// </summary>
//    public virtual void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//    }
//
//    /// <summary>
//    /// Called on the first frame after the transition from the state has started.  Note that if the transition has a duration of less than a frame, this will not be called.
//    /// </summary>
//    public virtual void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//    }
//
//    /// <summary>
//    /// Called after OnSLStatePreExit every frame during transition to the state.
//    /// </summary>
//    public virtual void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//    }
//
//    /// <summary>
//    /// Called after Updates when execution of the state first finshes (after transition from the state).
//    /// </summary>
//    public virtual void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//    }
//
//    /// <summary>
//    /// Called before Updates when execution of the state first starts (on transition to the state).
//    /// </summary>
//    public virtual void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
//        AnimatorControllerPlayable controller)
//    {
//    }
//
//    /// <summary>
//    /// Called after OnSLStateEnter every frame during transition to the state.
//    /// </summary>
//    public virtual void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
//        AnimatorControllerPlayable controller)
//    {
//    }
//
//    /// <summary>
//    /// Called on the first frame after the transition to the state has finished.
//    /// </summary>
//    public virtual void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
//        AnimatorControllerPlayable controller)
//    {
//    }
//
//    /// <summary>
//    /// Called every frame when the state is not being transitioned to or from.
//    /// </summary>
//    public virtual void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
//        AnimatorControllerPlayable controller)
//    {
//    }
//
//    /// <summary>
//    /// Called on the first frame after the transition from the state has started.  Note that if the transition has a duration of less than a frame, this will not be called.
//    /// </summary>
//    public virtual void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
//        AnimatorControllerPlayable controller)
//    {
//    }
//
//    /// <summary>
//    /// Called after OnSLStatePreExit every frame during transition to the state.
//    /// </summary>
//    public virtual void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
//        AnimatorControllerPlayable controller)
//    {
//    }
//
//    /// <summary>
//    /// Called after Updates when execution of the state first finshes (after transition from the state).
//    /// </summary>
//    public virtual void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
//        AnimatorControllerPlayable controller)
//    {
//    }
}

public abstract class SealedSMB : StateMachineBehaviour
{
    public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
