
using Spine;
using Spine.Unity;
using UnityEngine;

public static class SkeletonAnimationHelpers
{
    public static TrackEntry SetAnimation(this SkeletonAnimation animator, int trackIndex, Spine.Animation anim, bool loop, float timeScale = 1f)
    {
        return animator.state.SetAnimation(trackIndex, anim, loop, timeScale);
    }

    public static void FlipX(this SkeletonAnimation animator)
    {
        animator.skeleton.ScaleX *= -1;
    }
}
