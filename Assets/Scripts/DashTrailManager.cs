using DG.Tweening;
using UnityEngine;

public class DashTrailManager : MonoBehaviourSingleton<DashTrailManager>
{
    public SpriteRenderer DashSpritePrefab;

    public static void SpawnTrail(SpriteRenderer sprite, float fadeTime, Transform trans)
    {
//        var trail = Instantiate(Inst.DashSpritePrefab, trans.position, trans.rotation);
//        trail.sprite = sprite.sprite;
//        trail.transform.localScale = trans.localScale;
//
//        trail.DOFade(0f, fadeTime);
//        Destroy(trail.gameObject, fadeTime);
    }
}
