using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTimeline : MonoBehaviour
{

    public AnimationCurve ZombieSpawnProbability;
    public AnimationCurve SkeletonSpawnProbability;
    public AnimationCurve GhostSpawnProbability;
    public AnimationCurve SpawnAmount;

    public float MaxTime = 10 * 60;

    public float curTime;

    private float zombieSpawn, skeletonSpawn, ghostSpawn;
    private int spawnAmount;


    // Update is called once per frame
    private void Update()
    {
        curTime = Time.deltaTime;

        var t = Mathf.InverseLerp(0, MaxTime, curTime);

        zombieSpawn = ZombieSpawnProbability.Evaluate(t);
        skeletonSpawn = SkeletonSpawnProbability.Evaluate(t);
        ghostSpawn = GhostSpawnProbability.Evaluate(t);
    }
}
