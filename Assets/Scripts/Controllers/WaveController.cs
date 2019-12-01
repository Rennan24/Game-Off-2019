using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public Transform player;
    public WaveData[] WaveDatas;

    public int WaveIndex = 0;

    public int EnemiesLeft;

    public IEnumerator Start()
    {
        player = GameManager.Player.transform;

//        for (int i = 0; i < WaveDatas.Length; i++)
//        {
//            var waveData = WaveDatas[i];
//            yield return StartCoroutine(waveData.BeginWave(this));
//        }

        yield return null;
//        timer = WaveDatas[WaveIndex].SpawnDelay;
    }
}

public interface IWaveEnemy
{
    WaveController WaveController { get; set; }
}
